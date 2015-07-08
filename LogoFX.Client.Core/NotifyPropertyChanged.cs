// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

//note: I don't know who is responsible for writing out the most of this excellent staff
//note: If you feel you are somehow involved and not mentioned in credits - let me know

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LogoFX.Client.Core
{
    #region NotifyPropertyChangedBase

    /// <summary>
    /// A base class for classes that need to implement <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <typeparam name="TObject">The type of the derived class.</typeparam>
    public abstract class NotifyPropertyChangedBase<TObject> : INotifyPropertyChanged, ISuppressNotify
        where TObject : NotifyPropertyChangedBase<TObject>
    {
        /// <summary>
        /// The backing delegate for <see cref="PropertyChanged"/>.
        /// </summary>
        private PropertyChangedEventHandler _propertyChanged;

#if !SILVERLIGHT && !WinRT
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this._propertyChanged += value; }
            remove { this._propertyChanged -= value; }
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the Items[] property.
        /// </summary>
        protected void OnItemsPropertyChanged()
        {
            if (!_suppressNotify)
                this._propertyChanged.RaiseItems(this);
        }

        /// <summary>
        /// Notifies the of property change.(GLUE: compatibility to caliburn micro)
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> expression)
        {
            OnPropertyChanged(expression);
        }

        /// <summary>
        /// Notifies the of property change.
        /// </summary>        
        /// <param name="propInfo">The expression.</param>
        protected void NotifyOfPropertyChange(PropertyInfo propInfo)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            if (!_suppressNotify)
                this._propertyChanged.Raise((TObject)this, propInfo);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            if (!_suppressNotify)
                this._propertyChanged.Raise((TObject)this, expression);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <param name="name"></param>
        protected void NotifyOfPropertyChange([CallerMemberName]string name = "")
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            if (!_suppressNotify)
                this._propertyChanged.Raise((TObject)this, name);
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            this._propertyChanged.Raise((TObject)this, expression);
        }
        
        protected void NotifyOfPropertiesChange()
        {
            // The cast of "this" to TObject will always succeed due to the generic constraint on this class
            if (!_suppressNotify)
                this._propertyChanged.Raise((TObject)this);
        }

        /// <summary>
        /// Push notification suppression
        /// </summary>
        IDisposable ISuppressNotify.SuppressNotify
        {
            get { return this.SuppressNotify; }
        }

        /// <summary>
        /// Gets the suppress notify.
        /// </summary>
        /// <remarks>use this in <see langword="using"></see> statement</remarks>
        /// <value>The suppress notify.</value>
        protected IDisposable SuppressNotify
        {
            get { return new SuppressNotifyHelper<TObject>(this); }
        }

        private bool _suppressNotify;

        /// <summary>
        /// Helper for notification supression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class SuppressNotifyHelper<T> : IDisposable where T : NotifyPropertyChangedBase<T>
        {
            private readonly NotifyPropertyChangedBase<T> _source;

            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyPropertyChangedBase&lt;TObject&gt;.SuppressNotifyHelper&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            public SuppressNotifyHelper(NotifyPropertyChangedBase<T> source)
            {
                _source = source;
                source._suppressNotify = true;
            }

            public void Dispose()
            {
                _source._suppressNotify = false;
            }
        }
    }

    #endregion

    #region PropertyChangedEventHandlerExtensions

    /// <summary>
    /// Provides extension methods for <see cref="PropertyChangedEventHandler"/> delegates.
    /// </summary>
    public static class PropertyChangedEventHandlerExtensions
    {

        /// <summary>
        /// Subscribes a handler to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for a specific property.
        /// </summary>
        /// <typeparam name="TObject">The type implementing <see cref="INotifyPropertyChanged"/>.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The object implementing <see cref="INotifyPropertyChanged"/>.</param>
        /// <param name="expression">The lambda expression selecting the property.</param>
        /// <param name="handler">The handler that is invoked when the property changes.</param>
        /// <returns>The actual delegate subscribed to <see cref="INotifyPropertyChanged.PropertyChanged"/>.</returns>
        public static PropertyChangedEventHandler SubscribeToPropertyChanged<TObject, TProperty>(
            this TObject source,
            Expression<Func<TObject, TProperty>> expression,
            Action<TObject> handler)
            where TObject : INotifyPropertyChanged
        {

            // This is similar but not identical to:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            string propertyName = source.GetPropertyName(expression);
            PropertyChangedEventHandler ret = (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    handler(source);
                }
            };
            source.PropertyChanged += ret;
            return ret;
        }


        /// <summary>
        /// Notifies the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="propertySelector">The property selector.</param>
        public static void Notify<T>(this PropertyChangedEventHandler handler, Expression<Func<T>> propertySelector)
        {
            if (handler != null)
            {
                var memberExpression = propertySelector.GetMemberExpression();
                if (memberExpression != null)
                {
                    var sender = ((ConstantExpression)memberExpression.Expression).Value;
                    handler(sender, new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject, TProperty>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            Expression<Func<TObject, TProperty>> expression)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(sender.GetPropertyName(expression)));
            }
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject, TProperty>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            Expression<Func<TProperty>> expression)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(sender.GetPropertyName(expression)));
            }
        }

        /// <summary>
        /// Raises the delegate for the property identified by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>        
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        /// <param name="name">The lambda expression identifying the property that changed.</param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender,
           string name)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="info"></param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender,
            PropertyInfo info)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(info.Name));
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        public static void Raise<TObject>(
            this PropertyChangedEventHandler handler,
            TObject sender)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(""));
            }
        }

        /// <summary>
        /// Raises the delegate for the items property (with the name "Items[]").
        /// </summary>
        /// <param name="handler">The delegate to raise. If this parameter is <see langword="null"/>, then no action is taken.</param>
        /// <param name="sender">The object raising this event.</param>
        public static void RaiseItems(this PropertyChangedEventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs("Items[]"));
            }
        }
    }

    #endregion

    #region PropertyChangedExtensions

    /// <remarks>
    /// - ideas from http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
    /// </remarks>
    public static class PropertyChangedExtensions
    {
        private const string SELECTOR_MUSTBEPROP = "PropertySelector must select a property type.";

        /// <summary>
        /// Notifies the specified notifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="notifier">The notifier.</param>
        /// <param name="propertySelector">The property selector.</param>
        public static void Notify<T>(this Action<string> notifier, Expression<Func<T>> propertySelector)
        {
            if (notifier != null)
                notifier(GetPropertyName(propertySelector));
        }

        /// <summary>
        /// Retrieves the name of a property referenced by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The object containing the property.</param>
        /// <param name="expression">A lambda expression selecting the property from the containing object.</param>
        /// <returns>The name of the property referenced by <paramref name="expression"/>.</returns>
        public static string GetPropertyName<TObject, TProperty>(this TObject @this, Expression<Func<TObject, TProperty>> expression)
        {
            // For more information on the technique used here, see these blog posts:
            //   http://themechanicalbride.blogspot.com/2007/03/symbols-on-steroids-in-c.html
            //   http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
            //   http://joshsmithonwpf.wordpress.com/2009/07/11/one-way-to-avoid-messy-propertychanged-event-handling/
            // Note that the following blog post:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            // uses a similar technique, but must also account for implicit casts to object by checking for UnaryExpression.
            // Our solution uses generics, so this additional test is not necessary.
            return expression != null ? ((MemberExpression)expression.Body).Member.Name : ".";
        }
        /// <summary>
        /// Retrieves the name of a property referenced by a lambda expression.
        /// </summary>
        /// <typeparam name="TObject">The type of object containing the property.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The object containing the property.</param>
        /// <param name="expression">A lambda expression selecting the property from the containing object.</param>
        /// <returns>The name of the property referenced by <paramref name="expression"/>.</returns>
        public static string GetPropertyName<TObject, TProperty>(this TObject @this, Expression<Func<TProperty>> expression)
        {
            // For more information on the technique used here, see these blog posts:
            //   http://themechanicalbride.blogspot.com/2007/03/symbols-on-steroids-in-c.html
            //   http://michaelsync.net/2009/04/09/silverlightwpf-implementing-propertychanged-with-expression-tree
            //   http://joshsmithonwpf.wordpress.com/2009/07/11/one-way-to-avoid-messy-propertychanged-event-handling/
            // Note that the following blog post:
            //   http://www.ingebrigtsen.info/post/2008/12/11/INotifyPropertyChanged-revisited.aspx
            // uses a similar technique, but must also account for implicit casts to object by checking for UnaryExpression.
            // Our solution uses generics, so this additional test is not necessary.

            return expression != null ? ((MemberExpression)expression.Body).Member.Name : ".";
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(this Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertySelector.Body as UnaryExpression;
                if (unaryExpression != null) memberExpression = unaryExpression.Operand as MemberExpression;
            }
            if (memberExpression != null)
            {
#if WinRT
                Debug.Assert((memberExpression.Member is PropertyInfo),
                             "propertySelector" + SELECTOR_MUSTBEPROP);
#else
                Debug.Assert((memberExpression.Member.MemberType == MemberTypes.Property),
                             "propertySelector", SELECTOR_MUSTBEPROP);
#endif

                return memberExpression.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// Gets the member expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns></returns>
        public static MemberExpression GetMemberExpression<T>(this Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null)
            {
#if WinRT
                Debug.Assert((memberExpression.Member is PropertyInfo),
                             "propertySelector" + SELECTOR_MUSTBEPROP);
#else
                Debug.Assert((memberExpression.Member.MemberType == MemberTypes.Property),
                             "propertySelector", SELECTOR_MUSTBEPROP);
#endif
                return memberExpression;
            }

            // for WPF
            var unaryExpression = propertySelector.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var innerMemberExpression = unaryExpression.Operand as MemberExpression;
                if (innerMemberExpression != null)
                {
#if WinRT
                    Debug.Assert((memberExpression.Member is PropertyInfo),
                                 "propertySelector" + SELECTOR_MUSTBEPROP);
#else
                    Debug.Assert((memberExpression.Member.MemberType == MemberTypes.Property),
                                 "propertySelector", SELECTOR_MUSTBEPROP);
#endif
                    return innerMemberExpression;
                }
            }

            // all else
            return null;
        }
    }

    #endregion

    #region ISuppressNotify

    /// <summary>
    /// Designates object than can suppress some notification
    /// </summary>
    public interface ISuppressNotify
    {
        /// <summary>
        /// Gets the suppress notify.
        /// </summary>
        /// <remarks>Use this in "using" statement</remarks>
        /// <value>The suppress notify.</value>
        IDisposable SuppressNotify { get; }
    }

    #endregion

    #region NotifyPropertyChangedCore

    /// <summary>
    /// Implements <see cref="INotifyPropertyChanged"/> on behalf of a container class.
    /// </summary>
    /// <remarks>
    /// <para>Use <see cref="NotifyPropertyChangedBase{TObject}"/> instead of this class if possible.</para>
    /// </remarks>
    /// <typeparam name="T">The type of the containing class.</typeparam>
    public sealed class NotifyPropertyChangedCore<T>
    {
        /// <summary>
        /// The backing delegate for <see cref="PropertyChanged"/>.
        /// </summary>
        private PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// The object that contains this instance.
        /// </summary>
        private readonly T _obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedCore{T}"/> class that is contained by <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object that contains this instance.</param>
        public NotifyPropertyChangedCore(T obj)
        {
            this._obj = obj;
        }

        /// <summary>
        /// Provides notification of changes to a property value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this._propertyChanged += value;
            }

            remove
            {
                this._propertyChanged -= value;
            }
        }

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            this._propertyChanged.Raise(this._obj, expression);
        }
        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The lambda expression identifying the property that changed.</param>
        public void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression)
        {
            this._propertyChanged.Raise(this._obj, expression);
        }
    }

    #endregion
}
