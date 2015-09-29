namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents cloneable instance
    /// </summary>
    /// <typeparam name="T">Type of instance to be cloned</typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Cloned instance</returns>
        T Clone();
    }
}
