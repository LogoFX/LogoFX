using System;
using System.Globalization;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class AddConstantConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                parameter = 0;
            }

            try
            {
                return System.Convert.ToInt32(value) + System.Convert.ToInt32(parameter);
            }

            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                parameter = 0;
            }

            try
            {
                return System.Convert.ToInt32(value) - System.Convert.ToInt32(parameter);
            }

            catch (Exception)
            {
                return value;
            }
        }
    }
}