namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents an object that can be dirty and allows managing dirty state
    /// </summary>
    public interface ICanBeDirty
    {
        /// <summary>
        /// True, if there is a change in the object, false otherwise
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Marks object as dirty
        /// </summary>
        void MakeDirty();

        /// <summary>
        /// Clears dirty state of the object, optionally propagating the change to its children
        /// </summary>
        /// <param name="forceClearChildren">True, if children dirty state should be cleared too, false otherwise</param>
        void ClearDirty(bool forceClearChildren = false);
    }
}
