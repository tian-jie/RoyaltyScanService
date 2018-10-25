using System;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Interface;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthClientDomainRule : IVerfyOAuthRule<Result<object>>
    {
        public Result<object> Verfy(TokenContext context)
        {
            context.ClientDomain = new Uri(context.TargetUrl).Host;

            var configedDomain = new Uri(context.OAuthClient.ClientCallBackUrl).Host;

            if (string.Compare(configedDomain, context.ClientDomain, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return new Result<object> { Status = (int)OAuthTokenStatus.InvalidClientDomain, Message = OAuthTokenStatus.InvalidClientDomain.ToString() };
            }

            return new Result<object> { Status = (int)OAuthTokenStatus.Ok };
        }
    }
}
