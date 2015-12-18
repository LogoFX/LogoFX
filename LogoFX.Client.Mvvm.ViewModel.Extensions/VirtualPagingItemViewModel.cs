using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using LogoFX.Client.Mvvm.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public sealed class VirtualPagingItemViewModel<TItem, TModel> : Conductor<TItem>, IPagingItemViewModel
        where TItem : class, IModelWrapper<TModel>, ISelectable where TModel : class
    {
        #region Fields

        private readonly VirtualContainer<TModel> _container;
        private readonly Func<TModel, Task<TItem>> _createVmFunc;

        private bool _isSelected;

        #endregion

        #region Constructors

        public VirtualPagingItemViewModel(PagingScreenViewModel parent, VirtualContainer<TModel> container, Func<TModel, TItem> createVmFunc)
            : this(parent, container, model => Task<TItem>.Factory.StartNew(() => createVmFunc(model)))
        {
        }

        public VirtualPagingItemViewModel(PagingScreenViewModel parent, VirtualContainer<TModel> container, Func<TModel, Task<TItem>> createVmFunc)
        {
            SetParent(parent);
            Index = container.Index;
            _container = container;
            _createVmFunc = createVmFunc;
            _container.NotifyOn("Model", OnModelCame);
        }

        #endregion

        #region Events

        public event EventHandler SelectionStateChanged;

        #endregion

        #region Commands

        private ICommand _selectCommand;

        public ICommand SelectCommand
        {
            get
            {
                return _selectCommand ??
                       (_selectCommand = ActionCommand
                           .Do(() =>
                           {
                               IsSelected = !IsSelected;
                           }));
            }
        }

        #endregion

        #region Public Properties

        public int Index { get; private set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;
                NotifyOfPropertyChange();
                if (ActiveItem != null)
                {
                    ActiveItem.IsSelected = value;
                }
                OnSelectionStateChanged();
            }
        }

        #endregion

        #region Private Members

        private void OnSelectionStateChanged()
        {
            EventHandler handler;

            lock (this)
            {
                handler = SelectionStateChanged;
            }

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private async void OnModelCame(object newValue, object oldValue)
        {
            TItem item = await _createVmFunc((TModel)newValue);
            IsSelected = item.IsSelected;
            ActivateItem(item);
        }

        private void SetParent(PagingScreenViewModel parent)
        {
            Parent = parent;
        }

        #endregion
    }
}