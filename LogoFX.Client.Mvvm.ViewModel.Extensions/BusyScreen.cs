using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents screen with busy state
    /// </summary>
    public abstract class BusyScreen : Screen, ICanBeBusy
    {
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
                    return;

                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }
    }
}