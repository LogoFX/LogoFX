using System;
using Caliburn.Micro;

namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService
    {
        private sealed class GenericBuilder<T> : RootableNavigationBuilder<T>
        {
            protected override T GetValueInternal()
            {
                return IoC.Get<T>();
            }
        }

        private sealed class AttributeBuilder : NavigationBuilder
        {
            private readonly Type _vmType;
            
            public AttributeBuilder(Type vmType, NavigationViewModelAttribute attr)
            {
                _vmType = vmType;

                IsSingleton = attr.IsSingleton;
                ConductorType = attr.ConductorType;
                NotRemember = attr.NotRemember;
            }

            protected override object GetValue()
            {
                return IoC.GetInstance(_vmType, null);
            }
        }
    }
}