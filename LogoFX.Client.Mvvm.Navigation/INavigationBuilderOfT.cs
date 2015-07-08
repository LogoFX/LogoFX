namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationBuilder<out T> : INavigationBuilder
    {
        INavigationBuilder<T> AsSingleton();

        INavigationBuilder<T> AssignConductor<TConductor>()
            where TConductor : INavigationConductor;
        new T GetValue();
    }
}