// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LogoFX.Client.Core;
using LogoFX.Client.Model.Contracts;

namespace LogoFX.Client.Model
{
    /// <summary>
    /// Represents model for domain use
    /// </summary>
    /// <typeparam name="T">type of model identifier</typeparam>
    [DataContract]
    public class Model<T> : NotifyPropertyChangedBase<Model<T>>, IModel<T>,
#if !SILVERLIGHT && !WinRT
        IDataErrorInfo 
#else
        INotifyDataErrorInfo 
#endif
        where T:IEquatable<T> 
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
        /// Initializes a new instance of the <see cref="Model&lt;T&gt;"/> class.
        /// </summary>
        public Model()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model&lt;T&gt;"/> class from other instance.
        /// </summary>
        /// <param name="other">The other model</param>
        public Model(IModel<T> other)
        {
            _id = other.Id;
            _name = other.Name;
            _description = other.Description;
        }

        #region Public Properties 

        #region Id property

        /// <summary>
        /// Model identifier
        /// </summary>
        private T _id;

        public T Id
        {
            get { return _id; }
            set
            {
                if (_id.Equals(value))
                    return;

                T oldValue = _id;
                _id = value;
                OnIdChangedOverride(value, oldValue);
                OnPropertyChanged(()=>Id);
            }
        }

        protected virtual void OnIdChangedOverride(T newValue, T oldValue)
        {
        }

        #endregion

        #region Name property

        /// <summary>
        /// Model name
        /// </summary>
        [DataMember(Name="Name")]
        private string _name;

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

        protected virtual void OnNameChangedOverride(string newValue, string oldValue)
        {
        }

        #endregion

        #region Description property

        /// <summary>
        /// Model description
        /// </summary>
        [DataMember(Name = "Description")]
        private string _description;

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

        protected virtual void OnDescriptionChangedOverride(string newValue, string oldValue)
        {
        }

        #endregion


#if !SILVERLIGHT && !WinRT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]        
#endif
        public IEnumerable<IPropertyData> Properties
        {
            get
            {

                return GetType()
#if WinRT
                    .GetTypeInfo().DeclaredProperties
#else
                    .GetProperties()
#endif
                    .Select(pi => new PropertyData(this, pi))
                    .Cast<IPropertyData>().ToList();
               
            }
        }

        #endregion

        #region IsReadOnly property

        /// <summary>
        /// IsReadOnly property
        /// </summary>
        private bool _isReadOnly;

        private IList<ValidationResult> _validationErrors;

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

        protected virtual void OnIsReadOnlyChangedOverride(bool newValue, bool oldValue)
        {
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return String.IsNullOrWhiteSpace(Name)?base.ToString():Name;
        }

        #endregion
#if !SILVERLIGHT && !WinRT
        public virtual string this[string columnName]
        {
            get { return null; }
        }

        public virtual string Error
        {
            get { return null; }
        }
#else
        public virtual IEnumerable GetErrors(string propertyName)
        {
            return null;
        }

        public bool HasErrors
        {
            get { return false; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void NotifyOfErrorsChanged(DataErrorsChangedEventArgs e)
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler != null) handler(this, e);
        }
#endif
        protected internal IList<ValidationResult> ValidationErrors
        {
            get { return _validationErrors ?? (_validationErrors = new ObservableCollection<ValidationResult>()); }
            internal set { _validationErrors = value; }
        }
      
    }

    [DataContract]
    public class Model:Model<int>,IModel
    {
        public Model()
        {            
        }
        public Model(Model other):base(other)
        {
        }
    }
}
