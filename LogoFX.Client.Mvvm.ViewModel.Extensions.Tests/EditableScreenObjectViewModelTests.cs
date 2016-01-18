using System.Linq;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Mvvm.ViewModel.Services;
using LogoFX.Client.Mvvm.ViewModel.Shared;
using LogoFX.Client.Tests.NUnit;
using LogoFX.Client.Tests.Shared.Caliburn.Micro;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    [TestFixture]
    class EditableScreenObjectViewModelTests : IntegrationTestsBase<ExtendedSimpleContainerAdapter, TestConductorViewModel,TestBootstrapper>
    {
        //Note: may use here IntegrationTestsBaseWithActivation as well - package still not available.
        protected override TestConductorViewModel CreateRootObjectOverride(TestConductorViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }

        protected override void OnAfterTeardown()
        {
            base.OnAfterTeardown();
            TestHelper.Teardown();
        }

        [Test]        
        public void ModelIsChanged_WhenViewModelIsClosed_MessageBoxIsDisplayed()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();

            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);            
            simpleModel.Name = DataGenerator.ValidName;
            screenObjectViewModel.CloseCommand.Execute(null);

            var wasCalled = mockMessageService.WasCalled;
            Assert.IsTrue(wasCalled);
        }

        [Test]        
        public void ModelIsNotChanged_WhenViewModelIsClosed_MessageBoxIsNotDisplayed()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();

            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);            
            screenObjectViewModel.CloseCommand.Execute(null);

            var wasCalled = mockMessageService.WasCalled;
            Assert.IsFalse(wasCalled);
        }

        [Test]        
        public void ModelIsChanged_WhenViewModelIsClosedAndMessageResultIsYes_ModelIsSaved()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();
            mockMessageService.SetMessageResult(MessageResult.Yes);

            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);
            const string expectedValue = DataGenerator.ValidName;
            simpleModel.Name = expectedValue;
            screenObjectViewModel.CloseCommand.Execute(null);

            var isDirty = simpleModel.IsDirty;
            Assert.False(isDirty);
            var actualValue = simpleModel.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        [RequiresSTA]
        public void ModelIsChanged_WhenViewModelIsClosedAndMessageResultIsNo_ModelIsNotSaved()
        {
            string initialValue = string.Empty;
            var simpleModel = new SimpleEditableModel(initialValue,20);
            var mockMessageService = new FakeMessageService();
            mockMessageService.SetMessageResult(MessageResult.No);
            RegisterInstance<IMessageService>(mockMessageService);

            var rootObject = CreateRootObject();         
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);                        
            simpleModel.Name = DataGenerator.ValidName;
            screenObjectViewModel.CloseCommand.Execute(null);

            var isDirty = simpleModel.IsDirty;
            Assert.False(isDirty);
            var actualValue = simpleModel.Name;
            Assert.AreEqual(initialValue, actualValue);
        }

        [Test]
        [RequiresSTA]
        public void ModelIsChanged_WhenViewModelIsClosedAndMessageResultIsCancel_ModelIsSaved()
        {
            string initialValue = string.Empty;
            const string newValue = DataGenerator.ValidName;
            var simpleModel = new SimpleEditableModel(initialValue, 20);
            var mockMessageService = new FakeMessageService();
            mockMessageService.SetMessageResult(MessageResult.Cancel);

            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);
            simpleModel.Name = newValue;
            screenObjectViewModel.CloseCommand.Execute(null);            

            var isDirty = simpleModel.IsDirty;
            Assert.True(isDirty);
            var actualValue = simpleModel.Name;
            Assert.AreEqual(DataGenerator.ValidName, actualValue);
        }

        [Test]
        [RequiresSTA]
        public void ModelIsChanged_WhenViewModelIsClosedAndMessageResultIsNo_ThenOnChangesCancelingIsCalled()
        {
            string initialValue = string.Empty;
            var simpleModel = new SimpleEditableModel(initialValue, 20);
            var mockMessageService = new FakeMessageService();
            mockMessageService.SetMessageResult(MessageResult.No);
            RegisterInstance<IMessageService>(mockMessageService);

            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            rootObject.ActivateItem(screenObjectViewModel);
            simpleModel.Name = DataGenerator.ValidName;
            screenObjectViewModel.CloseCommand.Execute(null);

            var wasCalled = screenObjectViewModel.WasCancelingChangesCalled;
            Assert.True(wasCalled);
        }

        [Test]
        public void WhenModelIsChangedAndChangesAreAppliedAndModelIsChangedAndChangesAreCancelled_ThenCorrectModelIsDisplayed()
        {
            var initialPhones = new[] { 546, 432 };
            var compositeModel = new CompositeEditableModel("Here", initialPhones);
            var mockMessageService = new FakeMessageService();            
            
            var rootObject = CreateRootObject();
            var screenObjectViewModel = new TestEditableScreenCompositeObjectViewModel(mockMessageService, compositeModel);
            rootObject.ActivateItem(screenObjectViewModel);
            compositeModel.AddPhone(647);
            screenObjectViewModel.ApplyCommand.Execute(null);
            compositeModel.AddPhone(555);
            screenObjectViewModel.CancelChangesCommand.Execute(null);

            var phones = ((ICompositeEditableModel)compositeModel).Phones.ToArray();
            var expectedPhones = new[] { 546, 432, 647 };
            CollectionAssert.AreEqual(expectedPhones, phones);
        }
    }
}