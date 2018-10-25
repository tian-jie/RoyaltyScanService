using System.Web.Mvc;
using Infrastructure.Web.Mvc.Routing;

namespace Innocellence.RoyaltyScan
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(AreaRegistrationContext routes)
        {
            routes.MapRoute("RoyaltyScan", "RoyaltyScan/{controller}/{action}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Innocellence.RoyaltyScan.Controllers", "Innocellence.RoyaltyScan.Admin.Controllers", "Innocellence.RoyaltyScan.Controllers.BackendController" }
           );
            routes.MapRoute(
               name: "RoyaltyScanPra",
               url: "RoyaltyScan/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Innocellence.RoyaltyScan.Controllers", "Innocellence.RoyaltyScan.Admin.Controllers" }
           );
        }

        public int Priority
        {
            get
            {
                return 101;
            }
        }

        public string ModuleName
        {
            get { return "RoyaltyScan"; }
        }
    }
}
