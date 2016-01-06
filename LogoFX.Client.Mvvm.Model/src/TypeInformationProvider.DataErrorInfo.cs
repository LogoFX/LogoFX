using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LogoFX.Client.Mvvm.Model
{    
    partial class TypeInformationProvider
    {
        private static readonly IErrorInfoManager DataErrorInfoSource =
            new ConcurrentErrorInfoManager();

        public static IEnumerable<string> GetDataErrorInfoSources(Type type)
        {
            var props = type.GetProperties().ToArray();
            return props.Where(t => IsPropertyDataErrorInfoSourceInternal(type, t.Name)).Select(k => k.Name);
        }

        /// <summary>
        /// Determines whether property is an error source
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>True if property is an error source, false otherwise</returns>
        internal static bool IsPropertyDataErrorInfoSource(Type type, string propertyName)
        {
            return IsPropertyDataErrorInfoSourceInternal(type, propertyName);
        }

        private static bool IsPropertyDataErrorInfoSourceInternal(Type type, string propertyName)
        {
            return IsPropertyErrorInfoSourceInternal<IDataErrorInfo>(type, propertyName, DataErrorInfoSource);
        }

        /// <summary>
        /// Retrieves property value for error source properties
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Property value if found, null otherwise</returns>
        internal static object GetDataErrorInfoSourceValue(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyErrorInfoSourceInternal<IDataErrorInfo>(type, propertyName, DataErrorInfoSource);
            return containsProperty == false ? null : CalculateDataErrorInfoSourceValueBoxed(type, propertyName, propertyContainer);
        }

        /// <summary>
        /// Retrieves collection of error sources from the given property container
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Collection of error sources</returns>
        internal static IEnumerable<IDataErrorInfo> GetDataErrorInfoSourceValuesUnboxed(Type type, object propertyContainer)
        {
            return GetErrorInfoSourceValuesUnboxedInternal<IDataErrorInfo>(type, propertyContainer, DataErrorInfoSource);
        }

        private static object CalculateDataErrorInfoSourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return CalculateErrorInfoSourceValueBoxedInternal(type, propertyName, propertyContainer, DataErrorInfoSource);
        }
    }
}
