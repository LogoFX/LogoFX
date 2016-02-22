using System;
using Attest.Fake.Setup;
using Attest.Fake.Setup.Contracts;

namespace LogoFX.Client.Data.Fake.ProviderBuilders
{
    [Serializable]
    public abstract class FakeBuilderBase<TProvider> : Attest.Fake.Builders.FakeBuilderBase<TProvider> where TProvider : class
    {
        protected FakeBuilderBase() : base(FakeFactoryContext.Current)
        {
        }

        protected IHaveNoMethods<TProvider> CreateInitialSetup()
        {
            return ServiceCallFactory.CreateServiceCall(FakeService);
        }
    }
}
