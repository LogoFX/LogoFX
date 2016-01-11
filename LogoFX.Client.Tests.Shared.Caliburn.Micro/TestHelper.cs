using Caliburn.Micro;

namespace LogoFX.Client.Tests.Shared.Caliburn.Micro
{
    /// <summary>
    /// Helper class for setting the tests scheduler and dispatcher.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Performs the required teardown actions for Caliburn.Micro based client applications.
        /// </summary>
        public static void Teardown()
        {
            AssemblySource.Instance.Clear();
        }
    }
}
