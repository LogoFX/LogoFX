// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>Sunday, November 21, 2010</lastedit>

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

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using LogoFX.Client.Core;
using LogoFX.Core;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents collection <c>ViewModel</c> with selection support
    /// </summary>
    /// <typeparam name="THead">The type of the head model.</typeparam>
    /// <typeparam name="TChild">The type of the child models.</typeparam>
    public class SelectorViewModel<THead, TChild> : SimpleObjectsListViewModel<THead, TChild>, ISelect, IUnselect
    {
        private const uint SingleSelectionMask = (uint)(SelectionMode.One | SelectionMode.ZeroOrOne);
        private const uint MultipleSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.ZeroOrMore);
        private const uint RequiredSelectionMask = (uint)(SelectionMode.OneOrMore | SelectionMode.One);
        const SelectionMode DefaultSelectionMode = SelectionMode.ZeroOrMore;
        readonly ReentranceGuard _selectionManagement = new ReentranceGuard();

        private readonly SelectionMode _selectionMode;
        private readonly ObservableCollection<IObjectViewModel<TChild>> _selection = new ObservableCollection<IObjectViewModel<TChild>>();
        private PropertyChangedEventHandler _internalSelectionHandler;


        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorViewModel&lt;THead, TChild&gt;"/> class.
        /// </summary>
        /// <param name="head">The head model.</param>
        /// <param name="models">The child models.</param>
        /// <param name="creator">The creator method.</param>
        /// <param name="selectionMode">The selection mode.</param>
        public SelectorViewModel(THead head, IEnumerable models,
            Func<TChild, IObjectViewModel<TChild>> creator,
            SelectionMode selectionMode = DefaultSelectionMode)
            : this(head, models, null, creator, selectionMode)
        {
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorViewModel&lt;THead, TChild&gt;"/> class.
        /// </summary>
        /// <param name="head">The head model.</param>
        /// <param name="models">The child  models.</param>
        /// <param name="selectionHandler">Selection handler.</param>
        /// <param name="selectionMode">Selection mode.</param>
        public SelectorViewModel(THead head, IEnumerable models,
            Action<SelectorViewModel<THead, TChild>> selectionHandler,
            SelectionMode selectionMode = DefaultSelectionMode)
            : this(head, models, selectionHandler, null, selectionMode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorViewModel&lt;THead, TChild&gt;"/> class.
        /// </summary>
        /// <param name="head">The head model.</param>
        /// <param name="models">The child models.</param>
        /// <param name="selectionHandler">Selection handler.</param>
        /// <param name="creator"><c>ViewModels</c> creator.</param>
        /// <param name="selectionMode">The selection mode.</param>
        public SelectorViewModel(THead head, IEnumerable models,
            Action<SelectorViewModel<THead, TChild>> selectionHandler,
            Func<TChild, IObjectViewModel<TChild>> creator,
            SelectionMode selectionMode = DefaultSelectionMode)
            : base(head, models, creator)
        {

            _selectionMode = selectionMode;

            if (selectionHandler != null)
            {
                //week needed:potential foreign
                SelectionChanged += WeakDelegate.From<SelectionChangedEventArgs<IObjectViewModel<TChild>>>((a, b) => selectionHandler(this));
            }
        }

        /// <summary>
        /// Occurs when selection changed
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        protected virtual void OnChildrenSelectionChanged(SelectionChangedEventArgs<IObjectViewModel<TChild>> e)
        {

        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <note>BUGGGGGGGGGG!!! this need to be not TChild</note>
        /// <value>The selected items.</value>
        public ObservableCollection<IObjectViewModel<TChild>> SelectedItems
        {
            get { return _selection; }
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public IObjectViewModel<TChild> SelectedItem
        {
            get { return _selection.Count == 1 ? _selection[0] : null; }
            set
            {
                if (value != null)
                    Select(value);
                else
                    ClearSelection();
            }
        }

#if SILVERLIGHT
        public
#else
        private
#endif

 void InternalIsSelectedChanged(object o, PropertyChangedEventArgs args)
        {
            if (o != null && (o is IObjectViewModel) && !Children.Contains((IObjectViewModel)o))
                ((INotifyPropertyChanged)o).PropertyChanged -= _internalSelectionHandler;
            else if (args.PropertyName == "IsSelected")
                Dispatch.Current.OnUiThread(() => HandleSelected(((IObjectViewModel)o)));
        }

        /// <summary>
        /// Occurs when children are changed
        /// </summary>
        /// <param name="args">The  instance containing the event data.</param>
        protected override void OnChildrenChanged(ChildEventArgs<IObjectViewModel> args)
        {
            if (_internalSelectionHandler == null)
                _internalSelectionHandler = WeakDelegate.From(InternalIsSelectedChanged);

            switch (args.Action)
            {
                case ChildOperation.Add:
                    args.Item.PropertyChanged += _internalSelectionHandler;
                    if (!InternalChildren.Any(a => a.IsSelected) && IsSelectionRequired)
                        args.Item.IsSelected = true;
                    break;
                case ChildOperation.Remove:
                    args.Item.PropertyChanged -= _internalSelectionHandler;
                    _selection.Remove(args.Item as IObjectViewModel<TChild>);
                    if (!InternalChildren.Any(a => a.IsSelected) && IsSelectionRequired && (InternalChildren.Count > 0))
                        InternalChildren[0].IsSelected = true;
                    break;
            }
            base.OnChildrenChanged(args);
        }

        private bool IsSelectionRequired
        {
            get { return ((uint)_selectionMode & RequiredSelectionMask) != 0; }
        }


        private void HandleSelected(IObjectViewModel obj)
        {
            using (_selectionManagement.Raise())
            {
                if (obj == null)
                {
                    SelectedItems.Clear();
                }
                else if (obj.IsSelected)
                {
                    if (((uint)_selectionMode & SingleSelectionMask) != 0)
                    {
                        InternalChildren.Where(a => !a.Equals(obj)).ForEach(e => e.IsSelected = false);
                    }

                    SelectedItems.Add((IObjectViewModel<TChild>)obj);
                }
                else
                {
                    SelectedItems.Remove((IObjectViewModel<TChild>)obj);
                    if (!_selectionManagement.IsLocked && IsSelectionRequired)
                    {
                        if (!InternalChildren.Any(a => a.IsSelected) && IsSelectionRequired && (InternalChildren.Count > 0))
                            InternalChildren[0].IsSelected = true;
                    }
                }

                OnPropertyChanged(() => SelectedItem);

                if (!_selectionManagement.IsLocked)
                {
                    OnChildrenSelectionChanged(new SelectionChangedEventArgs<IObjectViewModel<TChild>>(_selection));
                    InvokeSelectionChanged(new SelectionChangedEventArgs<IObjectViewModel<TChild>>(_selection));
                }
            }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ResetSelection()
        {
            Dispatch.Current.OnUiThread(() =>
            {
                InternalChildren.ForEach(ret => ret.IsSelected = false);
                if (IsSelectionRequired && InternalChildren.Count > 0)
                    InternalChildren[0].IsSelected = true;
            });
        }

        /// <summary>
        /// Resets selection status of this instance.
        /// </summary>
        [Obsolete("Use ClearSelection instead")]
        public void Reset()
        {
            ResetSelection();
        }

        /// <summary>
        /// Resets selection status of this instance.
        /// </summary>
        public void ClearSelection()
        {
            ResetSelection();
        }

        #region SelectionChanged
        /// <summary>
        /// Occurs when selection changed.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs<IObjectViewModel<TChild>>> SelectionChanged;
        private void InvokeSelectionChanged(SelectionChangedEventArgs<IObjectViewModel<TChild>> e)
        {
            EventHandler<SelectionChangedEventArgs<IObjectViewModel<TChild>>> handler = SelectionChanged;
            if (handler != null) handler(this, e);
        }
        #endregion

        #region Implementation of ISelect

        public bool Select(object item, bool notify = true)
        {

            if (item == null)
                throw new ArgumentException("item");

            IViewModel ivm = item as IViewModel;

            Debug.Assert(ivm != null, "Unsupported item");

            if (!notify)
            {
                using (_selectionManagement.Raise())
                {
                    ivm.IsSelected = true;
                }
            }
            else
            {
                ivm.IsSelected = true;
            }

            return true;

        }

        #endregion

        #region Implementation of IUnselect

        public bool Unselect(object item, bool notify = true)
        {
            if (item == null)
                throw new ArgumentException("item");

            IViewModel ivm = item as IViewModel;

            Debug.Assert(ivm != null, "Unsupported item");

            if (!notify)
            {
                using (_selectionManagement.Raise())
                {
                    ivm.IsSelected = false;
                }
            }
            else
            {
                ivm.IsSelected = false;
            }
            return true;
        }

        #endregion
    }
}
