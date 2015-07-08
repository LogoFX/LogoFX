namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationViewModel
    {
        void OnNavigated(NavigationDirection direction, object argument);
    }
}