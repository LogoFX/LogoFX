using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Practices.IoC
{
    public class ExtendedSimpleContainer : SimpleContainer
    {
        private enum RegisterAsKind
        {
            PerRequest,
            Singleton,
        }        

        private readonly Dictionary<Type, RegisterAsKind> _types =
            new Dictionary<Type, RegisterAsKind>();

        private readonly WeakDictionary<Type, object> _singletones =
            new WeakDictionary<Type, object>();

        public void RegisterAuto(Type service, string key, Type implementation)
        {
            var kind = GetKind(implementation);
            switch (kind)
            {
                case RegisterAsKind.PerRequest:
                    _types.Add(implementation, RegisterAsKind.PerRequest);
                    RegisterPerRequest(service, key, implementation);
                    break;
                case RegisterAsKind.Singleton:
                    _types.Add(implementation, RegisterAsKind.Singleton);
                    RegisterSingleton(service, key, implementation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        private object _syncObject = new object();

        protected override object ActivateInstance(Type type, object[] args)
        {
            object result;
            if (_singletones.TryGetValue(type, out result))
            {
                return result;
            }

            RegisterAsKind kind;
            lock (_syncObject)
            {                
                if (!_types.TryGetValue(type, out kind))
                {
                    kind = GetKind(type);
                    _types.Add(type, kind);
                }    
            }
            
            result = base.ActivateInstance(type, args);

            if (kind == RegisterAsKind.Singleton)
            {
                _singletones.Add(type, result);
            }

            return result;
        }

        private RegisterAsKind GetKind(Type type)
        {
            var attr = type.GetCustomAttribute<SingletonAttribute>();

            if (attr != null)
            {
                return RegisterAsKind.Singleton;
            }

            return RegisterAsKind.PerRequest;
        }
    }    
}
