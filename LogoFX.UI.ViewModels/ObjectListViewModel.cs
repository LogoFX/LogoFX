// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>Monday, November 22, 2010</lastedit>

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the 'Software'), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//
// <remarks>Part of this software based on various internet sources, mostly on the works
// of members of Wpf Disciples group http://wpfdisciples.wordpress.com/
// Also project may contain code from the frameworks: 
//        Nito 
//        OpenLightGroup
//        nRoute
// </remarks>
// ====================================================================================//


#if WinRT
using Windows.UI.Core;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using LogoFX.Core;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels
{

    /// <summary>
    /// Operation type
    /// </summary>
    public enum ChildOperation
    {
        /// <summary>
        /// Add operation
        /// </summary>
        Add,
        /// <summary>
        /// Remove operation
        /// </summary>
        Remove
    }

    /// <summary>
    /// Child Operation Event Arguments
    /// </summary>
    /// <typeparam name="T"> type of child</typeparam>
    public class ChildEventArgs<T> : EventArgs
    {
        private readonly ChildOperation _operation;
        private readonly T _item;
        private readonly int _index;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="item"></param>
        /// <param name="index"></param>
        public ChildEventArgs(ChildOperation operation, T item, int index)
        {
            _operation = operation;
            _item = item;
            _index = index;
        }
        /// <summary>
        /// Added item
        /// </summary>
        public T Item { get { return _item; } }
        /// <summary>
        /// Changed index
        /// </summary>
        public int Index { get { return _index; } }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public ChildOperation Action
        {
            get { return _operation; }
        }
    }

    /// <summary>
    /// Base class for any <c>ViewModels</c> that are wrapping some objects list
    /// </summary>
    public class ObjectsListViewModel : ObjectViewModel, IHierarchicalViewModel, IObjectViewModelFactory
    {
        #region Members
        private ObservableCollection<IEnumerable> _modelLists;
        private readonly ObservableCollection<IEnumerable> _modelListsLazy = new ObservableCollection<IEnumerable>();
        private readonly ObservableCollection<object> _internalList = new ObservableCollection<object>();
        private readonly IObjectViewModelFactory _modelfactory;
        private readonly ObservableViewModelsCollection<IObjectViewModel> _children = new ObservableViewModelsCollection<IObjectViewModel>();
        private IObjectViewModel _loadingViewModel;

#if !SILVERLIGHT && !WinRT
        private DispatcherPriority _updatePriority = DispatcherPriority.DataBind;
#endif

        /// <summary>
        /// Occurs when content of children collection is changed.
        /// </summary>
        public event EventHandler<ChildEventArgs<IObjectViewModel>> ChildrenChanged = null;
        #endregion

        #region ctor's
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsListViewModel"/> class.
        /// </summary>
        /// <param name="model">The head model.</param>
        public ObjectsListViewModel(object model)
            : this(model, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsListViewModel"/> class.
        /// </summary>
        /// <param name="model">The head model.</param>
        /// <param name="models">The child models.</param>
        public ObjectsListViewModel(object model, IEnumerable models)
            : this(model, models, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsListViewModel"/> class.
        /// </summary>
        /// <param name="model">The head model.</param>
        /// <param name="viewModelFactory">The factory for creating child <c>ViewModels</c>.</param>
        public ObjectsListViewModel(object model, IObjectViewModelFactory viewModelFactory)
            : this(model, null, viewModelFactory)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectsListViewModel"/> class.
        /// </summary>
        /// <param name="model">The head model.</param>
        /// <param name="models">The child models.</param>
        /// <param name="viewModelFactory">The factory for creating child <c>ViewModels</c>.</param>
        public ObjectsListViewModel(object model, IEnumerable models, IObjectViewModelFactory viewModelFactory)
            : base(model)
        {
            _modelfactory = viewModelFactory ?? this;


            if (models != null)
                _modelListsLazy.Add(models);
            _modelListsLazy.Add(_internalList);

        }
        #endregion

        public IObjectViewModel LoadingViewModel
        {
            get { return _loadingViewModel; }
            set
            {
                
                if (value != null && InternalChildren.Count == 0)
                {
                    InternalChildren.Add(value);
                }
                else if(_loadingViewModel!=null)
                {
                    InternalChildren.Remove(_loadingViewModel);
                }
                _loadingViewModel = value;
            }
        }

        #region Private
        private IObjectViewModel CreateViewModel(object model)
        {
            return _modelfactory.CreateViewModel(this, model);
        }
        #endregion

        #region Lists management
        private void ModelListsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(i =>
                        {
                            i.OfType<object>().ToList().ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, 0)));
                            if (i is INotifyCollectionChanged)
								((INotifyCollectionChanged)i).CollectionChanged += WeakDelegate.From(ListCollectionChanged);

                        }
                        );
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(i =>
                        {
                            if (i is INotifyCollectionChanged)
                                ((INotifyCollectionChanged)i).CollectionChanged -= WeakDelegate.From(ListCollectionChanged);
                            i.OfType<object>().ToList().ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0)));
                        }
                        );
                    break;
                case NotifyCollectionChangedAction.Replace:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(i =>
                        {
                            if (i is INotifyCollectionChanged)
                                ((INotifyCollectionChanged)i).CollectionChanged -= WeakDelegate.From(ListCollectionChanged);
                            i.OfType<object>().ToList().ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0)));
                        }
                        );
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(i =>
                        {
                            i.OfType<object>().ToList().ForEach(item => ListCollectionChanged(i, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, 0)));
                            if (i is INotifyCollectionChanged)
                                ((INotifyCollectionChanged)i).CollectionChanged += WeakDelegate.From(ListCollectionChanged);

                        }
                        );
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

#if !SILVERLIGHT && !WinRT
        private 
#else
        public
#endif
            void ListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach (object item in e.NewItems)
                    {
#if SILVERLIGHT                        
                        Dispatch.Current.BeginOnUiThread(
#elif WinRT
                        Dispatch.Current.OnUiThread(
#else
                        Dispatch.Current.BeginOnUiThread(_updatePriority,
#endif
                        () =>
                        {
                             var viewmodel = CreateViewModel(item);
                             InternalChildren.Add(viewmodel);
                             OnChildrenChangedInternal(
                                 new ChildEventArgs<IObjectViewModel>(ChildOperation.Add, viewmodel, InternalChildren.IndexOf(viewmodel)));
                        });                        
                    }


                    break;

                case NotifyCollectionChangedAction.Remove:
                    HashSet<IObjectViewModel> hss;


#if SILVERLIGHT                        
                    Dispatch.Current.BeginOnUiThread(
#elif WinRT
                    Dispatch.Current.OnUiThread(
#else
                    Dispatch.Current.BeginOnUiThread(_updatePriority,
#endif
() =>
                    {
                        hss = new HashSet<IObjectViewModel>(Children);

                        if (hss == null)
                            return;

                        IList<object> oldItems = e.OldItems
                            .Cast<object>()
                            .ToList();

                        IEnumerable<IObjectViewModel> itemsToRemove = hss.Where(a => oldItems.Contains(a.Model));
                        foreach (var item in itemsToRemove)
                        {
                            InternalChildren.Remove(item);
                            OnChildrenChangedInternal(new ChildEventArgs<IObjectViewModel>(ChildOperation.Remove, item, -1));
                            item.Dispose();
                        }
                    });

                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException("Replace is not supported");

                case NotifyCollectionChangedAction.Reset:
                    var newResetList = Models.Select(CreateViewModel);
#if SILVERLIGHT                        
                    Dispatch.Current.BeginOnUiThread(
#elif WinRT
                    Dispatch.Current.OnUiThread(
#else
                    Dispatch.Current.BeginOnUiThread(_updatePriority,
#endif
() =>
                    {
                       
                        HashSet<IObjectViewModel> hs = new HashSet<IObjectViewModel>(Children);                        
                        foreach (IObjectViewModel c in hs)
                        {
                            InternalChildren.Remove(c);
                            OnChildrenChangedInternal(new ChildEventArgs<IObjectViewModel>(ChildOperation.Remove, c, -1));
                            c.Dispose();                            
                        }
                        foreach (IObjectViewModel a in newResetList)
                        {
                            InternalChildren.Add(a);
                            OnChildrenChangedInternal(new ChildEventArgs<IObjectViewModel>(ChildOperation.Add, a, InternalChildren.Count-1));                            
                        }
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnChildrenChangedInternal(ChildEventArgs<IObjectViewModel> args)
        {
            try
            {
                if(args.Action==ChildOperation.Add && _loadingViewModel != null)
                {
                    InternalChildren.Remove(_loadingViewModel);
                }
                OnChildrenChanged(args);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
                
            }

            try
            {
                if (ChildrenChanged != null)
                    ChildrenChanged(this, args);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
            }

        }
        #endregion

        #region Public properties

#if !SILVERLIGHT && !WinRT
        /// <summary>
        /// Gets or sets the update priority.
        /// </summary>
        /// <value>The update priority.</value>
        public DispatcherPriority UpdatePriority
        {
            get { return _updatePriority; }
            set { _updatePriority = value; }
        }

#endif

        /// <summary>
        /// Gets the child <c>ViewModels</c> list.
        /// </summary>
        /// <value>The children.</value>
        public virtual IViewModelsCollection<IObjectViewModel> Children
        {
            get
            {

                //lazy init))
                if (_modelLists == null)
                {
                    IsBusy = true;
                    _modelLists = new ObservableCollection<IEnumerable>();
                    _modelLists.CollectionChanged += ModelListsCollectionChanged;
                    //_modelListsLazy.Apply(a=>_modelLists.Add(a));
                    for (int i = 0; i < _modelListsLazy.Count; i++)
                    {

                        _modelLists.Add(_modelListsLazy[i]);

                    }
                    IsBusy = false;
                }
                return InternalChildren;
            }
        }

        public virtual IViewModelsCollection<IObjectViewModel> Items
        {
            get { return Children; }
        }

        /// <summary>
        /// Gets the models lists collection.
        /// </summary>
        /// <value>The model lists.</value>
        public ObservableCollection<IEnumerable> ModelLists
        {
            get
            {
                return _modelLists ?? _modelListsLazy;
            }
        }

        /// <summary>
        /// Gets the models list
        /// </summary>
        /// <value>The models.</value>
        public ObservableCollection<object> Models
        {
            get
            {
                return _internalList;
            }
        }


        #endregion

        #region Protected Implementation

        internal ObservableViewModelsCollection<IObjectViewModel> InternalChildren
        {
            get { return _children; }
        }

        /// <summary>
        /// Occurs when children are changed
        /// </summary>
        /// <param name="args">The  instance containing the event data.</param>
        protected virtual void OnChildrenChanged(ChildEventArgs<IObjectViewModel> args)
        {

        }

        #endregion

        #region IObjectViewModelFactory
        /// <summary>
        /// Creates view model
        /// </summary>
        /// <param name="parent">Parent model</param>
        /// <param name="obj">Object for which we making model</param>
        /// <returns></returns>
        public virtual IObjectViewModel CreateViewModel(IViewModel parent, object obj)
        {
            return new ObjectViewModel<object>(obj);
        }
        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            ModelLists
                .OfType<INotifyCollectionChanged>()
                .ForEach(ml => ml.CollectionChanged -= ListCollectionChanged);
        }
        #endregion
    }
}
