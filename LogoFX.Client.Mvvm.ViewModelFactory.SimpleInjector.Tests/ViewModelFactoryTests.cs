using LogoFX.Client.Mvvm.ViewModel;
using NUnit.Framework;
using SimpleInjector;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector.Tests
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
            var container = new Container();   
            container.Options.RegisterModelParameterConvention();
            container.Register<TestObjectViewModel>();

            var viewModelFactory = new ViewModelFactory(container);
            var modelWrapper = viewModelFactory.CreateModelWrapper<string, TestObjectViewModel>(model);

            Assert.AreEqual(model, modelWrapper.Model);
        }
    }

    class TestObjectViewModel : ObjectViewModel<string>
    {
        public TestObjectViewModel(string model)
            :base(model)
        {
            
        }
    }
}
