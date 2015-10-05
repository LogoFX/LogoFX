using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    using DataErrorInfoDictionary = Dictionary<string, PropertyInfo>;

    partial class TypeInformationProvider
    {
        private static readonly ConcurrentDictionary<Type, DataErrorInfoDictionary> NotifyDataErrorInfoSource =
            new ConcurrentDictionary<Type, DataErrorInfoDictionary>();

        /// <summary>
        /// Determines whether property is an error source
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>True if property is an error source, false otherwise</returns>
        internal static bool IsPropertyNotifyDataErrorInfoSource(Type type, string propertyName)
        {
            return IsPropertyNotifyDataErrorInfoSourceInternal(type, propertyName);
        }        

        /// <summary>
        /// Retrieves property value for error source properties
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Property value if found, null otherwise</returns>
        internal static object GetNotifyDataErrorInfoSourceValue(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyNotifyDataErrorInfoSourceInternal(type, propertyName);
            return containsProperty == false ? null : CalculateNotifyDataErrorInfoSourceValueBoxed(type, propertyName, propertyContainer);
        }

        private static bool IsPropertyNotifyDataErrorInfoSourceInternal(Type type, string propertyName)
        {
            return IsPropertyErrorInfoSourceInternal<INotifyDataErrorInfo>(type, propertyName, NotifyDataErrorInfoSource);
        }

        /// <summary>
        /// Retrieves collection of error sources from the given property container
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Collection of error sources</returns>
        internal static IEnumerable<INotifyDataErrorInfo> GetNotifyDataErrorInfoSourceValuesUnboxed(Type type, object propertyContainer)
        {
            return GetErrorInfoSourceValuesUnboxed<INotifyDataErrorInfo>(type, propertyContainer,
                NotifyDataErrorInfoSource);
        }

        private static INotifyDataErrorInfo GetNotifyValueUnboxed(Type type, string propertyName, object propertyContainer)
        {
            return GetValueUnboxedInternal<INotifyDataErrorInfo>(type, propertyName, propertyContainer,
                NotifyDataErrorInfoSource);
        }

        private static object CalculateNotifyDataErrorInfoSourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return CalculateErrorInfoSourceValueBoxedInternal(type, propertyName, propertyContainer,
                NotifyDataErrorInfoSource);
        }

        private static void AddNotifyDataErrorInfoDictionary(Type type)
        {
            AddErrorInfoDictionaryInternal<INotifyDataErrorInfo>(type, NotifyDataErrorInfoSource);
        }

        internal static IEnumerable<TErrorInfo> GetErrorInfoSourceValuesUnboxed<TErrorInfo>(Type type, object propertyContainer, ConcurrentDictionary<Type, DataErrorInfoDictionary> dictionary)
        {
            if (dictionary.ContainsKey(type) == false)
            {
                AddErrorInfoDictionaryInternal<TErrorInfo>(type, dictionary);
            }
            return
                dictionary[type].Select(
                    entry => GetValueUnboxedInternal<TErrorInfo>(type, entry.Key, propertyContainer, dictionary));
        }

        private static bool IsPropertyErrorInfoSourceInternal<TErrorInfo>(Type type, string propertyName, ConcurrentDictionary<Type, DataErrorInfoDictionary> dictionary)
        {
            if (dictionary.ContainsKey(type) == false)
            {
                AddErrorInfoDictionaryInternal<TErrorInfo>(type,dictionary);
            }
            return dictionary[type].ContainsKey(propertyName);
        }

        private static TErrorInfo GetValueUnboxedInternal<TErrorInfo>(Type type, string propertyName, object propertyContainer, ConcurrentDictionary<Type, DataErrorInfoDictionary> dictionary)
        {
            var containsProperty = IsPropertyErrorInfoSourceInternal<TErrorInfo>(type, propertyName, dictionary);
            return containsProperty == false
                ? default(TErrorInfo)
                : (TErrorInfo)
                    CalculateErrorInfoSourceValueBoxedInternal(type, propertyName, propertyContainer, dictionary);
        }

        private static object CalculateErrorInfoSourceValueBoxedInternal(Type type, string propertyName, object propertyContainer, ConcurrentDictionary<Type, DataErrorInfoDictionary> dictionary)
        {
            return dictionary[type][propertyName].GetValue(propertyContainer);
        }

        private static void AddErrorInfoDictionaryInternal<TErrorInfo>(Type type,
            ConcurrentDictionary<Type, DataErrorInfoDictionary> dictionary)
        {
            var props = type.GetProperties();
            var dataErrorInfoDictionary =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(TErrorInfo)))
                    .ToDictionary(t => t.Name, t => t);
            dictionary.TryAdd(type, dataErrorInfoDictionary);
        }
    }
}
