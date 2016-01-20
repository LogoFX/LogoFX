using Attest.Tests.Core;
using LogoFX.Client.Tests.Contracts;
using LogoFX.Client.Tests.EndToEnd;
using LogoFX.Client.Tests.EndToEnd.White;

namespace LogoFX.Client.Tests.SpecFlow
{
    /// <summary>
    /// Base class for client end-to-end tests.
    /// </summary>    
    public abstract class EndToEndTestsBase :
        Attest.Tests.SpecFlow.EndToEndTestsBase        
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndToEndTestsBase"/> class.
        /// </summary>
        protected EndToEndTestsBase()
        {
            ScenarioHelper.Add(new StartApplicationService(), typeof(IStartApplicationService));
            ScenarioHelper.Add(new BuilderRegistrationService(), typeof(IBuilderRegistrationService));
            RegisterScreenObjectsCore();
        }

        private void RegisterScreenObjectsCore()
        {
            RegisterScreenObjects();
        }

        /// <summary>
        /// Override this method to register the screen objects.
        /// </summary>
        protected virtual void RegisterScreenObjects()
        {
            
        }

        /// <summary>
        /// Called when the teardown finishes
        /// </summary>
        protected override void OnAfterTeardown()
        {
            base.OnAfterTeardown();
            if (ApplicationContext.Application != null)
            {
                ApplicationContext.Application.Dispose();
            }
            ScenarioHelper.Clear();
        }
    }
}