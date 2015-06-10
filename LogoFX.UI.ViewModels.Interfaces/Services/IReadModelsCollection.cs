using System.Collections.Generic;

namespace LogoFX.UI.ViewModels.Interfaces.Services
{
    public interface IReadModelsCollection<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
