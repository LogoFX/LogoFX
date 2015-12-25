using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Practices.IoC;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Adapters.SimpleContainer
{
    /// <summary>
    /// Represents implementation of IoC container and bootstrapper adapter using Extended Simple Container
    /// </summary>
    public class ExtendedSimpleIocContainer : IIocContainer, IBootstrapperAdapter
    {
        private readonly ExtendedSimpleContainer _container = new ExtendedSimpleContainer();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSimpleIocContainer"/> class.
        /// </summary>
        public ExtendedSimpleIocContainer()
        {
            _container.RegisterInstance(typeof(ExtendedSimpleContainer), null, _container);
            _container.RegisterInstance(typeof(Practices.IoC.SimpleContainer), null, _container);
        }
        
        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <typeparam name="TImplementation">Type of implementation</typeparam>
        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterPerRequest(typeof(TService), null, typeof(TImplementation));
        }

        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public void RegisterTransient<TService>() where TService : class
        {
            RegisterTransient<TService, TService>();
        }

        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _container.RegisterPerRequest(serviceType, null, implementationType);
        }

        /// <summary>
        /// Registers service in singleton lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterSingleton(typeof(TService), null, typeof(TImplementation));
        }

        /// <summary>
        /// Registers service in singleton lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterSingleton<TService, TImplementation>(string key) where TImplementation : class, TService
        {
            _container.RegisterSingleton(typeof(TService), key, typeof(TImplementation));
        }

        /// <summary>
        /// Registers the instance of the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _container.RegisterInstance(typeof(TService), null, instance);
        }

        /// <summary>
        /// Determines whether the specified service has handler.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool HasHandler(Type service, string key)
        {
            return _container.HasHandler(service, key);
        }

        /// <summary>
        /// Registers the handler.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void RegisterHandler(Type service, string key, Func<Practices.IoC.SimpleContainer, object> handler)
        {
            _container.RegisterHandler(service,key,(container,args) => handler(container));
        }

        /// <summary>
        /// Registers the service in external object scope lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="lifetimeScopeAccess">The lifetime scope access.</param>
        public void RegisterPerLifetime<TService, TImplementation>(Func<object> lifetimeScopeAccess)
        {
            _container.RegisterPerLifetime(lifetimeScopeAccess,typeof(TService), null, typeof(TImplementation));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public TService GetInstance<TService>(Type serviceType) where TService : class
        {
            return (TService)_container.GetInstance(serviceType, null);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public TService GetInstance<TService>() where TService : class
        {
            return GetInstance<TService>(typeof(TService));
        }

        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="key">Optional service key</param>
        /// <returns>Instance of service</returns>
        public object GetInstance(Type serviceType, string key)
        {
            return _container.GetInstance(serviceType, null);
        }

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>All instances of requested service</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        /// <summary>
        /// Resolves instance's dependencies and injects them into the instance
        /// </summary>
        /// <param name="instance">Instance to get injected with dependencies</param>
        public void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public TService Resolve<TService>() where TService : class
        {
            return GetInstance<TService>();
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable) _container).Dispose();
        }
    }
}