using Attest.Fake.Builders;

namespace LogoFX.Client.Tests.Contracts
{
    public interface IBuilderRegistrationService
    {
        void RegisterBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class;
    }
}
