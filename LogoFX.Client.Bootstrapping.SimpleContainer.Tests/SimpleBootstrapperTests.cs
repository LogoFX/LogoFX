using NUnit.Framework;

namespace LogoFX.Client.Bootstrapping.SimpleContainer.Tests
{
    [TestFixture]
    public class SimpleBootstrapperTests
    {
        [Test]
        public void
            GivenThereIsOneSimpleModuleAndThisModuleRegistersOneDependency_WhenBootstrapperIsCreated_ThenDependencyIsResolvedCorrectly
            ()
        {
            var container = new ExtendedSimpleIocContainer();
            var bootstrapper = new SimpleBootstrapper<TestRootViewModel>(container, useApplication:false);

            var testSimpleDependency = container.Resolve<ITestSimpleDependency>();
            Assert.IsNotNull(testSimpleDependency);
        }
    }
}
