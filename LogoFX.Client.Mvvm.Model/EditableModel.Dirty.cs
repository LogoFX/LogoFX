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

        private bool SourceValuesAreDirty()
        {
            return TypeInformationProvider.GetDirtySourceValuesUnboxed(_type, this).Any(dirtySource => dirtySource.IsDirty);
        }        

        private bool SourceCollectionsAreDirty()
        {
            var propertyInfos = TypeInformationProvider.GetPropertyDirtySourceCollections(_type, this).ToArray();
            return propertyInfos
                .Select(propertyInfo => propertyInfo.GetValue(this)).OfType<IEnumerable<IEditableModel>>()
                .Any(dirtySourceCollection => dirtySourceCollection.Any(t => t.IsDirty));
        }

        private bool OwnDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
            }
        }

        public virtual void ClearDirty(bool forceClearChildren = false)
        {
            OwnDirty = false;
            CanCancelChanges = false;
            if (forceClearChildren)
            {
                var children = TypeInformationProvider.GetDirtySourceValuesUnboxed(_type, this);
                foreach (var dirtyChild in children)
                {
                    dirtyChild.ClearDirty(true);
                }
            }
        }

        private void InitDirtyListener()
        {
            ListenToDirtyPropertyChange();
            //will be moved to another method:
            var propertyInfos = TypeInformationProvider.GetPropertyDirtySourceCollections(_type, this).ToArray();
            foreach (var propertyInfo in propertyInfos)
            {
                var notifyCollectionChanged = propertyInfo.GetValue(this) as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += WeakDelegate.From(NotifyCollectionChangedOnCollectionChanged);
                }
            }
        }

        private void NotifyCollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                default :
                    NotifyOfPropertyChange(() => IsDirty);
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
                propertyValue.NotifyOn("IsDirty", (o, o1) => NotifyOfPropertyChange(() => IsDirty));
            }
        }
    }
}
