using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract class FilterViewModelBase<T> : ObjectViewModel<T>
        where T : IFilterModel
    {
        #region Fields

        private readonly DispatcherTimer _refreshTimer;

        #endregion

        #region Constructors

        protected FilterViewModelBase(T model)
        {            
            TimerEnabled = true;
            SetModel(model);

            RefreshInterval = TimeSpan.FromMilliseconds(750);
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = RefreshInterval;
            _refreshTimer.Tick += RefreshTimer_Tick;
        }

        #endregion

        #region Events

        public event EventHandler<FilterEventArgs<T>> Filter;

        #endregion

        #region Commands

        private ICommand _expandCommand;

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

        public TimeSpan RefreshInterval { get; set; }

        public bool TimerEnabled { get; set; }

        #endregion

        #region Protected Members

        protected void CancelRefresh()
        {
            _refreshTimer.Stop();
        }

        protected virtual void OnFilter()
        {
            CancelRefresh();
        }

        protected virtual void OnModelChanging(T newModel, T oldModel)
        {

        }

        protected virtual void OnModelChanged(T newModel, T oldModel)
        {

        }

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

    public sealed class FilterEventArgs<T> : EventArgs
    {
        public T FilterModel { get; private set; }

        public FilterEventArgs(T filterModel)
        {
            FilterModel = filterModel;
        }
    }
}
