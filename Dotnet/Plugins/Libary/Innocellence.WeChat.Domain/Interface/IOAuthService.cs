using System.Linq;
using System.Net;
using System.Web;
using Infrastructure.Core;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;
using EntityFramework.Extensions;

namespace Innocellence.WeChat.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOAuthService : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        string GetToken(string clientId, string userId, string returnUrl);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="clientId"></param>
        ///// <param name="userId"></param>
        ///// <param name="returnUrl"></param>
        ///// <param name="expiredTime">minites</param>
        ///// <returns></returns>
        //string GetToken(string clientId, string userId, string returnUrl, int expiredTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Result<object> GetUserId(string clientId, string token);

        /// <summary>
        /// 
        /// </summary>
        TokenContext TokenContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GenerateToken(string clientId, string userId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVerfyOAuthRule<out T> : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        T Verfy(TokenContext context);
    }

    /// <summary>
    /// 
    /// </summary>
    public class TokenContext
    {
        /// <summary>
        /// 
        /// </summary>
        public OAuthClientsEntity OAuthClient { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OAuthTokenEntity AuthToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TargetToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientDomain { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OAuthClientStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Enabled,
        /// <summary>
        /// 
        /// </summary>
        Disabled
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OAuthTokenStatus
    {
        /// <summary>
        /// 
        /// </summary>
        InvalidClientId = 100,

        /// <summary>
        /// 
        /// </summary>
        InvalidToken = 101,

        /// <summary>
        /// 
        /// </summary>
        ExpiredToken = 102,

        /// <summary>
        /// 
        /// </summary>
        InvalidClientDomain = 103,

        /// <summary>
        /// 
        /// </summary>
        Reused = 104,

        /// <summary>
        /// 
        /// </summary>
        Ok = 200,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class OAuthBaseService
    {
        protected IOauthClientDataService ClientDataService;
        protected IOauthTokenDataService OauthTokenDataService;
        protected string Identity;
        private TokenContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientDataService"></param>
        /// <param name="oauthTokenDataService"></param>
        protected OAuthBaseService(IOauthClientDataService clientDataService, IOauthTokenDataService oauthTokenDataService)
        {
            ClientDataService = clientDataService;
            OauthTokenDataService = oauthTokenDataService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual TokenContext GetTokenContext()
        {
            _context = new TokenContext();


            if (string.IsNullOrEmpty(Identity))
            {
                return _context;
            }

            //var client = ClientDataService.Repository.Entities.Where(x => x.ClientId == Identity).Select(x => new OAuthClientsEntity { ClientCallBackUrl = x.ClientCallBackUrl }).Include(x => x.Tokens.Select(y => new OAuthTokenEntity { Token = y.Token, UserId = y.UserId }).OrderByDescending(y => y.CreatedDateUtc).FirstOrDefault()).FirstOrDefault();
            var clientQuery = ClientDataService.Repository.Entities.Where(x => x.ClientId == Identity).FutureFirstOrDefault();
            var tokenQuery =
                OauthTokenDataService.Repository.Entities.Where(x => x.ClientId == Identity).OrderByDescending(x => x.Id).FutureFirstOrDefault();

            var client = clientQuery.Value;
            var token = tokenQuery.Value;

            _context.OAuthClient = client;
            _context.AuthToken = token;

            return _context;
        }

        /// <summary>
        /// 
        /// </summary>
        public TokenContext TokenContext
        {
            get
            {
                return _context ?? GetTokenContext();
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class HttpRequestBaseExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static string GetDomain(this HttpRequestBase httpRequest)
        {
            if (httpRequest == null || httpRequest.UserHostAddress == null)
            {
                return string.Empty;
            }

            var ip = IPAddress.Parse(httpRequest.UserHostAddress);
            var host = Dns.GetHostEntry(ip);

            return host.HostName;
        }
    }
}
