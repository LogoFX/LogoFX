namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation conductor; it is able to navigate to the requested navigation target
    /// </summary>
    public interface INavigationConductor
    {
        /// <summary>
        /// Navigates to the requested navigation target
        /// </summary>
        /// <param name="viewModel">The navigation target</param>
        /// <param name="argument">The navigation argument.</param>
        void NavigateTo(object viewModel, object argument);
    }
}