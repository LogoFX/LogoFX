using System;
using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<Type, DataErrorInfoDictionary> DirtySource =
            new ConcurrentDictionary<Type, DataErrorInfoDictionary>();

        /// <summary>
        /// Determines whether property is a dirty source
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>True if property is a dirty source, false otherwise</returns>
        internal static bool IsPropertyDirtySource(Type type, string propertyName)
        {
            return IsPropertyDirtySourceImpl(type, propertyName);
        }

        private static bool IsPropertyDirtySourceImpl(Type type, string propertyName)
        {
            DirtySource.AddIfMissing(type, GetDirtyDictionary);
            return DirtySource[type].ContainsKey(propertyName);
        }

        /// <summary>
        /// Retrieves property value for dirty source properties
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Property value if found, null otherwise</returns>
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

        /// <summary>
        /// Retrieves collection of dirty sources from the given property container
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Collection of dirty sources</returns>
        internal static IEnumerable<ICanBeDirty> GetDirtySourceValuesUnboxed(Type type, object propertyContainer)
        {
            DirtySource.AddIfMissing(type, GetDirtyDictionary);
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

        private static Dictionary<string, PropertyInfo> GetDirtyDictionary(Type type)
        {
            var props = type.GetProperties();
            return props
                .Where(t => t.PropertyType.GetInterfaces().Contains(typeof (ICanBeDirty)))
                .ToDictionary(t => t.Name, t => t);
        }

        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> DirtySourceCollection =
            new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// Retrieves collection of dirty source collections from the given property container
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="properyContainer">Property container</param>
        /// <returns>Collection of dirty source collections</returns>
        internal static IEnumerable<PropertyInfo> GetPropertyDirtySourceCollections(Type type, object properyContainer)
        {
            if (DirtySourceCollection.ContainsKey(type) == false)
            {
                DirtySourceCollection.TryAdd(type, CalculateDirtySourceCollectionProperties(type, properyContainer));
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

        /// <summary>
        /// Retrieves dirty source collections' values from the given property container in the format of flat list
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Dirty source values that originate in dirty source collections</returns>
        internal static IEnumerable<ICanBeDirty> GetDirtySourceCollectionsUnboxed(Type type, object propertyContainer)
        {
            if (DirtySourceCollection.ContainsKey(type) == false)
            {
                DirtySourceCollection.TryAdd(type, CalculateDirtySourceCollectionProperties(type, propertyContainer));
            }
            return DirtySourceCollection[type]
                .Select(propertyInfo => propertyInfo.GetValue(propertyContainer)).OfType<IEnumerable<IEditableModel>>()
                .SelectMany(dirtySourceCollection => dirtySourceCollection.ToArray());
        }

        private static bool IsPropertyDirtySourceCollection(PropertyInfo propertyInfo, object propertyContainer)
        {
            var isEnumerable = typeof(IEnumerable<ICanBeDirty>).IsAssignableFrom(propertyInfo.PropertyType);
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
