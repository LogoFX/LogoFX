using TestStack.White;

namespace LogoFX.Client.Tests.EndToEnd.White
{
    /// <summary>
    /// Helper class for starting an application using <see cref="TestStack.White"/> framework
    /// </summary>
    public static class StartApplicationHelper
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="startupPath">The startup path.</param>
        public static void StartApplication(string startupPath)
        {
            ApplicationContext.Application = Application.Launch(startupPath);
            ApplicationContext.Application.WaitWhileBusy();
        }
    }
}