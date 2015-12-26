using System;
using System.ComponentModel;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingScreenViewModel<TItem, TModel>
    {
        /// <summary>
        /// Represents a screen with support for paging, virtualization and filtering specified type object view models.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        public new abstract class WithFilter<TFilter> : PagingScreenViewModel<VirtualPagingItemViewModel<TItem, TModel>, TModel>.WithFilter<TFilter>
            where TFilter : class, IFilterModel
        {
            /// <summary>
            /// Gets the filtered items count.
            /// </summary>
            /// <value>
            /// The filtered items count.
            /// </value>
            public abstract override int FilteredCount { get; }

            /// <summary>
            /// Gets the selected items count.
            /// </summary>
            /// <value>
            /// The selected items count.
            /// </value>
            public abstract override int SelectedCount { get; }

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
}
