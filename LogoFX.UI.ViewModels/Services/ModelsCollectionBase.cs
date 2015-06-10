using System;
using LogoFX.UI.ViewModels.Interfaces.Services;

namespace LogoFX.UI.ViewModels.Services
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