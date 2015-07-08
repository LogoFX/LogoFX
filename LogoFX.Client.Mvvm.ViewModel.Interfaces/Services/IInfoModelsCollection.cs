using System;

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces.Services
{
    public interface IInfoModelsCollection
    {
        int ItemsCount { get; }
        bool HasItems { get; }
        event EventHandler HasItemsChanged;
    }
}