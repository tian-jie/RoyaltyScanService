
using Infrastructure.Core;
using Infrastructure.Core.Infrastructure;
using Infrastructure.Core.Logging;
using Infrastructure.Web.Domain.Entity;
using Infrastructure.Web.UI;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;
using Innocellence.WeChat.Domain.Service;
using Innocellence.WeChat.Domain.Services;
using Innocellence.WeChat.Domain.ViewModel;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
//using Innocellence.WeChatMain.Service.Common;


namespace Innocellence.WeChat.Domain
{
    /// <summary>
    /// 基类BaseController，过滤器
    /// </summary>
    // [HandleError]

    //[FilterError]
    //[CustomAuthorize]

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class WeChatBaseController<T, T1> : ParentController<T, T1>
        where T : EntityBase<int>, new()
        where T1 : IViewModel, new()
    {
        /// <summary>
        /// 保存自己的ClaimEntity对象，BaseController里反序列化，Controller里可以直接用
        /// </summary>
        public virtual IClaimEntity ClaimEntity { get; set; }

        /// <summary>
        /// 当前的DBService
        /// </summary>
        public IBaseService<T> _newsService;

        private WeChatAppUserService _weChatAppUserService = new WeChatAppUserService();
        private IAddressBookService _addressBookService = EngineContext.Current.Resolve<IAddressBookService>();
        private IWechatMPUserService _wechatMPUserService = EngineContext.Current.Resolve<IWechatMPUserService>();

        /// <summary>
        /// 全局用户对象，当前的登录用户
        /// </summary>
        public SysUser objLoginInfo;

        /// <summary>
        /// 配置这个appid的config项目
        /// </summary>
        public readonly string AppIdConfig;

        private string _strConfigKey;


        private string backRedirectUrl = string.Empty;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger _logger = LogManager.GetLogger("WeChat");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        public WeChatBaseController(IBaseService<T> newsService)
            : base(newsService)
        {
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.DateFormatHandling
            //= Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            _newsService = newsService;


            objLoginInfo = new SysUser();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        /// <param name="strKey"></param>
        public WeChatBaseController(IBaseService<T> newsService, string strKey)
            : base(newsService)
        {
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.DateFormatHandling
            //= Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            _newsService = newsService;

            _logger.Debug("WeChatBaseController Constructor, strKey=" + strKey);

            _strConfigKey = strKey;
            objLoginInfo = new SysUser();

        }

        /// <summary>
        /// OnBaseActionExecuting
        /// </summary>
        /// <param name="filterContext"></param>
        protected void OnBaseActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// GetFirstLoginUrl
        /// </summary>
        /// <returns></returns>
        public string GetFirstLoginUrl()
        {
            return this.backRedirectUrl;
        }

        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Get AppId
            //string strwechatid = Request["wechatid"];

            //if (string.IsNullOrEmpty(strwechatid) && RouteData.Values.ContainsKey("appid"))
            //{
            //    strwechatid = RouteData.Values["appid"].ToString();
            //}
            //else if (string.IsNullOrEmpty(strwechatid))
            //{
            //    log.Error("wechatid not found!" + Request.Url);
            //    filterContext.Result = new ContentResult() { Content = "wechatid not found!" };
            //}
            //AppId = int.Parse(strwechatid);
            #endregion


            _logger.Debug("OnActionExecuting Not WxDetail:{0}", filterContext.RequestContext.HttpContext.Request.Url, filterContext.RequestContext.HttpContext.Request.RawUrl);

            backRedirectUrl = string.Empty;
            // 想办法获取用户的XXX id（或者用户id）
            ViewBag.WeChatUserID = "";

            string wechatUserId = GetWeChatUserID(filterContext);

            if (string.IsNullOrEmpty(wechatUserId))
            {
                NoPermissionRedirectTo(filterContext);
            }

            _logger.Debug("GetWeChatUserID() : " + objLoginInfo.WeChatUserID);
            ViewBag.WeChatUserID = objLoginInfo.WeChatUserID;
            ViewBag.WeChatUserName = objLoginInfo.UserName;

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 记录用户行为日志
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="type"></param>
        /// <param name="typecontent"></param>
        /// <param name="content"></param>
        public void ExecuteBehavior(int appid, int type, string typecontent, string content = null)
        {
            ////记录用户行为
            //WebRequestPost wr = new WebRequestPost();
            string url = Request.RawUrl.ToUpper();//带参数
            ////将id作为内容插入到数据库中

            string functionid = Request.FilePath.ToUpper();//不带参数（不带？后面的参数，但如果是/123则带）



            Task.Run(() =>
            {
                IUserBehaviorService _objService = new UserBehaviorService();
                _objService.Repository.Insert(new UserBehavior()
                {
                    UserId = ViewBag.WeChatUserID,
                    FunctionId = functionid,
                    AppId = appid,
                    Content = (string.IsNullOrEmpty(typecontent) ? content : typecontent),
                    Url = url,
                    ContentType = type,
                    Device = Request.UserAgent,
                    ClientIp = Request.UserHostAddress,
                    CreatedTime = DateTime.Now
                });
            });

        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="_string"></param>
        /// <returns></returns>
        public static bool isNumberic1(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取已经登陆用户的用户信息
        /// </summary>
        /// <returns></returns>
        private string GetLoginUserInfo(ActionExecutingContext filterContext)
        {
            backRedirectUrl = string.Empty;
            if (Request.UserAgent.IndexOf("MicroMessenger") < 0)
            {
                _logger.Debug("Not MicroMessenger. current agent: {0}", Request.UserAgent);
                NoPermissionRedirectTo(filterContext);
                return string.Empty;
            }


            // 不知道Identity和IsAuthenticated这2个到底是干什么的...，合并处理吧
            var windowsIdentity = User.Identity as ClaimsIdentity;
            _logger.Debug("HttpContext.Request.IsAuthenticated={0}, windowsIdentity={1}", HttpContext.Request.IsAuthenticated, windowsIdentity == null ? "null" : windowsIdentity.Name);
            if (!HttpContext.Request.IsAuthenticated || windowsIdentity == null || string.IsNullOrEmpty(windowsIdentity.Name))
            {
                NoPermissionRedirectTo(filterContext);
                return string.Empty;
            }

            // 判断cookies里的用户类型和当前要使用的用户类型是否匹配，如果不匹配则引导重新登录
            var claimConfigType = windowsIdentity.Claims.FirstOrDefault(x => x.Type == "ConfigType");
            var claimConfig = windowsIdentity.Claims.FirstOrDefault(x => x.Type == "ConfigClaim");
            _logger.Debug("claimConfigType={0}, claimConfig={1}, currentConfigType={2}", claimConfigType, claimConfig, _strConfigKey);

            if (claimConfig == null || claimConfigType == null || !claimConfigType.Value.Equals(_strConfigKey + "Claim", StringComparison.OrdinalIgnoreCase))
            {
                _logger.Debug("Claim Not found or ConfigType not match! Redirecting...");
                NoPermissionRedirectTo(filterContext);
                return String.Empty;
            }

            switch (_strConfigKey)
            {
                case "WeChatLccpHcp":
                    // { openId:xxx, hcpId: xxx, dctrId: xxx}
                    ClaimEntity = JsonConvert.DeserializeObject<WeChatLccpHcpClaim>(claimConfig.Value);
                    if (string.Equals(ClaimEntity.UserId, "GuestToRegist"))
                    {
                        _logger.Debug("Prelogin user for regist {0}.",windowsIdentity.Name);
                        NoPermissionRedirectTo(filterContext);
                    }
                    break;

                case "WeChatLccpNurse":
                    // { openId:xxx, projectNurseId: xxx, nurseId: xxx}
                    ClaimEntity = JsonConvert.DeserializeObject<WeChatLccpNurseClaim>(claimConfig.Value);
                    break;

                case "WeChatLccpRep":
                    // { employeeId:xxx, globalId: xxx, role: xxx}
                    ClaimEntity = JsonConvert.DeserializeObject<WeChatLccpRepClaim>(claimConfig.Value);
                    break;

                case "WeChatLccpDrugStore":
                    // { employeeId:xxx, drugStoreId: xxx, repId: xxx, AAA:xxx}
                    ClaimEntity = JsonConvert.DeserializeObject<WeChatLccpDrugStoreClaim>(claimConfig.Value);
                    break;

                //case "WeChatCorpAssistMiniProgram":
                //    // { employeeId:xxx, drugStoreId: xxx, repId: xxx, AAA:xxx}
                //    ClaimEntity = JsonConvert.DeserializeObject<WeChatLccpDrugStoreClaim>(claimConfig.Value);
                //    break;

                //case ClaimTypes.Name:
                //    objLoginInfo.UserName = claimConfig.Value;
                //    break;

                //case ClaimTypes.NameIdentifier:
                //    int Id;
                //    if (int.TryParse(claim.Value, out Id))
                //    {
                //        objLoginInfo.Id = Id;
                //    }
                //    break;

                default:
                    break;
            }

            //// 如果上面没有mapping上，或者跟当前的不一样
            //if (ClaimEntity == null || ClaimEntity.ConfigType != _strConfigKey + "Claim")
            //{
            //    NoPermissionRedirectTo(filterContext);
            //    return string.Empty;
            //}

            //log.Debug("windowsIdentity：type:{0} value:{1} String:{2}", a.Type, a.Value, a.ToString());


            ViewBag.WeChatUserID = objLoginInfo.WeChatUserID;
            ViewBag.WeChatUserName = objLoginInfo.UserName;

            return windowsIdentity.Name;
        }

        private string GetWeChatUserID(ActionExecutingContext filterContext)
        {
            string strToUrl = Request.RawUrl.Replace(":5001", ""); //处理反向代理

            Session["ReturnRoute"] = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName;// Request.Url.ToString();
            //var objLoginInfo = Session["UserInfo"] as WechatUser;

            //LogManager.GetLogger(this.GetType()).Debug("objLoginInfo : " + (objLoginInfo == null?"NULL":objLoginInfo.WeChatUserID));
            ////判断用户是否为空
            //if (objLoginInfo == null)

            //LogManager.GetLogger(this.GetType()).Debug("objLoginInfo is null");

            //判断是否已经登陆
            var User = GetLoginUserInfo(filterContext);
            if (!string.IsNullOrEmpty(User) && (ClaimEntity != null || Request["_Callback"] == "1" || Request.Url.AbsoluteUri.Contains("_Callback=1")) /*防止死循环*/)
            {
                return User;
            }



            return string.Empty;
        }

        /// <summary>
        /// 要求子类实现，各自的子类实现各自的身份验证跳转方式
        /// </summary>
        /// <returns></returns>
        protected virtual void NoPermissionRedirectTo(ActionExecutingContext filterContext, string configKey = "")
        {
            throw new NotImplementedException("Not allowed to use this.");
        }
        /// <summary>
        /// 验证可见范围
        /// </summary>
        /// <param name="news"></param>
        /// <param name="isCrop"></param>
        /// <returns></returns>
        public string ValidateNewsVisibleScope(ArticleInfo news, bool isCrop)
        {
            if (null != news)
            {
                //只验证发消息推送的图文
                //if (news.ArticleType == 1)
                //{
                return ValidateNewsIsVisibleToCurrentUser(news.SecurityLevel, news, isCrop);
                //}
                //return string.Empty;
            }
            return "/cropNoPermission.html";
        }

        /// <summary>
        /// 第一次执行时, 获取backRedirectUrl
        /// 第二次执行时, 返回WeChatUserID, 并进行可见范围判断
        /// </summary>
        /// <param name="news"></param>
        /// <param name="isCrop"></param>
        /// <param name="isSecond"></param>
        /// <returns></returns>
        public string ExecuteAuthorityFilterForNews(ArticleInfo news, bool isCrop, bool isSecond = false)
        {
            _logger.Debug("begin to validte news {0}.", news.SecurityLevel == null ? "null" : news.SecurityLevel.ToString());
            _logger.Debug("objLoginInfo.Id :{0}", objLoginInfo == null ? "null" : objLoginInfo.Id.ToString());
            //不是所有人可见，但是还没有关注的情况
            if (null != news && news.SecurityLevel != (int)SecurityLevel.AllPeople && objLoginInfo.Id == 0)
            {
                _logger.Debug("no permission");
                return "/noNewsPermission.html";
            }
            else
            {
                _logger.Debug("begin to check :{0}", isCrop);
                return ValidateNewsVisibleScope(news, isCrop);
            }
        }

        private string ValidateNewsIsVisibleToCurrentUser(int? securityLevel, ArticleInfo news, bool isCrop)
        {
            try
            {
                _logger.Debug("begin to validate news visible {0}, {1}", news.ArticleTitle, news.SecurityLevel == null ? "null" : news.SecurityLevel.ToString());
                switch (securityLevel)
                {
                    //对不起，您不具备查看该信息的权限。
                    case (int)SecurityLevel.JustReceivedUser:
                        return ValidateJustReceivedUser(news.ToDepartment, news.ToTag, news.ToUser, isCrop) ? string.Empty : "/noNewsPermission.html";
                    //服务号没有该级别
                    case (int)SecurityLevel.AllUserInApp:
                        return ValidateUserInApp(news.AppId) ? string.Empty : "/noNewsPermission.html";
                    //对不起，目前尚不具备查看该信息的权限。请先长按扫描上面的二维码图片关注并认证。
                    case (int)SecurityLevel.AllUserInAccoumentManagement:
                        return !string.IsNullOrEmpty(ViewBag.WeChatUserID) ? string.Empty : "/noNewsPermission.html";
                    case (int)SecurityLevel.AllPeople:
                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return "/noNewsPermission.html";
        }

        private bool ValidateJustReceivedUser(string toDepartment, string toTag, string toUser, bool isCrop)
        {
            bool isVisible = false;
            try
            {
                if (isCrop)
                {
                    #region 企业号
                    SysAddressBookMember user = GetCropUser();
                    if (null != user)
                    {
                        if (!string.IsNullOrWhiteSpace(toDepartment))
                        {
                            if (!string.IsNullOrWhiteSpace(user.Department))
                            {
                                var userDepartment = JsonConvert.DeserializeObject<List<int>>(user.Department);
                                var newsDepartment = toDepartment.Split(',').ToList();
                                isVisible = newsDepartment.Any(x => userDepartment.Contains(int.Parse(x)));
                                _logger.Debug("user is {0} in department", isVisible ? string.Empty : "not");
                            }
                        }
                        if (!isVisible && !string.IsNullOrWhiteSpace(toTag))
                        {
                            if (!string.IsNullOrWhiteSpace(user.TagList))
                            {
                                var userTag = JsonConvert.DeserializeObject<List<int>>(user.TagList);
                                var newsTag = toTag.Split(',').ToList();
                                isVisible = newsTag.Any(x => userTag.Contains(int.Parse(x)));
                                _logger.Debug("user is {0} in tag", isVisible ? string.Empty : "not");
                            }
                        }
                        if (!isVisible && !string.IsNullOrWhiteSpace(toUser))
                        {
                            if (!string.IsNullOrWhiteSpace(user.UserId))
                            {
                                var newsDepartment = toUser.Split(',').ToList();
                                isVisible = newsDepartment.Contains(user.UserId);
                                _logger.Debug("user is {0} in sended user", isVisible ? string.Empty : "not");
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 服务号
                    var mpUser = GetMPUser();
                    if (null != mpUser)
                    {
                        if (!isVisible && !string.IsNullOrWhiteSpace(toTag))
                        {
                            if (!string.IsNullOrWhiteSpace(mpUser.TagIdList))
                            {
                                var userTag = mpUser.TagIdList.Split(',').ToList();
                                var newsTag = toTag.Split(',').ToList();
                                isVisible = newsTag.Any(x => userTag.Contains(x));
                                _logger.Debug("mpUser is {0} in tag", isVisible ? string.Empty : "not");
                            }
                        }
                        if (!isVisible && !string.IsNullOrWhiteSpace(toUser))
                        {
                            if (!string.IsNullOrWhiteSpace(mpUser.OpenId))
                            {
                                var newsDepartment = toUser.Split(',').ToList();
                                isVisible = newsDepartment.Contains(mpUser.OpenId);
                                _logger.Debug("mpUser is {0} in sended user", isVisible ? string.Empty : "not");
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return isVisible;
        }

        /// <summary>
        /// 通过API获取APP的可见范围, 判断该User是否在其中
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        private bool ValidateUserInApp(int? appId)
        {
            if (null != appId)
            {
                SysAddressBookMember user = GetCropUser();
                if (null != user && null != user.AccountManageId)
                {
                    return WeChatCommonService.IsValidateUser((int)appId, user, (int)user.AccountManageId);
                }
            }
            return false;
        }

        private SysAddressBookMember GetCropUser()
        {
            string wechatUserId = ViewBag.WeChatUserID;
            _logger.Debug("begin to get {0}", string.IsNullOrEmpty(wechatUserId) ? "null" : wechatUserId);
            if (!string.IsNullOrWhiteSpace(wechatUserId))
            {
                var user = _addressBookService.GetMemberByUserId(wechatUserId);
                _logger.Debug("user is {0}", user == null ? "null" : user.UserName);
                return user;
            }
            return null;
        }

        private WechatMPUserView GetMPUser()
        {
            string wechatUserId = ViewBag.WeChatUserID;
            _logger.Debug("begin to get MP {0}", string.IsNullOrEmpty(wechatUserId) ? "null" : wechatUserId);
            if (!string.IsNullOrWhiteSpace(wechatUserId))
            {
                var user = _wechatMPUserService.GetUserByOpenId(wechatUserId);
                _logger.Debug("MPuser is {0}", user == null ? "null" : user.NickName);
                return user;
            }
            return null;
        }
    }
}
