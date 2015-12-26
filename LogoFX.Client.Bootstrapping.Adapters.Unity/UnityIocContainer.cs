using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Microsoft.Practices.Unity;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Adapters.Unity
{
    /// <summary>
    /// Represents implementation of IoC container and bootstrapper adapter using Unity
    /// </summary>
    public class UnityIocContainer : IIocContainer, IBootstrapperAdapter
    {
        private readonly UnityContainer _container = new UnityContainer();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityIocContainer"/> class.
        /// </summary>
        public UnityIocContainer()
        {
            _container.RegisterInstance(_container);
        }

        /// <summary>
        /// Registers dependency in a transient lifetime style
        /// </summary>
        /// <typeparam name="TService">Type of dependency declaration</typeparam><typeparam name="TImplementation">Type of dependency implementation</typeparam>
        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterType<TService, TImplementation>();
        }

        /// <summary>
        /// Registers dependency in a transient lifetime style
        /// </summary>
        /// <typeparam name="TService">Type of dependency</typeparam>
        public void RegisterTransient<TService>() where TService : class
        {
            _container.RegisterType<TService>();
        }

        /// <summary>
        /// Registers dependency in a transient lifetime style
        /// </summary>
        /// <param name="serviceType">Type of dependency declaration</param><param name="implementationType">Type of dependency implementation</param>
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _container.RegisterType(serviceType, implementationType);
        }

        /// <summary>
        /// Registers dependency as a singleton
        /// </summary>
        /// <typeparam name="TService">Type of dependency declaration</typeparam><typeparam name="TImplementation">Type of dependency implementation</typeparam>
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Registers an instance of dependency
        /// </summary>
        /// <typeparam name="TService">Type of dependency</typeparam><param name="instance">Instance of dependency</param>
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Resolves an instance of service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns/>
        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        /// <summary>
        /// Resolves an instance of service according to the service type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns/>
        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }        

        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="key">Optional service key</param>
        /// <returns>Instance of service</returns>
        public object GetInstance(Type serviceType, string key)
        {
            return _container.Resolve(serviceType);
        }

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>All instances of requested service</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>        
        public void Dispose()
        {
            ((IDisposable) _container).Dispose();
        }
    }
}
