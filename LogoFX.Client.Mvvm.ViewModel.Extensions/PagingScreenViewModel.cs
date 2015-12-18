using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{    
    public abstract class PagingScreenViewModel : Screen, IPagingViewModel, ICanBeBusy
    {
        private bool _isBusy;

        #region Protected

        public abstract int TotalPages { get; protected set; }

        public abstract double PageLeft { get; protected set; }

        public abstract double PageWidth { get; protected set; }

        public abstract int CurrentPage { get; set; }

        public abstract int SelectedCount { get; }

        public virtual bool AllowSelection
        {
            get { return true; }
        }

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