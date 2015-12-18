using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class PagingItemListViewModel<TItem, TModel> : PagingItemListViewModelBase<TItem>
        where TItem : class, IPagingItemViewModel
        where TModel : class
    {
        #region Fields

        private readonly Dictionary<TModel, TItem> _cache =
            new Dictionary<TModel, TItem>();

        private readonly IList<TModel> _source;

        #endregion

        #region Constructors

        protected PagingItemListViewModel(object parent, IList<TModel> source)
        {
            _source = source;
            Parent = parent;
            var notify = source as INotifyCollectionChanged;
            if (notify != null)
            {
                notify.CollectionChanged += SourceCollectionChanged;
            }
        }

        #endregion

        #region Public Properties

        public object Parent { get; private set; }

        public override IEnumerable<TItem> CachedItems
        {
            get { return _cache.Values; }
        }

        #endregion

        #region Protected

        protected abstract TItem CreateItem(TModel model);

        protected virtual void OnCreated(TItem item)
        {

        }

        protected object SyncObject
        {
            get { return _cache; }
        }

        protected TItem GetItem(TModel model)
        {
            TItem item;
            lock (SyncObject)
            {
                if (!_cache.TryGetValue(model, out item))
                {
                    item = CreateItem(model);
                    _cache.Add(model, item);
                    OnCreated(item);
                }
            }
            return item;
        }

        #endregion

        #region Private Members

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            NotifyCollectionChangedEventArgs newArgs;

            IList newItems = null;
            if (args.NewItems != null)
            {
                newItems = args.NewItems.Cast<TModel>().Select(GetItem).ToList();
            }

            IList oldItems = null;
            if (args.OldItems != null)
            {
                oldItems = args.OldItems.Cast<TModel>().Select(GetItem).ToList();
            }

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    newArgs = new NotifyCollectionChangedEventArgs(args.Action, newItems, args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    newArgs = new NotifyCollectionChangedEventArgs(args.Action, oldItems, args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Debug.Assert(newItems != null, "newItems != null");
                    Debug.Assert(oldItems != null, "oldItems != null");
                    newArgs = new NotifyCollectionChangedEventArgs(args.Action, newItems, oldItems, args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    newArgs = new NotifyCollectionChangedEventArgs(args.Action, newItems, args.NewStartingIndex, args.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    newArgs = new NotifyCollectionChangedEventArgs(args.Action, oldItems, args.NewStartingIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnCollectionChanged(newArgs);
            NotifyOfPropertyChange(() => Count);
        }

        #endregion

        #region Overrides

        protected override IEnumerator<TItem> GetEnumerator()
        {
            foreach (TModel model in _source)
            {
                yield return GetItem(model);
            }
        }

        protected override int GetCount()
        {
            return _source.Count;
        }

        protected override TItem Get(int index)
        {
            return GetItem(_source[index]);
        }

        protected override int IndexOf(object value)
        {
            return _source.IndexOf(value as TModel);
        }

        #endregion
    }
}