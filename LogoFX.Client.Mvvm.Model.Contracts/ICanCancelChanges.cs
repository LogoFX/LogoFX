namespace LogoFX.Client.Mvvm.Model.Contracts
{
    public interface ICanCancelChanges
    {
        /// <summary>
        /// Cancels all changes and rolls back to the initial object state
        /// </summary>
        void CancelChanges();

        /// <summary>
        /// Sets or gets the value which determines whether object changes can be cancelled
        /// </summary>
        bool CanCancelChanges { get; set; }
    }
}