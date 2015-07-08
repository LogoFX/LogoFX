namespace LogoFX.Client.Mvvm.Navigation
{
    internal sealed partial class NavigationService
    {
        private abstract class NavigationBuilder<T> : NavigationBuilder, INavigationBuilder<T>
        {
            public INavigationBuilder<T> AsSingleton()
            {
                IsSingleton = true;
                return this;
            }

            public INavigationBuilder<T> AssignConductor<TConductor>()
                where TConductor : INavigationConductor
            {
                ConductorType = typeof (TConductor);
                return this;
            }

            protected abstract T GetValueInternal();

            T INavigationBuilder<T>.GetValue()
            {
                return GetValueInternal();
            }

            protected sealed override object GetValue()
            {
                return GetValueInternal();
            }
        }

    }
}