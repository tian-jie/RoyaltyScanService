using Infrastructure.Core;
using Infrastructure.Web.Domain.Service;
using Infrastructure.Web.UI;
using Innocellence.WeChat.Domain.Common;
using Innocellence.Weixin.QY.AdvancedAPIs;
using System.Linq;
using System.Net;
using System.Web;
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
    public class WeChatBaseLccpRepController<T, T1> : WeChatBaseController<T, T1>
        where T : EntityBase<int>, new()
        where T1 : IViewModel, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        public WeChatBaseLccpRepController(IBaseService<T> newsService)
            : base(newsService, "WeChatLccpRep")
        {


        }

        /// <summary>
        /// 要求子类实现，各自的子类实现各自的身份验证跳转方式
        /// </summary>
        /// <returns></returns>
        protected override void NoPermissionRedirectTo(ActionExecutingContext filterContext, string configKey = "WeChatLccpRep")
        {
            string strRet = CommonService.GetSysConfig("UserBackUrl", "");
            string webUrl = CommonService.GetSysConfig("WeChatUrl", "");
            string returnUrl = HttpUtility.UrlEncode(Request.RawUrl);
            string strBackUrl = string.Format("{0}{1}?ConfigKey={2}&returnUrl={3}", webUrl, strRet.Trim('/'), "WeChatLccpRep", returnUrl);
            _logger.Debug("strBackUrl :" + strBackUrl);


            var strAppId = CommonService.GetSysConfig("WeChatLccpRepAppId", "");
            var appId = int.Parse(strAppId);

            // 根据appid配置的ID到sysWeChatConfig表中找到对应的内容，就能找到对应的appid啥的了
            var wechatConfig = WeChatCommonService.lstSysWeChatConfig.FirstOrDefault(a => a.Id == appId);

            var strUrl = OAuth2Api.GetCode(wechatConfig.WeixinCorpId, Server.UrlEncode(strBackUrl), returnUrl);


            _logger.Debug("strUrl:" + strUrl);
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                AjaxResult<int> result = new AjaxResult<int>();
                result.Message = new JsonMessage((int)HttpStatusCode.Unauthorized, strUrl);
                filterContext.Result = Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _logger.Debug("filterContext.Result = new RedirectResult(strUrl) : " + strUrl);
                filterContext.Result = new RedirectResult(strUrl);
            }


        }
    }
}
