namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationConductor
    {
        void NavigateTo(object viewModel, object argument);
    }
}