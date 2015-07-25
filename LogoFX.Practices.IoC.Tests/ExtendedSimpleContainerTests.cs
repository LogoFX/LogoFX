using NUnit.Framework;

namespace LogoFX.Practices.IoC.Tests
{   
    [TestFixture]
    class ExtendedSimpleContainerTests
    {
        [Test]
        public void
            GivenDependencyHasOneNamedParameter_WhenDependencyIsRegisteredAndDependencyIsResolvedWithParameter_ThenResolvedDependencyValueIsCorrect
            ()
        {
            var container = new ExtendedSimpleContainer();
            container.RegisterPerRequest(typeof(ITestParameterDependency), null, typeof(TestParameterDependency));
            const string model = "5";
            var dependency = container.GetInstance(typeof (ITestParameterDependency), null,
                new IParameter[] {new NamedParameter("model", model)}) as ITestParameterDependency;

            var actualModel = dependency.Model;
            Assert.AreEqual(model, actualModel);
        }
    }
}
