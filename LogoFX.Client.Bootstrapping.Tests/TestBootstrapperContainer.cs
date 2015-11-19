using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;

namespace LogoFX.Client.Bootstrapping.Tests
{
    internal class TestBootstrapperContainer : BootstrapperContainerBase<TestViewModel, ExtendedSimpleIocContainer>
    {
        public TestBootstrapperContainer(ExtendedSimpleIocContainer iocContainer) :
            base(iocContainer)
        {
            
        }
    }
}
