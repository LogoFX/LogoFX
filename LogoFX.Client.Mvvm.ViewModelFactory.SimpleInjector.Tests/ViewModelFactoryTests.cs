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

        [Test]
        [Ignore("Simple Injector doesn't support ctor parameters resolution per each call")]
        public void
            GivenDependencyHasOneParameterNamedModelAndDependencyIsRegisteredPerRequest_WhenTwoModelWrappersAreCreated_ThenModelWrappersAreDifferent
            ()
        {
            ISimpleModel modelOne = new SimpleModel("6");
            ISimpleModel modelTwo = new SimpleModel("7");
            var container = new Container();
            container.Options.RegisterModelParameterConvention();
            container.Register<TestObjectViewModel>();

            var viewModelFactory = new ViewModelFactory(container);
            var modelWrapperOne = viewModelFactory.CreateModelWrapper<ISimpleModel, TestObjectViewModel>(modelOne);
            var modelWrapperTwo = viewModelFactory.CreateModelWrapper<ISimpleModel, TestObjectViewModel>(modelTwo);

            Assert.AreEqual(modelTwo, modelWrapperTwo.Model,
                "Expected name is " + modelTwo.Name + " but the actual name is " + modelWrapperTwo.Model.Name);
            Assert.AreNotSame(modelWrapperTwo.Model, modelWrapperOne.Model);
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
