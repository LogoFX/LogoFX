using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    using ValidationInfoDictionary = Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>;

    partial class TypeInformationProvider
    {
        private static readonly ConcurrentDictionary<Type, ValidationInfoDictionary> ValidationInfoSource =
            new ConcurrentDictionary<Type, ValidationInfoDictionary>();

        internal static Tuple<PropertyInfo, ValidationAttribute[]> GetValidationInfo(Type type, string propertyName)
        {
            return GetValidationInfoImpl(type, propertyName);
        }

        internal static object GetValidationInfoValue(Type type, string propertyName, object propertyContainer)
        {
            var validationInfo = GetValidationInfoImpl(type, propertyName);
            return validationInfo.Item1.GetValue(propertyContainer);
        }

        private static Tuple<PropertyInfo, ValidationAttribute[]> GetValidationInfoImpl(Type type, string propertyName)
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
                    propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true)
                        .Cast<ValidationAttribute>()
                        .ToArray();
                if (validationAttr.Length > 0)
                {
                    validationInfoDictionary.Add(propertyInfo.Name,
                        new Tuple<PropertyInfo, ValidationAttribute[]>(propertyInfo, validationAttr));
                }
            }
            ValidationInfoSource.TryAdd(type, validationInfoDictionary);
        }
    }
}
