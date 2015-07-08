// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Diagnostics;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// represents model that wraps foreign object
    /// </summary>
    public class ObjectModel:ObjectModel<object>,IObjectModel
    {
        public ObjectModel(ObjectModel other)
            : base(other)
        {
        }

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

        public ObjectModel()
        {
            
        }        
        public ObjectModel(ObjectModel<T> other):base(other)
        {
            _object = other.Object;
        }

        public ObjectModel(T param)
        {
            _object = param;
        }

        #region Object property

        /// <summary>
        /// Object property
        /// </summary>
        private T _object;

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

        protected virtual void OnObjectChangedOverride(T newValue, T oldValue)
        {
        }

        #endregion

        #region Parent property

        /// <summary>
        /// Parent property
        /// </summary>
        private IModel _parent;

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

        protected virtual void OnParentChangedOverride(IModel newValue, IModel oldValue)
        {
        }

        #endregion

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

        protected virtual void OnSaveOverride()
        {            
        }

        public virtual object Clone()
        {
            return new ObjectModel<T>(this);
        }
    }
}