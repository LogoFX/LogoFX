using System;
using LogoFX.Client.Mvvm.ViewModel.Interfaces.Services;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    public abstract class ModelsCollectionBase : IInfoModelsCollection
    {
        public abstract int ItemsCount { get; }
        public abstract bool HasItems { get; }
        public event EventHandler HasItemsChanged;

        protected void SafeRaiseHasItemsChanged()
        {
            if (HasItemsChanged != null)
            {
                HasItemsChanged(this, new EventArgs());
            }
        }
    }
}