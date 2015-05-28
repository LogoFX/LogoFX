using System.Collections.Generic;

namespace LogoFX.UI.Model.Contracts
{
    public interface IReadModelsCollection<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
