using System;
using System.Web.Http;

namespace LogoFX.Server.Web.Core
{
    public class HttpConfigurationProxy : IHttpConfigurationProxy
    {
        public HttpConfigurationProxy(HttpConfiguration httpConfiguration)
        {
            HttpConfiguration = httpConfiguration;
        }        

        public IHttpConfigurationProxy MapHttpRoutes()
        {
            HttpConfiguration.MapHttpAttributeRoutes();

            HttpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            return this;
        }

        public IHttpConfigurationProxy ReplaceService<TService>(TService service)
        {
            HttpConfiguration.Services.Replace(typeof(TService), service);
            return this;
        }

        public HttpConfiguration HttpConfiguration { get; private set; }
    }
}