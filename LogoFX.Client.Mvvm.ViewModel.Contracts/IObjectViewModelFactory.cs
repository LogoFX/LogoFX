namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// <see cref="IViewModel"/> factory
    /// </summary>
    public interface IObjectViewModelFactory
    {
        /// <summary>
        /// Creates view model
        /// </summary>
        /// <param name="parent">Parent model</param>
        /// <param name="obj">Object for which we making model</param>
        /// <returns></returns>
        IObjectViewModel CreateViewModel(IViewModel parent, object obj);
    }
}