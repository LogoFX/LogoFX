using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Base class for collection of models, supporting collection change notifications
    /// </summary>
    public abstract class ModelsCollectionBase : IInfoModelsCollection
    {
        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>
        /// The items count.
        /// </value>
        public abstract int ItemsCount { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has items; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasItems { get; }

        /// <summary>
        /// Raised when items collection is changed. 
        /// </summary>
        public event EventHandler HasItemsChanged;

        /// <summary>
        /// Raises the items collection change event
        /// </summary>
        protected void SafeRaiseHasItemsChanged()
        {
            if (HasItemsChanged != null)
            {
                HasItemsChanged(this, new EventArgs());
            }
        }
    }
}