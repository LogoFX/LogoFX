namespace LogoFX.UI.Navigation
{
    public interface IRootableNavigationBuilder<out T> : INavigationBuilder<T>
    {
        void AsRoot();
    }
}