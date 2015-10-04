namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents object that allows managing collection of external errors
    /// </summary>
    public interface IHaveExternalErrors
    {
        /// <summary>
        /// Sets external error to the specific property
        /// </summary>
        /// <param name="error">External error</param>
        /// <param name="propertyName">Property name</param>
        void SetError(string error, string propertyName);

        /// <summary>
        /// Clears external error from the specific property
        /// </summary>
        /// <param name="propertyName">Property name</param>
        void ClearError(string propertyName);
    }

    /// <summary>
    /// Represents object that allows managing collection of errors
    /// </summary>
    public interface IHaveErrors
    {
        /// <summary>
        /// True if object has errors, false otherwise
        /// </summary>
        bool HasErrors { get; }        
    }
}