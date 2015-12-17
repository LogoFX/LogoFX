namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation builder which allows additional setup options
    /// </summary>
    /// <typeparam name="T">Type of navigation target</typeparam>
    public interface INavigationBuilder<out T> : INavigationBuilder
    {
        /// <summary>
        /// Sets up the navigation target as a singleton
        /// </summary>
        /// <returns>Navigation target</returns>
        INavigationBuilder<T> AsSingleton();

        /// <summary>
        /// Assigns the conductor to the navigation target
        /// </summary>
        /// <typeparam name="TConductor">The type of the conductor.</typeparam>
        /// <returns>Navigation target</returns>
        INavigationBuilder<T> AssignConductor<TConductor>()
            where TConductor : INavigationConductor;

        /// <summary>
        /// Gets the navigation target
        /// </summary>
        /// <returns>Navigation target</returns>
        new T GetValue();
    }
}