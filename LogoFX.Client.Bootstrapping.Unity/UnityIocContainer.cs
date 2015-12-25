using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Microsoft.Practices.Unity;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Unity
{
    public class UnityIocContainer : IIocContainer, IBootstrapperAdapter
    {
        private readonly UnityContainer _container = new UnityContainer();

        public UnityIocContainer()
        {
            _container.RegisterInstance(_container);
        }        

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterType<TService, TImplementation>();
        }

        public void RegisterTransient<TService>() where TService : class
        {
            _container.RegisterType<TService>();
        }

        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _container.RegisterType(serviceType, implementationType);
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterType<TService, TImplementation>(new ContainerControlledLifetimeManager());
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public TService GetInstance<TService>(Type serviceType) where TService : class
        {
            return (TService)_container.Resolve(serviceType);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }

        public void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        public void Dispose()
        {
            ((IDisposable) _container).Dispose();
        }
    }
}
