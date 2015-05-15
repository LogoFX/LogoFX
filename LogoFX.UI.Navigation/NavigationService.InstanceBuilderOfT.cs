namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService
    {
        private sealed class InstanceBuilder<T> : RootableNavigationBuilder<T>
        {
            private readonly T _instance;

            public InstanceBuilder(INavigationService service, T instance)
                : base()
            {
                _instance = instance;
            }

            protected override T GetValueInternal()
            {
                return _instance;
            }
        }

    }
}