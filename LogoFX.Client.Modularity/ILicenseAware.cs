namespace LogoFX.Client.Modularity
{
    /// <summary>
    /// Represents an object that is bound by a license.
    /// </summary>
    public interface ILicenseAware
    {
        /// <summary>
        /// Gets a value indicating whether this instance may be used with respect to the current license.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is licensed; otherwise, <c>false</c>.
        /// </value>
        bool IsLicensed { get; }
    }
}