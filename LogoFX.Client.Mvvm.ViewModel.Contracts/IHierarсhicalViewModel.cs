namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents view model with child items.
    /// </summary>
    public interface IHierarchicalViewModel
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        IViewModelsCollection<IObjectViewModel> Children { get; }

        /// <summary>
        /// Gets the items.(GLUE:compatibility to caliburn micro)
        /// </summary>
        IViewModelsCollection<IObjectViewModel> Items { get; }        
    }
}