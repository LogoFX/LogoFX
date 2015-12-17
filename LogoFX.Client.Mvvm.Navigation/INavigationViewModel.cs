namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation view model which allows custom handling of the navigation event
    /// </summary>
    public interface INavigationViewModel
    {
        /// <summary>
        /// Called when the view model is navigated to or navigated from.
        /// </summary>
        /// <param name="direction">The navigation direction.</param>
        /// <param name="argument">The navigation argument.</param>
        void OnNavigated(NavigationDirection direction, object argument);
    }
}