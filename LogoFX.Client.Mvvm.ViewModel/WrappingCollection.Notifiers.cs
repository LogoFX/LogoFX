using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using LogoFX.Client.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    public partial class WrappingCollection
    {
        readonly Dictionary<object, IndexedDictionary<object, object>> _dictionary = new Dictionary<object, IndexedDictionary<object, object>>();

        private NotifyCollectionChangedEventHandler _weakHandler = null;

#if !SILVERLIGHT && !WinRT
        private DispatcherPriority _updatePriority = Consts.DispatcherPriority;
        public DispatcherPriority UpdatePriority
        {
            get { return _updatePriority; }
            set { _updatePriority = value; }
        }
#endif

        private object GetWrapper(object list, object model)
        {
            if (!_dictionary.ContainsKey(list) || !_dictionary[list].ContainsKey(model))
                return null;
            return _dictionary[list][model];
        }
        private void PutWrapper(object list, object model, object wrapper)
        {
            if (!_dictionary.ContainsKey(list))
                _dictionary.Add(list, new IndexedDictionary<object, object>());
            _dictionary[list].Add(model,wrapper);
        }

        private void PutWrapperAt(object list, object o, object wrapper, int index)
        {
            if (!_dictionary.ContainsKey(list))
                _dictionary.Add(list, new IndexedDictionary<object, object>());
            _dictionary[list].AddAt(index,o,wrapper);            
        }

        private void RemoveWrapper(object list, object model)
        {
            if (!_dictionary.ContainsKey(list) || !_dictionary[list].ContainsKey(model))
                return;
            _dictionary[list].Remove(model);
        }

        private object GetWrapperAt(object list, int index)
        {

            if (!_dictionary.ContainsKey(list))            
                _dictionary.Add(list, new IndexedDictionary<object, object>());            
            return _dictionary[list].Count>=index+1?_dictionary[list][index]:null;
        }

        private Dictionary<object, object> GetListWrappers(object list)
        {
            if (!_dictionary.ContainsKey(list))
                return new Dictionary<object, object>();
            return _dictionary[list];
        }

        private void InvokeOnUiThread(Action a)
        {
#if SILVERLIGHT                        
            Dispatch.Current.BeginOnUiThread(
#elif WinRT
            Dispatch.Current.OnUiThread(
#else
            Dispatch.Current.BeginOnUiThread(UpdatePriority,
#endif
a);
        }

        private void AddList(IEnumerable i)
        {
            //make sure we catch collection changes
            IList<object> l;
            do
            {
                try
                {
                    l = i.Cast<object>().ToList();
                }
                catch
                {
                   continue;                     
                }
                break;
            } while (true);

            l.ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, -1)));

            if (i is INotifyCollectionChanged)
            {
                if (_weakHandler == null)
                {
                    _weakHandler = WeakDelegate.From(ListCollectionChanged);
                }

                ((INotifyCollectionChanged)i).CollectionChanged += _weakHandler;
            }
        }
        private void RemoveList(IEnumerable i)
        {
            //make sure we catch collection changes
            IList<object> l;
            do
            {
                try
                {
                    l = i.Cast<object>().ToList();
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            if (i is INotifyCollectionChanged && _weakHandler != null)
                ((INotifyCollectionChanged)i).CollectionChanged -= _weakHandler;

            l.ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, -1)));
        }
        private void SourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(AddList);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(RemoveList);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(RemoveList);
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(AddList);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("Clear() is not supported on Sources. Use Remove() instead.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Action<object> RemoveHandler = (a) =>
            {
                object o = GetWrapper(sender, a);
                RemoveWrapper(sender, a);
                _collectionManager.Remove(o);
                if (o is IDisposable)
                    ((IDisposable)o).Dispose();
            };
            Action<IEnumerable<object>> RemoveRangeHandler = collection =>
            {
                var wrappers = collection.Select(r => new Tuple<object, object>(GetWrapper(sender, r), r)).ToArray();
                wrappers.ForEach(a => RemoveWrapper(sender, a.Item2));
                _collectionManager.RemoveRange(wrappers.Select(t => t.Item1));
                wrappers.ForEach(o =>
                {
                    if (o is IDisposable)
                        ((IDisposable) o).Dispose();
                });
            };
            Action<object> AddHandler = (a) =>
            {
                object wrapper = CreateWrapper(a);
                PutWrapper(sender, a, wrapper);
                _collectionManager.Add(wrapper);
            };
            Action<IEnumerable<object>> AddRangeHandler = collection =>
            {
                var wrapperPairs = collection.Select(a => Tuple.Create(a, CreateWrapper(a))).ToArray();
                wrapperPairs.ForEach(r => PutWrapper(sender, r.Item1, r.Item2));
                _collectionManager.AddRange(wrapperPairs.Select(t => t.Item2));
            };
            Action<object, int> InsertHandler = (a, index) =>
            {
                object wrapper = CreateWrapper(a);
                object oldWrapper = GetWrapperAt(sender, index);
                PutWrapperAt(sender, a, wrapper, index);
                if(oldWrapper!=null)
                {
                    int oldIndex = _collectionManager.IndexOf(oldWrapper);
                    _collectionManager.Insert(oldIndex, wrapper);
                }
                else
                {
                    _collectionManager.Add(wrapper);
                }
            };
            Action<IEnumerable<object>, int> InsertRangeHandler = (collection, index) =>
            {
                var initialIndex = index;
                var wrappers = collection.Select(a => Tuple.Create(a, CreateWrapper(a), initialIndex++)).ToArray();                                
                var rangeWrappers = new List<object>();
                wrappers.ForEach(r =>
                {
                    var oldWrapper = GetWrapperAt(sender, r.Item3);
                    int oldIndex = _collectionManager.IndexOf(oldWrapper);
                    PutWrapperAt(sender, r.Item1, r.Item2, r.Item3);
                    if (oldWrapper != null && oldIndex >= 0)
                    {
                        _collectionManager.Insert(oldIndex, r.Item2);                      
                    }
                    else
                    {
                        rangeWrappers.Add(r.Item2);
                    }
                });
                if (rangeWrappers.Count > 0)
                {
                    _collectionManager.AddRange(rangeWrappers);
                }
            };
           
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:                    
                    if (e.NewStartingIndex == -1)
                    {
                        InvokeOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));                        
                    }
                    else
                    {
                        InvokeOnUiThread(() =>
                        {
                            int newindex = e.NewStartingIndex;
                            InsertRangeHandler(e.NewItems.Cast<object>(), newindex);
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    InvokeOnUiThread(() => RemoveRangeHandler(e.OldItems.Cast<object>()));
                    break;
                case NotifyCollectionChangedAction.Move:
                    InvokeOnUiThread(() =>
                    {
                        RemoveRangeHandler(e.OldItems.Cast<object>());

                        if (e.NewStartingIndex == -1)
                        {
                            InvokeOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));
                        }
                        else
                        {
                            InvokeOnUiThread(() =>
                            {
                                int newindex = e.NewStartingIndex;
                                InsertRangeHandler(e.NewItems.Cast<object>(), newindex);
                            });
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Replace:
                    InvokeOnUiThread(() =>
                    {
                        RemoveRangeHandler(e.OldItems.Cast<object>());

                        if (e.NewStartingIndex == -1)
                        {
                            InvokeOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));
                        }
                        else
                        {
                            InvokeOnUiThread(() =>
                            {
                                int newindex = e.NewStartingIndex;
                                InsertRangeHandler(e.NewItems.Cast<object>(), newindex);
                            });
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Dictionary<object, object> listwrappers = GetListWrappers(sender);

                    InvokeOnUiThread(() =>
                    {
                        if (listwrappers.Count > 0)
                        {
                            RemoveRangeHandler(listwrappers.Select(a => a.Key).ToList());
                            listwrappers.Clear();                                                    
                        }                        
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
