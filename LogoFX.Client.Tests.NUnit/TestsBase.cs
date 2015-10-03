using Attest.Fake.Core;
using Attest.Tests.Core;
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

        protected override void TearDownOverride()
        {
            base.TearDownOverride();
            if (_resolutionStyle == InitializationParametersResolutionStyle.PerRequest)
            {
                TestHelper.Teardown();
            }
        }
    }
}
