#if !NET45
using System.Threading;
using System.Threading.Tasks;
#endif
using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Windows.Threading
{
    /// <summary>
    /// Represents UI-thread dispatcher
    /// </summary>
    public interface IDispatch
    {
        /// <summary>
        /// Begins the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        void BeginOnUiThread(Action action);

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="prio">Desired priority</param>
        /// <param name="action">Action</param>
        void BeginOnUiThread(DispatcherPriority prio, Action action);

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        void OnUiThread(Action action);
        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        void OnUiThread(DispatcherPriority priority, Action action);

        /// <summary>
        /// Initializes the dispatcher
        /// </summary>
        void InitializeDispatch();
    }

    /// <summary>
    /// Default UI-thread dispatcher
    /// </summary>
    public class DefaultDispatch : IDispatch
    {
        private Action<Action, bool, DispatcherPriority> s_dispatch;        

        private void EnsureDispatch()
        {
            if (s_dispatch == null)
            {
                throw new InvalidOperationException("Dispatch is not initialized correctly");
            }
        }

        /// <summary>
        /// Initializes the framework using the current dispatcher.
        /// </summary>
        public void InitializeDispatch()
        {
#if NET45
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");
#else
            SynchronizationContext context = SynchronizationContext.Current;
#endif

            //#if !WinRT
            //            Dispatcher dispatcher = null;
            //#else
            //            CoreDispatcher dispatcher = null;
            //#endif

            //#if SILVERLIGHT
            //            dispatcher = Deployment.Current.Dispatcher;
            //#elif WinRT
            //            dispatcher = new UserControl().Dispatcher;
            //#else
            //            dispatcher = Dispatcher.CurrentDispatcher;
            //            //if (Application.Current != null && Application.Current.Dispatcher != null)
            //            //    dispatcher = Application.Current.Dispatcher;
            //#endif            


            s_dispatch = (action, @async, prio) =>
            {
#if NET45
                if (!@async && dispatcher.CheckAccess())
#else
                if (!@async)
#endif
                {
                    action();
                }               
                else
                {
#if NET45
                    dispatcher.BeginInvoke(action, prio);
#else
                    
                    var taskSource = new TaskCompletionSource<object>();
                    Action method = () => {
                        try
                        {
                            action();
                            taskSource.SetResult(null);
                        }
                        catch (Exception ex)
                        {
                            taskSource.SetException(ex);
                        }
                    };
                    context.Post(d => method(), null);                    
#endif
//#if WinRT
//#pragma warning disable 4014
//                    dispatcher.RunAsync(prio,()=>action());
//#pragma warning restore 4014
//#elif SILVERLIGHT
//                    dispatcher.BeginInvoke(action);
//#else
//                    dispatcher.BeginInvoke(action, prio);
//#endif
                }
            };
        }

        /// <summary>
        /// Begins the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(Action action)
        {
            BeginOnUiThread(Consts.DispatcherPriority, action);

            //#if SILVERLIGHT
            //            EnsureDispatch();
            //            s_dispatch(action, true);            

            //#elif WinRT
            //            EnsureDispatch();
            //            s_dispatch(action, true,CoreDispatcherPriority.Normal);
            //#else
            //            BeginOnUiThread(Consts.DispatcherPriority, action);
            //#endif
        }

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="prio">Desired priority</param>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(DispatcherPriority prio, Action action)
        {
            EnsureDispatch();
            s_dispatch(action, true, prio);
        }

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        public void OnUiThread(Action action)
        {
            OnUiThread(Consts.DispatcherPriority, action);
        }

        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void OnUiThread(DispatcherPriority priority, Action action)
        {
            EnsureDispatch();
            s_dispatch(action, false, priority);
        }
    }
}
