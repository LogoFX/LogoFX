using System;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
  /// <summary>
  /// Provides a way to apply interval formating to a binding.
  /// </summary>
  public class IntervalConverter : IValueConverter
  {

    #region IValueConverter Members

    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      TimeSpan? t = value as TimeSpan?;
      return (t == null ? "" : t.Value.Minutes.ToString("00") + ":" + t.Value.Seconds.ToString("00") + "." + t.Value.Milliseconds.ToString("00"));
    }

    /// <summary>
    /// Converts a value back.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string s = value as string;
      return (s == null ? TimeSpan.FromSeconds(0) : TimeSpan.Parse(s));
    }

    #endregion
  }
}
