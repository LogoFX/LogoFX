using System;
using System.Globalization;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using CultureInfo = System.String;
#else
using System.Windows.Data;

#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
  /// <summary>
  /// Conditionally extracts <see cref="FrameworkElement.DataContext"/> from value.
  /// </summary>
  public class ItemToContextConverter:IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is FrameworkElement)
        value = ((FrameworkElement) value).DataContext;

      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
