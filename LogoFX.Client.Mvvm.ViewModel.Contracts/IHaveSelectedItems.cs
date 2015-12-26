using System.Collections;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Object that have multiple selected items
    /// </summary>
    /// <typeparam name="T">type of selected items</typeparam>
    public interface 
#if !SILVERLIGHT

        IHaveSelectedItems<out T> 
#else
        IHaveSelectedItems<T> 
#endif
        : IHaveSelectedItems
    {
        /// <summary>
        /// Selected items
        /// </summary>
        new IEnumerable<T> SelectedItems { get; }
    }

    /// <summary>
    /// Object that have multiple selected items
    /// </summary>
    public interface IHaveSelectedItems
    {
        /// <summary>
        /// Selected items
        /// </summary>
        IEnumerable SelectedItems { get; }
    }
}