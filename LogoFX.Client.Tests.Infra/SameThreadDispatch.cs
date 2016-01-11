using System;
using System.Windows.Threading;

namespace LogoFX.Client.Tests.Infra
{
    /// <summary>
    /// Represents dispatcher which executes the actions on the calling thread.
    /// Used for tests to check the view model logic.
    /// </summary>
    /// <seealso cref="System.Windows.Threading.IDispatch" />
    public class SameThreadDispatch : IDispatch
    {
        /// <summary>
        /// Begins the action on the calling thread.
        /// </summary>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(Action action)
        {
            OnUiThread(action);
        }

        /// <summary>
        /// Executes the action on the calling thread.
        /// </summary>
        /// <param name="action">Action</param>
        public void OnUiThread(Action action)
        {
            action();
        }

        /// <summary>
        /// Executes the action on the calling thread.
        /// </summary>
        /// <param name="priority">Desired priority. Not in use.</param>
        /// <param name="action">Action</param>
        public void OnUiThread(DispatcherPriority priority, Action action)
        {
            OnUiThread(action);
        }

        /// <summary>
        /// Initializes the dispatcher
        /// </summary>
        public void InitializeDispatch()
        {
            
        }

        /// <summary>
        /// Begins the action on the calling thread.
        /// </summary>
        /// <param name="prio">Desired priority. Not in use.</param>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(DispatcherPriority prio, Action action)
        {
            OnUiThread(action);
        }
    }
}
