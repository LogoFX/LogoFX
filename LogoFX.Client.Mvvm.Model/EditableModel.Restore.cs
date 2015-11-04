using System.Diagnostics;
using Solid.Patterns.Memento;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        private IMemento<EditableModel<T>> _undoBuffer;

        private void SetUndoBuffer(IMemento<EditableModel<T>> memento)
        {
            _undoBuffer = memento;
        }

        private void RestoreFromUndoBuffer()
        {
            if (OwnDirty)
            {
                _undoBuffer.Restore(this);
                ClearUndoBuffer();
            }
        }

        private void ClearUndoBuffer()
        {
            ClearDirty();
            _undoBuffer = null;
        }
    }
}
