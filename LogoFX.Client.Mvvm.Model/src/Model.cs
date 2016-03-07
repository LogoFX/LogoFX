using System;
using System.Collections.Generic;
#if NET45
using System.ComponentModel;
#endif
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LogoFX.Client.Core;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents model for domain use
    /// </summary>
    /// <typeparam name="T">Type of model identifier</typeparam>
    [DataContract]
    public partial class Model<T> : NotifyPropertyChangedBase<Model<T>>, IModel<T>        
        where T : IEquatable<T> 
    {
        /// <summary>
        /// Metadata info class
        /// </summary>
        /// <remarks>used for generic object display</remarks>
        class PropertyData : IPropertyData
        {
            private string _description;
            private readonly Model<T> _object;
            private readonly PropertyInfo _propInfo;
            private readonly string _displayName;

            /// <summary>
            /// Initializes a new instance of the <see cref="Model&lt;T&gt;.PropertyData"/> class.
            /// </summary>
            /// <param name="o">The o.</param>
            /// <param name="prop">The prop.</param>
            public PropertyData(Model<T> o, PropertyInfo prop)
            {
                _object = o;
                _propInfo = prop;

                DisplayAttribute a = prop.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
                if (a != null)
                {
                    _description = a.Description;
                    _displayName = a.Name;
                }
            }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value
            {
                get
                {
                    
                    return _propInfo.Name == "Item" ?null:_propInfo.GetValue(_object, null);
                }
                set
                {
                    if (_propInfo.Name != "Item")
                    {
                        _propInfo.SetValue(_object, value,null);
                        _object.NotifyOfPropertyChange(_propInfo);
                    }
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return _propInfo.Name; }
            }

            /// <summary>
            /// Gets a value indicating whether property have description.
            /// </summary>
            /// <value>
            ///   <c>true</c> if have description; otherwise, <c>false</c>.
            /// </value>
            public bool HaveDescription
            {
                get { return !string.IsNullOrWhiteSpace(_description); }
            }

            /// <summary>
            /// Property description.
            /// </summary>
            /// <value>
            /// The description.
            /// </value>
            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            /// <summary>
            /// Display name.
            /// </summary>
            public string DisplayName
            {
                get { return _displayName ?? Name; }
            }
        }

        /// <summary>
        /// Returns current object type.
        /// </summary>
        protected readonly Type Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Model&lt;T&gt;"/> class.
        /// </summary>
        public Model()
        {
            Type = GetType();
            InitErrorListener();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model&lt;T&gt;"/> class from other instance.
        /// </summary>
        /// <param name="other">The other model</param>
        public Model(IModel<T> other)
            :this()
        {
            _id = other.Id;
            _name = other.Name;
            _description = other.Description;
        }

#region Public Properties 

#region Id property
        
        private T _id;
        /// <summary>
        /// Model identifier
        /// </summary>
        public T Id
        {
            get { return _id; }
            set
            {
                if (Equals(_id, value))
                {
                    return;
                }                

                T oldValue = _id;
                _id = value;
                OnIdChangedOverride(value, oldValue);
                OnPropertyChanged(()=>Id);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during id set operation.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void OnIdChangedOverride(T newValue, T oldValue)
        {
        }

#endregion

#region Name property
        
        [DataMember(Name="Name")]
        private string _name;
        /// <summary>
        /// Model name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                string oldValue = _name;
                _name = value;
                OnNameChangedOverride(value, oldValue);
                NotifyOfPropertyChange(() => Name);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during name set operation..
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void OnNameChangedOverride(string newValue, string oldValue)
        {
        }

#endregion

#region Description property
        
        [DataMember(Name = "Description")]
        private string _description;
        /// <summary>
        /// Model description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                string oldValue = _description;
                _description = value;
                OnDescriptionChangedOverride(value, oldValue);
                OnPropertyChanged(()=>Description);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during name set operation..
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void OnDescriptionChangedOverride(string newValue, string oldValue)
        {
        }

#endregion


        /// <summary>
        /// Gets the list of properties associated with the current type.
        /// </summary>
        [Obsolete]
#if NET45
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]        
#endif
       
        public IEnumerable<IPropertyData> Properties
        {
            get
            {

                return GetType()
#if NET45
                    .GetProperties()                    
#else
                    .GetTypeInfo().DeclaredProperties
#endif
                    .Select(pi => new PropertyData(this, pi))
                    .Cast<IPropertyData>().ToList();
               
            }
        }

#endregion

#region IsReadOnly property
        
        private bool _isReadOnly;
        /// <summary>
        /// IsReadOnly property
        /// </summary>
        [Obsolete]
        public virtual bool IsReadOnly
        {
            get { return _isReadOnly; }
            protected set
            {
                if (_isReadOnly == value)
                    return;

                bool oldValue = _isReadOnly;
                _isReadOnly = value;
                OnIsReadOnlyChangedOverride(value, oldValue);
                OnPropertyChanged(()=>IsReadOnly);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during read only set operation..
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        protected virtual void OnIsReadOnlyChangedOverride(bool newValue, bool oldValue)
        {
        }

#endregion

#region Overrides  
              
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name)?base.ToString():Name;
        }

#endregion
    }

    /// <summary>
    /// Represents model with <see cref="int"/> as identifier.
    /// </summary>
    [DataContract]
    public class Model : Model<int>, IModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class
        /// </summary>
        public Model()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Model(Model other):base(other)
        {
        }
    }
}
