namespace LogoFX.Client.Tests.EndToEnd.White
{
    /// <summary>
    /// Represents start application service for End-To-End tests.
    /// </summary>
    /// <seealso cref="LogoFX.Client.Tests.Contracts.IStartApplicationService" />
    public class StartApplicationService : StartApplicationServiceBase
    {
        /// <summary>
        /// Implement this method to start application using concrete automation framework.
        /// </summary>
        /// <param name="startupPath">The startup path.</param>
        protected override void StartApplicationImpl(string startupPath)
        {
           StartApplicationHelper.StartApplication(startupPath);
        }
    }
}
