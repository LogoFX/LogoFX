using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{    
     using DataErrorInfoDictionary = Dictionary<string, PropertyInfo>;

    partial class TypeInformationProvider
    {
        private static readonly Dictionary<Type, DataErrorInfoDictionary> DirtySource =
            new Dictionary<Type, DataErrorInfoDictionary>(); 

        internal static bool IsPropertyDirtySource(Type type, string propertyName)
        {
            return IsPropertyDirtySourceImpl(type, propertyName);
        }

        private static bool IsPropertyDirtySourceImpl(Type type, string propertyName)
        {
            if (DirtySource.ContainsKey(type) == false)
            {
                AddDirtyDictionary(type);
            }
            return DirtySource[type].ContainsKey(propertyName);
        }

        internal static object GetDirtySourceValue(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyDirtySourceImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return CalculateDirtySourceValueBoxed(type, propertyName, propertyContainer);
            }
        }

        internal static IEnumerable<ICanBeDirty> GetDirtySourceValuesUnboxed(Type type, object propertyContainer)
        {
            if (DirtySource.ContainsKey(type) == false)
            {
                AddDirtyDictionary(type);
            }
            return DirtySource[type].Select(entry => GetDirtySourceValueUnboxed(type, entry.Key, propertyContainer));
        }

        private static ICanBeDirty GetDirtySourceValueUnboxed(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyDirtySourceImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return (ICanBeDirty)CalculateDirtySourceValueBoxed(type, propertyName, propertyContainer);
            }
        }

        private static object CalculateDirtySourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return DirtySource[type][propertyName].GetValue(propertyContainer);
        }

        private static void AddDirtyDictionary(Type type)
        {
            var props = type.GetProperties();
            var dirtySourceDictionary =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(ICanBeDirty)))
                    .ToDictionary(t => t.Name, t => t);
            DirtySource.Add(type, dirtySourceDictionary);
        }
    }
    
}
