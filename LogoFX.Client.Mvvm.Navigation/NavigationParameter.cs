namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation parameter. Implement to provide custom navigation behavior.
    /// </summary>
    public abstract class NavigationParameter
    {
        /// <summary>
        /// Navigates to the navigation target.
        /// </summary>
        public abstract void Navigate();
    }
}