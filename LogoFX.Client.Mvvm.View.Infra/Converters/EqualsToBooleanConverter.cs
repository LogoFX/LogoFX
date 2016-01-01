using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    public class EqualsToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter != null)
            {
                return false;
            }

            if (value != null && parameter == null)
            {
                return false;
            }

            if (value == null && parameter == null)
            {
                return true;
            }

            object compareTo = null;

            if (value is Enum)
            {
                try
                {
                    compareTo = Enum.Parse(value.GetType(), (string)parameter, false);
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EqualsToBooleanConverter" + ex);
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
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}