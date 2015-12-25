using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents read-only collection of models
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public interface IReadModelsCollection<TItem>
    {
        /// <summary>
        /// Gets the collection items
        /// </summary>
        IEnumerable<TItem> Items { get; }
    }
}
