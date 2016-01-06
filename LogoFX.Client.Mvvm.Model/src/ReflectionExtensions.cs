using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    static class ReflectionExtensions
    {
        internal static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return type
#if NET45
                .GetProperties()
#else
                .GetTypeInfo().DeclaredProperties
#endif
                ;
        }

        internal static IEnumerable<PropertyInfo> GetProperties(this Type type, BindingFlags flags)
        {
            return type
#if NET45
                .GetProperties(flags)
#else
                .GetRuntimeProperties()
#endif
                ;
        }

        internal static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type
#if NET45
                .GetInterfaces()
#else
                .GetTypeInfo()
                .ImplementedInterfaces
#endif
                ;
        }
    }
}
