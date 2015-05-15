using Caliburn.Micro;
using LogoFX.UI.Modularity;

namespace LogoFX.UI.Navigation
{
    public interface INavigationModule : IUiModule<IScreen>
    {
        INavigationService NavigationService { get; set; }
    }
}
