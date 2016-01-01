using System;
using System.Globalization;
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
  /// Provides a way to apply custom formating to a binding value multiplied by 100.
  /// </summary>
  public class PercentageFormatConverter : IValueConverter
  {
    #region IValueConverter Members

    /// <summary>
    /// Modifies the source data before passing it to the target for display in the UI.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
      {
        return value;
      }
      int ri;
      double rd;
      if (int.TryParse(value.ToString(), out ri))
      {
        return string.Format(parameter.ToString(), ri * 100);
      }
      if (double.TryParse(value.ToString(), out rd))
      {
        return string.Format(parameter.ToString(), rd * 100.0);
      }
      return string.Format(parameter.ToString(), value);
    }

    /// <summary>
    /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
      {
        return value;
      }
      int ri;
      double rd;
      if (int.TryParse(value.ToString(), out ri))
      {
        return ri / 100;
      }
      if (double.TryParse(value.ToString(), out rd))
      {
        return rd / 100.0;
      }
      return value.ToString();
    }

    #endregion
  }
}
