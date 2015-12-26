namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class VirtualPagingItemListViewModel<TItem, TModel>
    {
        /// <summary>
        /// Represents paging list manager for specific model and suport for virtualization and selection.
        /// </summary>
        public new abstract class WithSelection : PagingItemListViewModel<VirtualPagingItemViewModel<TItem, TModel>, VirtualContainer<TModel>>.WithSelection
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WithSelection"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="source">The source.</param>
            protected WithSelection(object parent, VirtualContainer<TModel>.Collection source)
                : base(parent, source)
            {
            }
        }
    }
}
