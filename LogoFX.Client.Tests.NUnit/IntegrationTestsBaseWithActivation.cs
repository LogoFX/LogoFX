using Attest.Fake.Core;
using Attest.Tests.Core;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit
{
    public abstract class IntegrationTestsBaseWithActivation<TContainer, TFakeFactory, TRootViewModel, TBootstrapper> :
        IntegrationTestsBase<TContainer, TFakeFactory, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new()
        where TFakeFactory : IFakeFactory, new()
        where TRootViewModel : class
    {
        protected IntegrationTestsBaseWithActivation(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            :base(resolutionStyle)
        {
            
        }

        protected override TRootViewModel CreateRootObjectOverride(TRootViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }        
    }
}