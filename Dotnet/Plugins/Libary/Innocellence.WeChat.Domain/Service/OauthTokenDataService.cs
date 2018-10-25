using Infrastructure.Core.Data;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class OauthTokenDataService : BaseService<OAuthTokenEntity>, IOauthTokenDataService
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class OauthClientDataService : BaseService<OAuthClientsEntity>, IOauthClientDataService
    {
    }
}
