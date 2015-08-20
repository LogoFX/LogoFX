using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> DirtySourceCollection =
            new Dictionary<Type, IEnumerable<PropertyInfo>>();

        internal static IEnumerable<PropertyInfo> GetPropertyDirtySourceCollections(Type type, object properyContainer)
        {
            if (DirtySourceCollection.ContainsKey(type) == false)
            {
                DirtySourceCollection.Add(type, CalculateDirtySourceCollectionProperties(type, properyContainer));
            }
            return DirtySourceCollection[type];
        }

        private static IEnumerable<PropertyInfo> CalculateDirtySourceCollectionProperties(Type type, object propertyContainer)
        {
            var props = type.GetProperties();
            // ReSharper disable once LoopCanBeConvertedToQuery - Becomes unreadable
            foreach (var prop in props)
            {
                var isDirtySourceCollection = IsPropertyDirtySourceCollection(prop, propertyContainer);
                if (isDirtySourceCollection)
                {
                    yield return prop;
                }
            }
        }

        internal static IEnumerable<ICanBeDirty> GetDirtySourceCollectionsUnboxed(Type type, object propertyContainer)
        {
            if (DirtySourceCollection.ContainsKey(type) == false)
            {
                DirtySourceCollection.Add(type, CalculateDirtySourceCollectionProperties(type, propertyContainer));
            }
            return DirtySourceCollection[type]
                .Select(propertyInfo => propertyInfo.GetValue(propertyContainer)).OfType<IEnumerable<IEditableModel>>()
                .SelectMany(dirtySourceCollection => dirtySourceCollection.ToArray());
        }

        private static bool IsPropertyDirtySourceCollection(PropertyInfo propertyInfo, object propertyContainer)
        {
            var isEnumerable = typeof (IEnumerable<ICanBeDirty>).IsAssignableFrom(propertyInfo.PropertyType);
            if (isEnumerable == false)
            {
                return false;
            }            
            var actualValue = propertyInfo.GetValue(propertyContainer);
            var isTraceable = actualValue is INotifyCollectionChanged;                             
            return isTraceable;
        }
    }    
}
