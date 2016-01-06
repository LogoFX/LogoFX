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
            _history.Do(memento);
            OnEndEdit();
        }

        void IEditableObject.CancelEdit()
        {
            OnCancelEdit();
            _editStack.Pop().Restore(this);
        }

        /// <summary>
        /// Override this method to inject custom logic when the editing operation starts.
        /// </summary>
        protected virtual void OnBeginEdit()
        {

        }

        /// <summary>
        /// Override this method to inject custom logic when the editing operation completes.
        /// </summary>
        protected virtual void OnEndEdit()
        {

        }

        /// <summary>
        /// Override this method to inject custom logic when the editing operation is cancelled.
        /// </summary>
        protected virtual void OnCancelEdit()
        {

        }
    }
}
