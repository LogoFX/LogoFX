using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    using ValidationInfoDictionary = Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>;

    using DataErrorInfoDictionary = Dictionary<string, PropertyInfo>;

    internal static class TypeInformationProvider
    {
        private static readonly Dictionary<Type, ValidationInfoDictionary> ValidationInfoSource =
            new Dictionary<Type, ValidationInfoDictionary>();

        private static readonly Dictionary<Type, DataErrorInfoDictionary> DataErrorInfoSource = 
            new Dictionary<Type, DataErrorInfoDictionary>();

        internal static Tuple<PropertyInfo, ValidationAttribute[]> GetValidationInfo(Type type, string propertyName)
        {
            if (ValidationInfoSource.ContainsKey(type) == false)
            {
                AddValidationInfoDictionary(type);
            }
            return ValidationInfoSource[type][propertyName];
        }

        internal static ValidationInfoDictionary GetValidationInfoCollection(Type type)
        {
            if (ValidationInfoSource.ContainsKey(type) == false)
            {
                AddValidationInfoDictionary(type);
            }
            return ValidationInfoSource[type];
        }

        private static void AddValidationInfoDictionary(Type type)
        {
            var validationInfoDictionary = new ValidationInfoDictionary();
            var props = type.GetProperties().ToArray();
            foreach (var propertyInfo in props)
            {
                var validationAttr =
                    propertyInfo.GetCustomAttributes(typeof (ValidationAttribute), true)
                        .Cast<ValidationAttribute>()
                        .ToArray();
                if (validationAttr.Length > 0)
                {
                    validationInfoDictionary.Add(propertyInfo.Name,
                        new Tuple<PropertyInfo, ValidationAttribute[]>(propertyInfo, validationAttr));
                }
            }
            ValidationInfoSource.Add(type, validationInfoDictionary);
        }

        internal static bool ContainsProperty(Type type, string propertyName)
        {
            return ContainsPropertyImpl(type, propertyName);
        }

        private static bool ContainsPropertyImpl(Type type, string propertyName)
        {
            if (DataErrorInfoSource.ContainsKey(type) == false)
            {
                AddDataErrorInfoDictionary(type);
            }
            return DataErrorInfoSource[type].ContainsKey(propertyName);
        }

        internal static object GetValue(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = ContainsPropertyImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return CalculateValueBoxed(type, propertyName, propertyContainer);          
            }
        }        

        internal static IEnumerable<IDataErrorInfo> GetValuesUnboxed(Type type, object propertyContainer)
        {
            if (DataErrorInfoSource.ContainsKey(type) == false)
            {
                AddDataErrorInfoDictionary(type);
            }
            return DataErrorInfoSource[type].Select(entry => GetValueUnboxed(type, entry.Key, propertyContainer));
        }

        private static IDataErrorInfo GetValueUnboxed(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = ContainsPropertyImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return (IDataErrorInfo)CalculateValueBoxed(type, propertyName, propertyContainer);
            }
        }

        private static object CalculateValueBoxed(Type type, string propertyName, object propertyContainer)
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
