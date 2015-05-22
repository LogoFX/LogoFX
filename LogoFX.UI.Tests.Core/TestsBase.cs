using Attest.Fake.Core;
using Attest.Tests.Core;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.UI.Tests.Core
{
    public abstract class TestsBase<TContainer,TFakeFactory,TRootViewModel> : 
        IntegrationTestsBase<TContainer,TFakeFactory,TRootViewModel> 
        where TContainer : IIocContainer, new() 
        where TFakeFactory : IFakeFactory, new() 
        where TRootViewModel : class
    {
        protected override TRootViewModel CreateRootObjectOverride(TRootViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }
    }
}
