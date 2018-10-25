using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Innocellence.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("Innocellence.Authentication.SSO",
                   "OWinLogin/ssoresult",
                   new { controller = "OWinLogin", action = "ssoresult", id = UrlParameter.Optional },
                   new[] { "Innocellence.Authentication.Controllers" }
              );
            routes.MapRoute(
              name: "Innocellence.Authentication.SsoResult",
              url: "Sso/SsoResult",
              defaults: new { controller = "sso", action = "ssoresult", id = UrlParameter.Optional },
              namespaces: new[] { "Innocellence.Authentication.Controllers" }
          );
        }
    }
}
