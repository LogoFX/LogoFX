using System.Windows.Threading;
using LogoFX.Client.Tests.Infra;
using Solid.Practices.Scheduling;

namespace LogoFX.Client.Tests.Shared
{
    /// <summary>
    /// Helper class for setting the tests scheduler and dispatcher.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Setups integration test by ensuring that all actions are executed on the same thread.
        /// </summary>
        public static void Setup()
        {
            TaskScheduler.Current = new SameThreadTaskScheduler();
            Dispatch.Current = new SameThreadDispatch();
        }        
    }
}
