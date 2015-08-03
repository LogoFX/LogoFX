using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LogoFX.Practices.Modularity;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.SimpleContainer.Tests
{
    [Export(typeof(ICompositionModule))]
    class TestSimpleModule : ISimpleModule
    {
        public void RegisterModule(Practices.IoC.SimpleContainer container, Func<object> lifetimeScopeAccess)
        {
            container.RegisterPerLifetime(lifetimeScopeAccess, typeof (ITestSimpleDependency), null,
                typeof (TestSimpleDependency));
        }
    }

    interface ITestSimpleDependency
    {
        
    }

    class TestSimpleDependency : ITestSimpleDependency
    {
        
    }

    class TestRootViewModel : PropertyChangedBase
    {
        
    }
}