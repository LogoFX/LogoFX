using System.Windows;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents paging information
    /// </summary>
    public interface IPageInfo
    {
        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        int PageCount { get; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        int CurrentPage { get; set; }

        /// <summary>
        /// Gets the current page rectangle.
        /// </summary>
        /// <value>
        /// The current page rectangle.
        /// </value>
        Rect CurrentPageRect { get; }

        /// <summary>
        /// Begins the update.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// Ends the update.
        /// </summary>
        void EndUpdate();
    }
}