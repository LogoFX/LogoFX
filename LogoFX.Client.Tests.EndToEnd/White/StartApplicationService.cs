using LogoFX.Client.Tests.Contracts;
using LogoFX.Client.Tests.EndToEnd.Shared;
using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd.White
{
    /// <summary>
    /// Represents start application service for End-To-End tests.
    /// </summary>
    /// <seealso cref="LogoFX.Client.Tests.Contracts.IStartApplicationService" />
    public class StartApplicationService : IStartApplicationService
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path. In the integration tests case, it may be left empty.</param>
        public void StartApplication(string startupPath)
        {
            BuildersCollectionContext.SerializeBuilders();
            ApplicationContext.Application = Application.Launch(startupPath);
            ApplicationContext.Application.WaitWhileBusy();          
        }
    }    
}
