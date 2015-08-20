namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        private Snapshot _undoBuffer;

        private void SetUndoBuffer(Snapshot snapshot)
        {
            _undoBuffer = snapshot;
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
