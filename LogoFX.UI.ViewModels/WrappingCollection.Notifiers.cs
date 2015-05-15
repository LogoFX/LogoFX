﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using LogoFX.Core;

namespace LogoFX.UI.ViewModels
{
    public partial class WrappingCollection
    {
        readonly Dictionary<object, IndexedDictionary<object, object>> _dictionary = new Dictionary<object, IndexedDictionary<object, object>>();

        private NotifyCollectionChangedEventHandler _weakHandler = null;

#if !SILVERLIGHT && !WinRT
        private DispatcherPriority _updatePriority = DispatcherPriority.DataBind;
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
                InternalChildren.Remove(o);
                if (o is IDisposable)
                    ((IDisposable)o).Dispose();
            };
            Action<object> AddHandler = (a) =>
            {
                object wrapper = CreateWrapper(a);
                PutWrapper(sender, a, wrapper);
                InternalChildren.Add(wrapper);
            };
            Action<object, int> InsertHandler = (a, index) =>
            {
                object wrapper = CreateWrapper(a);
                object oldWrapper = GetWrapperAt(sender, index);
                PutWrapperAt(sender, a, wrapper, index);
                if(oldWrapper!=null)
                {
                    int oldIndex = InternalChildren.IndexOf(oldWrapper);
                    InternalChildren.Insert(oldIndex, wrapper);
                }
                else
                {
                    InternalChildren.Add(wrapper);
                }
            };

           
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:                    
                    if (e.NewStartingIndex == -1)
                    {
                        InvokeOnUiThread(() => e.NewItems.Cast<object>().ForEach(AddHandler));                        
                    }
                    else
                    {
                        InvokeOnUiThread(() =>
                        {
                            int newindex = e.NewStartingIndex;
                            e.NewItems.Cast<object>().ForEach((a) =>
                            {
                                if (GetWrapper(sender, a) != null)
                                    //already done by other party
                                    return;
                                InsertHandler(a, newindex++);
                            });
                        });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:

                    InvokeOnUiThread(() => e.OldItems.Cast<object>().ForEach(RemoveHandler));
                    break;

                case NotifyCollectionChangedAction.Replace:

                    InvokeOnUiThread(() =>
                    {
                        e.OldItems.Cast<object>().ForEach(RemoveHandler);

                        if (e.NewStartingIndex == -1)
                        {
                            InvokeOnUiThread(() => e.NewItems.Cast<object>().ForEach(AddHandler));
                        }
                        else
                        {
                            InvokeOnUiThread(() =>
                            {
                                int newindex = e.NewStartingIndex;
                                e.NewItems.Cast<object>().ForEach((a) =>
                                {
                                    if (GetWrapper(sender, a) != null)
                                        //already done by other party
                                        return;
                                    InsertHandler(a, newindex++);
                                });
                            });
                        }
                    });

                    break;
                case NotifyCollectionChangedAction.Reset:

                    Dictionary<object, object> listwrappers = GetListWrappers(sender);

                    InvokeOnUiThread(() =>
                    {
                        listwrappers.Select(a=>a.Key).ToList().ForEach(RemoveHandler);                        
                        listwrappers.Clear();                                                
                    });

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}