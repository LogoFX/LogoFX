using Attest.Fake.Core;
using Attest.Tests.Core;
using LogoFX.Client.Tests.Shared;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit
{
    public abstract class IntegrationTestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> : Attest.Tests.NUnit.IntegrationTestsBase<TContainer,TFakeFactory,TRootViewModel, TBootstrapper> 
        where TContainer : IIocContainer, new()        
        where TFakeFactory : IFakeFactory, new() 
        where TRootViewModel : class 
    {
        private readonly InitializationParametersResolutionStyle _resolutionStyle;

        protected IntegrationTestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
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
