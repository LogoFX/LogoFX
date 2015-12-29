using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;

namespace LogoFX.Client.Bootstrapping.Tests
{
    internal class TestBootstrapperContainer : BootstrapperContainerBase<TestViewModel, ExtendedSimpleContainerAdapter>
    {
        public TestBootstrapperContainer(ExtendedSimpleContainerAdapter iocContainerAdapter) :
            base(iocContainerAdapter)
        {
            
        }
    }
}
