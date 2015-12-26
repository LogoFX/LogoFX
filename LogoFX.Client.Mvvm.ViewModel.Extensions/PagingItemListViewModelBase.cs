using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents base class for paging list manager.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public abstract class PagingItemListViewModelBase<TItem> : PropertyChangedBase, IList<TItem>, IList, INotifyCollectionChanged
    {
        #region Fields

        private readonly object _sync = new object();

        #endregion

        #region Protected

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetCount();

        /// <summary>
        /// Gets the item at specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        protected abstract TItem Get(int index);

        /// <summary>
        /// Gets the index of specified item.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected abstract int IndexOf(object value);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        protected abstract IEnumerator<TItem> GetEnumerator();

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler handler;

            lock (this)
            {
                handler = CollectionChanged;
            }

            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the cached items.
        /// </summary>
        /// <value>
        /// The cached items.
        /// </value>
        public abstract IEnumerable<TItem> CachedItems { get; }

        #endregion

        #region Enumerable<TItem>

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection<TItem>

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return GetCount(); }
        }

        bool ICollection<TItem>.Remove(TItem item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<TItem>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<TItem>.Add(TItem item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TItem>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<TItem>.Contains(TItem item)
        {
            throw new NotImplementedException();
        }

        void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        object ICollection.SyncRoot
        {
            get { return _sync; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        #endregion

        #region IList<TItem>

        int IList<TItem>.IndexOf(TItem item)
        {
            return IndexOf(item);
        }

        void IList<TItem>.Insert(int index, TItem item)
        {
            throw new NotImplementedException();
        }

        void IList<TItem>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public TItem this[int index]
        {
            get { return Get(index); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IList

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get { return Get(index); }
            set { throw new NotImplementedException(); }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        #endregion

        #region INotifyCollectionChanged

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion        
    }
}