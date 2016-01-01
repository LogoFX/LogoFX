using System;
using System.Globalization;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;

#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
  /// <summary>
  /// Converts boolean to constant value
  /// </summary>
  public class BoolToConstConverter:IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      object parameterTrue = parameter;
      object parameterFalse = DependencyProperty.UnsetValue;
      if(parameter is String)
      {
        string[] elements = ((string) parameter).Split('|');
        parameterTrue = elements[0];
        if(elements.Length>1)
        {
          parameterFalse = elements[1];
        }

      }
      if (value is bool)
      {
        return (bool) value ? parameterTrue : parameterFalse;
      }
      if (value is string)
      {
        bool res;
        if (Boolean.TryParse((string) value, out res) && res)
        {
          return parameterTrue;
        }
      }
      return parameterFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}
