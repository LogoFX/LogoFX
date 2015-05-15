using System.Threading.Tasks;

namespace LogoFX.UI.Navigation
{
    public interface IAsyncNavigationViewModel : INavigationViewModel
    {
        Task<bool> BeforeNavigationOutAsync(NavigationDirection direction);
    }
}