using System.Windows.Threading;
using Caliburn.Micro;
using LogoFX.UI.Tests.Infra;
using Solid.Practices.Scheduling;

namespace LogoFX.UI.Tests.Shared
{
    public static class TestHelper
    {
        public static void Setup()
        {
            TaskScheduler.Current = new SameThreadTaskScheduler();
            Dispatch.Current = new SameThreadDispatch();
        }

        public static void Teardown()
        {
            AssemblySource.Instance.Clear();
        }
    }
}
