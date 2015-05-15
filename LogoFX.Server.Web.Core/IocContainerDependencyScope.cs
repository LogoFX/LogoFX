using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using LogoFX.Practices.IoC;

namespace LogoFX.Web.Core
{
    public class IocContainerDependencyScope : IDependencyScope
    {
        private readonly IIocContainer _container;
        private readonly object _scope;

        public IocContainerDependencyScope(IIocContainer container)
        {
            _container = container;
            //_scope = container.
        }


        public void Dispose()
        {
            // TODO: Currently isn't applicable since SimpleContainer doesn't provide corresponding functionality
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }
    }
}