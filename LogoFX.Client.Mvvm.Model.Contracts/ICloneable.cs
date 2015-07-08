namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents cloneable instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
