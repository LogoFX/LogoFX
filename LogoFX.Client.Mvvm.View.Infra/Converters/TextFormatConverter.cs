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
    /// Provides a way to apply custom formating to a binding value.
    /// </summary>
    public class TextFormatConverter : IValueConverter
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
            if (value == null)
            {
                return value;
            }
            int ri;
            double rd;
            DateTime dt;
            if (int.TryParse(value.ToString(), out ri))
            {
                return string.Format(parameter.ToString(), ri);
            }
            if (double.TryParse(value.ToString(), out rd))
            {
                return string.Format(parameter.ToString(), rd);
            }
            if(DateTime.TryParse(value.ToString(),out dt))
            {
                return dt.ToString(parameter.ToString());
            }
            return string.Format(parameter.ToString(), value);
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
            if (value == null)
            {
                return value;
            }
            int ri;
            double rd;
            if (int.TryParse(value.ToString(), out ri))
            {
                return ri;
            }
            if (double.TryParse(value.ToString(), out rd))
            {
                return rd;
            }
            return value.ToString();
        }

        #endregion
    }
}
