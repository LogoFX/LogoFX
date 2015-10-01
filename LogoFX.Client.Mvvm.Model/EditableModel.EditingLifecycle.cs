using System.Collections.Generic;
using System.ComponentModel;
using Solid.Patterns.Memento;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {
        private readonly Stack<IMemento<EditableModel<T>>> _editStack = new Stack<IMemento<EditableModel<T>>>();

        void IEditableObject.BeginEdit()
        {
            var memento = new SnapshotMementoAdapter(this);
            _editStack.Push(memento);
            OnBeginEdit();
        }

        void IEditableObject.EndEdit()
        {
            var memento = _editStack.Pop();
            SetUndoBuffer(memento);
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
