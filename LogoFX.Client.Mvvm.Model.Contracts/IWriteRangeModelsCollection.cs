using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IWriteRangeModelsCollection<TItem>
    {
        void AddRange(IEnumerable<TItem> items);
        void RemoveRange(IEnumerable<TItem> items);
    }
}