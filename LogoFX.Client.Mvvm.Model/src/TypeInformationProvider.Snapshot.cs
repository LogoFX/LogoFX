using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    partial class TypeInformationProvider
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> InnerDictionary = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Retrieves collection of storable properties for the given type
        /// </summary>
        /// <param name="type">Type of property container</param>
        /// <returns>Collection of storable properties</returns>
        internal static PropertyInfo[] GetStorableProperties(Type type)
        {
            InnerDictionary.TryAdd(type, GetStorablePropertiesImpl(type));                              
            return InnerDictionary[type];
        }

        private static PropertyInfo[] GetStorablePropertiesImpl(Type type)
        {
            var storableProperties = new HashSet<PropertyInfo>();
            var modelProperties = GetStorableCandidates(type).ToArray();
            foreach (var modelProperty in modelProperties)
            {
                storableProperties.Add(modelProperty);
            }
            var declaredInterfaces = type.GetInterfaces();
            var explicitProperties = declaredInterfaces.SelectMany(GetStorableCandidates);
            foreach (var explicitProperty in explicitProperties)
            {
                storableProperties.Add(explicitProperty);
            }
            return storableProperties.ToArray();
        }

        private static IEnumerable<PropertyInfo> GetStorableCandidates(Type modelType)
        {
            return modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        }
    }
}
