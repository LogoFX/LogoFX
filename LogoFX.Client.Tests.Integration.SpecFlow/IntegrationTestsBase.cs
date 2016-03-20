using Attest.Testing.Core;
using LogoFX.Client.Testing.Shared;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.Integration.SpecFlow
{
    /// <summary>
    /// Base class for client integration tests.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TRootViewModel"></typeparam>
    /// <typeparam name="TBootstrapper"></typeparam>
    public abstract class IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper> : 
        Attest.Testing.SpecFlow.IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new() where TRootViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolutionStyle">The resolution style.</param>
        protected IntegrationTestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            :base(resolutionStyle)
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
