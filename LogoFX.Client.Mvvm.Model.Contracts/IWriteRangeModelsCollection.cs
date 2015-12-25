using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents collection of models which supports bulk operations
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IWriteRangeModelsCollection<TItem>
    {
        /// <summary>
        /// Adds the collection of items as bulk operation.
        /// </summary>
        /// <param name="items">The items.</param>
        void AddRange(IEnumerable<TItem> items);

        /// <summary>
        /// Removes the collection of items as bulk operation.
        /// </summary>
        /// <param name="items">The items.</param>
        void RemoveRange(IEnumerable<TItem> items);
    }
}