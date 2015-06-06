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
        private readonly ICollectionManager _collectionManager = CollectionManagerFactory.CreateRegularManager();
        private Func<object, object> _factoryMethod;
        private Func<object,object> DefaultFactoryMethod =
            a => new { Model = a }
            ; 

        private IModelWrapper _loadingViewModel;

        public WrappingCollection(bool isBulk = false) : this()
        {
            _collectionManager = isBulk
                ? CollectionManagerFactory.CreateRegularManager() : CollectionManagerFactory.CreateRangeManager();
        }

        public WrappingCollection(IEnumerable source):this()
        {
            if(source == null)
                throw new ArgumentNullException("source");
            _sources.Add(source);
        }

        public WrappingCollection()
        {
            _collectionManager.CollectionChangedSource.CollectionChanged += OnCollectionChangedCore;
            _sources.CollectionChanged += SourcesCollectionChanged;
        }

        public IModelWrapper LoadingViewModel
        {
            get { return _loadingViewModel; }
            set
            {

                if (value != null && _collectionManager.ItemsCount == 0)
                {
                    _collectionManager.Add(value);
                }
                else if (_loadingViewModel != null)
                {
                    _collectionManager.Remove(_loadingViewModel);
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

        public Func<object,object> FactoryMethod
        {
            get { return _factoryMethod; }
            set { _factoryMethod = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _collectionManager.GetEnumerator();
        }

        protected virtual object CreateWrapper(object obj)
        {
            return FactoryMethod != null ? FactoryMethod(obj) : DefaultFactoryMethod(obj);
        }

        private void OnCollectionChangedCore(object o,NotifyCollectionChangedEventArgs args)
        {
            if (args.Action != NotifyCollectionChangedAction.Reset && _loadingViewModel != null)
            {
                _collectionManager.Remove(_loadingViewModel);
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
            add { _collectionManager.CollectionChangedSource.CollectionChanged += value; }
            remove { _collectionManager.CollectionChangedSource.CollectionChanged -= value; }
        }

        public void Dispose()
        {
            _collectionManager.CollectionChangedSource.CollectionChanged -= OnCollectionChangedCore;            

            Sources
                .OfType<INotifyCollectionChanged>()
                .ForEach(ml => ml.CollectionChanged -= _weakHandler);
        } 

    }
}
