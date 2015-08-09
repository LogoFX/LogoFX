using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract class EditableScreenObjectViewModel<T> : ScreenObjectViewModel<T>, IEditableViewModel, ICanBeBusy, IDataErrorInfo
        where T : IEditableModel
    {
        #region Constructors

        protected EditableScreenObjectViewModel(T model)
            : base(model)
        {
            Model.NotifyOn("IsDirty", (o, o1) => NotifyOfPropertyChange(() => IsDirty));
            Model.NotifyOn("CanCancelChanges", (o, o1) => NotifyOfPropertyChange(() => CanCancelChanges));
            Model.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => HasErrors));
        }

        #endregion

        #region Commands

        private ICommand _applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                       (_applyCommand = ActionCommand
                           .When(() => (ForcedDirty || Model.IsDirty) && !Model.HasErrors)
                           .Do(async () =>
                           {
                               await SaveAsync();
                           })
                           .RequeryOnPropertyChanged(this, () => ForcedDirty)
                           .RequeryOnPropertyChanged(this, () => IsDirty)
                           .RequeryOnPropertyChanged(this, () => HasErrors));
            }
        }

        private ICommand _canCancelChangesCommand;
        public ICommand CancelChangesCommand
        {
            get
            {
                return _canCancelChangesCommand ??
                       (_canCancelChangesCommand = ActionCommand
                           .When(() => CanCancelChanges && IsDirty)
                           .Do(CancelChanges)
                           .RequeryOnPropertyChanged(this, () => CanCancelChanges)
                           .RequeryOnPropertyChanged(this, () => IsDirty));
            }
        }

        #endregion

        #region Public Properties

        private bool _forcedDirty;
        public bool ForcedDirty
        {
            get { return _forcedDirty; }
            set
            {
                if (_forcedDirty == value)
                {
                    return;
                }

                _forcedDirty = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Events

        public event EventHandler Saving;

        public event EventHandler<ResultEventArgs> Saved;

        #endregion

        #region Protected Members

        protected abstract Task<bool> SaveMethod(T model);
        
        protected async Task<bool> SaveAsync()
        {
            OnSaving();
            bool result = await SaveMethod(Model);
            OnSaved(result);
            return result;
        }

        protected virtual void OnSaving()
        {
            var handler = Saving;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSaved(bool successfull)
        {
            var handler = Saved;

            if (handler != null)
            {
                handler(this, new ResultEventArgs(successfull));
            }
        }

        private void CancelChanges()
        {
            if (Model.CanCancelChanges)
            {
                Model.CancelChanges();
            }
            else
            {
                Model.ClearDirty();
            }

            OnChangesCanceled();
        }

        protected virtual void OnChangesCanceled()
        {

        }

        #endregion

        #region IEditableViewModel

        public bool IsDirty
        {
            get { return Model.IsDirty; }
        }

        public bool CanCancelChanges
        {
            get { return Model.CanCancelChanges; }
            set { Model.CanCancelChanges = value; }
        }

        bool IEditableViewModel.HasErrors
        {
            get { return Model.HasErrors; }
        }

        void IEditableViewModel.CancelChanges()
        {
            CancelChanges();
        }

        Task<bool> IEditableViewModel.SaveAsync()
        {
            return SaveAsync();
        }

        #endregion

        #region IDataErrorInfo

        public virtual string this[string columnName]
        {
            get
            {
                var errors = GetErrors(columnName);

                if (errors == null)
                {
                    return null;
                }

                return errors.FirstOrDefault().ToString();
            }
        }

        public virtual string Error
        {
            get { return null; }
        }

        #endregion
    }
}