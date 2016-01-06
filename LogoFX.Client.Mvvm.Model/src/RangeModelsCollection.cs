using System.Collections.Generic;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents collection of models, supporting collection change notifications. 
    /// Supports bulk operations efficiently.
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public class RangeModelsCollection<TItem> : ModelsCollectionBase, IModelsCollection<TItem>, IWriteRangeModelsCollection<TItem>
    {
        private readonly RangeObservableCollection<TItem> _items = new RangeObservableCollection<TItem>();
        private RangeObservableCollection<TItem> Items
        {
            get { return _items; }
        }

        IEnumerable<TItem> IReadModelsCollection<TItem>.Items
        {
            get { return Items; }
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>
        /// The items count.
        /// </value>
        public override int ItemsCount { get { return Items.Count; } }

        /// <summary>
        /// Gets a value indicating whether this instance has items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has items; otherwise, <c>false</c>.
        /// </value>
        public override bool HasItems { get { return ItemsCount > 0; } }

        /// <summary>
        /// Adds item to the end of the collection and raises notification.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(TItem item)
        {
            Items.Add(item);
            SafeRaiseHasItemsChanged();
        }

        /// <summary>
        /// Removes the specified item from the collection and raises notification.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(TItem item)
        {
            Items.Remove(item);
            SafeRaiseHasItemsChanged();
        }

        /// <summary>
        /// Clears the collection and updates its contents with the specified items; notification is raised in the end of the update operation.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Update(IEnumerable<TItem> items)
        {
            Items.Clear();
            Items.AddRange(items);
            SafeRaiseHasItemsChanged();
        }

        /// <summary>
        /// Clears the collection and raises the notification in the end.
        /// </summary>
        public void Clear()
        {
            Items.Clear();            
            SafeRaiseHasItemsChanged();
        }

        /// <summary>
        /// Adds collection of items as bulk operation; notification is raised in the end of the add operation.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<TItem> items)
        {
            Items.AddRange(items);
            SafeRaiseHasItemsChanged();
        }

        /// <summary>
        /// Removes the collection of items as bulk operation; notification is raised in the end of the remove operation.
        /// </summary>
        /// <param name="items">The items.</param>
        public void RemoveRange(IEnumerable<TItem> items)
        {
            Items.RemoveRange(items);
            SafeRaiseHasItemsChanged();
        }
    }
}