using Attest.Testing.Core;
using LogoFX.Client.Testing.Contracts;

namespace LogoFX.Client.Tests.Integration
{
    /// <summary>
    /// Represents start application service for integration tests.
    /// </summary>
    /// <seealso cref="IStartApplicationService" />    
    public class StartApplicationServiceBase : IStartApplicationService
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path. In the integration tests case, it may be left empty.</param>
        public void StartApplication(string startupPath)
        {
            RegisterFakes();
            OnStartCore();
            OnStart(ScenarioHelper.RootObject);
        }

        /// <summary>
        /// Override this method to register fakes into the container before the application starts.
        /// </summary>
        protected virtual void RegisterFakes()
        {            
        }

        /// <summary>
        /// Override this method to inject custom logic with regard to the application root object immediately after the object has been created.
        /// </summary>
        /// <param name="rootObject">The root object.</param>
        protected virtual void OnStart(object rootObject)
        {            
        }

        private static void OnStartCore()
        {
            ScenarioHelper.CreateRootObject();
        }
    }
}
