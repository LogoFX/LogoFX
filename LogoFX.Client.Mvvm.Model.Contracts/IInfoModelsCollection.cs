using System;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface IInfoModelsCollection
    {
        int ItemsCount { get; }
        bool HasItems { get; }
        event EventHandler HasItemsChanged;
    }
}