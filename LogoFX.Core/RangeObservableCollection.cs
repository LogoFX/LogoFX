using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LogoFX.Core
{
    /// <summary>
    /// Observable collection that allows performing addition and removal
    /// of collection of items as single operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    public class RangeObservableCollection<T> : ObservableCollection<T>, IRangeCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeObservableCollection{T}"/> class.
        /// </summary>
        public RangeObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public RangeObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        private bool _suppressNotification;

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Adds range of items as single operation.
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<T> range)
        {
            if (range == null)
                return;

            int initialindex = Count;

            var enumerable = range as T[] ?? range.ToArray();
            if (!enumerable.Any())
                return;

            _suppressNotification = true;
           
            foreach (var item in enumerable)
            {
                Add(item);
            }
           
            _suppressNotification = false;
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                                                                     new List<T>(enumerable)));
        }

        /// <summary>
        /// Removes the range of items as single operation.
        /// </summary>
        /// <param name="range">The range.</param>
        public void RemoveRange(IEnumerable<T> range)
        {
            if (range == null)
                throw new ArgumentNullException("range");

            _suppressNotification = true;            
            var enumerable = range as T[] ?? range.ToArray();
            var count = enumerable.Length;
            var index = -1;
            T singleItem = default(T);
            if (count == 1)
            {
                singleItem = enumerable[0];
                index = IndexOf(singleItem);

            }
            foreach (var item in enumerable)
            {
                Remove(item);
            }            
            _suppressNotification = false;
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(count == 1
                ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, singleItem,
                    index)
                : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.PropertyChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (_suppressNotification == false)
            {
                base.OnPropertyChanged(e);
            }
        }
    }
}
