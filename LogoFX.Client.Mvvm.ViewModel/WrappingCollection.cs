using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents collection of view models which enables synchronization with its data source(s).
    /// </summary>
    public partial class WrappingCollection : IEnumerable, INotifyCollectionChanged, IHaveLoadingViewModel<IModelWrapper>, IDisposable
    {
        private readonly ObservableCollection<IEnumerable> _sources = new ObservableCollection<IEnumerable>();
        private readonly ICollectionManager _collectionManager;
        private Func<object, object> _factoryMethod;
        private Func<object,object> DefaultFactoryMethod =
            a => new { Model = a }
            ;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingCollection"/> class.
        /// </summary>
        public WrappingCollection()
            :this(isBulk:false)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingCollection"/> class.
        /// </summary>
        /// <param name="isBulk">if set to <c>true</c> [is bulk].</param>
        public WrappingCollection(bool isBulk = false)
        {
            _collectionManager = isBulk
                ? CollectionManagerFactory.CreateRangeManager() : CollectionManagerFactory.CreateRegularManager();
            _collectionManager.CollectionChangedSource.CollectionChanged += OnCollectionChangedCore;
            _sources.CollectionChanged += SourcesCollectionChanged;
        }

        private IModelWrapper _loadingViewModel;
        /// <summary>
        /// Gets or sets the view model which is displayed on loading the collection.
        /// </summary>
        /// <value>
        /// The loading view model.
        /// </value>
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

        /// <summary>
        /// Gets the collection of data sources.
        /// </summary>
        public IEnumerable<IEnumerable> Sources
        {
            get { return _sources; }
        }

        /// <summary>
        /// Adds data source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Removes the source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public bool RemoveSource(IEnumerable source)
        {
            if (_sources.Contains(source))
            {
                _sources.Remove(source);
                return true;
            }
            return false;
            
        }

        /// <summary>
        /// Clears the data sources.
        /// </summary>
        public void ClearSources()
        {
            HashSet<IEnumerable> hs = new HashSet<IEnumerable>(_sources);
            hs.ForEach(a => _sources.Remove(a));
        }        

        /// <summary>
        /// Gets or sets the factory method which is used to create view model from a data source item.
        /// </summary>
        public Func<object,object> FactoryMethod
        {
            get { return _factoryMethod; }
            set { _factoryMethod = value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return _collectionManager.GetEnumerator();
        }

        /// <summary>
        /// Creates model wrapper using the specified model.
        /// </summary>
        /// <param name="obj">The specified model.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Override this method to inject custom logic on collection change.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collectionManager.CollectionChangedSource.CollectionChanged += value; }
            remove { _collectionManager.CollectionChangedSource.CollectionChanged -= value; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _collectionManager.CollectionChangedSource.CollectionChanged -= OnCollectionChangedCore;            

            Sources
                .OfType<INotifyCollectionChanged>()
                .ForEach(ml => ml.CollectionChanged -= _weakHandler);
        } 
    }
}
