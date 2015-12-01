using Attest.Fake.Core;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.SpecFlow
{
    public abstract class EndToEndTestsBase<TContainer, TFakeFactory> :
        Attest.Tests.SpecFlow.EndToEndTestsBase<TContainer, TFakeFactory>
        where TContainer : IIocContainer, new()
        where TFakeFactory : IFakeFactory, new()
    {
        protected EndToEndTestsBase(TContainer container) : base(container)
        {
        }
    }
}