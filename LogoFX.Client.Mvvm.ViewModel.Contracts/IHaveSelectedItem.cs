namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Object that have one selected item
    /// </summary>
    /// <typeparam name="T">type of selected item supported</typeparam>
    public interface IHaveSelectedItem<out T>:IHaveSelectedItem
    {
        /// <summary>
        /// Selected item
        /// </summary>
        new T SelectedItem { get; }
    }

    /// <summary>
    /// Object that have one selected item
    /// </summary>
    public interface IHaveSelectedItem
    {
        /// <summary>
        /// Selected item
        /// </summary>
        object SelectedItem { get; }
    }
}