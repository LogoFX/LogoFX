namespace LogoFX.UI.Navigation
{
    public interface INavigationViewModel
    {
        void OnNavigated(NavigationDirection direction, object argument);
    }
}