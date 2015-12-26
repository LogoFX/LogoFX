namespace LogoFX.Client.Mvvm.ViewModel.Shared
{
    /// <summary>
    /// Available message results.
    /// </summary>
    public enum MessageResult
    {
        /// <summary>
        /// No result available.
        /// </summary>
        None = 0,

        /// <summary>
        /// Message is acknowledged.
        /// </summary>
        OK = 1,

        /// <summary>
        /// Message is canceled.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// Message is acknowledged with yes.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// Message is acknowledged with no.
        /// </summary>
        No = 7
    }
}