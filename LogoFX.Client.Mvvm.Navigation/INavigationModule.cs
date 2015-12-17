using Caliburn.Micro;
using LogoFX.Client.Modularity;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Represents composable part of the application with navigation capabilities
    /// </summary>
    public interface INavigationModule : IUiModule<IScreen>
    {
        /// <summary>
        /// Gets or sets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        INavigationService NavigationService { get; set; }
    }
}
