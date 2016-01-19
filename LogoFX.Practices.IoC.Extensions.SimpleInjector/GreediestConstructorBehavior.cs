using System;
using System.Linq;
using System.Reflection;
using SimpleInjector.Advanced;

namespace LogoFX.Practices.IoC.Extensions.SimpleInjector
{
    class GreediestConstructorBehavior : IConstructorResolutionBehavior
    {
        public ConstructorInfo GetConstructor(Type serviceType, Type implementationType)
        {
            return (
                from ctor in implementationType.GetConstructors()
                orderby ctor.GetParameters().Length descending
                select ctor)
                .First();
        }
    }
}