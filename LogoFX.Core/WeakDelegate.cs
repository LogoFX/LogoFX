using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace LogoFX.Core
{
    /// <summary>
    /// Static class used to convert real (strong) delegates into weak
    /// delegates.
    /// </summary>
    public static class WeakDelegate
    {
        /// <summary>
        /// Verifies if a handler is already a weak delegate.
        /// </summary>
        /// <param name="handler">The handler to verify.</param>
        /// <returns><see langword="true"/> if the handler is already a weak delegate, <see langword="false"/> otherwise.</returns>
        public static bool IsWeakDelegate(Delegate handler)
        {
            return handler.Target != null && handler.Target is WeakDelegateBase;
        }        

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static Action From(Action strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakActionWrapper(strongHandler);
            return wrapper.Execute;
        }        

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static Action<T> From<T>(Action<T> strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakActionWrapper<T>(strongHandler);
            return wrapper.Execute;
        }
        
        /// <summary>
        /// Creates a weak delegate from an Action  delegate.
        /// </summary>
        public static Action<T1, T2> From<T1, T2>(Action<T1, T2> strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakActionWrapper<T1, T2>(strongHandler);
            return wrapper.Execute;
        }

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static Action<T1, T2, T3> From<T1, T2, T3>(Action<T1, T2, T3> strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakActionWrapper<T1, T2, T3>(strongHandler);
            return wrapper.Execute;
        }

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static Action<T1, T2, T3, T4> From<T1, T2, T3, T4>(Action<T1, T2, T3, T4> strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakActionWrapper<T1, T2, T3, T4>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakActionWrapper :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action handler) :
                base(handler)
            {
            }

            internal void Execute()
            {
                Invoke(null);
            }
        }

        private sealed class WeakActionWrapper<T> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T> handler) :
                base(handler)
            {
            }

            internal void Execute(T parameter)
            {
                Invoke(new object[] { parameter });
            }
        }

        private sealed class WeakActionWrapper<T1, T2> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2)
            {
                Invoke(new object[] { parameter1, parameter2 });
            }
        }

        private sealed class WeakActionWrapper<T1, T2, T3> :
            WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2, T3> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2, T3 parameter3)
            {
                Invoke(new object[] { parameter1, parameter2, parameter3 });
            }
        }

        private sealed class WeakActionWrapper<T1, T2, T3, T4> :
           WeakDelegateBase
        {
            internal WeakActionWrapper(Action<T1, T2, T3, T4> handler) :
                base(handler)
            {
            }

            internal void Execute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
            {
                Invoke(new object[] { parameter1, parameter2, parameter3, parameter4 });
            }
        }

        private static void AssertIsWeakDelegate(Delegate strongHandler)
        {
            if (IsWeakDelegate(strongHandler))
            {
                throw new ArgumentException("Your delegate is already a weak-delegate.");
            }                
        }               

        private sealed class WeakEventHandlerWrapper :
            WeakDelegateBase
        {
            internal WeakEventHandlerWrapper(EventHandler handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, EventArgs e)
            {
                Invoke(new[] { sender, e });
            }
        }

        /// <summary>
        /// Creates a weak delegate from an <see cref="EventHandler"/> delegate.
        /// </summary>
        public static EventHandler From(EventHandler strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);   
             
            var wrapper = new WeakEventHandlerWrapper(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakEventHandlerWrapper<TEventArgs> :
            WeakDelegateBase
        where
            TEventArgs : EventArgs
        {
            internal WeakEventHandlerWrapper(EventHandler<TEventArgs> handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, TEventArgs e)
            {
                Invoke(new[] { sender, e });
            }
        }

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static EventHandler<TEventArgs> From<TEventArgs>(EventHandler<TEventArgs> strongHandler)
        where
            TEventArgs : EventArgs
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakEventHandlerWrapper<TEventArgs>(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakNotifyCollectionChangedEventHandler :
            WeakDelegateBase
        {
            internal WeakNotifyCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, NotifyCollectionChangedEventArgs e)
            {
                Invoke(new[] { sender, e });
            }
        }

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static NotifyCollectionChangedEventHandler From(NotifyCollectionChangedEventHandler strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakNotifyCollectionChangedEventHandler(strongHandler);
            return wrapper.Execute;
        }

        private sealed class WeakPropertyChangedEventHandler :
            WeakDelegateBase
        {
            internal WeakPropertyChangedEventHandler(PropertyChangedEventHandler handler) :
                base(handler)
            {
            }

            internal void Execute(object sender, PropertyChangedEventArgs e)
            {
                Invoke(new[] { sender, e });
            }
        }

        /// <summary>
        /// Creates a weak delegate from an Action delegate.
        /// </summary>
        public static PropertyChangedEventHandler From(PropertyChangedEventHandler strongHandler)
        {
            AssertIsWeakDelegate(strongHandler);

            var wrapper = new WeakPropertyChangedEventHandler(strongHandler);
            return wrapper.Execute;
        }
    }

    /// Provides a weak reference to a null target object, which, unlike
    /// other weak references, is always considered to be alive. This
    /// facilitates handling null dictionary values, which are perfectly
    /// legal.
    public class WeakNullReference<T> : WeakReference<T> where T : class
    {
        /// <summary>
        /// The instance of <see cref="WeakNullReference{T}"/>
        /// </summary>
        public static readonly WeakNullReference<T> Singleton = new WeakNullReference<T>();

        private WeakNullReference() : base(null) { }

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="WeakReference{T}"/> object has been garbage collected.
        /// </summary>
        /// 
        /// <returns>
        /// true if the object referenced by the current <see cref="WeakReference{T}"/> object has not been garbage collected and is still accessible; otherwise, false.
        /// </returns>
        public override bool IsAlive
        {
            get { return true; }
        }
    }

    /// <summary>
    /// Adds strong typing to WeakReference.Target using generics. Also,
    /// the Create factory method is used in place of a constructor
    /// to handle the case where target is null, but we want the
    /// reference to still appear to be alive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakReference<T> where T : class
    {
        private readonly WeakReference _inner;


        /// <summary>
        /// Creates <see cref="WeakReference{T}"/> from the provided target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static WeakReference<T> Create(T target)
        {
            if (target == null)
                return WeakNullReference<T>.Singleton;

            return new WeakReference<T>(target);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        protected WeakReference(T target)
        {
            if (target == null) throw new ArgumentNullException("target");
            _inner = new WeakReference(target);

        }

        /// <summary>
        /// Gets the weak reference's target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public T Target
        {
            get { return (T)_inner.Target; }
        }

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="WeakReference{T}"/> object has been garbage collected.
        /// </summary>
        /// 
        /// <returns>
        /// true if the object referenced by the current <see cref="WeakReference{T}"/> object has not been garbage collected and is still accessible; otherwise, false.
        /// </returns>
        public virtual bool IsAlive
        {
            get
            {
                return this._inner.IsAlive;
            }
        }
    }

    /// <summary>
    /// A class used as the base class to implement weak delegates.
    /// See <see cref="WeakDelegate"/>.From method implementations to see how it works.
    /// </summary>
    public class WeakDelegateBase :
        WeakReference<object>
    {
        /// <summary>
        /// Creates this weak-delegate class based as a copy of the given 
        /// delegate handler.
        /// </summary>
        /// <param name="handler">The handler to copy information from.</param>
        public WeakDelegateBase(Delegate handler) :
            base(handler.Target)
        {
#if WinRT
            Method = (MethodInfo)handler.GetType().GetRuntimeProperty("Method").GetValue(handler);
#elif NET45            
            Method = handler.Method;
#else
            Method = handler.GetMethodInfo();
#endif
        }

        /// <summary>
        /// Gets the method used by this delegate.
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Invokes this delegate with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters to be used by the delegate.</param>
        public void Invoke(object[] parameters)
        {
            object target = Target;
            if (target != null || Method.IsStatic)
            {
                Method.Invoke(target, parameters);
            }                
        }
    }
}
