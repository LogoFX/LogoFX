namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object that is able to cancel model changes
    /// </summary>
    public interface ICanCancelChanges
    {
        /// <summary>
        /// Cancels all changes and rolls back to the initial object state
        /// </summary>
        void CancelChanges();

        /// <summary>
        /// Gets or sets the value indicating whether object changes can be cancelled
        /// </summary>
        bool CanCancelChanges { get; set; }
    }
}