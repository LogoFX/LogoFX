using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    using DataErrorInfoDictionary = Dictionary<string, PropertyInfo>;    

    interface IErrorInfoManager
    {
        bool ContainsType(Type type);
        void Add(Type key, Dictionary<string, PropertyInfo> dictionary);
        Dictionary<string, PropertyInfo> this[Type key] { get; }
    }

    class ConcurrentErrorInfoManager : IErrorInfoManager
    {
        private static readonly ConcurrentDictionary<Type, DataErrorInfoDictionary> Storage =
            new ConcurrentDictionary<Type, DataErrorInfoDictionary>();

        public bool ContainsType(Type type)
        {
            return Storage.ContainsKey(type);
        }

        public void Add(Type key, DataErrorInfoDictionary dictionary)
        {
            Storage.TryAdd(key, dictionary);
        }

        public DataErrorInfoDictionary this[Type key]
        {
            get { return Storage[key]; }
        }
    }
}