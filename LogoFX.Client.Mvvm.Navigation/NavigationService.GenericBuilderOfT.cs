using System;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    internal sealed partial class NavigationService
    {
        private sealed class GenericBuilder<T> : RootableNavigationBuilder<T> where T : class
        {
            private readonly IIocContainer _container;

            public GenericBuilder(IIocContainer container)
            {
                _container = container;
            }

            protected override T GetValueInternal()
            {                
                return _container.Resolve<T>();
            }
        }

        private sealed class AttributeBuilder : NavigationBuilder
        {
            private readonly Type _vmType;
            private readonly IIocContainer _container;

            public AttributeBuilder(Type vmType, NavigationViewModelAttribute attr, IIocContainer container)
            {
                _vmType = vmType;
                _container = container;

                IsSingleton = attr.IsSingleton;
                ConductorType = attr.ConductorType;
                NotRemember = attr.NotRemember;
            }

            protected override object GetValue()
            {
                return _container.Resolve(_vmType);
            }
        }
    }
}