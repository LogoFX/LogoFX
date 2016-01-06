using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LogoFX.Client.Mvvm.Model
{    
    partial class TypeInformationProvider
    {
        private static readonly IErrorInfoManager NotifyDataErrorInfoSource =
            new ConcurrentErrorInfoManager();

        public static IEnumerable<string> GetNotifyDataErrorInfoSources(Type type)
        {
            var props = type.GetProperties().ToArray();
            return props.Where(t => IsPropertyNotifyDataErrorInfoSourceInternal(type, t.Name)).Select(k => k.Name);
        }

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
            return GetErrorInfoSourceValuesUnboxedInternal<INotifyDataErrorInfo>(type, propertyContainer,
                NotifyDataErrorInfoSource);
        }

        private static object CalculateNotifyDataErrorInfoSourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return CalculateErrorInfoSourceValueBoxedInternal(type, propertyName, propertyContainer,
                NotifyDataErrorInfoSource);
        }

        private static IEnumerable<TErrorInfo> GetErrorInfoSourceValuesUnboxedInternal<TErrorInfo>(
            Type type, 
            object propertyContainer, 
            IErrorInfoManager errorInfoManager)
        {
            if (errorInfoManager.ContainsType(type) == false)
            {
                AddErrorInfoDictionaryInternal<TErrorInfo>(type, errorInfoManager);
            }
            return
                errorInfoManager[type].Select(
                    entry => GetValueUnboxedInternal<TErrorInfo>(type, entry.Key, propertyContainer, errorInfoManager));
        }

        private static bool IsPropertyErrorInfoSourceInternal<TErrorInfo>(Type type, string propertyName, IErrorInfoManager errorInfoManager)
        {
            if (errorInfoManager.ContainsType(type) == false)
            {
                AddErrorInfoDictionaryInternal<TErrorInfo>(type,errorInfoManager);
            }
            return errorInfoManager[type].ContainsKey(propertyName);
        }

        private static TErrorInfo GetValueUnboxedInternal<TErrorInfo>(
            Type type, 
            string propertyName, 
            object propertyContainer, 
            IErrorInfoManager errorInfoManager)
        {
            var containsProperty = IsPropertyErrorInfoSourceInternal<TErrorInfo>(type, propertyName, errorInfoManager);
            return containsProperty == false
                ? default(TErrorInfo)
                : (TErrorInfo)
                    CalculateErrorInfoSourceValueBoxedInternal(type, propertyName, propertyContainer, errorInfoManager);
        }

        private static object CalculateErrorInfoSourceValueBoxedInternal(
            Type type, 
            string propertyName, 
            object propertyContainer, 
            IErrorInfoManager errorInfoManager
            )
        {
            return errorInfoManager[type][propertyName].GetValue(propertyContainer);
        }               
    }
}
