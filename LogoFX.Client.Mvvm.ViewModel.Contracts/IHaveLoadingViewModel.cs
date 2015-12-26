namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents object which is able to display loading view model when the data source is loading.
    /// </summary>
    public interface IHaveLoadingViewModel<TLoadingViewModel>
    {
        /// <summary>
        /// Gets or sets the view model which is displayed on loading the collection.
        /// </summary>
        /// <value>
        /// The loading view model.
        /// </value>
        TLoadingViewModel LoadingViewModel { get; set; }
    }
}