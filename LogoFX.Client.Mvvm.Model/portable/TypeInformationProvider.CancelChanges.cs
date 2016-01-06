using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    partial class TypeInformationProvider
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> CancelChangesSource =
            new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

        private static bool IsPropertyCanCancelChangesSourceImpl(Type type, string propertyName)
        {
            CancelChangesSource.AddIfMissing(type, GetCanCancelChangesDictionary);
            return CancelChangesSource[type].ContainsKey(propertyName);
        }

        /// <summary>
        /// Retrieves collection of dirty sources from the given property container
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <param name="propertyContainer">Property container</param>
        /// <returns>Collection of <see cref="ICanCancelChanges"/> sources</returns>
        internal static IEnumerable<ICanCancelChanges> GetCanCancelChangesSourceValuesUnboxed(Type type, object propertyContainer)
        {
            CancelChangesSource.AddIfMissing(type, GetCanCancelChangesDictionary);
            return CancelChangesSource[type].Select(entry => GetCanCancelChangesSourceValueUnboxed(type, entry.Key, propertyContainer));
        }        

        private static ICanCancelChanges GetCanCancelChangesSourceValueUnboxed(Type type, string propertyName, object propertyContainer)
        {
            var containsProperty = IsPropertyCanCancelChangesSourceImpl(type, propertyName);
            if (containsProperty == false)
            {
                //TODO: consider throwing an exception
                return null;
            }
            else
            {
                return (ICanCancelChanges)CalculateCanCancelChangesSourceValueBoxed(type, propertyName, propertyContainer);
            }
        }

        private static object CalculateCanCancelChangesSourceValueBoxed(Type type, string propertyName, object propertyContainer)
        {
            return CancelChangesSource[type][propertyName].GetValue(propertyContainer);
        }

        private static Dictionary<string, PropertyInfo> GetCanCancelChangesDictionary(Type type)
        {
            var props = type.GetProperties();
            return props
                .Where(t => t.PropertyType
                .GetInterfaces()
                .Contains(typeof (ICanCancelChanges)))
                .ToDictionary(t => t.Name, t => t);
        }

        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> CancelChangesSourceCollection =
            new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

        private static IEnumerable<PropertyInfo> CalculateCanCancelChangesSourceCollectionProperties(Type type, object propertyContainer)
        {
            var props = type.GetProperties();
                ;
            // ReSharper disable once LoopCanBeConvertedToQuery - Becomes unreadable
            foreach (var prop in props)
            {
                var isCancelChangesSourceCollection = IsPropertyCanCancelChangesSourceCollection(prop, propertyContainer);
                if (isCancelChangesSourceCollection)
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
        internal static IEnumerable<ICanCancelChanges> GetCanCancelChangesSourceCollectionsUnboxed(Type type, object propertyContainer)
        {
            if (CancelChangesSourceCollection.ContainsKey(type) == false)
            {
                CancelChangesSourceCollection.TryAdd(type, CalculateCanCancelChangesSourceCollectionProperties(type, propertyContainer));
            }
            return CancelChangesSourceCollection[type]
                .Select(propertyInfo => propertyInfo.GetValue(propertyContainer)).OfType<IEnumerable<IEditableModel>>()
                .SelectMany(sourceCollection => sourceCollection.ToArray());
        }

        private static bool IsPropertyCanCancelChangesSourceCollection(PropertyInfo propertyInfo, object propertyContainer)
        {
            var isEnumerable = typeof (IEnumerable<ICanCancelChanges>).IsAssignableFrom(propertyInfo.PropertyType);
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