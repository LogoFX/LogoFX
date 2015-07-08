using Caliburn.Micro;
using LogoFX.Client.Modularity;

namespace LogoFX.UI.Navigation
{
    public interface INavigationModule : IUiModule<IScreen>
    {
        INavigationService NavigationService { get; set; }
    }
}
