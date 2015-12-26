using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// ObservableViewModelsCollection
    /// </summary>
    internal class ObservableViewModelsCollection<T> : ObservableCollection<T>, IObjectViewModelsCollection<T> where T : IObjectViewModel
    {
    }
}
