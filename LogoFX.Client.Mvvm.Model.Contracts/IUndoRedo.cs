namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object which is capable of managing the history of changes 
    /// and supports undo-redo operations
    /// </summary>
    public interface IUndoRedo
    {
        /// <summary>
        /// Gets the value indicating whether the last change can be undone.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be undone; otherwise, <c>false</c>.
        /// </value>
        bool CanUndo { get; }

        /// <summary>
        /// Gets the value indicating whether the last change can be redone.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be redone; otherwise, <c>false</c>.
        /// </value>
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