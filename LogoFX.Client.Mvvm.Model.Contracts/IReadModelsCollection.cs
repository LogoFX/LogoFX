using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IReadModelsCollection<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
