using System.Collections.Generic;
using Attest.Fake.Moq;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.SimpleContainer;
using NUnit.Framework;

namespace LogoFX.Client.Bootstrapping.Tests
{
    [TestFixture]
    public class BootstrapperContainerTests
    {
        private FakeFactory _fakeFactory;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _fakeFactory = new FakeFactory();
        }

        [Test]
        [Ignore("WPF Application can't be created in tests")]
        public void Initialization_IocIsSet_RootViewModelIsDisplayed()
        {
            var mockWindowManager = _fakeFactory.CreateFake<IWindowManager>();

            var bootstrapperContainerUnderTest = new TestBootstrapperContainer(new ExtendedSimpleIocContainer());

            mockWindowManager.VerifyCall(
                t => t.ShowWindow(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
        }
    }
}
