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
    public class SubConstantConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = 0;

            Decimal dec = System.Convert.ToDecimal(value);

            return dec - System.Convert.ToDecimal(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                parameter = 0;

            return System.Convert.ToDecimal(value) + System.Convert.ToDecimal(parameter);
        }
    }
}
