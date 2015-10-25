using Attest.Fake.Core;
using Attest.Tests.Core;
using Attest.Tests.SpecFlow;
using LogoFX.Client.Tests.Shared;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.SpecFlow
{
    public abstract class TestsBase<TContainer, TFakeFactory, TRootViewModel, TBootstrapper> :
        IntegrationTestsBase<TContainer, TFakeFactory, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new()
        where TFakeFactory : IFakeFactory, new()
        where TRootViewModel : class
    {
        private readonly InitializationParametersResolutionStyle _resolutionStyle;

        protected TestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            :base(resolutionStyle)
        {
            _resolutionStyle = resolutionStyle;
        }

        protected override void SetupOverride()
        {
            base.SetupOverride();
            TestHelper.Setup();
        }

        protected override void OnAfterTeardown()
        {
            base.OnAfterTeardown();
            if (_resolutionStyle == InitializationParametersResolutionStyle.PerRequest)
            {
                TestHelper.Teardown();    
            }            
        }
    }
}
