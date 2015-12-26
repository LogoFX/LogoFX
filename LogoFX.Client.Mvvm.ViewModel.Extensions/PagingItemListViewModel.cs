using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents paging list manager for specific model.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract partial class PagingItemListViewModel<TItem, TModel> : PagingItemListViewModelBase<TItem>
        where TItem : class, IPagingItem
        where TModel : class
    {
        #region Fields

        private readonly Dictionary<TModel, TItem> _cache =
            new Dictionary<TModel, TItem>();

        private readonly IList<TModel> _source;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingItemListViewModel{TItem, TModel}"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="source">The source.</param>
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

        #region Public Properties

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public object Parent { get; private set; }

        /// <summary>
        /// Gets the cached items.
        /// </summary>
        /// <value>
        /// The cached items.
        /// </value>
        public override IEnumerable<TItem> CachedItems
        {
            get { return _cache.Values; }
        }

        #endregion

        #region Protected

        /// <summary>
        /// Creates the item from the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected abstract TItem CreateItem(TModel model);

        /// <summary>
        /// Override this method to inject custom logic on item creation.
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void OnCreated(TItem item)
        {

        }

        /// <summary>
        /// Gets the synchronize object.
        /// </summary>
        /// <value>
        /// The synchronize object.
        /// </value>
        protected object SyncObject
        {
            get { return _cache; }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected TItem GetItem(TModel model)
        {
            TItem item;
            //TODO: Don't use the data as the synchronization object. Don't do this. Ever.
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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        protected override IEnumerator<TItem> GetEnumerator()
        {
            foreach (TModel model in _source)
            {
                yield return GetItem(model);
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        protected override int GetCount()
        {
            return _source.Count;
        }

        /// <summary>
        /// Gets the item at specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected override TItem Get(int index)
        {
            return GetItem(_source[index]);
        }

        /// <summary>
        /// Gets the index of specified item.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override int IndexOf(object value)
        {
            return _source.IndexOf(value as TModel);
        }

        #endregion
    }
}