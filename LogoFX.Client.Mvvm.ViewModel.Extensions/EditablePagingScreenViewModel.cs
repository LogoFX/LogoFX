using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class EditablePagingScreenViewModel<TItem, TModel> : PagingScreenViewModel<TItem, TModel>
        where TModel : class, IEditableModel, IHaveErrors 
        where TItem : EditablePagingItemViewModel<TModel>, IPagingItemViewModel
    {
        #region Fields

        private ICommand _editCommand;
        private ICommand _closeCommand;

        #endregion

        #region Commands

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

        protected abstract bool ItemSave(TModel model);

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

        protected abstract Task<MessageResult> OnSaveChangesPrompt();

        protected abstract Task OnSaveChangesWithErrors();

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