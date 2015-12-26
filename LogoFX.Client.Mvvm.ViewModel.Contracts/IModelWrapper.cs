namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Object that wraps some other object
    /// </summary>
    public interface IModelWrapper
    {
        /// <summary>
        /// Wrapped object
        /// </summary>
        object Model { get; }
    }

    /// <summary>
    /// Object that wraps some other object of T
    /// </summary>
    public interface IModelWrapper<out T> : IModelWrapper
    {
        /// <summary>
        /// Wrapped object
        /// </summary>
        new T Model { get; }
    }
}
