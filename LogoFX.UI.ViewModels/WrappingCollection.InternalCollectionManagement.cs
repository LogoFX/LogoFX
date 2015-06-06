using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LogoFX.Core;

namespace LogoFX.UI.ViewModels
{
    partial class WrappingCollection
    {
        internal class CollectionManagerFactory
        {
            ICollectionManager CreateRegularCollectionManager()
            {
                return new RegularCollectionManager();
            }

            ICollectionManager CreateRangeCollectionManager()
            {
                return new RangeCollectionManager();
            }            
        }

        internal interface ICollectionManager
        {
            INotifyCollectionChanged CollectionChangedSource { get; }
            IEnumerator GetEnumerator();
            void Add(object item);
            void AddRange(IEnumerable<object> items);
            void Remove(object item);
            void RemoveRange(IEnumerable<object> items);
        }

        private class RegularCollectionManager : ICollectionManager
        {
            private readonly ObservableCollection<object> _items = new ObservableCollection<object>();
            public INotifyCollectionChanged CollectionChangedSource
            {
                get { return _items; }
            }

            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            public void AddRange(IEnumerable<object> items)
            {
                items.ForEach(_items.Add);
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void RemoveRange(IEnumerable<object> items)
            {
                items.ForEach(r => _items.Remove(r));
            }
        }

        private class RangeCollectionManager : ICollectionManager
        {
            private readonly RangeObservableCollection<object> _items = new RangeObservableCollection<object>();
            public INotifyCollectionChanged CollectionChangedSource
            {
                get { return _items; }
            }

            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            public void AddRange(IEnumerable<object> items)
            {
                _items.AddRange(items);
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void RemoveRange(IEnumerable<object> items)
            {
                _items.RemoveRange(items);
            }
        }
    }
}
