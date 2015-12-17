namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation builder which allows setting the navigation target as root
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRootableNavigationBuilder<out T> : INavigationBuilder<T>
    {
        /// <summary>
        /// Sets the navigation target as root.
        /// </summary>
        void AsRoot();
    }
}