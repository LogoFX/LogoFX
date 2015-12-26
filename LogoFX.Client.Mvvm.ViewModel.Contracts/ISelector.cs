namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Selector that supports selection of items.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public interface 
#if !SILVERLIGHT
        ISelector<out T> 
#else
        ISelector<T> 
#endif
 :  ISelector, IHaveSelectedItem<T>, IHaveSelectedItems<T> where T : ISelectable
    {
    }

    /// <summary>
    /// Selector that supports single selection
    /// </summary>
    public interface ISelector : IHaveSelectedItem, IHaveSelectedItems, INotifySelectionChanged, ISelect, IUnselect
    {
    }
}