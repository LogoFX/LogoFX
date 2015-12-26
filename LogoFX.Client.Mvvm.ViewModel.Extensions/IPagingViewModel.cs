namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents paging view model, i.e. an object that can display a collection of view models in pages.
    /// </summary>
    public interface IPagingViewModel
    {
        /// <summary>
        /// Gets or sets the total pages count.
        /// </summary>
        /// <value>
        /// The total pages count.
        /// </value>
        int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the left value of the page rectangle.
        /// </summary>
        /// <value>
        /// The left value of the page rectangle.
        /// </value>
        double PageLeft { get; set; }

        /// <summary>
        /// Gets or sets the width of the page.
        /// </summary>
        /// <value>
        /// The width of the page.
        /// </value>
        double PageWidth { get; set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        int CurrentPage { get; set; }

        /// <summary>
        /// Restores the selection.
        /// </summary>
        void RestoreSelection();
    }
}
