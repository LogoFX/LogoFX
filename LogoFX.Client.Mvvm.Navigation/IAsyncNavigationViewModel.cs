using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation view model which allows asynchronous handling of navigating out event
    /// </summary>
    public interface IAsyncNavigationViewModel : INavigationViewModel
    {
        /// <summary>
        /// Allows handling of navigating out event in asynchronous fashion.
        /// </summary>
        /// <param name="direction">The navigation direction.</param>
        /// <returns></returns>
        Task<bool> BeforeNavigationOutAsync(NavigationDirection direction);
    }
}