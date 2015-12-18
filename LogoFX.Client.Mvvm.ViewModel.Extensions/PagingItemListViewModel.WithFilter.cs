using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Caliburn.Micro;
using LogoFX.Client.Core;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract partial class PagingItemListViewModel<TItem, TModel>
    {
        public abstract class WithSelection : PagingItemListViewModel<TItem, TModel>, IWithSelection<TItem>
        {
            #region Fields

            private readonly ObservableCollection<TItem> _selectedItems = new ObservableCollection<TItem>();
            private const uint SingleSelectionMask = (uint)(SelectionMode.One | SelectionMode.ZeroOrOne);
            private const uint MultipleSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.ZeroOrMore);
            private const uint RequiredSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.One);
            private const SelectionMode DefaultSelectionMode = SelectionMode.ZeroOrMore;
            private readonly ReentranceGuard _selectionManagement = new ReentranceGuard();
            private readonly SelectionMode _selectionMode;
            private EventHandler<SelectionChangingEventArgs> _currentHandler;
            private Action<object, SelectionChangingEventArgs> _selectionHandler;
            private PropertyChangedEventHandler _internalSelectionHandler;

            #endregion

            #region Constructors

            protected WithSelection(object parent, IList<TModel> source, SelectionMode selectionMode = DefaultSelectionMode)
                : base(parent, source)
            {
                _selectionMode = selectionMode;
                _selectedItems.CollectionChanged += (a, b) => OnSelectionChanged();
            }

            #endregion

            #region Events

            public event EventHandler SelectionChanged;

            protected void InvokeSelectionChanged(EventArgs e)
            {
                EventHandler handler = SelectionChanged;
                if (handler != null) handler(this, e);
            }

            public event EventHandler<SelectionChangingEventArgs> SelectionChanging;

            protected void InvokeSelectionChanging(SelectionChangingEventArgs e)
            {
                EventHandler<SelectionChangingEventArgs> handler = SelectionChanging;
                if (handler != null) handler(this, e);
            }

            #endregion

            #region Public Properties

            public Action<object, SelectionChangingEventArgs> SelectionHandler
            {
                get { return _selectionHandler; }
                set
                {
                    if (_currentHandler != null)
                    {
                        SelectionChanging -= _currentHandler;
                    }

                    _selectionHandler = value;
                    SelectionChanging += (_currentHandler = WeakDelegate.From<SelectionChangingEventArgs>((a, b) => _selectionHandler(this, b)));
                }
            }

            public TItem SelectedItem
            {
                get { return _selectedItems.Count > 0 ? _selectedItems[0] : null; }
            }


            public IEnumerable<TItem> SelectedItems
            {
                get { return _selectedItems; }
            }

            #endregion

            #region Private implementation

            private bool HandleItemSelectionChanged(TItem item, bool isSelecting)
            {
                using (_selectionManagement.Raise())
                {
                    if (_selectionManagement.IsLocked)
                    {
                        return false;
                    }

                    if (_selectedItems.Contains(item) && isSelecting)
                    {
                        //redundant call, skip
                        return true;
                    }

                    var args = new SelectionChangingEventArgs(item, isSelecting);
                    InvokeSelectionChanging(args);
                    if (args.Cancel)
                    {
                        //cancel selection change
                        item.IsSelected = !isSelecting;
                        return false;
                    }

                    if (isSelecting)
                    {
                        if (((uint)_selectionMode & SingleSelectionMask) != 0)
                        {
                            _selectedItems.Apply(a =>
                            {
                                a.IsSelected = false;
                            });
                            _selectedItems.Clear();
                        }
                        _selectedItems.Add(item);
                        item.IsSelected = true;
                    }
                    else
                    {
                        _selectedItems.Remove(item);
                        item.IsSelected = false;

                        if (IsSelectionRequired && _selectedItems.Count == 0 && Count > 0)
                        {
                            _selectedItems.Add(this[0]);
                            item.IsSelected = true;
                        }
                    }

                    InvokeSelectionChanged(new EventArgs());
                    NotifyOfPropertyChange(() => SelectedItem);
                    return true;
                }
            }

            private void InternalIsSelectedChanged(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == "IsSelected" && sender is ISelectable)
                {
                    Execute.OnUIThread(() => HandleItemSelectionChanged((TItem)sender, ((ISelectable)sender).IsSelected));
                }
            }

            private bool IsSelectionRequired
            {
                get { return ((uint)_selectionMode & RequiredSelectionMask) != 0; }
            }

            #endregion

            #region Overrides

            protected override void OnCreated(TItem item)
            {
                if (_internalSelectionHandler == null)
                {
                    _internalSelectionHandler = WeakDelegate.From(InternalIsSelectedChanged);
                }

                Action<TItem> addHandler = (a) =>
                {
                    if (a is INotifyPropertyChanged)
                        ((INotifyPropertyChanged)a).PropertyChanged += _internalSelectionHandler;

                    if (_selectedItems.Count == 0 && IsSelectionRequired)
                    {
                        Select(a);
                    }
                };

                addHandler(item);
            }

            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                if (_internalSelectionHandler == null)
                {
                    _internalSelectionHandler = WeakDelegate.From(InternalIsSelectedChanged);
                }

                Action<TItem> removeHandler = a =>
                {
                    if (a is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)a).PropertyChanged -= _internalSelectionHandler;
                    }

                    Unselect(a);
                };

                Action<TItem> addHandler = (a) =>
                {
                    if (a is INotifyPropertyChanged)
                        ((INotifyPropertyChanged)a).PropertyChanged += _internalSelectionHandler;

                    if (_selectedItems.Count == 0 && IsSelectionRequired)
                    {
                        Select(a);
                    }
                };

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.Cast<TItem>().Apply(addHandler);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems.Cast<TItem>().Apply(removeHandler);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        e.OldItems.Cast<TItem>().Apply(removeHandler);
                        e.NewItems.Cast<TItem>().Apply(addHandler);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Debug.Assert(false, "We should never be here. Check base class impl");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();

                }
                base.OnCollectionChanged(e);
            }

            #endregion

            #region Protected

            protected virtual void OnSelectionChanged()
            {
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Selection operation
            /// </summary>
            /// <param name="newSelection">item to select </param>
            /// <returns>old selected item if available</returns>
            public bool Select(TItem newSelection)
            {
                return HandleItemSelectionChanged(newSelection, true);
            }

            /// <summary>
            /// Un-selects object
            /// </summary>
            /// <param name="newSelection"></param>
            /// <returns><see langword="true"/> if succeeded, otherwise <see langword="false"/></returns>
            public bool Unselect(TItem newSelection)
            {
                return HandleItemSelectionChanged(newSelection, false);
            }

            #endregion

            //#region Implementation of INotifyPropertyChanged

            //public event PropertyChangedEventHandler PropertyChanged;

            //protected void OnPropertyChanged(string p)
            //{
            //    PropertyChangedEventHandler handler = PropertyChanged;
            //    if (handler != null) handler(this, new PropertyChangedEventArgs(p));
            //}

            //#endregion
            object IHaveSelectedItem.SelectedItem
            {
                get { return SelectedItem; }
            }

            IEnumerable IHaveSelectedItems.SelectedItems
            {
                get { return SelectedItems; }
            }
        }
    }
}
