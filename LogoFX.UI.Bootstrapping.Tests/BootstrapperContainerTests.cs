using System.Collections.Generic;
using Caliburn.Micro;
using NUnit.Framework;
using Solid.Fake.Moq;

namespace LogoFX.UI.Bootstrapping.Tests
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

            var bootstrapperContainerUnderTest = new TestBootstrapperContainer();

            mockWindowManager.VerifyCall(
                t => t.ShowWindow(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<IDictionary<string, object>>()));
        }
    }
}
