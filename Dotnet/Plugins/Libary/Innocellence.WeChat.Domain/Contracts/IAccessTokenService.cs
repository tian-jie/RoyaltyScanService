using Infrastructure.Core;
using Innocellence.WeChat.Domain.Contracts.ViewModel;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace Innocellence.WeChat.Domain.Contracts
{
    /// <summary>
    /// IAccessTokenService
    /// </summary>
    public interface IAccessTokenService : IDependency, IBaseService<AccessTokenEntity>
    {
        /// <summary>
        /// 根据AccessToken获取UserId，具体UserID怎么用，由各个业务系统自行使用
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        string GetUserIdByToken(string token);

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
        string CreateAccessToken(int appid, string userId, string openId, string unionId, string userType, int expiresIn = 5);


        /// <summary>
        /// 根据token验证合法性，并且将token置为无效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        AccessTokenView ValidateAccessToken(string token);
    }
}
