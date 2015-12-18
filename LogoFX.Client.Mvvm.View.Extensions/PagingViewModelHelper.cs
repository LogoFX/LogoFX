using System;
using System.Windows;
using System.Windows.Data;
using LogoFX.Client.Mvvm.ViewModel.Extensions;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.View.Extensions.Controls;

namespace LogoFX.Client.Mvvm.View.Extensions
{
    public class PagingViewModelHelper : DependencyObject
    {
        #region Fields

        private static readonly Type ThisType = typeof(PagingViewModelHelper);

        #endregion

        #region Attached Dependency Properties

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.RegisterAttached(
                "ViewModel",
                typeof(IPagingViewModel), ThisType,
                new PropertyMetadata(
                    null,
                    ViewModel_Changed));

        public static IPagingViewModel GetViewModel(DependencyObject obj)
        {
            return (IPagingViewModel)obj.GetValue(ViewModelProperty);
        }

        public static void SetViewModel(DependencyObject obj, IPagingViewModel value)
        {
            obj.SetValue(ViewModelProperty, value);
        }

        #endregion

        #region Private Members

        private static void ViewModel_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IPageInfo pageInfo = d as IPageInfo;

            if (pageInfo == null)
            {
                return;
            }

            IPagingViewModel vm = e.OldValue as IPagingViewModel;
            if (vm != null)
            {
                pageInfo.UnNotifyOn("PageCount");
                pageInfo.UnNotifyOn("CurrentPageRect");
            }

            vm = e.NewValue as IPagingViewModel;
            if (vm != null)
            {
                pageInfo.NotifyOn("PageCount", (newValue, oldValue) =>
                {
                    vm.TotalPages = (int)newValue;
                    if (vm.TotalPages > 0)
                    {
                        vm.RestoreSelection();                        
                    }
                });
                pageInfo.NotifyOn("CurrentPageRect", (newValue, oldValue) =>
                {
                    Rect rect = (Rect)newValue;
                    if (rect.IsEmpty)
                    {
                        return;
                    }
                    vm.PageWidth = rect.Width;
                    vm.PageLeft = rect.Left;
                });
                var binding = new Binding("CurrentPage")
                {
                    Source = vm,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding(d, VirtualizingSmartPanel.CurrentPageProperty, binding);
                var element = d as FrameworkElement;
                if (element != null)
                {
                    element.Unloaded += (sender, args) =>
                    {
                        BindingOperations.ClearBinding((DependencyObject)sender, VirtualizingSmartPanel.CurrentPageProperty);
                    };
                }
            }
        }

        #endregion
    }
}
