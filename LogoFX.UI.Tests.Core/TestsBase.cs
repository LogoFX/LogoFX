using Attest.Fake.Core;
using Attest.Tests.NUnit;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.UI.Tests.Core
{
    public abstract class TestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> : 
        IntegrationTestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> 
        where TContainer : IIocContainer, new() 
        where TFakeFactory : IFakeFactory, new() 
        where TRootViewModel : class
    {
        protected override TRootViewModel CreateRootObjectOverride(TRootViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }

        protected override void TearDownOverride()
        {
            base.TearDownOverride();
            AssemblySource.Instance.Clear();
        }
    }
}
