using System;

namespace LogoFX.UI.ViewModels.Interfaces.Services
{
    public interface IInfoModelsCollection
    {
        int ItemsCount { get; }
        bool HasItems { get; }
        event EventHandler HasItemsChanged;
    }
}