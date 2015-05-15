using System.Collections.Generic;

namespace LogoFX.UI.Model.Contracts
{
    public interface IItemsDataManager<TItem>
    {
        void Add(TItem item);
        void Remove(TItem item);
        void Update(IEnumerable<TItem> items);
        void Clear();
    }
}