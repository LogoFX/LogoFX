#if !WINDOWS_PHONE
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using LogoFX.Core;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {
        #region Nested Types

        protected sealed class Snapshot
        {
            #region Fields

            private readonly IList<ValidationResult> _validationErrors;

            private readonly IDictionary<PropertyInfo, object> _state = new Dictionary<PropertyInfo, object>();

            private readonly IDictionary<PropertyInfo, IList<object>> _listsState = new Dictionary<PropertyInfo, IList<object>>();

            private readonly bool _isDirty;

            #endregion

            #region Constructors

            public Snapshot(EditableModel<T> model)
            {
                foreach (PropertyInfo propertyInfo in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                {
                    if (propertyInfo.IsDefined(typeof(EditableListAttribute), true) &&
                        typeof(IList).IsAssignableFrom(propertyInfo.GetValue(model, null).GetType()))
                    {
                        _listsState.Add(new KeyValuePair<PropertyInfo, IList<object>>(propertyInfo,
                            new List<object>(((IList)propertyInfo.GetValue(model, null)).OfType<object>())));
                    }
                    else if (propertyInfo.CanWrite && propertyInfo.CanRead && propertyInfo.GetSetMethod() != null)
                    {
                        _state.Add(new KeyValuePair<PropertyInfo, object>(propertyInfo,
                                                                          propertyInfo.GetValue(model, null)));
                    }
                }
                //foreach (EventInfo eventInfo in model.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                //{
                //    _eventState.Add(new KeyValuePair<EventInfo, object>(eventInfo,
                //                                                      eventInfo.GetRaiseMethod(true),eventInfo.));                    
                //}
                _validationErrors = model.ValidationErrors;
                _isDirty = model.IsDirty;
            }

            #endregion

            #region Public Properties

            public bool IsDirty
            {
                get { return _isDirty; }
            }

            #endregion

            #region Public Methods

            public static Snapshot Take(EditableModel<T> model)
            {
                return new Snapshot(model);
            }

            public void Restore(EditableModel<T> model)
            {
                foreach (KeyValuePair<PropertyInfo, object> result in _state)
                {
                    if (result.Key.GetCustomAttributes(typeof(EditableSingleAttribute), true).Count() > 0 && result.Value is ICloneable<object>)
                    {
                        result.Key.SetValue(model, (result.Value as ICloneable<object>).Clone(), null);
                    }
                    else
                    {
                        result.Key.SetValue(model, result.Value, null);
                    }
                }
                
                foreach (KeyValuePair<PropertyInfo, IList<object>> result in _listsState)
                {
                    IList il = (IList)result.Key.GetValue(model, null);
                    //todo:optimize this
                    il.Clear();
                    if (((EditableListAttribute)result.Key.GetCustomAttributes(typeof(EditableListAttribute), true)[0]).CloneItems)
                        result.Value.ForEach(a => il.Add(a is ICloneable<object> ? ((ICloneable<object>)a).Clone() : a));
                    else
                        result.Value.ForEach(a => il.Add(a));
                    //
                }
                model.ValidationErrors = _validationErrors;
#if SILVERLIGHT
                model.NotifyOfErrorsChanged(new DataErrorsChangedEventArgs(null));
#else
                model.NotifyOfPropertyChange(() => model.Error);
#endif
                model.IsDirty = IsDirty;
            }

            #endregion
        }

        #endregion

        #region Fields

        private Snapshot _undoBuffer;        

        private bool _isDirty;        

        #endregion

        public EditableModel()
        {            
            InitErrorListener();
        }

        #region Protected Methods

        protected virtual void OnBeginEdit()
        {

        }

        protected virtual void OnEndEdit()
        {

        }

        protected virtual void OnCancelEdit()
        {

        }        

        #endregion

        #region Private Members

        private void SetUndoBuffer(Snapshot snapshot)
        {
            _undoBuffer = snapshot;            
            CanUndo = true;
        }

        private void RestoreFromUndoBuffer()
        {                        
            _undoBuffer.Restore(this);
            ClearUndoBuffer();
        }

        private void ClearUndoBuffer()
        {         
            CanUndo = false;
            ClearDirty();
        }

        #endregion              

        #region ICanBeDirty

        public virtual bool IsDirty
        {
            get { return _isDirty; }
            private set
            {
                if (_isDirty == value)
                {
                    return;
                }

                _isDirty = value;
                OnPropertyChanged(() => IsDirty);
            }
        }

        public virtual void ClearDirty()
        {
            IsDirty = false;
            CanUndo = false;
        }

        #endregion

        #region IEditableModel

        public void Undo()
        {
            RestoreFromUndoBuffer();
        }

        public virtual void MakeDirty()
        {
            if (IsDirty && CanUndo)
            {
                return;
            }

            IsDirty = true;
            SetUndoBuffer(new Snapshot(this));
        }

        private bool _canUndo;

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                if (_canUndo == value)
                {
                    return;
                }

                _canUndo = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion
    }

    public class EditableModel : EditableModel<int>
    {
        
    }
}