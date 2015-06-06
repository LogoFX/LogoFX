using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using LogoFX.Core;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels
{
#if !WinRT && !SILVERLIGHT
#endif

    public partial class WrappingCollection:IEnumerable,INotifyCollectionChanged,IDisposable
    {
        private readonly ObservableCollection<IEnumerable> _sources = new ObservableCollection<IEnumerable>();
        private readonly ObservableCollection<object> _items = new ObservableCollection<object>();
        private Func<object, object> _factoryMethod;
        private Func<object,object> DefaultFactoryMethod =
            a => new { Model = a }
            ; 

        private IModelWrapper _loadingViewModel;


        public WrappingCollection(IEnumerable source):this()
        {
            if(source == null)
                throw new ArgumentNullException("source");
            _sources.Add(source);
        }

        public WrappingCollection()
        {
            _items.CollectionChanged += OnCollectionChangedCore;
            _sources.CollectionChanged += SourcesCollectionChanged;
        }

        public IModelWrapper LoadingViewModel
        {
            get { return _loadingViewModel; }
            set
            {

                if (value != null && InternalChildren.Count == 0)
                {
                    InternalChildren.Add(value);
                }
                else if (_loadingViewModel != null)
                {
                    InternalChildren.Remove(_loadingViewModel);
                }
                _loadingViewModel = value;
            }
        }

        public IEnumerable<IEnumerable> Sources
        {
            get { return _sources; }
        }

        public bool AddSource(IEnumerable source)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            if (!_sources.Contains(source))
            {
                _sources.Add(source);
                return true;
            }
            return false;
        }

        public bool RemoveSource(IEnumerable source)
        {
            if (_sources.Contains(source))
            {
                _sources.Remove(source);
                return true;
            }
            return false;
            
        }

        public void ClearSources()
        {
            HashSet<IEnumerable> hs = new HashSet<IEnumerable>(_sources);
            hs.ForEach(a => _sources.Remove(a));
        }

        internal ObservableCollection<object> InternalChildren
        {
            get
            {
                return _items;
            }
        }

        public Func<object,object> FactoryMethod
        {
            get { return _factoryMethod; }
            set { _factoryMethod = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return InternalChildren.GetEnumerator();
        }

        protected virtual object CreateWrapper(object obj)
        {
            return FactoryMethod != null ? FactoryMethod(obj) : DefaultFactoryMethod(obj);
        }

        private void OnCollectionChangedCore(object o,NotifyCollectionChangedEventArgs args)
        {
            if (args.Action != NotifyCollectionChangedAction.Reset && _loadingViewModel != null)
            {
                InternalChildren.Remove(_loadingViewModel);
            }

            try
            {
                OnCollectionChanged(args);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { InternalChildren.CollectionChanged += value; }
            remove { InternalChildren.CollectionChanged -= value; }
        }

        public void Dispose()
        {
            _items.CollectionChanged -= OnCollectionChangedCore;            

            Sources
                .OfType<INotifyCollectionChanged>()
                .ForEach(ml => ml.CollectionChanged -= _weakHandler);
        } 

    }
}
