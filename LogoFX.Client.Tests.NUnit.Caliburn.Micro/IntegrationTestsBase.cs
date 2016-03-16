using Attest.Tests.Core;
using LogoFX.Client.Testing.Shared;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit.Caliburn.Micro
{
    /// <summary>
    /// Base class for Caliburn.Micro based client integration tests.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    /// <seealso cref="Attest.Tests.NUnit.IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}" />
    public abstract class IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper> :
        Attest.Tests.NUnit.IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new() where TRootViewModel : class
    {
        private readonly InitializationParametersResolutionStyle _resolutionStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolutionStyle">The resolution style.</param>
        protected IntegrationTestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            : base(resolutionStyle)
        {
            _resolutionStyle = resolutionStyle;
        }

        /// <summary>
        /// Provides additional opportunity to modify the test setup logic
        /// </summary>
        protected override void SetupOverride()
        {
            base.SetupOverride();
            TestHelper.Setup();
        }

        /// <summary>
        /// Called when the teardown finishes
        /// </summary>
        protected override void OnAfterTeardown()
        {
            base.OnAfterTeardown();
            if (_resolutionStyle == InitializationParametersResolutionStyle.PerRequest)
            {
                Shared.Caliburn.Micro.TestHelper.Teardown();
            }
        }
    }
}
