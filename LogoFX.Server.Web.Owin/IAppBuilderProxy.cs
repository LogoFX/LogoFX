using LogoFX.Server.Web.Core;

namespace LogoFX.Server.Web.Owin
{
    public interface IAppBuilderProxy
    {
        IAppBuilderProxy UseWebApi(IHttpConfigurationProxy httpConfigurationProxy);
        IAppBuilderProxy UseOAuth();
        IAppBuilderProxy UseErrorPage();
        IAppBuilderProxy UseCors();
    }
}