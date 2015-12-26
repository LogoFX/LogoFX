namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents save changes result arguments.
    /// </summary>
    public sealed class ResultEventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the save changes is successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if successful; otherwise, <c>false</c>.
        /// </value>
        public bool Successful { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultEventArgs"/> class.
        /// </summary>
        /// <param name="successful">if set to <c>true</c> [successful].</param>
        public ResultEventArgs(bool successful)
        {
            Successful = successful;
        }
    }
}