using Attest.Fake.Builders;

namespace LogoFX.Client.Tests.Contracts
{

    /// <summary>
    /// The Builders should be registered via this API
    /// </summary>
    public interface IBuilderRegistrationService
    {
        /// <summary>
        /// Registers the builder into IoC container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="builder">The builder.</param>
        void RegisterBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class;
    }
}
