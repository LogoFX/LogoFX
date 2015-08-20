using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        private bool _isDirty;

        public virtual bool IsDirty
        {
            get
            {                
                return OwnDirty || SourceValuesAreDirty() || SourceCollectionsAreDirty();
            }
        }

        private bool _canCancelChanges = true;

        public bool CanCancelChanges
        {
            get { return _canCancelChanges && (SourceValuesAreDirty() || SourceCollectionsAreDirty()); }
            set
            {
                if (_canCancelChanges == value)
                {
                    return;
                }

                _canCancelChanges = value;
                NotifyOfPropertyChange();
            }
        }

        private bool SourceValuesAreDirty()
        {
            return TypeInformationProvider.GetDirtySourceValuesUnboxed(_type, this).Any(dirtySource => dirtySource.IsDirty);
        }        

        private bool SourceCollectionsAreDirty()
        {
            return
                TypeInformationProvider.GetDirtySourceCollectionsUnboxed(_type, this)
                    .Any(dirtySource => dirtySource.IsDirty);
        }

        private bool OwnDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
                NotifyOfPropertyChange(() => CanCancelChanges);
            }
        }

        public void CancelChanges()
        {
            RestoreFromUndoBuffer();                        
        }

        public virtual void MakeDirty()
        {
            if (OwnDirty && CanCancelChanges)
            {
                return;
            }            
            OwnDirty = true;
            SetUndoBuffer(new Snapshot(this));
        }   

        public virtual void ClearDirty(bool forceClearChildren = false)
        {
            OwnDirty = false;            
            if (forceClearChildren)
            {
                var dirtyProperties = TypeInformationProvider.GetDirtySourceValuesUnboxed(_type, this);
                foreach (var dirtyProperty in dirtyProperties)
                {
                    dirtyProperty.ClearDirty(true);
                }
                var dirtyCollectionItems = TypeInformationProvider.GetDirtySourceCollectionsUnboxed(_type, this);
                foreach (var dirtyCollectionItem in dirtyCollectionItems)
                {
                    dirtyCollectionItem.ClearDirty(true);
                }
            }
        }

        private void InitDirtyListener()
        {
            ListenToDirtyPropertyChange();            
            var propertyInfos = TypeInformationProvider.GetPropertyDirtySourceCollections(_type, this).ToArray();
            foreach (var propertyInfo in propertyInfos)
            {
                var actualValue = propertyInfo.GetValue(this); 
                var notifyCollectionChanged = actualValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += WeakDelegate.From(NotifyCollectionChangedOnCollectionChanged);
                }
                var enumerable = actualValue as IEnumerable<ICanBeDirty>;
                if (enumerable != null)
                {
                    foreach (var canBeDirty in enumerable)
                    {
                        NotifyOnInnerChange(canBeDirty);
                    }
                }
            }
        }

        private void NotifyCollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                    case NotifyCollectionChangedAction.Add:
                    var addedItems = notifyCollectionChangedEventArgs.NewItems;
                    foreach (var addedItem in addedItems)
                    {
                        NotifyOnInnerChange(addedItem);
                    }
                    NotifyOfPropertyChange(() => IsDirty);
                    NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Remove:
                    var removedItems = notifyCollectionChangedEventArgs.OldItems;
                    foreach (var removedItem in removedItems)
                    {
                        UnNotifyOnInnerChange(removedItem);
                    }
                    NotifyOfPropertyChange(() => IsDirty);
                    NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Reset:
                    NotifyOfPropertyChange(() => IsDirty);
                    NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Move: case NotifyCollectionChangedAction.Replace:                    
                    break;                    
            }
        }

        private void ListenToDirtyPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnDirtyPropertyChanged);
        }

        private void OnDirtyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            if (changedPropertyName == "IsDirty")
            {                
                if (IsDirty)
                {
                    if (_undoBuffer == null)
                    {
                        SetUndoBuffer(new Snapshot(this));
                    }    
                }                
            }
            if (TypeInformationProvider.IsPropertyDirtySource(_type, changedPropertyName) == false)
            {
                return;
            }
            
            var propertyValue = TypeInformationProvider.GetDirtySourceValue(_type, changedPropertyName, this);
            if (propertyValue != null)
            {
                NotifyOnInnerChange(propertyValue);
            }
        }

        private void NotifyOnInnerChange(object notifyingObject)
        {
            notifyingObject.NotifyOn("IsDirty", (o, o1) => NotifyOfPropertyChange(() => IsDirty));
            notifyingObject.NotifyOn("CanCancelChanges", (o, o1) => NotifyOfPropertyChange(() => CanCancelChanges));
        }

        private void UnNotifyOnInnerChange(object notifyingObject)
        {
            notifyingObject.UnNotifyOn("IsDirty");
            notifyingObject.UnNotifyOn("CanCancelChanges");
        }
    }
}
