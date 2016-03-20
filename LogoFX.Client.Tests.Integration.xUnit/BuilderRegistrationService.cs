using Attest.Fake.Builders;
using Attest.Testing.Core;
using Attest.Testing.xUnit;
using LogoFX.Client.Testing.Contracts;
using Solid.Practices.IoC;

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

    /// <summary>
    /// Base class for client integration tests.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    /// <seealso cref="Attest.Testing.xUnit.IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}" />
    public abstract class IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper> :
        Attest.Testing.xUnit.IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new() where TRootViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolutionStyle">The resolution style.</param>
        protected IntegrationTestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            : base(resolutionStyle)
        {
        }

        /// <summary>
        /// Provides additional opportunity to modify the test setup logic
        /// </summary>
        protected override void SetupOverride()
        {
            base.SetupOverride();
            TestHelper.Setup();
        }
    }
}
