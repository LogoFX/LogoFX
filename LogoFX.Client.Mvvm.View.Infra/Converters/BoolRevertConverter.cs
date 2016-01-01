using System;

#if WinRT
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class BoolRevertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            {
                bool b = (bool) value;
                return !b;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
        //    throw new NotImplementedException();
            if (value is bool)
            {
                bool b = (bool)value;
                return !b;
            }
            return value;
        }
    }
}
