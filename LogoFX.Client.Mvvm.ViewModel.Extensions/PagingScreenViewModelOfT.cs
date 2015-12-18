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
    public abstract partial class PagingScreenViewModel<TItem, TModel> : PagingScreenViewModel
        where TModel : class
        where TItem : class, IPagingItemViewModel
    {
        #region Fields

        private ICollectionView _collectionView;
        private PagingItemListViewModelBase<TItem> _items;

        private IList _internalCache;

        private int _totalPages;
        private int _currentPage;
        private double _pageWidth;
        private double _pageLeft;

        private int _oldPageCount;
        private int? _oldCurrentPage;

        private ICommand _clearAllSelectionCommand;
        private ICommand _selectAllItemsCommand;
        private ICommand _revertAllSelectionCommand;

        #endregion

        #region Commands

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

        public int StartIndex
        {
            get { return CurrentPage * PageCapacity; }
        }

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

        protected virtual void OnSelectAll()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = true);
        }

        protected virtual void OnRevertAllSelection()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = !item.IsSelected);
        }

        protected virtual void OnClearAllSelection()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.OfType<TItem>().ForEach(item => item.IsSelected = false);
        }

        protected virtual void OnRefresh()
        {

        }

        protected virtual void RefreshData()
        {
            if (CollectionView == null)
            {
                return;
            }

            CollectionView.Refresh();
            OnRefresh();
        }

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

            Items = await CreateItems();

            Items.NotifyOn("Count", (i, i1) =>
            {
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => FilteredCount);
            });

            NotifyOfPropertyChange(() => Count);
            NotifyOfPropertyChange(() => FilteredCount);
        }

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

        protected abstract Task<PagingItemListViewModelBase<TItem>> CreateItems();

        protected abstract void UpdateSortDescriptors();

        protected abstract bool OnItemFilter(TItem item);

        protected virtual ICollectionView CreateCollectionView()
        {
            var collectionView = CollectionViewSource.GetDefaultView(Items);
            collectionView.Filter += o => OnItemFilter((TItem)o);
            return collectionView;
        }

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
