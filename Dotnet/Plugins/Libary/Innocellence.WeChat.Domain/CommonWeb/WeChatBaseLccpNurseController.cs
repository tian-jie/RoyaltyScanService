
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Infrastructure.Utility.Extensions;
using Infrastructure.Web.UI;
using Infrastructure.Web;
using Infrastructure.Core;
using Infrastructure.Core.Data;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;

using System.Linq.Expressions;
using System.Net;
using System.Security.Principal;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.Service;
using System.Configuration;
using System.Web.Configuration;
using Innocellence.Weixin.HttpUtility;
using Innocellence.Weixin.QY.AdvancedAPIs.OAuth2;
using Infrastructure.Core.Logging;
using Infrastructure.Web.ImageTools;
using Infrastructure.Web.Domain.Entity;
using Infrastructure.Web.Domain.Service;
using Innocellence.Weixin.QY.AdvancedAPIs;
using Innocellence.WeChat.Domain.Services;
using System.Security.Claims;
using Innocellence.WeChat.Domain.ModelsView;
using Infrastructure.Core.Infrastructure;
using Newtonsoft.Json;
using Innocellence.WeChat.Domain.ViewModel;
using System.Threading.Tasks;
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
    public class WeChatBaseLccpNurseController<T, T1> : WeChatBaseController<T, T1>
        where T : EntityBase<int>, new()
        where T1 : IViewModel, new()
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="newsService"></param>
        public WeChatBaseLccpNurseController(IBaseService<T> newsService)
            : base(newsService, "WeChatLccpNurse")
        {


        }

    }
}
