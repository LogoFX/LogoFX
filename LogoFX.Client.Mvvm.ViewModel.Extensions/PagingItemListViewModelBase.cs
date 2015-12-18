using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Caliburn.Micro;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
   public abstract class PagingItemListViewModelBase<TItem> : PropertyChangedBase, IList<TItem>, IList, INotifyCollectionChanged
    {
        #region Fields

        private readonly object _sync = new object();

        #endregion

        #region Protected

        protected abstract int GetCount();

        protected abstract TItem Get(int index);

        protected abstract int IndexOf(object value);

        protected abstract IEnumerator<TItem> GetEnumerator();

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

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion        
    }
}