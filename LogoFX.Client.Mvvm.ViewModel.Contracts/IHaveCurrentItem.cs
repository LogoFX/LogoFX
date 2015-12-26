namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Object that have current item
    /// </summary>
    public interface IHaveCurrentItem
    {
        /// <summary>
        /// Current item
        /// </summary>
        /// <remarks>Usually synchronized with focus</remarks>
        object CurrentItem { get; }
    }
    /// <summary>
    /// Object that have current item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHaveCurrentItem<out T> : IHaveCurrentItem
    {
        /// <summary>
        /// Current item
        /// </summary>
        /// <remarks>Usually synchronized with focus</remarks>
        new T CurrentItem { get; }
    }

}