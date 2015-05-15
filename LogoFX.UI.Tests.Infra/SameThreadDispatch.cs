using System;
using System.Windows.Threading;

namespace LogoFX.UI.Tests.Infra
{
    public class SameThreadDispatch : IDispatch
    {
        public void BeginOnUiThread(Action action)
        {
            OnUiThread(action);
        }

        public void OnUiThread(Action action)
        {
            action();
        }

        public void OnUiThread(DispatcherPriority priority, Action action)
        {
            OnUiThread(action);
        }

        public void BeginOnUiThread(DispatcherPriority prio, Action action)
        {
            OnUiThread(action);
        }
    }
}
