using LogoFX.Client.Tests.Contracts;
using LogoFX.Client.Tests.EndToEnd.Shared;

namespace LogoFX.Client.Tests.EndToEnd
{
    /// <summary>
    /// Base class for <see cref="IStartApplicationService"/> which prepares the fake provider builders
    /// </summary>
    /// <seealso cref="IStartApplicationService" />
    public abstract class StartApplicationServiceBase : IStartApplicationService
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path. In the integration tests case, it may be left empty.</param>
        public void StartApplication(string startupPath)
        {
            PrepareData();
            StartApplicationImpl(startupPath);
        }

        private void PrepareData()
        {
            BuildersCollectionContext.SerializeBuilders();
        }

        /// <summary>
        /// Implement this method to start application using concrete automation framework.
        /// </summary>
        /// <param name="startupPath">The startup path.</param>
        protected abstract void StartApplicationImpl(string startupPath);
    }
}