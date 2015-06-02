using System.Web.Mvc;
using System.Web.Routing;

namespace Seaman.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Application",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "Seaman.Web.Controllers" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
