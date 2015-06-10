using System;

namespace LogoFX.UI.Model.Contracts
{
    public interface IInfoModelsCollection
    {
        int ItemsCount { get; }
        bool HasItems { get; }
        event EventHandler HasItemsChanged;
    }
}