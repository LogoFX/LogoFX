using System;
using Attest.Fake.Setup;
using Attest.Fake.Setup.Contracts;

namespace LogoFX.Client.Data.Fake.ProviderBuilders
{
    /// <summary>
    /// Base provider builder with initial setup functionality.
    /// </summary>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    [Serializable]
    public abstract class FakeBuilderBase<TProvider> : Attest.Fake.Builders.FakeBuilderBase<TProvider> where TProvider : class
    {
        /// <summary>
        /// Creates initial template for the fake setup.
        /// </summary>
        /// <returns></returns>
        protected IHaveNoMethods<TProvider> CreateInitialSetup()
        {
            return ServiceCallFactory.CreateServiceCall(FakeService);
        }
    }
}
