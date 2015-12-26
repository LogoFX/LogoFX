using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents proxy to the real property on the model.
    /// </summary>
    public class ModelProxyAttribute : Attribute
    {
        private string _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProxyAttribute"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public ModelProxyAttribute(string property)
        {
            _property = property;
        }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }
    }

    /// <summary>
    /// Represents object view model for the specified model.
    /// </summary>
    /// <typeparam name="T">The specified model.</typeparam>
    public class ObjectViewModel<T> : ObjectViewModel,IObjectViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        public ObjectViewModel()
            : this(default(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ObjectViewModel(T model)
            : base(model)
        {
        }

        #region ObjectModel property

        /// <summary>
        /// ObjectModel property
        /// </summary>
#if !WinRT
        [Obsolete("Use Model instead")]
#endif
        public  T ObjectModel
        {
            get { return (T)base.InternalModel; }
        }

        /// <summary>
        /// Model property
        /// </summary>
#if WinRT
        [Obsolete("Use ObjectModel instead")]
#endif
        public new T Model
        {get
        {
#if WinRT
            throw new NotImplementedException();
#else
            return (T)InternalModel;
#endif
        }
        }



        #endregion
    }

    /// <summary>
    /// <c>ViewModel</c> that wraps arbitrary object
    /// </summary>
    [DebuggerDisplay("Model={_model}")]
    public class ObjectViewModel : ViewModelBase, IObjectViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ObjectViewModel(object model)
        {
            _model = model;
            if(model is INotifyPropertyChanged)
            {
                (model as INotifyPropertyChanged).PropertyChanged += WeakDelegate.From(OnModelPropertyChangedCore);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel"/> class.
        /// </summary>
        public ObjectViewModel()
            : this(null)
        {
        }

        #region ObjectModel property

        private object _model;

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public  object Model
        {
            get { return InternalModel; }
            set
            {
                InternalModel = value;
            }
        }

        internal object InternalModel
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyOfPropertyChange(()=>DisplayName);
            }            
        }

        #endregion

        #region overrides
        
        private string _externalDisplayName;
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [ModelProxy("Name")]
        public override string DisplayName
        {
            get
            {
                return _externalDisplayName ?? (InternalModel!=null?InternalModel.ToString():GetType().Name);
            }
            set
            {
                _externalDisplayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        /// <summary>
        /// Called when some models property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when a model's property is changed.
        /// </summary>
        public event PropertyChangedEventHandler ModelsPropertyChanged;

        private void InvokeModelPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = ModelsPropertyChanged;
            if (handler != null) handler(this, e);
        }

        private Dictionary<string, IList<PropertyInfo>> _proxyNotifiers; 
#if SILVERLIGHT 
        public
#else
        private
#endif
        void OnModelPropertyChangedCore(object sender, PropertyChangedEventArgs e)
        {
            if(_proxyNotifiers == null)
            {
                Action<PropertyInfo> act = (pi) =>
                {
                    string pr = pi.GetCustomAttributes(typeof(ModelProxyAttribute), true).OfType<ModelProxyAttribute>().First().Property;
                    if (!_proxyNotifiers.ContainsKey(pr))
                        _proxyNotifiers.Add(pr, new List<PropertyInfo>());
                    _proxyNotifiers[pr].Add(pi);
                };

                _proxyNotifiers = new Dictionary<string, IList<PropertyInfo>>();
#if WinRT
                GetType().GetTypeInfo().DeclaredProperties
#else
                GetType().GetProperties(BindingFlags.Instance|BindingFlags.FlattenHierarchy|BindingFlags.Public)
#endif
                    .Where(a => a.IsDefined(typeof(ModelProxyAttribute), true))
                    .ForEach(act);
            }
            IList<PropertyInfo> toNotify;
            if(_proxyNotifiers.TryGetValue(e.PropertyName,out toNotify))
            {
                toNotify.ForEach(NotifyOfPropertyChange);
            }
            OnModelPropertyChanged(sender,e);
            InvokeModelPropertyChanged(e);
        }

        #endregion
    }
}
