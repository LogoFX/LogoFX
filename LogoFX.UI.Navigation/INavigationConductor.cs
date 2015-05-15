namespace LogoFX.UI.Navigation
{
    public interface INavigationConductor
    {
        void NavigateTo(object viewModel, object argument);
    }
}