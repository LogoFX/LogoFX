using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Practices.IoC.Extensions.SimpleInjector;
using SimpleInjector;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Adapters.SimpleInjector
{
    /// <summary>
    /// Represents implementation of IoC container and bootstrapper adapter using Simple Injector
    /// </summary>
    public class SimpleInjectorAdapter : IIocContainer, IBootstrapperAdapter
    {
        private readonly Container _container = new Container();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorAdapter"/> class.
        /// </summary>
        public SimpleInjectorAdapter()
        {
            _container.Options.AllowOverridingRegistrations = true;
            _container.Options.RegisterModelParameterConvention();
            _container.RegisterSingleton(_container);
        }

        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {            
            RegisterTransient(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public void RegisterTransient<TService>() where TService : class
        {
            _container.Register<TService>(Lifestyle.Transient);
        }

        /// <summary>
        /// Registers service in transient lifetime style.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType);
        }

        /// <summary>
        /// Registers service in singleton lifetime style.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterSingleton(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Registers the singleton.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            _container.RegisterSingleton(serviceType, implementationType);
        }

        /// <summary>
        /// Registers the service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _container.RegisterSingleton(instance);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance(Type dependencyType, object instance)
        {
            _container.RegisterSingleton(dependencyType,instance);
        }

        /// <summary>
        /// Registers the dependency via the handler.
        /// </summary>
        /// <param name="dependencyType">Type of the dependency.</param><param name="handler">The handler.</param>
        public void RegisterHandler(Type dependencyType, Func<object> handler)
        {
            _container.Register(dependencyType, handler);
        }

        /// <summary>
        /// Registers the dependency via the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void RegisterHandler<TService>(Func<TService> handler) where TService : class
        {
            _container.Register(handler);
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public TService GetInstance<TService>(Type serviceType) where TService : class
        {
            return (TService)GetInstanceInternal(serviceType);
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public TService GetInstance<TService>() where TService : class
        {
            return (TService) GetInstanceInternal(typeof (TService));
        }

        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="key">Optional service key</param>
        /// <returns>Instance of service</returns>
        public object GetInstance(Type serviceType, string key)
        {
            return GetInstanceInternal(serviceType);                   
        }

        private object GetInstanceInternal(Type serviceType)
        {
            try
            {
                return _container.GetInstance(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>All instances of requested service</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            try
            {
                return _container.GetAllInstances(serviceType);
            }
            catch (ActivationException)
            {

                return new object[] {};
            }            
        }

        /// <summary>
        /// Resolves instance's dependencies and injects them into the instance
        /// </summary>
        /// <param name="instance">Instance to get injected with dependencies</param>
        public void BuildUp(object instance)
        {
            var producer = _container.GetRegistration(instance.GetType().BaseType, true);
            producer.Registration.InitializeInstance(instance);
        }

        /// <summary>
        /// Resolves the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public TService Resolve<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        /// <summary>
        /// Resolves the service. 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object Resolve(Type serviceType)
        {
            return _container.GetInstance(serviceType);
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
