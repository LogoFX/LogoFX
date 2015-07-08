using Attest.Fake.Core;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit
{
    public abstract class TestsBaseWithActivation<TContainer, TFakeFactory, TRootViewModel, TBootstrapper> :
        TestsBase<TContainer, TFakeFactory, TRootViewModel, TBootstrapper>
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