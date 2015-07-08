using Caliburn.Micro;
using LogoFX.Client.Modularity;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationModule : IUiModule<IScreen>
    {
        INavigationService NavigationService { get; set; }
    }
}
