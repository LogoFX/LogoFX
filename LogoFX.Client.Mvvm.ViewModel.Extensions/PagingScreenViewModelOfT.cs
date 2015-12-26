using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using LogoFX.Core;
using LogoFX.Client.Mvvm.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents a screen with support for paging specified type object view models.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract partial class PagingScreenViewModel<TItem, TModel> : PagingScreenViewModel
        where TModel : class
        where TItem : class, IPagingItem
    {
        #region Fields                                       

        private int _oldPageCount;
        private int? _oldCurrentPage;        

        #endregion

        #region Commands

        private ICommand _selectAllItemsCommand;
        /// <summary>
        /// Gets the select all items command.
        /// </summary>
        /// <value>
        /// The select all items command.
        /// </value>
        public ICommand SelectAllItemsCommand
        {
            get
            {
                return _selectAllItemsCommand ??
                       (_selectAllItemsCommand = ActionCommand
                           .When(() => AllowSelection)
                           .Do(OnSelectAll)
                           .RequeryOnPropertyChanged(this, () => AllowSelection));
            }
        }

        private ICommand _revertAllSelectionCommand;
        /// <summary>
        /// Gets the revert all selection command.
        /// </summary>
        /// <value>
        /// The revert all selection command.
        /// </value>
        public ICommand RevertAllSelectionCommand
        {
            get
            {
                return _revertAllSelectionCommand ??
                       (_revertAllSelectionCommand = ActionCommand
                           .When(() => AllowSelection)
                           .Do(OnRevertAllSelection)
                           .RequeryOnPropertyChanged(this, () => AllowSelection));
            }
        }

        private ICommand _clearAllSelectionCommand;
        /// <summary>
        /// Gets the clear all selection command.
        /// </summary>
        /// <value>
        /// The clear all selection command.
        /// </value>
        public ICommand ClearAllSelectionCommand
        {
            get
            {
                return _clearAllSelectionCommand ??
                       (_clearAllSelectionCommand = ActionCommand
                           .When(() => AllowSelection)
                           .Do(OnClearAllSelection)
                           .RequeryOnPropertyChanged(this, () => AllowSelection));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the overall number of selected items on the current page.
        /// </summary>
        public int SelectedOnPage
        {
            get
            {
                int count = 0;
                int end = StartIndex + PageCapacity;
                for (int index = StartIndex; index < end; ++index)
                {
                    var wrapper = Items[index];
                    if (wrapper.IsSelected)
                    {
                        ++count;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the overall items count.
        /// </summary>
        public virtual int Count
        {
            get
            {
                if (Items == null)
                {
                    return 0;
                }

                return Items.Count;
            }
        }

        /// <summary>
        /// Gets the filtered items count.
        /// </summary>
        /// <value>
        /// The filtered items count.
        /// </value>
        public virtual int FilteredCount
        {
            get
            {
                if (CollectionView == null)
                {
                    return 0;
                }

                return CollectionView.Cast<object>().Count();                
            }
        }

        private int _totalPages;
        /// <summary>
        /// Gets or sets the total pages count.
        /// </summary>
        /// <value>
        /// The total pages count.
        /// </value>
        public override int TotalPages
        {
            get { return _totalPages; }
            protected set
            {
                if (_totalPages == value)
                {
                    return;
                }

                _totalPages = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => PageCapacity);
                NotifyOfPropertyChange(() => StartIndex);
            }
        }

        private int _currentPage;
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public override int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage == value)
                {
                    return;
                }

                _currentPage = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => StartIndex);
            }
        }

        private double _pageWidth;
        /// <summary>
        /// Gets or sets the width of the page.
        /// </summary>
        /// <value>
        /// The width of the page.
        /// </value>
        public override double PageWidth
        {
            get { return _pageWidth; }
            protected set
            {
                if (_pageWidth == value)
                {
                    return;
                }

                _pageWidth = value;
                NotifyOfPropertyChange();
            }
        }

        private double _pageLeft;
        /// <summary>
        /// Gets or sets the left value of the page rectangle.
        /// </summary>
        /// <value>
        /// The left value of the page rectangle.
        /// </value>
        public override double PageLeft
        {
            get { return _pageLeft; }
            protected set
            {
                if (_pageLeft == value)
                {
                    return;
                }

                _pageLeft = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the starting index.
        /// </summary>
        public int StartIndex
        {
            get { return CurrentPage * PageCapacity; }
        }

        /// <summary>
        /// Gets the page capacity.
        /// </summary>
        public int PageCapacity
        {
            get
            {
                if (TotalPages == 0)
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)FilteredCount / TotalPages);
            }
        }

        private PagingItemListViewModelBase<TItem> _items;
        /// <summary>
        /// Gets the list of paged items.
        /// </summary>
        public PagingItemListViewModelBase<TItem> Items
        {
            get { return _items; }
            private set
            {
                if (_items == value)
                {
                    return;
                }

                _collectionView = null;
                _items = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => SelectedCount);
                UpdateSortDescriptors();
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Override this method to inject custom logic on select all execution.
        /// </summary>
        protected virtual void OnSelectAll()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = true);
        }

        /// <summary>
        /// Override this method to inject custom logic on reverting select all execution.
        /// </summary>
        protected virtual void OnRevertAllSelection()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = !item.IsSelected);
        }

        /// <summary>
        /// Override this method to inject custom logic on clearing selection.
        /// </summary>
        protected virtual void OnClearAllSelection()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = false);
        }

        /// <summary>
        /// Override this method to inject custom logic on refreshing the view.
        /// </summary>
        protected virtual void OnRefresh()
        {

        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        protected virtual void RefreshData()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.Refresh();
            OnRefresh();
        }

        /// <summary>
        /// Gets minimal selected index.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMinSelectedIndex()
        {
            if (CollectionView == null)
            {
                return -1;
            }

            var selectedList = CollectionView
                .AsParallel()
                .OfType<TItem>()
                .Where(x => x.IsSelected)
                .ToList();

            if (selectedList.Count == 0)
            {
                return -1;
            }

            var minSelectedIndex = CollectionView.OfType<TItem>().ToList().IndexOf(selectedList[0]);
            return minSelectedIndex;
        }

        /// <summary>
        /// Gets maximal selected index.
        /// </summary>
        /// <returns></returns>
        protected virtual int GetMaxSelectedIndex()
        {
            if (CollectionView == null)
            {
                return -1;
            }

            var selectedList = CollectionView
                .AsParallel()
                .OfType<TItem>()
                .Where(x => x.IsSelected)
                .ToList();

            if (selectedList.Count == 0)
            {
                return -1;
            }

            var maxSelectedIndex = CollectionView.OfType<TItem>().ToList().IndexOf(selectedList[selectedList.Count - 1]);
            return maxSelectedIndex;
        }

        /// <summary>
        /// Memorizes the current page and the overall page count values.
        /// </summary>
        protected virtual void MakePageLocationSnapshot()
        {
            if (TotalPages == 0)
            {
                _oldCurrentPage = null;
            }
            else
            {
                _oldCurrentPage = CurrentPage;
            }

            _oldPageCount = TotalPages;
        }

        /// <summary>
        /// Restores the selection.
        /// </summary>
        protected override void RestoreSelection()
        {
            if (!_oldCurrentPage.HasValue)
            {
                return;
            }

            //int minSelectedIndex = GetMinSelectedIndex();
            int lastSelectedIndex = GetMaxSelectedIndex();

            if (lastSelectedIndex < 0)
            {
                CurrentPage = (int)Math.Round(_oldCurrentPage.Value * ((double)TotalPages / _oldPageCount));
            }
            else
            {
                CurrentPage = lastSelectedIndex / PageCapacity;
            }
            _oldCurrentPage = null;
        }

        private IList _internalCache;
        /// <summary>
        /// Gets or sets the internal cache of paged items.
        /// </summary>
        protected IList InternalCache
        {
            get { return _internalCache; }
            set { SetInternalCache(value); }
        }

        private async void SetInternalCache(IList value)
        {
            _internalCache = value;

            if (Items != null)
            {
                Items.UnNotifyOn("Count");
            }

            Items = await CreateItemsAsync();

            Items.NotifyOn("Count", (i, i1) =>
            {
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => FilteredCount);
            });

            NotifyOfPropertyChange(() => Count);
            NotifyOfPropertyChange(() => FilteredCount);
        }

        /// <summary>
        /// Gets the selected items count.
        /// </summary>
        /// <value>
        /// The selected items count.
        /// </value>
        public override int SelectedCount
        {
            get
            {
                if (CollectionView == null)
                {
                    return 0;
                }

                return CollectionView.Cast<ISelectable>().Count(x => x.IsSelected);
            }
        }

        /// <summary>
        /// Creates the list of paged items asynchronously.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<PagingItemListViewModelBase<TItem>> CreateItemsAsync();

        /// <summary>
        /// Updates the sort descriptors.
        /// </summary>
        protected abstract void UpdateSortDescriptors();

        /// <summary>
        /// Override this method to inject custom logic when an item is filtered.
        /// </summary>
        /// <param name="item">The item being filtered.</param>
        /// <returns></returns>
        protected abstract bool OnItemFilter(TItem item);

        /// <summary>
        /// Creates the collection view.
        /// </summary>
        /// <returns></returns>
        protected virtual ICollectionView CreateCollectionView()
        {
            var collectionView = CollectionViewSource.GetDefaultView(Items);
            collectionView.Filter += o => OnItemFilter((TItem)o);
            return collectionView;
        }

        private ICollectionView _collectionView;
        /// <summary>
        /// Gets the collection view.
        /// </summary>
        /// <value>
        /// The collection view.
        /// </value>
        protected ICollectionView CollectionView
        {
            get
            {
                if (_collectionView == null && Items != null)
                {
                    _collectionView = CreateCollectionView();
                }

                return _collectionView;
            }
        }

        #endregion
    }
}
