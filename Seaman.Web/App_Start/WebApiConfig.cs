using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Seaman.Web.Code;

namespace Seaman.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), new SeamanExceptionHandler());
            // Web API routes
            config.MapHttpAttributeRoutes();

            FluentValidation.WebApi.FluentValidationModelValidatorProvider.Configure(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
