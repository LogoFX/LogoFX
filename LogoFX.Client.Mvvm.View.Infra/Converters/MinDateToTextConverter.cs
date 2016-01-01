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
    public class MinDateToTextConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime)
            {
                if (((DateTime)value) == DateTime.MinValue)
                    return parameter.ToString();                
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}