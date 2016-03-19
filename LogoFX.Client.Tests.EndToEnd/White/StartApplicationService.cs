using LogoFX.Client.Testing.Contracts;
using LogoFX.Client.Tests.EndToEnd.FakeData.Shared;

namespace LogoFX.Client.Tests.EndToEnd.White
{
    /// <summary>
    /// Represents start application service for End-To-End tests.
    /// </summary>
    /// <seealso cref="IStartApplicationService" />
    public abstract class StartApplicationService : IStartApplicationService
    {
        /// <summary>
        /// Represents start application service for End-To-End tests which use fake data providers.
        /// </summary>
        /// <seealso cref="IStartApplicationService" />
        public class WithFakeProviders : StartApplicationService
        {
            /// <summary>
            /// Starts the application.
            /// </summary>
            /// <param name="startupPath">The startup path.</param>
            public override void StartApplication(string startupPath)
            {
                BuildersCollectionContext.SerializeBuilders();
                StartApplicationHelper.StartApplication(startupPath);
            }
        }

        /// <summary>
        /// Represents start application service for End-To-End tests which use real data providers.
        /// </summary>
        /// <seealso cref="IStartApplicationService" />
        public class WithRealProviders : StartApplicationService
        {
            /// <summary>
            /// Starts the application.
            /// </summary>
            /// <param name="startupPath">The startup path.</param>
            public override void StartApplication(string startupPath)
            {
                StartApplicationHelper.StartApplication(startupPath);
            }
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path.</param>
        public abstract void StartApplication(string startupPath);
    }
}
