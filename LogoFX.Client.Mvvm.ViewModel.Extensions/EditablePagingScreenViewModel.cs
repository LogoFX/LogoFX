using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents a screen with support for paging and editing of specified type object view models.
    /// </summary>
    /// <typeparam name="TItem">The type of paging item.</typeparam>
    /// <typeparam name="TModel">The type of model.</typeparam>
    public abstract partial class EditablePagingScreenViewModel<TItem, TModel> : PagingScreenViewModel<TItem, TModel>
        where TModel : class, IEditableModel, IHaveErrors, IDataErrorInfo
        where TItem : EditablePagingItemViewModel<TModel>, IPagingItem
    {
        #region Commands

        private ICommand _closeCommand;
        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>
        /// The close command.
        /// </value>
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ??
                       (_closeCommand = ActionCommand<TItem>
                           .Do(x =>
                           {
                               EditingItem = null;
                           }));
            }
        }

        private ICommand _editCommand;
        /// <summary>
        /// Gets the edit command.
        /// </summary>
        /// <value>
        /// The edit command.
        /// </value>
        public ICommand EditCommand
        {
            get
            {
                return _editCommand ??
                       (_editCommand = ActionCommand<TItem>
                           .Do(x =>
                           {
                               EditingItem = x;
                           }));
            }
        }

        #endregion

        #region Public Properties

        private TItem _editingItem;
        /// <summary>
        /// Gets or sets the editing item.
        /// </summary>
        /// <value>
        /// The editing item.
        /// </value>
        public TItem EditingItem
        {
            get { return _editingItem; }
            protected set
            {
                if (_editingItem == value)
                {
                    return;
                }

                if (_editingItem != null)
                {
                    _editingItem.Saved -= OnItemSaved;
                    _editingItem.Saving -= OnItemSaving;
                    ((IEditableViewModel)_editingItem).CancelChanges();
                    ScreenExtensions.TryDeactivate(_editingItem, true);
                }
                _editingItem = value;
                if (_editingItem != null)
                {
                    _editingItem.ForcedDirty = _editingItem.IsNew;
                    ScreenExtensions.TryActivate(_editingItem);
                    _editingItem.Saving += OnItemSaving;
                    _editingItem.Saved += OnItemSaved;
                    _editingItem.OnSavingFunc = ItemSave;
                }
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Saves paging item's model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected abstract bool ItemSave(TModel model);

        /// <summary>
        /// Override this method to inject custom logic when the paging item's model is saved.
        /// </summary>
        protected virtual void OnItemSaved()
        {

        }

        #endregion

        #region Private Members

        private void OnItemSaving(object sender, EventArgs e)
        {
            IsBusy = true;
        }

        private void OnItemSaved(object sender, ResultEventArgs e)
        {
            IsBusy = false;

            if (!e.Successful)
            {
                return;
            }

            var item = (TItem)sender;

            if (item.IsNew)
            {
                item.IsNew = false;
                EditingItem = null;
            }

            //if (!_internalCache.Contains(item.Model))
            //{
            //    _internalCache.Add(item.Model);
            //}

            EditingItem = null;
            RefreshData();
            OnItemSaved();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Displays the save changes prompt and captures the selected prompt option.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<MessageResult> OnSaveChangesPrompt();

        /// <summary>
        /// Reacts to the save changes error.
        /// </summary>
        /// <returns></returns>
        protected abstract Task OnSaveChangesWithErrors();

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public override async void CanClose(Action<bool> callback)
        {
            bool isDirty = EditingItem != null && EditingItem.IsDirty;

            if (isDirty)
            {
                MessageResult retVal = await OnSaveChangesPrompt();

                switch (retVal)
                {
                    case MessageResult.Cancel:                        
                        callback(false);
                        return;
                    case MessageResult.Yes:                        
                        bool hasErrors = EditingItem.HasErrors;
                        if (hasErrors)
                        {
                            await OnSaveChangesWithErrors();
                            callback(false);
                            return;
                        }
                        
                        bool successful = await (EditingItem as IEditableViewModel).SaveAsync();
                        if (successful == false)
                        {
                            callback(false);
                            return;
                        }
                        break;
                    case MessageResult.No:                        
                        (EditingItem as IEditableViewModel).CancelChanges();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            base.CanClose(callback);
        }

        #endregion
    }
}