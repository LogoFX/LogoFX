namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Designates object with busy state.
    /// </summary>
    public interface ICanBeBusy
    {
        /// <summary>
        /// Gets or sets a value indicating whether the object is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is busy; otherwise, <c>false</c>.
        /// </value>
        bool IsBusy { get; set; }
    }
}
