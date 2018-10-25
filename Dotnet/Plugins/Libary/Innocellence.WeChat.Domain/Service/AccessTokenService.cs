using Infrastructure.Core.Data;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Contracts.ViewModel;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Linq;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// AccessTokenService
    /// </summary>
    public class AccessTokenService : BaseService<AccessTokenEntity>, IAccessTokenService
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AccessTokenService()
        {

        }

        /// <summary>
        /// GetUserIdByToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetUserIdByToken(string token)
        {
            var accessToken = Repository.Entities.FirstOrDefault(a => a.Token == token && a.ExpiredDate < DateTime.Now && a.IsDeleted != true);
            if (accessToken == null)
            {
                throw new UnauthorizedAccessException("AccessToken not exists! - " + token);
            }

            return accessToken.UserId;
        }

        /// <summary>
        /// 创建一个accessToken，并返回
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="unionId"></param>
        /// <param name="userType"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public string CreateAccessToken(int appid, string userId, string openId, string unionId, string userType, int expiresIn = 5)
        {
            var guid = Guid.NewGuid().ToString();
            InsertView(new AccessTokenView()
            {
                Token = guid,
                ExpiredDate = DateTime.Now.AddMinutes(expiresIn),
                AppId = appid.ToString(),
                UserId = userId,
                OpenId = openId,
                UnionId = unionId,
                UserType = userType,
                IsValid = 1
            });

            return guid;
        }


        /// <summary>
        /// 根据token验证合法性，并且将token置为无效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AccessTokenView ValidateAccessToken(string token)
        {
            // 取
            var accessToken = Repository.Entities.FirstOrDefault(a => a.Token == token);
            if(accessToken == null)
            {
                throw new InvalidOperationException("Token is not exists! - " + token);
            }

            // 验证时间
            if (accessToken.ExpiredDate < DateTime.Now)
            {
                throw new InvalidOperationException("Token is Expired! - " + token);
            }

            // 设置过期
            accessToken.IsValid = 0;

            return (AccessTokenView)(new AccessTokenView().ConvertAPIModel(accessToken));
        }

    }
}
