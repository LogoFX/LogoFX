using System;
using System.ComponentModel;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents a screen with support for paging and virtualization specified type object view models.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract partial class VirtualPagingScreenViewModel<TItem, TModel> : PagingScreenViewModel<VirtualPagingItemViewModel<TItem, TModel>, TModel>
        where TModel : class
        where TItem : class, IModelWrapper<TModel>, ISelectable
    {
        /// <summary>
        /// Gets the filtered items count.
        /// </summary>
        /// <value>
        /// The filtered items count.
        /// </value>
        public abstract override int FilteredCount { get; }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        protected abstract override void RefreshData();

        /// <summary>
        /// Override this method to inject custom logic when an item is filtered.
        /// </summary>
        /// <param name="item">The item being filtered.</param>
        /// <returns></returns>
        protected sealed override bool OnItemFilter(VirtualPagingItemViewModel<TItem, TModel> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the sort descriptors.
        /// </summary>
        protected sealed override void UpdateSortDescriptors()
        {

        }

        /// <summary>
        /// Creates the collection view.
        /// </summary>
        /// <returns></returns>
        protected override ICollectionView CreateCollectionView()
        {
            return null;
        }
    }
}