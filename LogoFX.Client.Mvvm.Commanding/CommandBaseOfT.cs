// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using LogoFX.Client.Core;

namespace LogoFX.Client.Mvvm.Commanding
{
    public abstract class CommandBase<T>
        : IActionCommand
    {
        protected const string ERROR_EXPECTED_TYPE = "Expected parameter for command ({0}) must be of {1} type";

        private EventHandler _canExecuteHandler;
        private bool _isActive = true;

        private Uri _imageUri;
        private bool _isAdvanced;
        private string _name;
        private string _description;
        protected CommandBase() { }

        protected CommandBase(bool isActive)
            : this()
        {
            _isActive = isActive;
        }

        #region Additional Methods

        public virtual bool CanExecute(T parameter)
        {
            return IsActive && OnCanExecute(parameter);
        }

        public virtual void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                OnExecute(parameter);
                OnCommandExecuted(new CommandEventArgs(parameter));
            }
        }

        protected virtual void OnRequeryCanExecute()
        {
            if (_canExecuteHandler != null) _canExecuteHandler(this, EventArgs.Empty);
        }

        #endregion

        #region Abstract Methods

        protected abstract bool OnCanExecute(T parameter);

        protected abstract void OnExecute(T parameter);

        #endregion

        #region IActionCommand Members

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged.Notify(() => IsActive);
                    RequeryCanExecute();
                }
            }
        }

        public void RequeryCanExecute()
        {
            OnRequeryCanExecute();
        }

        #endregion

        #region IReverseCommand Members

        public event EventHandler<CommandEventArgs> CommandExecuted;

        #endregion

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            CheckParameterType(parameter);
            return CanExecute(ParseParameter(parameter, typeof(T)));
        }
#if SILVERLIGHT || WinRT
        event EventHandler ICommand.CanExecuteChanged
        {
            add { _canExecuteHandler += value; }
            remove { _canExecuteHandler -= value; }
        }
#else
        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _canExecuteHandler += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                _canExecuteHandler -= value;
            }
        }
#endif

        void ICommand.Execute(object parameter)
        {
            CheckParameterType(parameter);
            Execute(ParseParameter(parameter, typeof(T)));
        }

        #endregion

        #region IReceiveEvent Members

        bool IReceiveEvent.ReceiveWeakEvent(EventArgs e)
        {
            RequeryCanExecute();
            return true;        // as in always listening
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<P>(Expression<Func<P>> propertySelector)
        {
            Guard.ArgumentNotNull(propertySelector, "propertySelector");
            PropertyChanged.Notify<P>(propertySelector);
        }

        #endregion

        #region Helpers

        protected void OnCommandExecuted(CommandEventArgs args)
        {
            if (CommandExecuted != null) CommandExecuted(this, args);
        }

        protected virtual T ParseParameter(object parameter, Type parseAsType)
        {
            if (parameter == null) return default(T);
#if WinRT
            if (parseAsType.IsEnum())
#else
            if (parseAsType.IsEnum)
#endif
            {
                return (T)Enum.Parse(parseAsType, Convert.ToString(parameter), true);
            }
#if WinRT
            else if (parseAsType.IsValueType())
#else
            else if (parseAsType.IsValueType)
#endif
            {
                return (T)Convert.ChangeType(parameter, parseAsType, null);
            }
            else
            {
                return (T)parameter;
            }
        }

        protected void CheckParameterType(object parameter)
        {
            if (parameter == null) return;
#if WinRT
            if (typeof(T).IsValueType()) return;
#else
            if (typeof(T).IsValueType) return;
#endif
            Guard.ArgumentValue((!typeof(T).IsAssignableFrom(parameter.GetType())), "parameter", ERROR_EXPECTED_TYPE,
                this.GetType().FullName, typeof(T).FullName);
        }

        #endregion

        #region Implementation of IExtendedCommand

        public string Name
        {
            get { return _name; }
            set { _name = value;
                NotifyPropertyChanged(() => Name); }
        }

        public string Description
        {
            get { return _description??Name; }
            set { _description = value;
                NotifyPropertyChanged(() => Description); }
        }

        public Uri ImageUri
        {
            get { return _imageUri; }
             set
            {
                _imageUri = value;
                NotifyPropertyChanged(() => ImageUri);
            }
        }

        public bool IsAdvanced
        {
            get { return _isAdvanced; }
            set { _isAdvanced = value;
                NotifyPropertyChanged(() => IsAdvanced); }
        }

        #endregion
    }
}
