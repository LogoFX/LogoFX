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
            while (_history.CanUndo && (_checkPoint == null ||(_checkPoint != null && _checkPoint.Equals(_history.PeekUndo()) == false)))
            {
                _history.Undo();
            }                        
            ClearDirty();
        }
    }
}
