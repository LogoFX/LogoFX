using System.Collections.ObjectModel;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels
{
    /// <summary>
    /// ObservableViewModelsCollection
    /// </summary>
    internal class ObservableViewModelsCollection<T> : ObservableCollection<T>, IObjectViewModelsCollection<T> where T : IObjectViewModel
    {
    }
}
