using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents object view model which wraps an editable model.
    /// </summary>
    /// <typeparam name="T">Type of editable model.</typeparam>
    public abstract class EditableObjectViewModel<T> : ObjectViewModel<T>, IEditableViewModel, ICanBeBusy, IDataErrorInfo
        where T : IEditableModel, IHaveErrors, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableObjectViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected EditableObjectViewModel(T model)
            : base(model)
        {
            Model.NotifyOn("IsDirty", (o, o1) => NotifyOfPropertyChange(() => IsDirty));
            Model.NotifyOn("CanCancelChanges", (o, o1) => NotifyOfPropertyChange(() => CanCancelChanges));
            Model.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => HasErrors));
        }

        #region Commands

        private ICommand _applyCommand;
        /// <summary>
        /// Gets the apply command which saves model changes.
        /// </summary>
        /// <value>
        /// The apply command.
        /// </value>
        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                    (_applyCommand = ActionCommand
                    .When(() => (ForcedDirty || Model.IsDirty) && !((IHaveErrors)Model).HasErrors)
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
        /// <summary>
        /// Gets the cancel changes command which cancels model changes.
        /// </summary>
        /// <value>
        /// The cancel changes command.
        /// </value>
        public ICommand CancelChangesCommand
        {
            get
            {
                return _canCancelChangesCommand ??
                       (_canCancelChangesCommand = ActionCommand
                           .When(() => CanCancelChanges && IsDirty)
                           .Do(CancelChangesAsync)
                           .RequeryOnPropertyChanged(this, () => CanCancelChanges)
                           .RequeryOnPropertyChanged(this, () => IsDirty));
            }
        }

        #endregion

        #region Public Properties

        private bool _forcedDirty;
        /// <summary>
        /// Gets or sets a value indicating whether view model's dirty state is forced.        .
        /// </summary>
        /// <value>
        ///   <c>true</c> if view model's dirty state is forced; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Occurs before the model changes are applied.
        /// </summary>
        public event EventHandler Saving;

        /// <summary>
        /// Occurs after the model changes are applied.
        /// </summary>
        public event EventHandler<ResultEventArgs> Saved;

        #endregion

        #region Protected Members

        /// <summary>
        /// Override this method to provide custom save changes logic.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected abstract Task<bool> SaveMethod(T model);

        private async Task<bool> SaveAsync()
        {
            await OnSaving();
            bool result = await SaveMethod(Model);
            if (result)
            {
                Model.ClearDirty(forceClearChildren: true);
            }
            await OnSaved(result);
            return result;
        }

        /// <summary>
        /// Override this method to inject custom logic before the model changes are saved.
        /// </summary>
        /// <returns></returns>
        protected async virtual Task OnSaving()
        {
            var handler = Saving;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic after the model changes are saved.
        /// </summary>
        /// <param name="successful">The result of save changes operation.</param>
        /// <returns></returns>
        protected async virtual Task OnSaved(bool successful)
        {
            var handler = Saved;

            if (handler != null)
            {
                handler(this, new ResultEventArgs(successful));
            }
        }

        private async void CancelChangesAsync()
        {
            try
            {
                await OnChangesCanceling();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                if (Model.CanCancelChanges)
                {
                    Model.CancelChanges();
                }
                else
                {
                    Model.ClearDirty();
                }
            }
            catch (Exception)
            {
                //TODO: add proper rollback
                throw;
            }

            await OnChangesCanceled();
        }

        /// <summary>
        /// Override this method to inject custom logic before the model changes are canceled.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnChangesCanceling()
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// Override this method to inject custom logic after the model changes are canceled.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnChangesCanceled()
        {
            return Task.Run(() => { });
        }

        #endregion

        #region IEditableViewModel

        /// <summary>
        /// Gets a value indicating whether the view model is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the view model is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get { return Model.IsDirty; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the changes can be cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the changes can be cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool CanCancelChanges
        {
            get { return Model.CanCancelChanges; }
            set { Model.CanCancelChanges = value; }
        }

        bool IHaveErrors.HasErrors
        {
            get { return ((IHaveErrors)Model).HasErrors; }
        }

        void IEditableViewModel.CancelChanges()
        {
            CancelChangesAsync();
        }

        Task<bool> IEditableViewModel.SaveAsync()
        {
            return SaveAsync();
        }

        #endregion

        #region IDataErrorInfo

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public virtual string this[string columnName]
        {
            get { return Model[columnName]; }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public virtual string Error
        {
            get { return Model.Error; }
        }

        #endregion
    }
}
