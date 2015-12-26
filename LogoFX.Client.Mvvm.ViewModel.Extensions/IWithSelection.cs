using System;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents an object with selection handler and selected items information.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface IWithSelection<out TItem> : IHaveSelectedItem<TItem>, IHaveSelectedItems<TItem>
    {
        /// <summary>
        /// Gets or sets the selection handler.
        /// </summary>
        /// <value>
        /// The selection handler.
        /// </value>
        Action<object, SelectionChangingEventArgs> SelectionHandler { get; set; }                
    }
}