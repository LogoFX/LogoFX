﻿using Caliburn.Micro;
using LogoFX.Client.Modularity;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Represents composable part of the application with navigation capabilities
    /// </summary>
    public interface INavigationModule : IUiModule<INavigationRootViewModel>
    {
        /// <summary>
        /// Gets or sets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        INavigationService NavigationService { get; set; }
    }

    /// <summary>
    /// The root view model of the module which can serve
    /// as navigation target.
    /// </summary>
    public interface INavigationRootViewModel : IRootViewModel, IScreen
    {
        
    }
}
