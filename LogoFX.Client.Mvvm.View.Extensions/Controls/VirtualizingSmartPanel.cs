using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using LogoFX.Client.Mvvm.ViewModel.Extensions;

namespace LogoFX.Client.Mvvm.View.Extensions.Controls
{
    /// <summary>
    /// This panel allows smart virtualization.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.VirtualizingPanel" />
    /// <seealso cref="System.Windows.Controls.Primitives.IScrollInfo" />
    /// <seealso cref="LogoFX.Client.Mvvm.ViewModel.Extensions.IPageInfo" />
    public sealed class VirtualizingSmartPanel : VirtualizingPanel, IScrollInfo, IPageInfo
    {
        #region Fields

        private static readonly Type ThisType = typeof(VirtualizingSmartPanel);

        private int _startIndex;
        private int _endIndex;
        private ItemsControl _itemsOwner;

        private bool _updating;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingSmartPanel"/> class.
        /// </summary>
        public VirtualizingSmartPanel()
        {
            CanVerticallyScroll = false;
            CanHorizontallyScroll = false;
            Loaded += (sender, args) => InvalidateMeasure();
        }

        #region Dependency Properties

        /// <summary>
        /// Defines a <see cref="DependencyProperty" /> for item width.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                "ItemWidth",
                typeof(double),
                ThisType,
                new FrameworkPropertyMetadata(
                    64d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> for item height.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                "ItemHeight",
                typeof(double),
                ThisType,
                new FrameworkPropertyMetadata(
                    64d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the item width.
        /// </summary>
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the item height.
        /// </summary>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value whether the paging is allowed.
        /// </summary>
        public bool AllowPaging { get; set; }

        #endregion

        #region Private Members

        private int CalculateRows(Size size, int count)
        {
            if (ItemHeight <= 0)
            {
                return 0;
            }

            return Math.Min((int)(size.Height / ItemHeight), count);
        }

        private int CalculateColumns(int rows, int count)
        {
            if (rows == 0)
            {
                return 0;
            }

            return (int)Math.Ceiling((double)count / rows);
        }

        private int CalculateColumns(Size size)
        {
            if (ItemWidth <= 0)
            {
                return 0;
            }

            //return Math.Max((int)Math.Floor(size.Width / ItemWidth), 1);

            return (int)Math.Floor(size.Width / ItemWidth);
        }

        private Size CalculatePageSize(Size availableSize, int count)
        {
            int rows = CalculateRows(availableSize, count);
            int columns = CalculateColumns(availableSize);
            if (rows == 0)
            {
                PageCount = 0;
            }
            else
            {
                PageCount = (int)Math.Ceiling((double)count / (rows * columns));
            }

            return new Size(columns * ItemWidth, rows * ItemHeight);
        }

        private double CalculateExtent(Size size, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            int rows = CalculateRows(size, count);
            int realColumns = CalculateColumns(rows, count);
            int sizedColumns = CalculateColumns(size);
            int columns = (int)Math.Ceiling((double)realColumns / sizedColumns) * sizedColumns;
            return columns * ItemWidth;
        }

        private void UpdateScrollInfo(Size availableSize)
        {
            // See how many items there are
            int itemCount = _itemsOwner.Items.Count;
            bool viewportChanged = false;
            bool extentChanged = false;

            double extent = CalculateExtent(availableSize, itemCount);
            _pageSize = CalculatePageSize(availableSize, _itemsOwner.Items.Count);
            if (AllowPaging)
            {
                extent += availableSize.Width - _pageSize.Width;
            }

            // Update extent
            if (!extent.Equals(ExtentWidth))
            {
                ExtentWidth = extent;
                extentChanged = true;
            }

            // Update viewport
            if (!availableSize.Width.Equals(ViewportWidth))
            {
                ViewportWidth = availableSize.Width;
                viewportChanged = true;
            }

            if ((extentChanged || viewportChanged) && ScrollOwner != null)
            {
                HorizontalOffset = CalculateHorizontalOffset(HorizontalOffset);
                CurrentPageRect = new Rect(new Point(0, 0), _pageSize);
                ScrollOwner.InvalidateScrollInfo();
            }
        }

        private void VirtualizeItems(Size availableSize)
        {
            UpdateIndexRange(availableSize);

            IItemContainerGenerator generator = _itemsOwner.ItemContainerGenerator;

            GeneratorPosition startPos = generator.GeneratorPositionFromIndex(_startIndex);
            int childIndex = startPos.Offset == 0 ? startPos.Index : startPos.Index + 1;
            using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
            {
                for (int i = _startIndex; i <= _endIndex; i++, childIndex++)
                {
                    bool isNewlyRealized;
                    var child = generator.GenerateNext(out isNewlyRealized) as UIElement;
                    if (isNewlyRealized)
                    {
                        if (childIndex >= InternalChildren.Count)
                        {
                            AddInternalChild(child);
                        }
                        else
                        {
                            InsertInternalChild(childIndex, child);
                        }
                        generator.PrepareItemContainer(child);
                    }
                }
            }
        }

        private void CleanupItems()
        {
            IItemContainerGenerator generator = _itemsOwner.ItemContainerGenerator;
            for (int i = InternalChildren.Count - 1; i >= 0; i--)
            {
                var position = new GeneratorPosition(i, 0);
                int itemIndex = generator.IndexFromGeneratorPosition(position);
                if (itemIndex < _startIndex || itemIndex > _endIndex)
                {
                    generator.Remove(position, 1);
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private void UpdateIndexRange(Size availableSize)
        {
            int count = _itemsOwner.Items.Count;
            double left = HorizontalOffset;
            double right = Math.Min(ExtentWidth, HorizontalOffset + ViewportWidth);
            int leftColumn = CalculateColumnFromOffset(left);
            int rightColumn = CalculateColumnFromOffset(right);
            int rows = CalculateRows(availableSize, count);
            _startIndex = leftColumn * rows;
            _endIndex = Math.Min(count, (rightColumn + 1) * rows) - 1;
        }

        private int CalculateColumnFromOffset(double offset)
        {
            return (int)(offset / ItemWidth);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally. 
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            _itemsOwner = ItemsControl.GetItemsOwner(this);
        }

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement"/>-derived class. 
        /// </summary>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (Double.IsNaN(availableSize.Width) || Double.IsInfinity(availableSize.Width))
            {
                throw new InvalidOperationException();
            }

            if (!IsLoaded)
            {
                return availableSize;
            }

            UpdateScrollInfo(availableSize);

            // Virtualize items
            VirtualizeItems(availableSize);

            var size = new Size(ItemWidth, ItemHeight);
            // Measure
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(size);
            }

            // Cleanup
            CleanupItems();

            return availableSize;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class. 
        /// </summary>
        /// <returns>
        /// The actual size used.
        /// </returns>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            int childCount = InternalChildren.Count;

            if (childCount == 0)
            {
                return finalSize;
            }

            int rows = CalculateRows(finalSize, childCount);
            int row = 0;
            int column = _startIndex / rows;

            for (int i = 0; i < childCount; i++)
            {
                double left = column * ItemWidth - HorizontalOffset;
                double top = row * ItemHeight;

                var arrangeRect = new Rect(left, top, ItemWidth, ItemHeight);
                var internalChild = InternalChildren[i];
                internalChild.Arrange(arrangeRect);

                ++row;
                if (row == rows)
                {
                    row = 0;
                    ++column;
                }
            }


            return finalSize;
        }

        #endregion

        #region IScrollInfo

        void IScrollInfo.LineUp()
        {
        }

        void IScrollInfo.LineDown()
        {
        }

        void IScrollInfo.PageUp()
        {
        }

        void IScrollInfo.PageDown()
        {
        }

        void IScrollInfo.MouseWheelUp()
        {
            LineLeftInternal();
        }

        void IScrollInfo.MouseWheelDown()
        {
            LineRightInternal();
        }

        void IScrollInfo.SetVerticalOffset(double offset)
        {
        }

        void IScrollInfo.LineLeft()
        {
            LineLeftInternal();
        }

        private void LineLeftInternal()
        {
            if (AllowPaging)
            {
                PageLeftInternal();
            }
            else
            {
                SetHorizontalOffsetInternal(HorizontalOffset - 16.0);
            }
        }

        void IScrollInfo.LineRight()
        {
            LineRightInternal();
        }

        private void LineRightInternal()
        {
            if (AllowPaging)
            {
                PageRightInternal();
            }
            else
            {
                SetHorizontalOffsetInternal(HorizontalOffset + 16.0);
            }
        }

        void IScrollInfo.PageLeft()
        {
            PageLeftInternal();
        }

        private void PageLeftInternal()
        {
            SetHorizontalOffsetInternal(HorizontalOffset - _pageSize.Width);
        }

        void IScrollInfo.PageRight()
        {
            PageRightInternal();
        }

        private void PageRightInternal()
        {
            SetHorizontalOffsetInternal(HorizontalOffset + _pageSize.Width);
        }

        void IScrollInfo.MouseWheelLeft()
        {
            SetHorizontalOffsetInternal(HorizontalOffset - SystemParameters.WheelScrollLines);
        }

        void IScrollInfo.MouseWheelRight()
        {
            SetHorizontalOffsetInternal(HorizontalOffset + SystemParameters.WheelScrollLines);
        }

        void IScrollInfo.SetHorizontalOffset(double offset)
        {
            SetHorizontalOffsetInternal(offset);
        }

        private void SetHorizontalOffsetInternal(double offset)
        {
//if (!ScrollOwner.IsLoaded)
            //{
            //    ScrollOwner.Loaded += (sender, args) =>
            //    {
            //        SetHorizontalOffset(offset);
            //    };
            //    return;
            //}

            HorizontalOffset = CalculateHorizontalOffset(offset);

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            // Force us to realize the correct children
            InvalidateMeasure();

            if (_pageSize.Width > 0.0)
            {
                int currentPage = _updating ? CurrentPage : (int) Math.Round(HorizontalOffset/_pageSize.Width);
                double left = currentPage*_pageSize.Width - HorizontalOffset;
                var pageRect = CurrentPageRect;
                if (!pageRect.Left.Equals(left))
                {
                    CurrentPageRect = new Rect(left, pageRect.Top, pageRect.Width, pageRect.Height);
                }

                if (!_updating)
                {
                    CurrentPage = currentPage;
                }
            }
        }

        private double CalculateHorizontalOffset(double offset)
        {
            if (offset < 0 || ViewportWidth >= ExtentWidth)
            {
                offset = 0;
            }
            else
            {
                if (offset + ViewportWidth >= ExtentWidth)
                {
                    offset = ExtentWidth - ViewportWidth;
                }
            }

            return offset;
        }

        Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle)
        {
            return rectangle;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether scrolling on the vertical axis is possible. 
        /// </summary>
        /// <returns>
        /// true if scrolling is possible; otherwise, false. This property has no default value.
        /// </returns>
        public bool CanVerticallyScroll { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.
        /// </summary>
        /// <returns>
        /// true if scrolling is possible; otherwise, false. This property has no default value.
        /// </returns>
        public bool CanHorizontallyScroll { get; set; }

        /// <summary>
        /// Gets the horizontal size of the extent.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the horizontal size of the extent. This property has no default value.
        /// </returns>
        public double ExtentWidth { get; set; }

        /// <summary>
        /// Gets the vertical size of the extent.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the vertical size of the extent.This property has no default value.
        /// </returns>
        public double ExtentHeight { get; set; }

        /// <summary>
        /// Gets the horizontal size of the viewport for this content.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the horizontal size of the viewport for this content. This property has no default value.
        /// </returns>
        public double ViewportWidth { get; set; }

        /// <summary>
        /// Gets the vertical size of the viewport for this content.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the vertical size of the viewport for this content. This property has no default value.
        /// </returns>
        public double ViewportHeight { get; set; }

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the horizontal offset. This property has no default value.
        /// </returns>
        public double HorizontalOffset { get; set; }

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Double"/> that represents, in device independent pixels, the vertical offset of the scrolled content. Valid values are between zero and the <see cref="P:System.Windows.Controls.Primitives.IScrollInfo.ExtentHeight"/> minus the <see cref="P:System.Windows.Controls.Primitives.IScrollInfo.ViewportHeight"/>. This property has no default value.
        /// </returns>
        public double VerticalOffset { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="T:System.Windows.Controls.ScrollViewer"/> element that controls scrolling behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ScrollViewer"/> element that controls scrolling behavior. This property has no default value.
        /// </returns>
        public ScrollViewer ScrollOwner { get; set; }

        #endregion

        #region IPageInfo

        /// <summary>
        /// Defines a <see cref="DependencyPropertyKey"/> for page count .
        /// </summary>
        public static readonly DependencyPropertyKey PageCountPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "PageCount",
                typeof(int),
                ThisType,
                new PropertyMetadata(
                    0,
                    PageCount_Changed,
                    PageCount_Coerce));

        private static object PageCount_Coerce(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        private static void PageCount_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            int pageCount = (int)e.NewValue;
            int currentPage = (int)d.GetValue(CurrentPageProperty);
            if (currentPage >= pageCount)
            {
                d.SetValue(CurrentPageProperty, pageCount - 1);
            }
            //else if (currentPage < 0 && pageCount > 0)
            //{
            //    d.SetValue(CurrentPageProperty, 0);
            //}
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> for page count.
        /// </summary>
        public static readonly DependencyProperty PageCountProperty =
            PageCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountPropertyKey, value); }
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> for current page.
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                "CurrentPage",
                typeof(int),
                ThisType,
                new FrameworkPropertyMetadata(
                    0,
                    CurrentPage_Changed,
                    CurrentPage_Coerce));

        private static object CurrentPage_Coerce(DependencyObject d, object basevalue)
        {
            int currentPage = (int)basevalue;
            //int pageCount = (int) d.GetValue(PageCountProperty);

            //if (pageCount == 0)
            //{
            //    currentPage = -1;
            //}
            //else if (currentPage >= pageCount)
            //{
            //    currentPage = pageCount - 1;
            //}
            //else if (currentPage < 0)
            //{
            //    currentPage = 0;
            //}

            return currentPage;
        }

        private static void CurrentPage_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            ((VirtualizingSmartPanel)d).ShowPage((int)e.NewValue);
        }

        private void ShowPage(int pageNumber)
        {
            if (!AllowPaging)
            {
                return;
            }

            Rect pageRect = CurrentPageRect;
            if (pageNumber < 0 || pageRect.IsEmpty)
            {
                return;
            }

            SetHorizontalOffsetInternal(pageNumber * pageRect.Width);
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        /// <summary>
        /// Defines a <see cref="DependencyPropertyKey"/> for current page rectangle.
        /// </summary>
        public static readonly DependencyPropertyKey CurrentPageRectPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "CurrentPageRect",
                typeof(Rect),
                ThisType,
                new PropertyMetadata(
                    Rect.Empty,
                    CurrentPageRectChanged));

        private static void CurrentPageRectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            var panel = (VirtualizingSmartPanel)d;

            if (panel._updating)
            {
                return;
            }

            panel.ShowPage(panel.CurrentPage);
        }

        /// <summary>
        /// Defines a <see cref="DependencyProperty"/> for current page rectangle.
        /// </summary>
        public static readonly DependencyProperty CurrentPageRectProperty =
            CurrentPageRectPropertyKey.DependencyProperty;

        private Size _pageSize;

        /// <summary>
        /// Gets the current page rectangle.
        /// </summary>
        /// <value>
        /// The current page rectangle.
        /// </value>
        public Rect CurrentPageRect
        {
            get { return (Rect)GetValue(CurrentPageRectProperty); }
            private set { SetValue(CurrentPageRectPropertyKey, value); }
        }

        /// <summary>
        /// Begins the update.
        /// </summary>
        public void BeginUpdate()
        {
            _updating = true;
        }

        /// <summary>
        /// Ends the update.
        /// </summary>
        public void EndUpdate()
        {
            _updating = false;
            SetHorizontalOffsetInternal(_pageSize.Width * (int)Math.Round(HorizontalOffset / _pageSize.Width));
        }

        #endregion
    }
}
