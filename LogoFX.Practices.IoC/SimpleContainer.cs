﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogoFX.Practices.IoC
{
    /// <summary>
    ///   A simple IoC container.
    /// </summary>
    public class SimpleContainer
    {
        #region Nested Types

        private class ContainerEntry : List<Func<SimpleContainer, object>>
        {
            public string Key { get; set; }
            public Type Service { get; set; }
        }

        private class FactoryFactory<T>
        {
            public Func<T> Create(SimpleContainer container)
            {
                return () => (T)container.GetInstance(typeof(T), null);
            }
        }

        #endregion

        #region Fields

#if WinRT
        private static readonly TypeInfo delegateType = typeof(Delegate).GetTypeInfo();
        private static readonly TypeInfo enumerableType = typeof(IEnumerable).GetTypeInfo();
#else
        private static readonly Type s_delegateType = typeof(Delegate);
        private static readonly Type s_enumerableType = typeof(IEnumerable);
#endif

        private readonly List<ContainerEntry> _entries;

        #endregion

        #region Constructors
        /// <summary>
        ///   Initializes a new instance of the <see cref = "SimpleContainer" /> class.
        /// </summary>
        public SimpleContainer()
        {
            _entries = new List<ContainerEntry>();
        }

        private SimpleContainer(IEnumerable<ContainerEntry> entries)
        {
            _entries = new List<ContainerEntry>(entries);
        }

        #endregion

        #region Public Methods

        public void RegisterPerLifetime(Func<object> lifeTime, Type service, string key, Type implementation)
        {
            WeakReference wr = null;
            object singleton = null;
            RegisterHandler(service, key,
                            (container) =>
                            {
                                GC.Collect();
                                if (wr == null || !wr.IsAlive || singleton == null)
                                {
                                    wr = new WeakReference(lifeTime());
                                    singleton = BuildInstance(implementation);
                                }
                                return singleton;
                            });
        }

        /// <summary>
        ///   Registers the instance.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <param name = "implementation">The implementation.</param>
        public void RegisterInstance(Type service, string key, object implementation)
        {
            RegisterHandler(service, key, container => implementation);
        }

        /// <summary>
        ///   Registers the class so that a new instance is created on every request.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <param name = "implementation">The implementation.</param>
        public void RegisterPerRequest(Type service, string key, Type implementation)
        {
            RegisterHandler(service, key, container => container.BuildInstance(implementation));
        }

        /// <summary>
        ///   Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <param name = "implementation">The implementation.</param>
        public void RegisterSingleton(Type service, string key, Type implementation)
        {
            object singleton = null;
            RegisterHandler(service, key, container => singleton ?? (singleton = container.BuildInstance(implementation)));
        }

        /// <summary>
        ///   Registers a custom handler for serving requests from the container.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <param name = "handler">The handler.</param>
        public void RegisterHandler(Type service, string key, Func<SimpleContainer, object> handler)
        {
            GetOrCreateEntry(service, key).Add(handler);
        }


#if WinRT
        /// <summary>
        ///   Requests an instance.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <returns>The instance, or null if a handler is not found.</returns>
        public object GetInstance(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null)
            {
                return entry.Single()(this);
            }

            var serviceInfo = service.GetTypeInfo();

            if (delegateType.IsAssignableFrom(serviceInfo))
            {
                var typeToCreate = serviceInfo.GenericTypeArguments[0];
                var factoryFactoryType = typeof(FactoryFactory<>).MakeGenericType(typeToCreate);
                var factoryFactoryHost = Activator.CreateInstance(factoryFactoryType);
                var factoryFactoryMethod = factoryFactoryType.GetTypeInfo().DeclaredMethods.First(x => x.Name == "Create");
                return factoryFactoryMethod.Invoke(factoryFactoryHost, new object[] { this });
            }

            if (enumerableType.IsAssignableFrom(serviceInfo))
            {
                var listType = service.GenericTypeArguments[0];
                var instances = GetAllInstances(listType).ToList();
                var array = Array.CreateInstance(listType, instances.Count);

                for (var i = 0; i < array.Length; i++)
                {
                    array.SetValue(instances[i], i);
                }

                return array;
            }

            return null;
        }
#else
        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registere; false otherwise.</returns>
        public bool HasHandler(Type service, string key)
        {
            return GetEntry(service, key) != null;
        }

        /// <summary>
        ///   Requests an instance.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <param name = "key">The key.</param>
        /// <returns>The instance, or null if a handler is not found.</returns>
        public object GetInstance(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null)
            {
                //last registration always wins - this is a convention amongst ioc containers
                return entry.Last()(this);
            }

            if (s_delegateType.IsAssignableFrom(service))
            {
                var typeToCreate = service.GetGenericArguments()[0];
                var factoryFactoryType = typeof(FactoryFactory<>).MakeGenericType(typeToCreate);
                var factoryFactoryHost = Activator.CreateInstance(factoryFactoryType);
                var factoryFactoryMethod = factoryFactoryType.GetMethod("Create");
                return factoryFactoryMethod.Invoke(factoryFactoryHost, new object[] { this });
            }

            if (s_enumerableType.IsAssignableFrom(service))
            {
                var listType = service.GetGenericArguments()[0];
                var instances = GetAllInstances(listType).ToList();
                var array = Array.CreateInstance(listType, instances.Count);

                for (var i = 0; i < array.Length; i++)
                {
                    array.SetValue(instances[i], i);
                }

                return array;
            }

            return null;
        }
#endif

        /// <summary>
        ///   Requests all instances of a given type.
        /// </summary>
        /// <param name = "service">The service.</param>
        /// <returns>All the instances or an empty enumerable if none are found.</returns>
        public IEnumerable<object> GetAllInstances(Type service)
        {
            var entry = GetEntry(service, null);
            return entry != null ? entry.Select(x => x(this)) : new object[0];
        }

        /// <summary>
        ///   Pushes dependencies into an existing instance based on interface properties with setters.
        /// </summary>
        /// <param name = "instance">The instance.</param>
        public void BuildUp(object instance)
        {
#if WinRT
            var injectables = from property in instance.GetType().GetTypeInfo().DeclaredProperties
                              where property.CanRead && property.CanWrite && property.PropertyType.GetTypeInfo().IsInterface
                              select property;
#else
            var injectables = instance.GetType()
                .GetProperties()
                .Where(property => property.CanRead && property.CanWrite && property.PropertyType.IsInterface);
#endif
            foreach (var propertyInfo in injectables)
            {
                var injection = GetAllInstances(propertyInfo.PropertyType).ToArray();
                if (injection.Any())
                {
                    propertyInfo.SetValue(instance, injection.First(), null);
                }
            }
        }

        /// <summary>
        /// Creates a child container.
        /// </summary>
        /// <returns>A new container.</returns>
        public SimpleContainer CreateChildContainer()
        {
            return new SimpleContainer(_entries);
        }

        #endregion

        #region Events

        /// <summary>
        ///   Occurs when a new instance is created.
        /// </summary>
        public event Action<object> Activated = delegate { };

        #endregion

        #region Protected

        /// <summary>
        ///   Actually does the work of creating the instance and satisfying it's constructor dependencies.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        protected object BuildInstance(Type type)
        {
            var args = DetermineConstructorArgs(type);
            return ActivateInstance(type, args);
        }

        /// <summary>
        ///   Creates an instance of the type with the specified constructor arguments.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <param name = "args">The constructor args.</param>
        /// <returns>The created instance.</returns>
        protected virtual object ActivateInstance(Type type, object[] args)
        {
            var instance = args.Length > 0 ? Activator.CreateInstance(type, args) : Activator.CreateInstance(type);
            Activated(instance);
            return instance;
        }

        #endregion

        #region Private Members

        private ContainerEntry GetOrCreateEntry(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry == null)
            {
                entry = new ContainerEntry { Service = service, Key = key };
                _entries.Add(entry);
            }

            return entry;
        }

        private ContainerEntry GetEntry(Type service, string key)
        {
            if (service == null)
            {
                return _entries.FirstOrDefault(x => x.Key == key);
            }

            if (key == null)
            {
                return _entries.FirstOrDefault(x => x.Service == service && x.Key == null) ??
                       _entries.FirstOrDefault(x => x.Service == service);
            }

            return _entries.FirstOrDefault(x => x.Service == service && x.Key == key);
        }

        private object[] DetermineConstructorArgs(Type implementation)
        {
            var args = new List<object>();
            var constructor = SelectEligibleConstructor(implementation);

            if (constructor != null)
                args.AddRange(constructor.GetParameters().Select(info => GetInstance(info.ParameterType, null)));

            return args.ToArray();
        }

#if WinRT
        private static ConstructorInfo SelectEligibleConstructor(Type type)
        {
            return (from c in type.GetTypeInfo().DeclaredConstructors
                    orderby c.GetParameters().Length descending
                    select c).FirstOrDefault();
        }
#else
        private static ConstructorInfo SelectEligibleConstructor(Type type)
        {
            return (type.GetConstructors().OrderByDescending(c => c.GetParameters().Length)).FirstOrDefault();
        }
#endif
        #endregion

    }
}
