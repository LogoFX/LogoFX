namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object that can be merged into another object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMergeable<in T>
    {
        /// <summary>
        /// Merges the specified object.
        /// </summary>
        /// <param name="tomerge">The object to merge.</param>
        void Merge(T tomerge);
    }
}
