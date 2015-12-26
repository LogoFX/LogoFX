using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents a screen with paging capabilities.
    /// </summary>
    public abstract class PagingScreenViewModel : Screen, IPagingViewModel, ICanBeBusy
    {
        #region Protected

        /// <summary>
        /// Gets or sets the total pages count.
        /// </summary>
        /// <value>
        /// The total pages count.
        /// </value>
        public abstract int TotalPages { get; protected set; }

        /// <summary>
        /// Gets or sets the left value of the page rectangle.
        /// </summary>
        /// <value>
        /// The left value of the page rectangle.
        /// </value>
        public abstract double PageLeft { get; protected set; }

        /// <summary>
        /// Gets or sets the width of the page.
        /// </summary>
        /// <value>
        /// The width of the page.
        /// </value>
        public abstract double PageWidth { get; protected set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public abstract int CurrentPage { get; set; }

        /// <summary>
        /// Gets the selected items count.
        /// </summary>
        /// <value>
        /// The selected items count.
        /// </value>
        public abstract int SelectedCount { get; }

        /// <summary>
        /// Gets a value indicating whether selection is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selection is allowed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AllowSelection
        {
            get { return true; }
        }

        /// <summary>
        /// Restores the selection.
        /// </summary>
        protected abstract void RestoreSelection();

        #endregion

        #region IPagingViewModel implementation

        int IPagingViewModel.TotalPages
        {
            get { return TotalPages; }
            set { TotalPages = value; }
        }

        double IPagingViewModel.PageLeft
        {
            get { return PageLeft; }
            set { PageLeft = value; }
        }

        double IPagingViewModel.PageWidth
        {
            get { return PageWidth; }
            set { PageWidth = value; }
        }

        int IPagingViewModel.CurrentPage
        {
            get { return CurrentPage; }
            set { CurrentPage = value; }
        }

        void IPagingViewModel.RestoreSelection()
        {
            RestoreSelection();
        }

        #endregion        

        #region ICanBusy implementation

        private bool _isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether the object is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is busy; otherwise, <c>false</c>.
        /// </value>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion
    }
}