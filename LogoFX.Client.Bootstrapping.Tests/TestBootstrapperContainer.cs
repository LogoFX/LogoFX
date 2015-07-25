using LogoFX.Client.Bootstrapping.SimpleContainer;

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
