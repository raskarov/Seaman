using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using Seaman.Web;
using Seaman.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]

namespace Seaman.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            InjectionConfig.Register();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureAuth(app);
        }
    }
}
