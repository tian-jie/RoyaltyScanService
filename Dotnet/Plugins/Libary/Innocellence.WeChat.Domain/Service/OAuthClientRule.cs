using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Interface;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthClientRule : IVerfyOAuthRule<Result<object>>
    {
        public Result<object> Verfy(TokenContext context)
        {
            if (context.OAuthClient == null || context.OAuthClient.Status == OAuthClientStatus.Disabled.ToString())
            {
                return new Result<object> { Status = (int)OAuthTokenStatus.InvalidClientId, Message = OAuthTokenStatus.InvalidClientId.ToString() };
            }
            
            return new Result<object> { Status = (int)OAuthTokenStatus.Ok };
        }
    }
}
