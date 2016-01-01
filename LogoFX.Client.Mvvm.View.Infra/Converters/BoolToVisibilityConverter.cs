using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
  public class BoolToVisibilityConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        bool bValue = false;
        if (value is bool)
            bValue = (bool)value;
        else if (value is string)
            Boolean.TryParse((string) value, out bValue);
        else
            return Visibility.Visible;

        bool result;

        if (!Boolean.TryParse((string)parameter, out result))
          result = true;

        if (result)
            return bValue ? Visibility.Visible : Visibility.Collapsed;
        else
          return bValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    #endregion
  }
}