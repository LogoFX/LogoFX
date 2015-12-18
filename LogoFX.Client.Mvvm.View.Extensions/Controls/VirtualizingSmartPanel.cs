using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using LogoFX.Client.Mvvm.ViewModel.Extensions;

namespace LogoFX.Client.Mvvm.View.Extensions.Controls
{
    public sealed class VirtualizingSmartPanel : VirtualizingPanel, IScrollInfo, IPageInfo
    {
        #region Fields

        private static readonly Type ThisType = typeof(VirtualizingSmartPanel);

        private int _startIndex;
        private int _endIndex;
        private ItemsControl _itemsOwner;

        private bool _updating;

        #endregion

        #region Constructors

        public VirtualizingSmartPanel()
        {
            CanVerticallyScroll = false;
            CanHorizontallyScroll = false;
            Loaded += (sender, args) => InvalidateMeasure();
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                "ItemWidth",
                typeof(double),
                ThisType,
                new FrameworkPropertyMetadata(
                    64d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

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

        protected override void OnInitialized(EventArgs e)
        {
            _itemsOwner = ItemsControl.GetItemsOwner(this);
        }

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

        public void LineUp()
        {
        }

        public void LineDown()
        {
        }

        public void PageUp()
        {
        }

        public void PageDown()
        {
        }

        public void MouseWheelUp()
        {
            LineLeft();
        }

        public void MouseWheelDown()
        {
            LineRight();
        }

        public void SetVerticalOffset(double offset)
        {
        }

        public void LineLeft()
        {
            if (AllowPaging)
            {
                PageLeft();
            }
            else
            {
                SetHorizontalOffset(HorizontalOffset - 16.0);
            }
        }

        public void LineRight()
        {
            if (AllowPaging)
            {
                PageRight();
            }
            else
            {
                SetHorizontalOffset(HorizontalOffset + 16.0);
            }
        }

        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - _pageSize.Width);
        }

        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + _pageSize.Width);
        }

        public void MouseWheelLeft()
        {
            SetHorizontalOffset(HorizontalOffset - SystemParameters.WheelScrollLines);
        }

        public void MouseWheelRight()
        {
            SetHorizontalOffset(HorizontalOffset + SystemParameters.WheelScrollLines);
        }

        public void SetHorizontalOffset(double offset)
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
                int currentPage = _updating ? CurrentPage : (int)Math.Round(HorizontalOffset / _pageSize.Width);
                double left = currentPage * _pageSize.Width - HorizontalOffset;
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

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return rectangle;
        }

        public bool CanVerticallyScroll { get; set; }
        public bool CanHorizontallyScroll { get; set; }
        public double ExtentWidth { get; set; }
        public double ExtentHeight { get; set; }
        public double ViewportWidth { get; set; }
        public double ViewportHeight { get; set; }
        public double HorizontalOffset { get; set; }

        public double VerticalOffset { get; set; }
        public ScrollViewer ScrollOwner { get; set; }

        #endregion

        #region IPageInfo

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

        public static readonly DependencyProperty PageCountProperty =
            PageCountPropertyKey.DependencyProperty;

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountPropertyKey, value); }
        }

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

            SetHorizontalOffset(pageNumber * pageRect.Width);
        }

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

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

        public static readonly DependencyProperty CurrentPageRectProperty =
            CurrentPageRectPropertyKey.DependencyProperty;

        private Size _pageSize;

        public Rect CurrentPageRect
        {
            get { return (Rect)GetValue(CurrentPageRectProperty); }
            private set { SetValue(CurrentPageRectPropertyKey, value); }
        }

        public void BeginUpdate()
        {
            _updating = true;
        }

        public void EndUpdate()
        {
            _updating = false;
            SetHorizontalOffset(_pageSize.Width * (int)Math.Round(HorizontalOffset / _pageSize.Width));
        }

        #endregion
    }
}
