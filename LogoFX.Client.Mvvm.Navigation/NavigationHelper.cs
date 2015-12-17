using System;
using System.Windows;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation helper which allows registering navigation commands on the specified view
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Registers the navigation commands on the specified view
        /// </summary>
        /// <param name="viewModelType">Type of the view model, i.e. the navigation commands source.</param>
        /// <param name="view">The specified view.</param>
        /// <param name="navigationService">The navigation service.</param>
        public static void RegisterNavigationCommands(
            Type viewModelType, 
            object view,
            INavigationService navigationService)
        {
            var commandBindings = new[]
            {CreateBrowseBackCommandBinding(navigationService), CreateBrowseForwardCommandBinding(navigationService)};

            foreach (var commandBinding in commandBindings)
            {
                CommandManager.RegisterClassCommandBinding(viewModelType, commandBinding);
                ((UIElement)view).CommandBindings.Add(commandBinding);
            }
            //What?!!!
            GC.Collect();
        }

        private static CommandBinding CreateBrowseBackCommandBinding(INavigationService navigationService)
        {
            return new CommandBinding(NavigationCommands.BrowseBack,
                (sender, args) => navigationService.Back(),
                (sender, args) => args.CanExecute = navigationService.CanNavigateBack);
        }

        private static CommandBinding CreateBrowseForwardCommandBinding(INavigationService navigationService)
        {
            return new CommandBinding(NavigationCommands.BrowseForward,
                (sender, args) => navigationService.Forward(),
                (sender, args) => args.CanExecute = navigationService.CanNavigateForward);
        }
    }
}
