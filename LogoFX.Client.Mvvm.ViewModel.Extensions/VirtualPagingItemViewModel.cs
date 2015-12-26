using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents a view model which can be used as paging item and support virtualization.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public sealed class VirtualPagingItemViewModel<TItem, TModel> : Conductor<TItem>, IPagingItem
        where TItem : class, IModelWrapper<TModel>, ISelectable where TModel : class
    {
        #region Fields

        private readonly VirtualContainer<TModel> _container;
        private readonly Func<TModel, Task<TItem>> _createVmFunc;

        private bool _isSelected;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPagingItemViewModel{TItem, TModel}"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="container">The container.</param>
        /// <param name="createVmFunc">The create vm function.</param>
        public VirtualPagingItemViewModel(PagingScreenViewModel parent, VirtualContainer<TModel> container, Func<TModel, TItem> createVmFunc)
            : this(parent, container, model => Task<TItem>.Factory.StartNew(() => createVmFunc(model)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPagingItemViewModel{TItem, TModel}"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="container">The container.</param>
        /// <param name="createVmFunc">The create vm function.</param>
        public VirtualPagingItemViewModel(PagingScreenViewModel parent, VirtualContainer<TModel> container, Func<TModel, Task<TItem>> createVmFunc)
        {
            SetParent(parent);
            Index = container.Index;
            _container = container;
            _createVmFunc = createVmFunc;
            _container.NotifyOn("Model", OnModelCame);
        }

        #region Events

        /// <summary>
        /// Occurs when selection state is changed.
        /// </summary>
        public event EventHandler SelectionStateChanged;

        #endregion

        #region Commands

        private ICommand _selectCommand;
        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <value>
        /// The select command.
        /// </value>
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

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; private set; }

        /// <summary>
        /// Selection status
        /// </summary>
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