using System;
using System.Collections.Generic;
using System.Linq;
#if WinRT
using Windows.UI.Xaml.Data;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    static class EnumHelper
    {
        private static readonly IDictionary<Type, object[]> Cache = new Dictionary<Type, object[]>();

        public static object GetBoxed(System.Enum s)
        {
            Type enumType = s.GetType();
            object ret = GetValues(enumType).FirstOrDefault(ss => ss.ToString() == s.ToString());
            return ret;
        }
        public static T[] GetValues<T>()
        {
            Type enumType = typeof(T);

            if (enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            object[] values;
            if (!Cache.TryGetValue(enumType, out values))
            {
                values = (from field in enumType.GetFields()
                          where field.IsLiteral
                          select field.GetValue(enumType)).ToArray();
                Cache[enumType] = values;
            }
            return values.Cast<T>().ToArray();
        }

        public static object[] GetValues(Type enumType)
        {
#if WinRT
            if (!enumType.IsEnum())
#else
            if (!enumType.IsEnum)
#endif
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            object[] values;
            if (!Cache.TryGetValue(enumType, out values))
            {
                values = (from field in enumType.GetFields()
                          where field.IsLiteral
                          select field.GetValue(enumType)).ToArray();
                Cache[enumType] = values;
            }
            return values;
        }

    }

    /// <summary>
    /// Provides a way to fix weird behavior of selectors in Silverlight
    /// </summary>
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object o = EnumHelper.GetBoxed(value as System.Enum);
            return o;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? EnumHelper.GetValues(targetType)[0];
        }
    }

    /// <summary>
    /// Provides collection of <see cref="Enum"/> values based on one value
    /// </summary>
    public class EnumSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else
                return EnumHelper.GetValues(value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
