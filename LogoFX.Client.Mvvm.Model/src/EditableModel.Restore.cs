namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        private readonly UndoRedoHistory<EditableModel<T>> _history;

        private void AddToHistory()
        {
            _history.Do(new SnapshotMementoAdapter(this));
        }

        private void RestoreFromHistory()
        {
            while (_history.CanUndo)
            {
                _history.Undo();
            }                        
            ClearDirty();
        }
    }
}
