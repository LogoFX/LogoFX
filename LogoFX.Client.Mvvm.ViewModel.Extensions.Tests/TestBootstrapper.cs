using JetBrains.Annotations;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    [UsedImplicitly]
    class TestBootstrapper : BootstrapperContainerBase<TestConductorViewModel,ExtendedSimpleContainerAdapter>
    {
        public TestBootstrapper(ExtendedSimpleContainerAdapter container)
            :base(container, false)
        {
            
        }
    }
}
