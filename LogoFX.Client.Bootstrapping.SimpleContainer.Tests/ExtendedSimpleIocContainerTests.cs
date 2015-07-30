using NUnit.Framework;

namespace LogoFX.Client.Bootstrapping.SimpleContainer.Tests
{
    [TestFixture]
    class ExtendedSimpleIocContainerTests
    {
        [Test]
        public void Given_WhenDependencyIsRegisteredPerLifetimeAndDependencyIsResolved_ThenResolvedDependencyIsNotNull()
        {
            var container = new ExtendedSimpleIocContainer();
            container.RegisterPerLifetime<ITestDependency, TestDependency>(() => TestLifetimeScopeProvider.Current);
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency = container.Resolve<ITestDependency>();

            Assert.IsNotNull(dependency);
        }

        [Test]
        public void Given_WhenDependencyIsRegisteredPerLifetimeAndDependencyIsResolvedAndLifetimeChangesAndDependencyIsResolved_ThenResolvedDependenciesAreDifferent()
        {
            var container = new ExtendedSimpleIocContainer();
            container.RegisterPerLifetime<ITestDependency, TestDependency>(() => TestLifetimeScopeProvider.Current);
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency1 = container.Resolve<ITestDependency>();
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency2 = container.Resolve<ITestDependency>();

            Assert.AreNotEqual(dependency1, dependency2);
        }

        [Test]
        public void Given_WhenDependencyIsRegisteredPerLifetimeAndDependencyIsResolvedAndLifetimeIsSetToNullAndDependencyIsResolved_ThenResolvedDependencyIsNull()
        {
            var container = new ExtendedSimpleIocContainer();
            container.RegisterPerLifetime<ITestDependency, TestDependency>(() => TestLifetimeScopeProvider.Current);
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency = container.Resolve<ITestDependency>();
            TestLifetimeScopeProvider.Current = null;
            dependency = container.Resolve<ITestDependency>();

            Assert.IsNull(dependency);
        }
    }
}
