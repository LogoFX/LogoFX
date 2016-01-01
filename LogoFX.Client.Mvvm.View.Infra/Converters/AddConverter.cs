using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class AddConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Select(x => x as IConvertible).Select(System.Convert.ToInt32).Sum().ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}