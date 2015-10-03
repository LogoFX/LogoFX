using Attest.Fake.Core;
using Attest.Tests.NUnit;
using LogoFX.Client.Tests.Shared;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit
{
    public abstract class TestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> : 
        IntegrationTestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> 
        where TContainer : IIocContainer, new()        
        where TFakeFactory : IFakeFactory, new() 
        where TRootViewModel : class 
    {
        protected override void SetupOverride()
        {
            base.SetupOverride();
            TestHelper.Setup();
        }

        protected override void TearDownOverride()
        {
            base.TearDownOverride();
            TestHelper.Teardown();
        }
    }
}
