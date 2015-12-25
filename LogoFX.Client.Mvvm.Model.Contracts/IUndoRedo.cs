namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object which is capable of managing the history of changes 
    /// and supports undo-redo operations
    /// </summary>
    public interface IUndoRedo
    {
        bool CanUndo { get; }
        bool CanRedo { get; }

        /// <summary>
        /// Cancels last change.
        /// </summary>
        void Undo();

        /// <summary>
        /// Re-applies latest change
        /// </summary>
        void Redo();
    }
}