using System;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingScreenViewModel<TItem, TModel>
    {
        public new abstract class WithFilter<TFilter> : PagingScreenViewModel<VirtualPagingItemViewModel<TItem, TModel>, TModel>.WithFilter<TFilter>
            where TFilter : class, IFilterModel
        {
            public abstract override int FilteredCount { get; }

            public abstract override int SelectedCount { get; }

            protected abstract override void RefreshData();

            protected sealed override bool OnItemFilter(VirtualPagingItemViewModel<TItem, TModel> item)
            {
                throw new NotImplementedException();
            }

            protected sealed override void UpdateSortDescriptors()
            {

            }

            protected override ICollectionView CreateCollectionView()
            {
                return null;
            }
        }

    }
}
