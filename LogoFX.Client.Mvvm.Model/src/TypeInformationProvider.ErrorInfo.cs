using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    partial class TypeInformationProvider
    {
        public static IEnumerable<PropertyInfo> GetPropertyErrorInfoSources<T>(Type type, Model<T> model) where T : IEquatable<T>
        {
            throw new NotImplementedException();
        }
    }
}
