using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingItemListViewModel<TItem, TModel> : PagingItemListViewModel<VirtualPagingItemViewModel<TItem, TModel>, VirtualContainer<TModel>>
        where TModel : class
        where TItem : class, IObjectViewModel<TModel>
    {
        protected VirtualPagingItemListViewModel(object parent, VirtualContainer<TModel>.Collection source)
            : base(parent, source)
        {
        }
    }
}