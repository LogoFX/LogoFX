namespace LogoFX.Client.Modularity
{
    /// <summary>
    /// Represents an object that has an available state
    /// </summary>
    public interface IAvailable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is available, i.e. the instance exists.
        /// Normally used when the instance belongs to a collection.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        bool IsAvailable { get; }
    }
}
