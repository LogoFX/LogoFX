using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Allows managing models collection
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public interface IWriteModelsCollection<TItem>
    {
        void Add(TItem item);
        void Remove(TItem item);
        void Update(IEnumerable<TItem> items);
        void Clear();
    }
}