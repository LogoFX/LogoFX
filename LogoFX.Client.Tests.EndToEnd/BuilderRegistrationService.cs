using Attest.Fake.Builders;
using LogoFX.Client.Tests.Contracts;
using LogoFX.Client.Tests.EndToEnd.Shared;

namespace LogoFX.Client.Tests.EndToEnd
{   
    /// <summary>
    /// Represents builder registration service for End-To-End tests.
    /// </summary>
    /// <seealso cref="IBuilderRegistrationService" />
    public class BuilderRegistrationService : IBuilderRegistrationService
    {
        /// <summary>
        /// Registers the builder into IoC container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="builder">The builder.</param>
        public void RegisterBuilder<TService>(FakeBuilderBase<TService> builder) where TService : class
        {
            BuildersCollectionContext.AddBuilder(builder);
        }
    }
}
