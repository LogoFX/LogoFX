namespace LogoFX.Client.Mvvm.Navigation
{
    public interface IRootableNavigationBuilder<out T> : INavigationBuilder<T>
    {
        void AsRoot();
    }
}