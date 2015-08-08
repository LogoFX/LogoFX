using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Caliburn.Micro;
using LogoFX.Client.Core;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public class ScreenObjectViewModel<T> : Screen, IObjectViewModel<T>
    {
        #region Ctor's

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        public ScreenObjectViewModel()
            : this(default(T))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ScreenObjectViewModel(T model)
        {
            _model = model;
        }

        #endregion

        #region ObjectModel property

        /// <summary>
        /// ObjectModel property
        /// </summary>
        [Obsolete("Use Model instead")]
        public T ObjectModel
        {
            get { return (T)_model; }
        }

        /// <summary>
        /// Model property
        /// </summary>
        public T Model
        {
            get { return (T)_model; }
            protected set
            {
                if (Equals(_model, value))
                {
                    return;
                }

                _model = value;
                NotifyOfPropertyChange();
            }
        }

        private object _model;

        #endregion

        #region IsEnabled property

        private bool _isEnabled = true;

        /// <summary>
        /// <c>IsEnabled</c> property
        /// </summary>
        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                bool oldValue = _isEnabled;
                _isEnabled = value;
                OnIsEnabledChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        /// <summary>
        /// Called when <see cref="IsEnabled"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnIsEnabledChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region DisplayName property

        protected bool SetExplicitly;
        private string _displayName = string.Empty;
        /// <summary>
        /// DisplayName property
        /// </summary>
        public virtual new string DisplayName
        {
            get { return SetExplicitly ? _displayName : "ViewModel"; }
            set
            {
                SetExplicitly = true;
                _displayName = value;
            }
        }
        #endregion

        #region IsExpanded property

        private bool _isExpanded;

        /// <summary>
        /// IsExpanded property
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;

                bool oldValue = _isExpanded;
                _isExpanded = value;
                OnIsExpandedChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => IsExpanded);
            }
        }

        /// <summary>
        /// Called when <see cref="IsExpanded"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnIsExpandedChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region IsCurrent property

        private bool _isCurrent;

        /// <summary>
        /// IsCurrent property
        /// </summary>
        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                if (_isCurrent == value)
                    return;

                bool oldValue = _isCurrent;
                _isCurrent = value;
                OnIsCurrentChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => IsCurrent);
            }
        }

        /// <summary>
        /// Called when <see cref="IsCurrent"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnIsCurrentChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region IsSelected property

        private bool _isSelected;

        /// <summary>
        /// IsSelected property
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                bool oldValue = _isSelected;
                _isSelected = value;
                OnIsSelectedChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        /// <summary>
        /// Called when <see cref="IsSelected"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnIsSelectedChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region LastError property

        private string _lastError;

        /// <summary>
        /// LastError property
        /// </summary>
        public string LastError
        {
            get { return _lastError; }
            set
            {
                if (_lastError == value)
                    return;

                string oldValue = _lastError;
                _lastError = value;
                OnLastErrorChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => LastError);
            }
        }

        /// <summary>
        /// Called when <see cref="LastError"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnLastErrorChangedOverride(string newValue, string oldValue)
        {
        }

        #endregion

        #region IsBusy property

        private bool _isBusy;

        /// <summary>
        /// IsBusy property
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;

                bool oldValue = _isBusy;
                _isBusy = value;
                OnIsBusyChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        /// <summary>
        /// Called when <see cref="IsBusy"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnIsBusyChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region Parent property

        private IViewModel _parent;

        /// <summary>
        /// Parent property
        /// </summary>
        public new IViewModel Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                IViewModel oldValue = _parent;
                _parent = value;
                OnParentChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => Parent);
            }
        }

        /// <summary>
        /// Called when <see cref="Parent"/> changed
        /// </summary>
        /// <param name="newValue">New Value</param>
        /// <param name="oldValue">Old Value</param>
        protected virtual void OnParentChangedOverride(IViewModel newValue, IViewModel oldValue)
        {
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {

        }
        #endregion

        private readonly Dictionary<string, List<string>> _currentErrors = new Dictionary<string, List<string>>();



        /// <summary>
        /// Gets the errors for property.
        /// </summary>
        /// <param name="propertyName">DomainName of the property to check.</param>
        /// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_currentErrors.ContainsKey(propertyName))
                return _currentErrors[propertyName];

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return (_currentErrors.Count > 0); }
        }

        // ReSharper disable UnusedParameter.Local
        void FireErrorsChanged(string property)
            // ReSharper restore UnusedParameter.Local
        {
#if SILVERLIGHT
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
            }
#endif
            NotifyOfPropertyChange(() => HasErrors);
        }

        /// <summary>
        /// Clears the error from property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression that designates the property.</param>
        public void ClearErrorFromProperty<TProperty>(Expression<Func<TProperty>> expression)
        {

            string property = expression.GetPropertyName();
            _currentErrors.Remove(property);
            FireErrorsChanged(property);
        }

        /// <summary>
        /// Adds the error for property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression that designates the property.</param>
        /// <param name="error">The error description.</param>
        public void AddErrorForProperty<TProperty>(Expression<Func<TProperty>> expression, string error)
        {
            string property = expression.GetPropertyName();
            _currentErrors.Remove(property);
            _currentErrors.Add(property, new List<string>(new[] { error }));
            FireErrorsChanged(property);
        }

#if SILVERLIGHT
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
#endif

        object IModelWrapper.Model
        {
            get { return _model; }
        }
    }
}