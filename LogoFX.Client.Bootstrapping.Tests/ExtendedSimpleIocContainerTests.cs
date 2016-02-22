using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using NUnit.Framework;

namespace LogoFX.Client.Bootstrapping.Tests
{
    [TestFixture]
    class ExtendedSimpleIocContainerTests
    {
        [Test]
        public void Given_WhenDependencyIsRegisteredPerLifetimeAndDependencyIsResolved_ThenResolvedDependencyIsNotNull()
        {
            var container = new ExtendedSimpleContainerAdapter(new ExtendedSimpleContainer());
            container.RegisterPerLifetime<ITestDependency, TestDependency>(() => TestLifetimeScopeProvider.Current);
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency = container.Resolve<ITestDependency>();

            Assert.IsNotNull(dependency);
        }

        [Test]
        public void Given_WhenDependencyIsRegisteredPerLifetimeAndDependencyIsResolvedAndLifetimeChangesAndDependencyIsResolved_ThenResolvedDependenciesAreDifferent()
        {
            var container = new ExtendedSimpleContainerAdapter(new ExtendedSimpleContainer());
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
            var container = new ExtendedSimpleContainerAdapter(new ExtendedSimpleContainer());
            container.RegisterPerLifetime<ITestDependency, TestDependency>(() => TestLifetimeScopeProvider.Current);
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency = container.Resolve<ITestDependency>();
            TestLifetimeScopeProvider.Current = null;
            dependency = container.Resolve<ITestDependency>();

            Assert.IsNull(dependency);
        }

        [Test]
        public void Given_WhenDependencyIsRegisteredViaHandlerAndDependencyIsResolved_ThenResolvedDependencyIsNotNull()
        {
            var container = new ExtendedSimpleContainerAdapter(new ExtendedSimpleContainer());
            container.RegisterHandler<ITestDependency>(() => new TestDependency());
            TestLifetimeScopeProvider.Current = new TestObject();
            var dependency = container.Resolve<ITestDependency>();

            Assert.IsNotNull(dependency);
        }

        [Test]
        public void Given_WhenDependencyIsRegisteredViaHandlerAndDependencyIsResolvedTwice_ThenResolvedDependenciesAreDifferent()
        {
            var container = new ExtendedSimpleContainerAdapter(new ExtendedSimpleContainer());
            container.RegisterHandler<ITestDependency>(() => new TestDependency());            
            var dependencyOne = container.Resolve<ITestDependency>();
            var dependencyTwo = container.Resolve<ITestDependency>();

            Assert.AreNotSame(dependencyTwo, dependencyOne);
        }
    }
}
