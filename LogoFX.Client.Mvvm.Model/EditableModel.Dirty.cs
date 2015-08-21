using System;
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
        private interface IInnerChangesSubscriber
        {
            void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate, Action isCanCancelChangesChangedDelegate);
            void UnsubscribeToNotifyingObjectChanges(object notifyingObject);
        }

        private class PropertyChangedInnerChangesSubscriber : IInnerChangesSubscriber
        {
            private Action _isDirtyChangedDelegate;
            private Action _isCanCancelChangesChangedDelegate;

            public void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate,
                Action isCanCancelChangesChangedDelegate)
            {
                _isDirtyChangedDelegate = isDirtyChangedDelegate;
                _isCanCancelChangesChangedDelegate = isCanCancelChangesChangedDelegate;
                var propertyChangedSource = notifyingObject as INotifyPropertyChanged;
                if (propertyChangedSource != null)
                {
                    propertyChangedSource.PropertyChanged += PropertyChangedSourceOnPropertyChanged;
                    propertyChangedSource.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "IsDirty")
                        {
                            isDirtyChangedDelegate.Invoke();
                        }
                        if (args.PropertyName == "CanCancelChanges")
                        {
                            isCanCancelChangesChangedDelegate.Invoke();
                        }
                    };
                }
            }

            public void UnsubscribeToNotifyingObjectChanges(object notifyingObject)
            {
                var propertyChangedSource = notifyingObject as INotifyPropertyChanged;
                if (propertyChangedSource != null)
                {
                    propertyChangedSource.PropertyChanged -= PropertyChangedSourceOnPropertyChanged;
                }
            }

            private void PropertyChangedSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
            {
                if (propertyChangedEventArgs.PropertyName == "IsDirty")
                {
                    _isDirtyChangedDelegate.Invoke();
                }
                if (propertyChangedEventArgs.PropertyName == "CanCancelChanges")
                {
                    _isCanCancelChangesChangedDelegate.Invoke();
                }
            }
        }

        private class BindNotifierInnerChangesSubscriber : IInnerChangesSubscriber
        {
            public void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate,
                Action isCanCancelChangesChangedDelegate)
            {
                notifyingObject.NotifyOn("IsDirty", (o, o1) =>
                {
                    isDirtyChangedDelegate();
                });
                notifyingObject.NotifyOn("CanCancelChanges", (o, o1) => isCanCancelChangesChangedDelegate());
            }

            public void UnsubscribeToNotifyingObjectChanges(object notifyingObject)
            {
                notifyingObject.UnNotifyOn("IsDirty");
                notifyingObject.UnNotifyOn("CanCancelChanges");
            }
        }

        private readonly IInnerChangesSubscriber _innerChangesSubscriber = new PropertyChangedInnerChangesSubscriber();

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
            SetUndoBuffer(new SnapshotMementoAdapter(this));
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
            _innerChangesSubscriber.SubscribeToNotifyingObjectChanges(notifyingObject, () =>
            {
                var dirtySource = notifyingObject as ICanBeDirty;
                if (dirtySource != null && _undoBuffer == null && dirtySource.IsDirty)
                {
                    MakeDirty();
                }
                NotifyOfPropertyChange(() => IsDirty);
            }, () => NotifyOfPropertyChange(() => CanCancelChanges));            
        }

        private void UnNotifyOnInnerChange(object removedItem)
        {
            _innerChangesSubscriber.UnsubscribeToNotifyingObjectChanges(removedItem);
        }
    }
}
