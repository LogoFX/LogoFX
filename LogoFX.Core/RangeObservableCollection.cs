using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                                                                     new List<T>(enumerable), initialindex));
        }

        public void RemoveRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;            
            var enumerable = list as T[] ?? list.ToArray();
            foreach (var item in enumerable)
            {
                Remove(item);
            }            
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(enumerable),0));
        }
    }
}
