using Caliburn.Micro;
using Solid.Fake.Core;
using Solid.Practices.IoC;
using Solid.Tests.NUnit;

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
