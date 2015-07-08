using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces.Services
{
    public interface IReadModelsCollection<TItem>
    {
        IEnumerable<TItem> Items { get; }
    }
}
