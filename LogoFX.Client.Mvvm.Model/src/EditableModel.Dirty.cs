using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
//// The BindNotifierSubscriber isn't used
//// therefore we can save LogoFX.Client.Mvvm.Core package.

//#if NET45
//using LogoFX.Client.Mvvm.Core;
//#endif
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        /// <summary>
        /// Represents an API for subscribing and unsubscribing to inner property notifications
        /// </summary>
        private interface IInnerChangesSubscriber
        {
            void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate, Action isCanCancelChangesChangedDelegate);
            void UnsubscribeToNotifyingObjectChanges(object notifyingObject);
        }

        //TODO
        /// <summary>
        /// An implementation of inner changes subscriber which is based on explicit INPC subscription
        /// This implementation does NOT use Weak Delegates internally due to the 
        /// fact that such an implementation fails to work and it is therefore necessary
        /// to explicitly unsubscribe from the notifications - potential source of leaks
        /// </summary>
        private class PropertyChangedInnerChangesSubscriber : IInnerChangesSubscriber
        {
            private readonly WeakKeyDictionary<object, Tuple<Action, Action>> _handlers = new WeakKeyDictionary<object, Tuple<Action, Action>>();            

            public void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate,
                Action isCanCancelChangesChangedDelegate)
            {
                if (_handlers.ContainsKey(notifyingObject) == false)
                {
                    _handlers.Add(notifyingObject, new Tuple<Action, Action>(isDirtyChangedDelegate, isCanCancelChangesChangedDelegate));
                }
                var propertyChangedSource = notifyingObject as INotifyPropertyChanged;
                if (propertyChangedSource != null)
                {
                    propertyChangedSource.PropertyChanged += PropertyChangedSourceOnPropertyChanged;                                              
                }
            }

            public void UnsubscribeToNotifyingObjectChanges(object notifyingObject)
            {
                var propertyChangedSource = notifyingObject as INotifyPropertyChanged;
                if (propertyChangedSource != null)
                {
                    propertyChangedSource.PropertyChanged -= PropertyChangedSourceOnPropertyChanged;
                    _handlers.Remove(notifyingObject);
                }
            }

            private void PropertyChangedSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
            {
                if (_handlers.ContainsKey(sender))
                {
                    var delegates = _handlers[sender];
                    if (propertyChangedEventArgs.PropertyName == "IsDirty")
                    {
                        delegates.Item1.Invoke();
                    }
                    if (propertyChangedEventArgs.PropertyName == "CanCancelChanges")
                    {
                        delegates.Item2.Invoke();
                    }
                }                
            }
        }

//// The BindNotifierSubscriber isn't used
//// therefore we can save LogoFX.Client.Mvvm.Core package.
       
//#if NET45
//        /// <summary>
//        /// An implementation of inner changes subscriber which is based on bind notifier mechanism
//        /// it is more efficient than the INPC-based one but it uses weak events internally
//        /// and fails to work in the real application 
//        /// </summary>
//        private class BindNotifierInnerChangesSubscriber : IInnerChangesSubscriber
//        {
//            public void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate,
//                Action isCanCancelChangesChangedDelegate)
//            {                
//                notifyingObject.NotifyOn("IsDirty", (o, o1) =>
//                {
//                    isDirtyChangedDelegate();
//                });
//                notifyingObject.NotifyOn("CanCancelChanges", (o, o1) => isCanCancelChangesChangedDelegate());
//            }

//            public void UnsubscribeToNotifyingObjectChanges(object notifyingObject)
//            {
//                notifyingObject.UnNotifyOn("IsDirty");
//                notifyingObject.UnNotifyOn("CanCancelChanges");
//            }
//        }
//#endif


        private readonly IInnerChangesSubscriber _innerChangesSubscriber = new PropertyChangedInnerChangesSubscriber();

        private bool _isOwnDirty;

        /// <summary>
        /// Returns the Dirty state of the Model
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {                
                return OwnDirty || SourceValuesAreDirty() || SourceCollectionsAreDirty();
            }
        }

        private bool _canCancelChanges = true;

        /// <summary>
        /// Returns the value that denotes whether the model's changes can be cancelled
        /// Setting this value explicitly to false will disable changes cancellation
        /// independently of the Dirty state of the Model
        /// </summary>
        public bool CanCancelChanges
        {
            get { return _canCancelChanges && IsDirty; }
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
            return
                TypeInformationProvider.GetDirtySourceValuesUnboxed(Type, this)
                    .Any(dirtySource => dirtySource != null && dirtySource.IsDirty);
        }        

        private bool SourceCollectionsAreDirty()
        {
            return
                TypeInformationProvider.GetDirtySourceCollectionsUnboxed(Type, this)
                    .Any(dirtySource => dirtySource != null && dirtySource.IsDirty);
        }

        /// <summary>
        /// This state is used to store the information about the Model's own Dirty state
        /// The overall Dirty state is influenced by this value as well as by the singular and collections Dirty states
        /// </summary>
        private bool OwnDirty
        {
            get { return _isOwnDirty; }
            set
            {
                _isOwnDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
                NotifyOfPropertyChange(() => CanCancelChanges);
            }
        }

        /// <summary>
        /// Cancels the current changes in the Model
        /// </summary>
        public void CancelChanges()
        {
            RestoreFromHistory();
            CancelProperties();
            CancelCollections();
        }

        private void CancelProperties()
        {
            var cancelChangesProperties = TypeInformationProvider.GetCanCancelChangesSourceValuesUnboxed(Type, this);
            foreach (var cancelChangesProperty in cancelChangesProperties.Where(x => x!= null &&  x.CanCancelChanges))
            {
                cancelChangesProperty.CancelChanges();
            }
        }

        private void CancelCollections()
        {
            var cancelChangesCollectionItems = TypeInformationProvider.GetCanCancelChangesSourceCollectionsUnboxed(Type, this);
            foreach (var cancelChangesCollectionItem in cancelChangesCollectionItems.Where(x => x!= null && x.CanCancelChanges))
            {
                cancelChangesCollectionItem.CancelChanges();
            }
        }

        /// <summary>
        /// Marks the Model as Dirty and stores its copy for possible restore
        /// </summary>
        public virtual void MakeDirty()
        {
            if (OwnDirty && CanCancelChanges)
            {
                return;
            }            
            OwnDirty = true;
            AddToHistory();
        }   

        /// <summary>
        /// Clears the Dirty state of the Model
        /// </summary>
        /// <param name="forceClearChildren">true, if the children's state should be cleared, false otherwise</param>
        public virtual void ClearDirty(bool forceClearChildren = false)
        {
            OwnDirty = false;            
            if (forceClearChildren)
            {
                var dirtyProperties = TypeInformationProvider.GetDirtySourceValuesUnboxed(Type, this);
                foreach (var dirtyProperty in dirtyProperties)
                {
                    dirtyProperty.ClearDirty(true);
                }
                var dirtyCollectionItems = TypeInformationProvider.GetDirtySourceCollectionsUnboxed(Type, this);
                foreach (var dirtyCollectionItem in dirtyCollectionItems)
                {
                    dirtyCollectionItem.ClearDirty(true);
                }
            }
        }

        private void InitDirtyListener()
        {
            ListenToDirtyPropertyChange();
            var propertyInfos = TypeInformationProvider.GetPropertyDirtySourceCollections(Type, this).ToArray();
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

        /// <summary>
        /// The internal Dirty Source Collections might change from time to time
        /// In order to keep track of Dirty state changes in their items
        /// we must listen to the INCC events and subscribe/unsubscribe accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notifyCollectionChangedEventArgs"></param>
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
            
            if (TypeInformationProvider.IsPropertyDirtySource(Type, changedPropertyName) == false)
            {
                return;
            }
            
            var propertyValue = TypeInformationProvider.GetDirtySourceValue(Type, changedPropertyName, this);
            if (propertyValue != null)
            {
                NotifyOnInnerChange(propertyValue);
            }
        }

        /// <summary>
        /// Subscribes to the Dirty state changes of the potential Dirty Source 
        /// </summary>
        /// <param name="notifyingObject"></param>
        private void NotifyOnInnerChange(object notifyingObject)
        {
            _innerChangesSubscriber.SubscribeToNotifyingObjectChanges(notifyingObject, () =>
            {
                //This is the case where an inner Model reports a change in its Dirty state
                //If the current Model is not Dirty yet it should be marked as one
                var dirtySource = notifyingObject as ICanBeDirty;
                if (dirtySource != null && _history.CanUndo == false && dirtySource.IsDirty)
                {
                    AddToHistory();
                }
                NotifyOfPropertyChange(() => IsDirty);
            }, () => NotifyOfPropertyChange(() => CanCancelChanges));            
        }

        /// <summary>
        /// Unsubscribes from the Dirty state changes of the potential Dirty Source 
        /// </summary>
        /// <param name="notifyingObject"></param>
        private void UnNotifyOnInnerChange(object notifyingObject)
        {
            _innerChangesSubscriber.UnsubscribeToNotifyingObjectChanges(notifyingObject);
        }
    }
}
