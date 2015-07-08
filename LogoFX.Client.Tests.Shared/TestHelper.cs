using System.Windows.Threading;
using Caliburn.Micro;
using LogoFX.Client.Tests.Infra;
using Solid.Practices.Scheduling;

namespace LogoFX.Client.Tests.Shared
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
