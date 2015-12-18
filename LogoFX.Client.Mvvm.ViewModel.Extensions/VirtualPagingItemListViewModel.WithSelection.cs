namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingItemListViewModel<TItem, TModel>
    {
        public new abstract class WithSelection : PagingItemListViewModel<VirtualPagingItemViewModel<TItem, TModel>, VirtualContainer<TModel>>.WithSelection
        {
            protected WithSelection(object parent, VirtualContainer<TModel>.Collection source)
                : base(parent, source)
            {
            }
        }
    }
}
