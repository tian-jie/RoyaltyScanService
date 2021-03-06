﻿using Infrastructure.Core;
using Infrastructure.Web.Domain.Entity;
using Infrastructure.Web.Domain.Service;
using Infrastructure.Web.Domain.Services;
using Infrastructure.Web.UI;
using Innocellence.WeChat.Domain.Authentication.Attribute;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.Contracts;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Innocellence.WeChat.Domain
{
    /// <summary>
    /// 基类BaseController，过滤器
    /// </summary>
    // [HandleError]

    //[FilterError]
    //[CustomAuthorize]
    [SSOOWinAuthorize(true)]
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class AdminBaseController<T, T1> : ParentController<T, T1>
        where T : EntityBase<int>, new()
        where T1 : IViewModel, new()
    {
        /// <summary>
        /// 当前的DBService
        /// </summary>
        public IBaseService<T> _objService;

        //全局用户对象，当前的登录用户
        public SysUser objLoginInfo;

        public int AppId;

        public int AccountManageID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        public AdminBaseController(IBaseService<T> newsService)
            : base(newsService)
        {
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.DateFormatHandling
            //= Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            _objService = newsService;
            //if (objLoginInfo != null)
            //{
            //    _newsService.LoginUsrID = objLoginInfo.UserID;
            //}


        }

        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.Cookies.AllKeys.Contains("AccountManageId"))
            {
                var AM = Request.Cookies["AccountManageId"];
                AccountManageID = int.Parse(AM.Value);
            }


            if (!string.IsNullOrEmpty(Request["AppID"])) { AppId = int.Parse(Request["AppID"]); }

            // var objAppID = Session["AppID"];
            // AppId = 0;
            // if (objAppID != null)
            // {
            //     AppId = int.Parse(objAppID.ToString());
            // }
            // else if (CommonService.lstWeChatConfig.Count>0)
            // {
            //     AppId = int.Parse(CommonService.lstWeChatConfig[0].WeixinAppId);
            //     Session["AppID"] = AppId;
            // }
            //// ViewBag.AppList = CommonService.lstWeChatConfig;
            //// ViewBag.CurAppID = AppId;



            //得到用户登录的信息
            objLoginInfo = Session["UserInfo"] as SysUser;



            ////判断用户是否为空   应该使用 AuthorizeAttribute,临时解决一下
            if ((objLoginInfo == null || !Request.IsAuthenticated) && (filterContext.ActionDescriptor.ActionName.ToLower() != "login" ||
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower() != "account"))
            {

                //windows登录,自动获取用户信息
                if (objLoginInfo == null && (Request.IsAuthenticated && (User.Identity is WindowsIdentity)))
                {
                    var windowsIdentity = (WindowsIdentity)User.Identity;

                    SysUserService objServ = new SysUserService();
                    var objUser = objServ.AutoLogin(windowsIdentity);
                    if (objUser != null)
                    {
                        objLoginInfo = objUser;
                        Session["UserInfo"] = objLoginInfo;
                    }
                    else
                    {

                    }

                }
                string redirectUrl = "";
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    AjaxResult<int> result = new AjaxResult<int>();
                    result.Message = new JsonMessage((int)HttpStatusCode.Unauthorized, "Please login");
                    filterContext.Result = Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var ssoUrl = CommonService.GetSysConfig("SSOUrl", string.Empty);
                    ssoUrl += "&TargetResource=" + Request.Url.ToString();
                    redirectUrl = ssoUrl;
                    filterContext.Result = Redirect(redirectUrl);
                }
                // return;
            }

            //if (objLoginInfo != null)
            //{
            //    _BaseService.LoginUserID = objLoginInfo.Id;
            //    _BaseService.LoginUserName = objLoginInfo.WeChatUserID;
            //    SetLanguage("EN");
            //}

            // System.Threading.Thread.Sleep(5000);

            //Logger log = Logger.GetLogger(filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName, CurrentUserInfo.USERREALNAME);
            //log.Debug(WEBUI.Common.LogHelper.GetActionInfo(filterContext));

            //lstMenus=Session["UserMenus"] as List<BASE_SYSMENU>;

            //base.OnActionExecuting(filterContext);
        }

        public string AccessToken
        {
            get { return WeChatCommonService.GetWeiXinToken(AppId); }
        }

        protected virtual void AppDataPermissionCheck(IDataPermissionCheck service, int targetAppId, int currentAppId)
        {
            if (service.AppDataCheck(targetAppId, currentAppId)) return;
            //log.Warn("user({0}) want to access {1} app that have not been authorization!", sysUser.UserName, targetAppId);
            if (HttpContext.Request.IsAjaxRequest())
            {
                throw new HttpException(403, "You are not authorized to access this page.");
            }
            HttpContext.Response.Redirect("/403.html", true);
        }

    }
}
