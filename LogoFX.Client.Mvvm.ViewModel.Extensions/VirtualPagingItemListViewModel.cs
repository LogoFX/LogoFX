using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents paging list manager for specific model and suport for virtualization.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract partial class VirtualPagingItemListViewModel<TItem, TModel> : PagingItemListViewModel<VirtualPagingItemViewModel<TItem, TModel>, VirtualContainer<TModel>>
        where TModel : class
        where TItem : class, IObjectViewModel<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPagingItemListViewModel{TItem, TModel}"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="source">The source.</param>
        protected VirtualPagingItemListViewModel(object parent, VirtualContainer<TModel>.Collection source)
            : base(parent, source)
        {
        }
    }
}