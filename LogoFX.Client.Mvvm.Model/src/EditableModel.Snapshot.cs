#if !NET45
using System;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;
using Solid.Patterns.Memento;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {
        interface ISnapshot
        {
            void Restore(EditableModel<T> model);
        }

        sealed class Snapshot : ISnapshot
        {
            private readonly IDictionary<PropertyInfo, object> _state = new Dictionary<PropertyInfo, object>();

            private readonly IDictionary<PropertyInfo, IList<object>> _listsState = new Dictionary<PropertyInfo, IList<object>>();

            private readonly bool _isDirty;

            public Snapshot(EditableModel<T> model)
            {                
                var storableProperties = TypeInformationProvider.GetStorableProperties(model.GetType());                
                foreach (PropertyInfo propertyInfo in storableProperties)
                {
                    if (propertyInfo.IsDefined(typeof(EditableListAttribute), true) &&
                        typeof(IList).IsAssignableFrom(propertyInfo.GetValue(model, null).GetType()))
                    {
                        _listsState.Add(new KeyValuePair<PropertyInfo, IList<object>>(propertyInfo,
                            new List<object>(((IList)propertyInfo.GetValue(model, null)).OfType<object>())));
                    }
                    else if (propertyInfo.CanWrite && propertyInfo.CanRead && propertyInfo.SetMethod != null)
                    {
                        _state.Add(new KeyValuePair<PropertyInfo, object>(propertyInfo,
                                                                          propertyInfo.GetValue(model, null)));
                    }
                }                
                _isDirty = model.IsDirty;
            }

            public void Restore(EditableModel<T> model)
            {
                foreach (KeyValuePair<PropertyInfo, object> result in _state)
                {
                    if (result.Key.GetCustomAttributes(typeof(EditableSingleAttribute), true).Any() && result.Value is ICloneable<object>)
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
                    //TODO:optimize this
                    il.Clear();
                    if (((EditableListAttribute)result.Key.GetCustomAttributes(typeof(EditableListAttribute), true).First()).CloneItems)
                        result.Value.ForEach(a => il.Add(a is ICloneable<object> ? ((ICloneable<object>)a).Clone() : a));
                    else
                        result.Value.ForEach(a => il.Add(a));                    
                }
#if SILVERLIGHT
                model.NotifyOfErrorsChanged(new DataErrorsChangedEventArgs(null));
#else
                model.NotifyOfPropertyChange(() => model.Error);
#endif
                model.OwnDirty = _isDirty;
            }
        }

        sealed class HierarchicalSnapshot : ISnapshot
        {            
            private readonly IDictionary<PropertyInfo, object> _state = new Dictionary<PropertyInfo, object>();

            private readonly IDictionary<PropertyInfo, IList<object>> _listsState = new Dictionary<PropertyInfo, IList<object>>();

            private readonly bool _isOwnDirty;

            public HierarchicalSnapshot(EditableModel<T> model)
            {                
                var storableProperties = TypeInformationProvider.GetStorableProperties(model.GetType());                
                foreach (PropertyInfo propertyInfo in storableProperties)
                {                    
                    if (propertyInfo.IsDefined(typeof(EditableListAttribute), true) )
                    {
                        var value = propertyInfo.GetValue(model, null);
                        if (typeof(IList).IsAssignableFrom(value.GetType()))
                        {
                            if (value is IEnumerable<IEditableModel>)
                            {
                                var unboxedValue = value as IEnumerable<IEditableModel>;
                                var serializedList =
                                    unboxedValue.Select(
                                        t => (t is ICloneable<object>) ? ((ICloneable<object>) t).Clone() : t).ToArray();
                                _listsState.Add(new KeyValuePair<PropertyInfo, IList<object>>(propertyInfo,
                                    serializedList));
                            }
                            else
                            {
                                _listsState.Add(new KeyValuePair<PropertyInfo, IList<object>>(propertyInfo,
                                    new List<object>(((IList) value).OfType<object>())));
                            }
                        }
                    }
                    else if (propertyInfo.CanWrite && propertyInfo.CanRead && propertyInfo.SetMethod != null)
                    {
                        _state.Add(new KeyValuePair<PropertyInfo, object>(propertyInfo,
                                                                          propertyInfo.GetValue(model, null)));
                    }
                }
                _isOwnDirty = model.OwnDirty;
            }

            public void Restore(EditableModel<T> model)
            {
                foreach (KeyValuePair<PropertyInfo, object> result in _state)
                {
                    if (result.Key.GetCustomAttributes(typeof(EditableSingleAttribute), true).Any() && result.Value is ICloneable<object>)
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
                    IList list = (IList)result.Key.GetValue(model, null);                    
                    list.Clear();
                    result.Value.ForEach(a => list.Add(a));                    
                }
                model.OwnDirty = _isOwnDirty;
            }
        }

        sealed class SnapshotMementoAdapter : IMemento<EditableModel<T>>
        {
            private readonly ISnapshot _snapshot;

            internal SnapshotMementoAdapter(EditableModel<T> model)
            {
                _snapshot = new HierarchicalSnapshot(model);
            }

            public IMemento<EditableModel<T>> Restore(EditableModel<T> target)
            {
                IMemento<EditableModel<T>> inverse = new SnapshotMementoAdapter(target);
                _snapshot.Restore(target);
                return inverse;
            }
        }
    }
}
