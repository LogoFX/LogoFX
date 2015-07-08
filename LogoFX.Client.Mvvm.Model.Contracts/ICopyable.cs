namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<T>
    {
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="to">To.</param>
        void CopyTo(T to);
    }
}
