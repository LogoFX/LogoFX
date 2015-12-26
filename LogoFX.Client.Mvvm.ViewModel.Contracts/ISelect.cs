namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents an object that can select.
    /// </summary>
    public interface ISelect
    {
        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="notify"></param>
        /// <returns>true if succeeded, otherwise false</returns>
        bool Select(object item, bool notify = true);
    }
}