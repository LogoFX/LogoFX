using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    using DataErrorInfoDictionary = Dictionary<string, PropertyInfo>;

    partial class TypeInformationProvider
    {
        private static readonly Dictionary<Type, DataErrorInfoDictionary> DataErrorInfoSource =
            new Dictionary<Type, DataErrorInfoDictionary>(); 

        internal static bool IsPropertyDataErrorInfoSource(Type type, string propertyName)
        {
            return IsPropertyDataErrorInfoSourceImpl(type, propertyName);
        }

        private static bool IsPropertyDataErrorInfoSourceImpl(Type type, string propertyName)
        {
            if (DataErrorInfoSource.ContainsKey(type) == false)
            {
                AddDataErrorInfoDictionary(type);
            }
            return DataErrorInfoSource[type].ContainsKey(propertyName);
        }

        internal static object GetDataErrorInfoSourceValue(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyDataErrorInfoSourceImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return CalculateDataErrorInfoSourceValueBoxed(type, propertyName, propertyContainer);
            }
        }

        internal static IEnumerable<IDataErrorInfo> GetDataErrorInfoSourceValuesUnboxed(Type type, object propertyContainer)
        {
            if (DataErrorInfoSource.ContainsKey(type) == false)
            {
                AddDataErrorInfoDictionary(type);
            }
            return DataErrorInfoSource[type].Select(entry => GetValueUnboxed(type, entry.Key, propertyContainer));
        }

        private static IDataErrorInfo GetValueUnboxed(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyDataErrorInfoSourceImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return (IDataErrorInfo)CalculateDataErrorInfoSourceValueBoxed(type, propertyName, propertyContainer);
            }
        }

        private static object CalculateDataErrorInfoSourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return DataErrorInfoSource[type][propertyName].GetValue(propertyContainer);
        }

        private static void AddDataErrorInfoDictionary(Type type)
        {
            var props = type.GetProperties();
            var dataErrorInfoDictionary =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(IDataErrorInfo)))
                    .ToDictionary(t => t.Name, t => t);
            DataErrorInfoSource.Add(type, dataErrorInfoDictionary);
        }
    }
}
