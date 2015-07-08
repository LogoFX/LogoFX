using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface IAsyncNavigationViewModel : INavigationViewModel
    {
        Task<bool> BeforeNavigationOutAsync(NavigationDirection direction);
    }
}