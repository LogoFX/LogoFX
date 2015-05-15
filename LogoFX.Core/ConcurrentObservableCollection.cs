using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogoFX.Core
{
    public class ConcurrentObservableCollection<T> : IList<T>, INotifyCollectionChanged 
    {        
        private readonly List<T> _items;
        private readonly ReaderWriterLockSlim _lock;

        public ConcurrentObservableCollection(IEnumerable<T> items)
            : this()
        {
            _items.AddRange(items);
        }

        public ConcurrentObservableCollection()
        {
            _items = new List<T>();
            _lock = new ReaderWriterLockSlim();
        }


        public int MaxSizeOfWaitingQueue { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            IList<T> array;
            EnterReadLock();
            try
            {
                array = _items.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            EnterWriteLock();
            try
            {
                _items.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            EnterWriteLock();
            try
            {
                _items.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            bool found;
            EnterReadLock();
            try
            {
                found = _items.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return found;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] arr;
            EnterReadLock();
            try
            {
                arr = _items.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
            foreach (T i in arr)
            {
                array.SetValue(i, arrayIndex);
                arrayIndex = arrayIndex + 1;
            }
        }

        public bool Remove(T item)
        {
            EnterWriteLock();
            try
            {
                if (!_items.Contains(item))
                {
                    return false;
                }
                _items.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return true;
        }

        public int Count
        {
            get { return _items.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            int indexOf;
            EnterReadLock();
            try
            {
                indexOf = _items.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return indexOf;
        }

        public void Insert(int index, T item)
        {
            EnterWriteLock();
            try
            {
                _items.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                                                                        new[] { item },
                                                                        index));
        }

        public void RemoveAt(int index)
        {
            T item;
            EnterWriteLock();
            try
            {
                item = _items[index];
                _items.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                                                                        new[] { item },
                                                                        index));
        }

        public T this[int index]
        {
            get
            {
                T item;
                EnterReadLock();
                try
                {
                    item = _items[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
                return item;
            }
            set
            {
                T old;
                EnterWriteLock();
                try
                {
                    old = _items[index];
                    _items[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                                                                            new[] { value },
                                                                            new[] { old },
                                                                            index));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void AddRange(IEnumerable<T> range)
        {
            var add = range == null ? null : range.ToArray();
            if (add == null || add.Length == 0)
            {
                return;
            }

            EnterWriteLock();

            try
            {
                _items.AddRange(add);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, add.ToList()));
        }

        public void RemoveRange(IEnumerable<T> range)
        {
            //'range' might be a Linq query of kind that is actualy realized on first access. If i don't unfold this query here, then it will
            //be unfolded inside write lock on call to ForEach. This will cause exception if 'range' is Linq query on this very same 
            //ConcurrentObservableCollection. The exception will happen because call to ForEach will try to acquire Read lock. But we already got
            //Write lock in the same thread (call to EnterWriteLock already happened). When you try to get Read lock over Write lock in same
            //thread, then ReadWriteSlimLock fires exception.

            T[] rangeArray = range.ToArray();
            if (rangeArray.Length == 0)
            {
                return;
            }

            EnterWriteLock();

            try
            {
                rangeArray.ForEach(i => _items.Remove(i));
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, rangeArray.ToList()));
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (_items.Count > 0)
            {
                _items.ForEach(i => builder.Append(i.ToString() + ", "));
                builder.Remove(builder.Length - 2, 2);
            }

            return builder.ToString();
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler temp = CollectionChanged;
            if (temp != null)
            {
                temp(this, args);
            }
        }

        private void EnterWriteLock()
        {
            if (_lock.IsReadLockHeld)
            {                
                throw new Exception("Read lock was already acquired by the same thread");
            }
            _lock.EnterWriteLock();
        }

        private void EnterReadLock()
        {
            while (_lock.IsWriteLockHeld)
            {                
                throw new Exception("Write lock was already acquired by the same thread.");
            }
            _lock.EnterReadLock();
        }
    }
}
