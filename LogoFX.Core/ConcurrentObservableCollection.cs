using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogoFX.Core
{
    /// <summary>
    /// Observable collection that allows concurrent operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    /// <seealso cref="INotifyCollectionChanged" />
    public class ConcurrentObservableCollection<T> : IList<T>, INotifyCollectionChanged, IRangeCollection<T>
    {        
        private readonly List<T> _items;
        private readonly ReaderWriterLockSlim _lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public ConcurrentObservableCollection(IEnumerable<T> items)
            : this()
        {
            _items.AddRange(items);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentObservableCollection{T}"/> class.
        /// </summary>
        public ConcurrentObservableCollection()
        {
            _items = new List<T>();
            _lock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Gets or sets the maximum size of waiting queue.
        /// </summary>
        /// <value>
        /// The maximum size of waiting queue.
        /// </value>
        public int MaxSizeOfWaitingQueue { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
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

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
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

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
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

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
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

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
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

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
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

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
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

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
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

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
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

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
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

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Adds range of items as single operation.
        /// </summary>
        /// <param name="range"></param>
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

        /// <summary>
        /// Removes the range of items as single operation.
        /// </summary>
        /// <param name="range">The range.</param>
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

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
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
