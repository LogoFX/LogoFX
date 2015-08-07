using System.ComponentModel;
using System.Linq;
using LogoFX.Client.Mvvm.Core;
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
                return OwnDirty || TypeInformationProvider.GetDirtySourceValuesUnboxed(_type, this).Any(dirtySource => dirtySource.IsDirty);
            }
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
            CanUndo = false;
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
