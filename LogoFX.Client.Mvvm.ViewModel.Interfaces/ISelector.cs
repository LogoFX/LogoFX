// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    /// <summary>
    /// Selector that supports single selection of T
    /// </summary>
    /// <typeparam name="T">type of items</typeparam>
    public interface 
#if !SILVERLIGHT
        ISelector<out T> 
#else
        ISelector<T> 
#endif
 :  ISelector, IHaveSelectedItem<T>,IHaveSelectedItems<T> where T : ISelectable
    {
    }

    /// <summary>
    /// Selector that supports single selection
    /// </summary>
    public interface ISelector : IHaveSelectedItem,IHaveSelectedItems, INotifySelectionChanged, ISelect, IUnselect
    {
    }
}