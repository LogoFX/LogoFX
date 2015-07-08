// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

#if WinRT
using Windows.UI.Xaml;
using Windows.System.Threading;
#else
#endif
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using LogoFX.Async;
using LogoFX.Client.Core;

namespace LogoFX.Client.Model.Fake
{
    /// <summary>
    /// Helper class to implement simulation service layers. 
    /// </summary>
    /// <typeparam name="T">type of service layer interface.</typeparam>
    public abstract class MockDataLayerBase<T>:NotifyPropertyChangedBase<T> where T:NotifyPropertyChangedBase<T>
    {
        public delegate TParam InvokeHandler<out TParam>();
        
        public delegate void InvokeHandler();

#if !WinRT
          /// <summary>
        /// Simulates asyncronous operation invocation
        /// </summary>
        /// <typeparam name="TParam">The type of the operational parameter</typeparam>
        /// <param name="invokeHandler">The invoke handler.</param>
        /// <param name="resultCallback">The result callback.</param>
        /// <param name="delay">Optional delay.</param>
        public void PostWithException<TParam>(InvokeHandler<TParam> invokeHandler, ResultCallback<TParam> resultCallback, TimeSpan? delay = null) 
        {
            AsyncOperation operation = AsyncOperationManager.CreateOperation(null);
            DispatcherTimer dt = null;
            if (delay != null)
            {
                dt = new DispatcherTimer {Interval = delay.Value};
            }
            EventHandler eh = null;
            DispatcherTimer dt1 = dt;
            eh = (a, e) =>
            {
                if (dt1 != null)
                {
                    dt1.Tick -= eh;
                    dt1.Stop();
                }

                ThreadPool.QueueUserWorkItem(o =>
                {
                    AsyncResponceEventArgs<TParam> args =
                        new AsyncResponceEventArgs<TParam>();

                    try
                    {
                        args.Responce = invokeHandler.Invoke();
                    }
                    catch (Exception exception)
                    {
                        args.Error = new Error("Unknown problem occured in data layer. See exception for details",exception);
                    }

                    operation.Post(state => resultCallback(args), null);
                });
            };

            if (delay != null)
            {
                dt.Tick += eh;
                dt.Start();
            }
            else
            {
                eh(null, null);
            }
        }

        public void PostWithException(InvokeHandler invokeHandler, ResultCallback resultCallback, TimeSpan? delay = null)
        {
            AsyncOperation operation = AsyncOperationManager.CreateOperation(null);
            DispatcherTimer dt = null;
            if (delay != null)
            {
                dt = new DispatcherTimer { Interval = delay.Value };
            }
            EventHandler eh = null;
            DispatcherTimer dt1 = dt;
            eh = (a, e) =>
            {
                if (dt1 != null)
                {
                    dt1.Tick -= eh;
                    dt1.Stop();
                }

                ThreadPool.QueueUserWorkItem(o =>
                {
                    AsyncResponceEventArgs args =
                        new AsyncResponceEventArgs();

                    try
                    {
                        invokeHandler.Invoke();
                    }
                    catch (Exception exception)
                    {
                        args.Error = new Error(exception.Message, exception);
                    }

                    operation.Post(state => resultCallback(args), null);
                });
            };

            if (delay != null)
            {
                dt.Tick += eh;
                dt.Start();
            }
            else
            {
                eh(null, null);
            }
        }

#else
        /// <summary>
        /// Simulates asyncronous operation invocation
        /// </summary>
        /// <typeparam name="TParam">The type of the operational parameter</typeparam>
        /// <param name="invokeHandler">The invoke handler.</param>
        /// <param name="resultCallback">The result callback.</param>
        /// <param name="delay">Optional delay.</param>
        public void PostWithException<TParam>(InvokeHandler<TParam> invokeHandler, ResultCallback<TParam> resultCallback, TimeSpan? delay = null)
        {
            DispatcherTimer dt = null;
            if (delay != null)
            {
                dt = new DispatcherTimer { Interval = delay.Value };
            }
            DispatcherTimer dt1 = dt;
            EventHandler<object> eh = null;
            eh = (a, e) => Worker(dt1, eh, invokeHandler, resultCallback);

            if (delay != null)
            {
                dt.Tick += eh;
                dt.Start();
            }
            else
            {
                eh(null, null);
            }
        }

        public void PostWithException(InvokeHandler invokeHandler, ResultCallback resultCallback, TimeSpan? delay = null)
        {           
            DispatcherTimer dt = null;
            if (delay != null)
            {
                dt = new DispatcherTimer { Interval = delay.Value };
            }
            DispatcherTimer dt1 = dt;
            EventHandler<object> eh = null;           
            eh = (a, e) => Worker(dt1, eh, invokeHandler, resultCallback);

            if (delay != null)
            {
                dt.Tick += eh;
                dt.Start();
            }
            else
            {
                eh(null, null);
            }
        }

        private async void Worker(DispatcherTimer dt1,EventHandler<object> eh, InvokeHandler invokeHandler, ResultCallback resultCallback)
        {
            if (dt1 != null)
            {
                dt1.Tick -= eh;
                dt1.Stop();
            }

            var args = new AsyncResponceEventArgs();
            await ThreadPool.RunAsync(o =>
            {                
                try
                {
                    invokeHandler.Invoke();
                }
                catch (Exception exception)
                {
                    args.Error = new Error(exception.Message, exception);
                }                
            });
            resultCallback(args);
        }

        private async void Worker<TParam>(DispatcherTimer dt1, EventHandler<object> eh, InvokeHandler<TParam> invokeHandler, ResultCallback<TParam> resultCallback)
        {
            if (dt1 != null)
            {
                dt1.Tick -= eh;
                dt1.Stop();
            }

            var args = new AsyncResponceEventArgs<TParam>();
            await ThreadPool.RunAsync(o =>
            {
                try
                {
                    invokeHandler.Invoke();
                }
                catch (Exception exception)
                {
                    args.Error = new Error(exception.Message, exception);
                }
            });
            resultCallback(args);
        }
#endif


    }
}
