using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Practices.IoC;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer.Tests
{
    [TestFixture]
    class ViewModelFactoryTests
    {
        [Test]
        public void
            GivenDependencyHasOneParameterNamedModelAndDependencyIsRegisteredPerRequest_WhenModelWrapperIsCreated_ThenModelWrapperIsNotNull
            ()
        {
            const string model = "6";           
            var container = new ExtendedSimpleContainer();
            container.RegisterPerRequest(typeof (ObjectViewModel<string>), null, typeof (ObjectViewModel<string>));

            var viewModelFactory = new ViewModelFactory(container);
            var modelWrapper = viewModelFactory.CreateModelWrapper<string, ObjectViewModel<string>>(model);

            Assert.AreEqual(model, modelWrapper.Model);
        }
    }
}
