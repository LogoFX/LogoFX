using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Allows managing models collection
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public interface IWriteModelsCollection<TItem>
    {
        /// <summary>
        /// Adds the item to the collection.
        /// </summary>
        /// <param name="item"></param>
        void Add(TItem item);

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        void Remove(TItem item);

        /// <summary>
        /// Updates the collecton with the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        void Update(IEnumerable<TItem> items);

        /// <summary>
        /// Clears the collection.
        /// </summary>
        void Clear();
    }
}