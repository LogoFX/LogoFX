using Attest.Fake.Builders;
using Attest.Testing.xUnit;
using LogoFX.Client.Tests.Contracts;

namespace LogoFX.Client.Tests.Integration.xUnit
{
    /// <summary>
    /// Represents builder registration service for NUnit-based integration tests.
    /// </summary>
    /// <seealso cref="StepsBase" />
    /// <seealso cref="IBuilderRegistrationService" />
    public class BuilderRegistrationService : StepsBase, IBuilderRegistrationService
    {
        void IBuilderRegistrationService.RegisterBuilder<TService>(FakeBuilderBase<TService> builder)
        {
            RegisterBuilder(builder);
        }
    }
}
