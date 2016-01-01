using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Windows.Controls;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    /// <summary>
    /// Intended to map ItemsControl's item to some other item inside converters Items list
    /// Usecase: providing distinct brush per item index
    /// </summary>
    public class ElementToItemConverter : IValueConverter
    {
            private object _default;
            private readonly List<object> _items = new List<object>();

            public List<object> Items
            {
                get { return _items; }
            }

            public object Default
            {
                get { return _default; }
                set { _default = value; }
            }

            #region Implementation of IValueConverter

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DependencyObject item = value as DependencyObject;

                ItemsControl view = ItemsControl.ItemsControlFromItemContainer(item);

                int i;
                try
                {
                    i = view.ItemContainerGenerator.IndexFromContainer(item);
                }
                catch (Exception)
                {

                    return Default;
                }

                return _items[i % _items.Count];

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        
    }
}
