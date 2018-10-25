using Infrastructure.Core.Logging;
using Infrastructure.Web.Domain.Contracts;
using Infrastructure.Web.Domain.Entity;
using Infrastructure.Web.Domain.Services;
using Innocellence.WeChat.Domain.CommonEntity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Innocellence.WeChat.Domain.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LccpCommon
    {
        /// <summary>
        /// 手工登录，登录的用户信息保存到Claims里。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="configType"></param>
        /// <param name="claim"></param>
        /// <param name="createPersistentCookie"></param>
        /// <returns></returns>
        public static void SignInNoDB(IUser<int> user, IClaimEntity claim, bool createPersistentCookie=false)
        {
            var logger = LogManager.GetLogger("LccpCommon");
            if (string.IsNullOrEmpty(user.UserName))
            {
                logger.Debug("User Name can't be null");
                throw new Exception("User Name can't be null");
            }
            logger.Debug("User Signin {0}", user.UserName);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            ISysUserService userManager = new SysUserService();
            userManager.UserContext = new UserManager<SysUser, int>(new UserStoreNoDB());

            var identity = userManager.UserContext.CreateIdentity((SysUser)user, DefaultAuthenticationTypes.ApplicationCookie);

            //identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            //identity.AddClaim(new Claim("ConfigType", configType));
            identity.AddClaim(new System.Security.Claims.Claim("ConfigType", claim.ConfigType));
            identity.AddClaim(new System.Security.Claims.Claim("ConfigClaim", JsonConvert.SerializeObject(claim)));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = createPersistentCookie }, identity);
            HttpContext.Current.Session["UserInfo"] = user;
            logger.Debug("User Identity{0}",  identity.Name);

        }

        private static IAuthenticationManager AuthenticationManager
        {
            get
            {
                return ((HttpContextBase)new HttpContextWrapper(HttpContext.Current)).GetOwinContext().Authentication;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LccpLogManage
    {
        /// <summary>
        /// get lccp  logger interface
        /// </summary>
        /// <returns></returns>
        public static ILogger GetLoger()
        {
            return LogManager.GetLogger("Lccp");
        }
    }


}
