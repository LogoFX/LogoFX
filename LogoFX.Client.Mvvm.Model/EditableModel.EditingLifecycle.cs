using System.Collections.Generic;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T> : IEditableObject
    {
        private readonly Stack<Snapshot> _editStack = new Stack<Snapshot>();

        void IEditableObject.BeginEdit()
        {
            var snapshot = new Snapshot(this);
            _editStack.Push(snapshot);
            OnBeginEdit();
        }

        void IEditableObject.EndEdit()
        {
            var snapshot = _editStack.Pop();
            SetUndoBuffer(snapshot);
            OnEndEdit();
        }

        void IEditableObject.CancelEdit()
        {
            OnCancelEdit();
            _editStack.Pop().Restore(this);
        }

        protected virtual void OnBeginEdit()
        {

        }

        protected virtual void OnEndEdit()
        {

        }

        protected virtual void OnCancelEdit()
        {

        }
    }
}
