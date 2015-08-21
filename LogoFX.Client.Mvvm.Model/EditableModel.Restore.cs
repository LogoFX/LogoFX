using LogoFX.Client.Mvvm.Model.Contracts;

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
            _undoBuffer.Restore(this);
            ClearUndoBuffer();
        }

        private void ClearUndoBuffer()
        {
            ClearDirty(forceClearChildren:true);
        }
    }
}
