using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// The base class for filter view model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FilterViewModelBase<T> : ObjectViewModel<T>
        where T : IFilterModel
    {
        #region Fields

        private readonly DispatcherTimer _refreshTimer;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected FilterViewModelBase(T model)
        {            
            TimerEnabled = true;
            SetModel(model);

            RefreshInterval = TimeSpan.FromMilliseconds(750);
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = RefreshInterval;
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        #region Events

        /// <summary>
        /// Occurs when filtering is executed.
        /// </summary>
        public event EventHandler<FilterEventArgs<T>> Filter;

        #endregion

        #region Commands

        private ICommand _expandCommand;
        /// <summary>
        /// Gets the expand command.
        /// </summary>
        /// <value>
        /// The expand command.
        /// </value>
        public ICommand ExpandCommand
        {
            get
            {
                return _expandCommand ??
                       (_expandCommand = ActionCommand
                           .When(() => !IsExpanded)
                           .Do(() =>
                           {
                               IsExpanded = true;
                           })
                           .RequeryOnPropertyChanged(this, () => IsExpanded));

            }
        }

        private ICommand _collapseCommand;
        /// <summary>
        /// Gets the collapse command.
        /// </summary>
        /// <value>
        /// The collapse command.
        /// </value>
        public ICommand CollapseCommand
        {
            get
            {
                return _collapseCommand ??
                       (_collapseCommand = ActionCommand
                           .When(() => IsExpanded)
                           .Do(() =>
                           {
                               IsExpanded = false;
                           })
                           .RequeryOnPropertyChanged(this, () => IsExpanded));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void SetModel(T model)
        {
            OnModelChanging(model, Model);

            //if (Equals(model, Model))
            //{
            //    return;
            //}

            T oldModel = Model;

            if (!ReferenceEquals(Model, null))
            {
                var notify = Model as INotifyPropertyChanged;
                if (notify != null)
                {
                    notify.PropertyChanged -= ModelPropertyChanged;
                }
            }

            ((ObjectViewModel)this).Model = model;
            NotifyOfPropertyChange(() => Model);

            if (!ReferenceEquals(Model, null))
            {
                var notify = Model as INotifyPropertyChanged;
                if (notify != null)
                {
                    notify.PropertyChanged += ModelPropertyChanged;
                }
            }

            OnModelChanged(model, oldModel);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the refresh interval.
        /// </summary>
        /// <value>
        /// The refresh interval.
        /// </value>
        public TimeSpan RefreshInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether timer is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if timer is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool TimerEnabled { get; set; }

        #endregion

        #region Protected Members

        /// <summary>
        /// Cancels the refresh.
        /// </summary>
        protected void CancelRefresh()
        {
            _refreshTimer.Stop();
        }

        /// <summary>
        /// Override this method to inject custom logic during filtering.
        /// </summary>
        protected virtual void OnFilter()
        {
            CancelRefresh();
        }

        /// <summary>
        /// Override this method to inject custom logic before the model is changed.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <param name="oldModel">The old model.</param>
        protected virtual void OnModelChanging(T newModel, T oldModel)
        {

        }

        /// <summary>
        /// Override this method to inject custom logic after the model is changed.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <param name="oldModel">The old model.</param>
        protected virtual void OnModelChanged(T newModel, T oldModel)
        {

        }

        /// <summary>
        /// Invokes the data change logic.
        /// </summary>
        protected void DataChanged()
        {
            if (!TimerEnabled || Model == null)
            {
                return;
            }

            _refreshTimer.Stop();
            _refreshTimer.Interval = RefreshInterval;
            _refreshTimer.Start();
        }

        #endregion

        #region Private Members

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            DataChanged();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            CancelRefresh();

            EventHandler<FilterEventArgs<T>> handler;

            lock (this)
            {
                handler = Filter;
            }

            if (handler == null)
            {
                return;
            }

            handler(this, new FilterEventArgs<T>(Model));
        }

        #endregion
    }

    /// <summary>
    /// Represents filtering event arguments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FilterEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the filter model.
        /// </summary>
        /// <value>
        /// The filter model.
        /// </value>
        public T FilterModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterEventArgs{T}"/> class.
        /// </summary>
        /// <param name="filterModel">The filter model.</param>
        public FilterEventArgs(T filterModel)
        {
            FilterModel = filterModel;
        }
    }
}
