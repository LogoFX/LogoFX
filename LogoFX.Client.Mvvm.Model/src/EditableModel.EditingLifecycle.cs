using System;
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
            MakeDirty();
            OnBeginEdit();
        }

        void IEditableObject.EndEdit()
        {
            if (CanCommitChanges == false)
            {
                throw new InvalidOperationException("Cannot commit changes. Please check that the object is in editing state.");
            }
            CommitChanges();
            OnEndEdit();
        }

        void IEditableObject.CancelEdit()
        {
            if (CanCancelChanges == false)
            {
                throw new InvalidOperationException(
                    "Cannot cancel changes. Please check that the object is in editing state and changes cancelation is permitted.");
            }
            CancelChanges();
            OnCancelEdit();
        }

        /// <summary>
        /// Override this method to inject custom logic after the editing operation starts.
        /// </summary>
        protected virtual void OnBeginEdit()
        {

        }

        /// <summary>
        /// Override this method to inject custom logic after the editing operation completes.
        /// </summary>
        protected virtual void OnEndEdit()
        {

        }

        /// <summary>
        /// Override this method to inject custom logic after the editing operation is cancelled.
        /// </summary>
        protected virtual void OnCancelEdit()
        {

        }
    }
}
