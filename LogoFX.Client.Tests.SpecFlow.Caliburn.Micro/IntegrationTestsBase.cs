using Attest.Tests.Core;
using LogoFX.Client.Tests.Shared.Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.SpecFlow.Caliburn.Micro
{
    /// <summary>
    /// Base class for client integration tests.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TRootViewModel"></typeparam>
    /// <typeparam name="TBootstrapper"></typeparam>
    public abstract class IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper> : 
        Attest.Tests.SpecFlow.IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new() where TRootViewModel : class
    {
        private readonly InitializationParametersResolutionStyle _resolutionStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolutionStyle">The resolution style.</param>
        protected IntegrationTestsBase(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            :base(resolutionStyle)
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
                TestHelper.Teardown();    
            }            
        }
    }
}
