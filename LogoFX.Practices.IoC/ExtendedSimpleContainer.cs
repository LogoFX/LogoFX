using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Represents <see cref="SimpleContainer" /> with advanaced capabilities
    /// </summary>
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

        /// <summary>
        /// Autoregisters the specified service using optional key and implementation type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">kind</exception>
        public void RegisterAuto(Type service, string key, Type implementation)
        {
            var kind = GetKind(implementation);
            switch (kind)
            {
                case RegisterAsKind.PerRequest:
                    lock (_syncObject)
                    {
                        _types.Add(implementation, RegisterAsKind.PerRequest);    
                    }                    
                    RegisterPerRequest(service, key, implementation);
                    break;
                case RegisterAsKind.Singleton:
                    lock (_syncObject)
                    {
                        _types.Add(implementation, RegisterAsKind.Singleton);                        
                    }
                    RegisterSingleton(service, key, implementation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        private readonly object _syncObject = new object();

        /// <summary>
        ///   Creates an instance of the type with the specified constructor arguments.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <param name = "args">The constructor args.</param>
        /// <returns>The created instance.</returns>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            base.Dispose();
            foreach (var singleton in _singletones)
            {
                var disposable = singleton.Value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }    
}
