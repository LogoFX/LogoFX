using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LogoFX.Core
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public RangeObservableCollection()
        {
        }

        public RangeObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        private bool _suppressNotification;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                return;

            int initialindex = Count;

            var enumerable = list as T[] ?? list.ToArray();
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

        public void RemoveRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;            
            var enumerable = list as T[] ?? list.ToArray();
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (_suppressNotification == false)
            {
                base.OnPropertyChanged(e);
            }
        }
    }
}
