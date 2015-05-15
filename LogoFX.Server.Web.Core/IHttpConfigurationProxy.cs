using System.Web.Http;

namespace LogoFX.Server.Web.Core
{
    public interface IHttpConfigurationProxy
    {        
        IHttpConfigurationProxy MapHttpRoutes();
        IHttpConfigurationProxy ReplaceService<TService>(TService service);
        HttpConfiguration HttpConfiguration { get; }
    }
}
