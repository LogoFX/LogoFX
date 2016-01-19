using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Practices.IoC.Extensions.SimpleInjector;
using NUnit.Framework;
using SimpleInjector;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector.Tests
{
    interface ISimpleModel
    {
         string Name { get; }
    }

    class SimpleModel : ISimpleModel
    {
        public SimpleModel(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [TestFixture]
    class ViewModelFactoryTests
    {
        [Test]
        public void
            GivenDependencyHasOneParameterNamedModelAndDependencyIsRegisteredPerRequest_WhenModelWrapperIsCreated_ThenModelWrapperIsNotNull
            ()
        {
            ISimpleModel model = new SimpleModel("6");
            var container = new Container();   
            container.Options.RegisterModelParameterConvention();
            container.Register<TestObjectViewModel>();

            var viewModelFactory = new ViewModelFactory(container);
            var modelWrapper = viewModelFactory.CreateModelWrapper<ISimpleModel, TestObjectViewModel>(model);

            Assert.AreEqual(model, modelWrapper.Model);
        }
    }

    class TestObjectViewModel : ObjectViewModel<ISimpleModel>
    {
        public TestObjectViewModel(ISimpleModel model)
            :base(model)
        {
            
        }
    }
}
