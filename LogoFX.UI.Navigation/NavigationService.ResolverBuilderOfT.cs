using System;

namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService
    {
        private sealed class ResolverBuilder<T> : RootableNavigationBuilder<T>
        {
            private readonly Func<T> _createFunc;

            public ResolverBuilder(INavigationService service, Func<T> createFunc)
                : base()
            {
                _createFunc = createFunc;
            }

            protected override T GetValueInternal()
            {
                return _createFunc();
            }
        }

    }
}