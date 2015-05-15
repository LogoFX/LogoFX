#region Copyright

// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

#endregion

#if !WinRT
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace LogoFX.UI.Views.Infra.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;

            Type t = value.GetType();
            DescriptionAttribute displayAttribute = null;
            displayAttribute = (DescriptionAttribute)value.GetType()
                                                          .GetField(value.ToString())
                                                          .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                          .FirstOrDefault();

            if (displayAttribute == null)
            {
                return value;
            }
            else
            {
                return displayAttribute.Description;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
#endif