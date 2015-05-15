using System.Collections.Generic;

namespace LogoFX.UI.Model.Contracts
{
    public interface IItemsDataProvider<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
