using System;
using System.Globalization;
using System.Threading;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class CultureDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if WinRT
            IFormatProvider ifp = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
#else
            IFormatProvider ifp = Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
#endif
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                if (parameter != null)
                    return dt.ToString(parameter.ToString(), ifp);
                else
                    return dt.ToString(ifp);
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
