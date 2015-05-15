using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using LogoFX.Core;
using LogoFX.UI.Core;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels
{
    public partial class WrappingCollection
    {
        public class WithSelection : WrappingCollection, ISelector, INotifyPropertyChanged
        {
            private readonly ObservableCollection<object> _selectedItems = new ObservableCollection<object>();
            private const uint SingleSelectionMask = (uint)(SelectionMode.One | SelectionMode.ZeroOrOne);
            private const uint MultipleSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.ZeroOrMore);
            private const uint RequiredSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.One);
            private const SelectionMode DefaultSelectionMode = SelectionMode.ZeroOrMore;
            private readonly ReentranceGuard _selectionManagement = new ReentranceGuard();
            private readonly SelectionMode _selectionMode;
            private EventHandler<SelectionChangingEventArgs> _currentHandler;
            private Action<object, SelectionChangingEventArgs> _selectionHandler;
            private PropertyChangedEventHandler _internalSelectionHandler;

            public WithSelection(IEnumerable source, SelectionMode selectionMode = DefaultSelectionMode)
                : base(source)
            {
                _selectionMode = selectionMode;
            }

            public WithSelection(SelectionMode selectionMode = DefaultSelectionMode)
            {
                _selectionMode = selectionMode;
                _selectedItems.CollectionChanged += (a, b) => OnSelectionChanged();
            }

            #region Private implementation
            private bool HandleItemSelectionChanged(object obj, bool isSelecting)
            {
                using (_selectionManagement.Raise())
                {
                    if (_selectionManagement.IsLocked)
                        return false;

                    if (_selectedItems.Contains(obj) && isSelecting)
                    {
                        //redundant call, skip
                        return true;
                    }

                    SelectionChangingEventArgs args = new SelectionChangingEventArgs(obj, isSelecting);
                    InvokeSelectionChanging(args);
                    if (args.Cancel)
                    {
                        //cancel selection change
                        if (obj is ISelectable)
                            ((ISelectable)obj).IsSelected = !isSelecting;
                        return false;
                    }

                    if (isSelecting)
                    {
                        if (((uint)_selectionMode & SingleSelectionMask) != 0)
                        {
                            _selectedItems.ForEach(a =>
                            {
                                if (a is ISelectable)
                                    ((ISelectable)obj).IsSelected = false;
                            });
                            _selectedItems.Clear();
                        }
                        _selectedItems.Add(obj);
                        if (obj is ISelectable)
                            ((ISelectable)obj).IsSelected = true;
                    }
                    else
                    {
                        _selectedItems.Remove(obj);
                        if (obj is ISelectable)
                            ((ISelectable)obj).IsSelected = false;

                        if (IsSelectionRequired && _selectedItems.Count == 0 && InternalChildren.Count > 0)
                        {
                            _selectedItems.Add(InternalChildren[0]);
                            if (obj is ISelectable)
                                ((ISelectable)obj).IsSelected = true;
                        }
                    }

                    InvokeSelectionChanged(new EventArgs());
                    OnPropertyChanged("SelectedItem");
                    OnPropertyChanged("SelectedItems");
                    OnPropertyChanged("SelectionCount");
                    return true;
                }
            }

#if SILVERLIGHT
            public
#else
            internal
#endif
 void InternalIsSelectedChanged(object o, PropertyChangedEventArgs args)
            {
                if (o != null && !InternalChildren.Contains(o))
                    ((INotifyPropertyChanged)o).PropertyChanged -= _internalSelectionHandler;
                else if (args.PropertyName == "IsSelected" && o is ISelectable)
                    Dispatch.Current.OnUiThread(() => HandleItemSelectionChanged(o, ((ISelectable)o).IsSelected));
            }

            private bool IsSelectionRequired
            {
                get { return ((uint)_selectionMode & RequiredSelectionMask) != 0; }
            }
            #endregion

            #region overrides
            protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                if (_internalSelectionHandler == null)
                    _internalSelectionHandler = WeakDelegate.From(InternalIsSelectedChanged);

                Action<object> RemoveHandler = (a) =>
                {
                    if (a is INotifyPropertyChanged)
                        ((INotifyPropertyChanged)a).PropertyChanged -= _internalSelectionHandler;
                    Unselect(a);
                };
                Action<object> AddHandler = (a) =>
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
                        e.NewItems.Cast<object>().ForEach(AddHandler);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems.Cast<object>().ForEach(RemoveHandler);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        e.OldItems.Cast<object>().ForEach(RemoveHandler);
                        e.NewItems.Cast<object>().ForEach(AddHandler);
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

            #region Select

            /// <summary>
            /// Selection operation
            /// </summary>
            /// <param name="newSelection">item to select </param>
            /// <param name="notify"></param>
            /// <returns>old selected item if available</returns>
            public bool Select(object newSelection, bool notify = true)
            {
                object item = InternalChildren.FirstOrDefault(i => i.Equals(newSelection));
                if (item != null)
                {
                    return HandleItemSelectionChanged(item, true);
                }
                return false;
            }
            #endregion

            #region Unselect

            /// <summary>
            /// Un-selects object
            /// </summary>
            /// <param name="newSelection"></param>
            /// <param name="notify"></param>
            /// <returns><see langword="true"/> if succeeded, otherwise <see langword="false"/></returns>
            public bool Unselect(object newSelection, bool notify = true)
            {
                if (newSelection != null)
                    return HandleItemSelectionChanged(newSelection, false);
                return false;
            }

            public void ClearSelection()
            {
                //TODO: refactor into more efficient approach
                foreach (var selectedItem in SelectedItems.OfType<object>().ToArray())
                {
                    Unselect(selectedItem);
                }
            }

            #endregion

            #region To Override
            protected virtual void OnSelectionChanged()
            {
            }
            #endregion

            #region public props
            public Action<object, SelectionChangingEventArgs> SelectionHandler
            {
                get { return _selectionHandler; }
                set
                {
                    if (_currentHandler != null)
                        SelectionChanging -= _currentHandler;
                    _selectionHandler = value;
                    SelectionChanging += (_currentHandler = WeakDelegate.From<SelectionChangingEventArgs>((a, b) => _selectionHandler(this, b)));
                }
            }

            public object SelectedItem
            {
                get { return _selectedItems.Count > 0 ? _selectedItems[0] : null; }
            }

            public int SelectionCount
            {
                get { return _selectedItems == null ? 0 :_selectedItems.Count; }
            }

            public IEnumerable SelectedItems
            {
                get { return _selectedItems; }
            }
            #endregion

            #region selectionevents
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

            #region Implementation of INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string p)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(p));
            }

            #endregion
        }
    }
}
