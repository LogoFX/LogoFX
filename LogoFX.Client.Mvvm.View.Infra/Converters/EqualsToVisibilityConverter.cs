using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class EqualsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter != null)
                return Visibility.Collapsed;
            if (value != null && parameter == null)
                return Visibility.Collapsed;
            if (value == null && parameter == null)
                return Visibility.Visible;

            object compareTo = null;
            if (value is Enum)
            {
                try
                {
                    compareTo = Enum.Parse(value.GetType(), (string)parameter, false);
                }
                catch (Exception)
                {
                }
            }
            else
            {
#if !WinRT
                compareTo = (from TypeConverterAttribute customAttribute in value.GetType().GetCustomAttributes(typeof(TypeConverterAttribute), true)
                             select (TypeConverter)Activator.CreateInstance(Type.GetType(customAttribute.ConverterTypeName))
                                 into tc
                                 where tc.CanConvertFrom(typeof(string))
                                 select tc.ConvertFrom(parameter)).FirstOrDefault();
#endif
            }


            if (value.Equals(compareTo))
                return Visibility.Visible;

            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
