using System.Web.Http;
using LogoFX.Server.Web.Bootstrapping;
using LogoFX.Server.Web.Core;
using Solid.Practices.IoC;

namespace LogoFX.Server.Web.Owin
{
    public interface IHttpConfigurationProvider
    {
        IHttpConfigurationProxy GetConfiguration();
    }

    public class OwinBootstrapper : BootstrapperBase, IHttpConfigurationProvider
    {
        private IHttpConfigurationProxy _httpConfigurationProxy;

        public OwinBootstrapper(IIocContainer iocContainer) : base(iocContainer)
        {
        }

        public IHttpConfigurationProxy GetConfiguration()
        {
            return _httpConfigurationProxy;
        }

        protected override void Configure()
        {
            _httpConfigurationProxy = new HttpConfigurationProxy(new HttpConfiguration());
            SetupHttpConfiguration(_httpConfigurationProxy);
        }
    }
}
