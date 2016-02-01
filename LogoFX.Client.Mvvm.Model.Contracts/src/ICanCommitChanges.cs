namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object that is able to commit model changes
    /// </summary>
    public interface ICanCommitChanges
    {
        /// <summary>
        /// Commits the changes and cleans up the dirty (being edited) object state.
        /// </summary>
        void CommitChanges();

        /// <summary>
        /// Gets a value indicating whether the model changes can be committed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the model changes can be committed; otherwise, <c>false</c>.
        /// </value>
        bool CanCommitChanges { get; }
    }
}