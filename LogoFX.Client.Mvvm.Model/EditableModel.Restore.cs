using Solid.Patterns.Memento;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        private IMemento<EditableModel<T>> _history;

        private void SetHistory(IMemento<EditableModel<T>> memento)
        {
            _history = memento;
        }

        protected virtual void RestoreFromHistory()
        {
            if (_history != null)
            {
                _history.Restore(this);    
            }            
            ClearHistory();
        }

        private void ClearHistory()
        {
            ClearDirty();
            _history = null;
        }
    }
}
