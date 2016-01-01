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
  /// Negates value
  /// </summary>
  public class NegativeConverter:IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double d;
      if (value is string && Double.TryParse((string)value, out d))
      {
        return -(d);
      }
      else 
      {
        try
        {
          return -(System.Convert.ToDouble(value));
        }
        catch (Exception)
        {
          
        }
        
      }
      return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value,targetType, parameter, culture);
    }
  }
}
