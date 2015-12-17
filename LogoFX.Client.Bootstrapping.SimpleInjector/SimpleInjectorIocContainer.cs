﻿using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Contracts;
using SimpleInjector;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.SimpleInjector
{
    public class SimpleInjectorContainer : IIocContainer, IBootstrapperAdapter
    {
        private readonly Container _container = new Container();

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            RegisterTransient(typeof(TService), typeof(TImplementation));
        }

        public void RegisterTransient<TService>() where TService : class
        {
            _container.Register<TService>(Lifestyle.Transient);
        }

        public void RegisterTransient(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType);
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.RegisterSingleton(typeof(TService), typeof(TImplementation));
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _container.RegisterSingleton(instance);
        }

        public TService GetInstance<TService>(Type serviceType) where TService : class
        {
            return (TService)_container.GetInstance(serviceType);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public object GetInstance(Type serviceType)
        {            
            return _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public void BuildUp(object instance)
        {
            var producer = _container.GetRegistration(instance.GetType().BaseType, true);
            producer.Registration.InitializeInstance(instance);
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public void Dispose()
        {
            ((IDisposable) _container).Dispose();
        }
    }
}
