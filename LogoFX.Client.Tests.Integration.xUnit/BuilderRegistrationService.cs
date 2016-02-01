using Attest.Fake.Builders;
using LogoFX.Client.Tests.Contracts;

namespace LogoFX.Client.Tests.Integration.xUnit
{
    /// <summary>
    /// Represents builder registration service for NUnit-based integration tests.
    /// </summary>
    /// <seealso cref="Attest.Tests.xUnit.StepsBase" />
    /// <seealso cref="IBuilderRegistrationService" />
    public class BuilderRegistrationService : Attest.Tests.xUnit.StepsBase, IBuilderRegistrationService
    {
        void IBuilderRegistrationService.RegisterBuilder<TService>(FakeBuilderBase<TService> builder)
        {
            RegisterBuilder(builder);
        }
    }
}
