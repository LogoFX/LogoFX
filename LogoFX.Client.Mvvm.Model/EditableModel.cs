#if !WINDOWS_PHONE
#endif
using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : Model<T>, IEditableModel
        where T : IEquatable<T>
    {
        private Snapshot _undoBuffer;

        private readonly Type _type;

        public EditableModel()
        {
            _type = GetType();
            InitErrorListener();
            InitDirtyListener();
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
            CanCancelChanges = true;
        }

        private void RestoreFromUndoBuffer()
        {                        
            _undoBuffer.Restore(this);
            ClearUndoBuffer();
        }

        private void ClearUndoBuffer()
        {         
            CanCancelChanges = false;
            ClearDirty();
        }

        #endregion                      

        #region IEditableModel

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

        private bool _canCancelChanges;

        public bool CanCancelChanges
        {
            get { return _canCancelChanges; }
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

        #endregion
    }

    public partial class EditableModel : EditableModel<int>
    {
        
    }
}