using Attest.Fake.Builders;
using LogoFX.Client.Tests.Contracts;

namespace LogoFX.Client.Tests.EndToEnd
{
    public class BuilderRegistrationService : IBuilderRegistrationService
    {
        public void RegisterBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            BuildersCollectionContext.AddBuilder(builder);
        }
    }
}
