using System;
using System.ComponentModel;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingScreenViewModel<TItem, TModel> : PagingScreenViewModel<VirtualPagingItemViewModel<TItem, TModel>, TModel>
        where TModel : class
        where TItem : class, IModelWrapper<TModel>, ISelectable
    {
        public abstract override int FilteredCount { get; }

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