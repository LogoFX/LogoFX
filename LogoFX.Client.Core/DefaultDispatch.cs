// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

#if WinRT
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif
using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Windows.Threading
{
    public interface IDispatch
    {
        void BeginOnUiThread(Action action);
        void BeginOnUiThread(DispatcherPriority prio, Action action);
        void OnUiThread(Action action);
        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="priority">priority</param>
        /// <param name="action">The action to execute.</param>
        void OnUiThread(DispatcherPriority priority, Action action);

        void InitializeDispatch();
    }

    public class DefaultDispatch : IDispatch
    {
#if SILVERLIGHT
        private Action<Action, bool> s_dispatch;
#elif   WinRT
        private Action<Action, bool, CoreDispatcherPriority> s_dispatch;
#else
        private Action<Action, bool, DispatcherPriority> s_dispatch;        

#endif
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
#if !WinRT
            Dispatcher dispatcher = null;
#else
            CoreDispatcher dispatcher = null;
#endif

#if SILVERLIGHT
            dispatcher = Deployment.Current.Dispatcher;
#elif WinRT
            dispatcher = new UserControl().Dispatcher;
#else
            dispatcher = Dispatcher.CurrentDispatcher;
            //if (Application.Current != null && Application.Current.Dispatcher != null)
            //    dispatcher = Application.Current.Dispatcher;
#endif
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");


#if !SILVERLIGHT
            s_dispatch = (action, @async, prio) =>
#else
            s_dispatch = (action, @async) =>
#endif
            {
#if!WinRT
                if (!@async && dispatcher.CheckAccess())
#else
                if (!@async && dispatcher.HasThreadAccess)
#endif
                {
                    action();
                }
                else if (!@async)
                {
#if WinRT
                var waitHandle = new ManualResetEvent(false);
                Exception exception = null;
#pragma warning disable 4014
                dispatcher.RunAsync(prio,() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    waitHandle.Set();
                });
#pragma warning restore 4014
                waitHandle.WaitOne();
                if (exception != null)
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);                    
#elif SILVERLIGHT
                var waitHandle = new ManualResetEvent(false);
                Exception exception = null;
                dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    waitHandle.Set();
                });
                waitHandle.WaitOne();
                if (exception != null)
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
                
#else
                    dispatcher.Invoke(prio, action);
#endif
                }
                else
                {
#if WinRT
#pragma warning disable 4014
                    dispatcher.RunAsync(prio,()=>action());
#pragma warning restore 4014
#elif SILVERLIGHT
                    dispatcher.BeginInvoke(action);
#else
                    dispatcher.BeginInvoke(action, prio);
#endif
                }
            };
        }

        public void BeginOnUiThread(Action action)
        {
#if SILVERLIGHT
            EnsureDispatch();
            s_dispatch(action, true);            

#elif WinRT
            EnsureDispatch();
            s_dispatch(action, true,CoreDispatcherPriority.Normal);
#else
            BeginOnUiThread(Consts.DispatcherPriority, action);
#endif
        }
#if SILVERLIGHT
        
#elif WinRT
        public static void BeginOnUiThread(CoreDispatcherPriority prio, Action action)
        {
            EnsureDispatch();
            s_dispatch(action, true, prio);
        }
#else
        public void BeginOnUiThread(DispatcherPriority prio, Action action)
        {            
            EnsureDispatch();
            s_dispatch(action, true, prio);
        }
#endif
        public void OnUiThread(Action action)
        {
#if SILVERLIGHT
            EnsureDispatch();
            s_dispatch(action, false);
#elif WinRT
            EnsureDispatch();
            s_dispatch(action, false,CoreDispatcherPriority.Normal);
#else
            OnUiThread(Consts.DispatcherPriority, action);
#endif
        }


#if SILVERLIGHT
#elif WinRT
        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="priority">priority</param>
        /// <param name="action">The action to execute.</param>
        public void OnUiThread(CoreDispatcherPriority priority, Action action)
        {
            EnsureDispatch();
            s_dispatch(action, false, priority);
        }
#else
        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="priority">priority</param>
        /// <param name="action">The action to execute.</param>
        public void OnUiThread(DispatcherPriority priority, Action action)
        {           
            EnsureDispatch();
            s_dispatch(action, false, priority);
        }
#endif
    }
}
