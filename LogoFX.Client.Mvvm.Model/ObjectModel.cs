using System;
using System.Diagnostics;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents model that wraps foreign object
    /// </summary>
    public class ObjectModel:ObjectModel<object>,IObjectModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        /// <param name="other"></param>
        public ObjectModel(ObjectModel other)
            : base(other)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel"/> class.
        /// </summary>
        /// <param name="param"></param>
        public ObjectModel(object param):base(param)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectModel<T> : Model, IObjectModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel{T}"/> class.
        /// </summary>
        public ObjectModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel{T}"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public ObjectModel(ObjectModel<T> other):base(other)
        {
            _object = other.Object;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectModel{T}"/> class.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public ObjectModel(T param)
        {
            _object = param;
        }

        #region Object property
        
        private T _object;
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public virtual T Object
        {
            get { return _object; }
            set
            {
                if ((object)_object == (object)value)
                    return;

                T oldValue = _object;
                _object = value;
                OnObjectChangedOverride(value, oldValue);
                OnPropertyChanged(()=>Object);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during object set operation.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void OnObjectChangedOverride(T newValue, T oldValue)
        {
        }

        #endregion

        #region Parent property

        private IModel _parent;
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public IModel Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                IModel oldValue = _parent;
                _parent = value;
                OnParentChangedOverride(value, oldValue);
                OnPropertyChanged(()=>Parent);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic during parent set operation.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        protected virtual void OnParentChangedOverride(IModel newValue, IModel oldValue)
        {
        }

        #endregion

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            try
            {
                OnSaveOverride();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Override this to inject custom logic during save operation.
        /// </summary>
        protected virtual void OnSaveOverride()
        {            
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return new ObjectModel<T>(this);
        }
    }
}