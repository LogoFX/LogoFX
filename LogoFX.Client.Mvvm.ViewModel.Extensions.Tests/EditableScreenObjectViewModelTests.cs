using Caliburn.Micro;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    [TestFixture]
    class EditableScreenObjectViewModelTests
    {
        [Test]
        [RequiresSTA]
        public void ModelIsChanged_WhenViewModelIsClosed_MessageBoxIsDisplayed()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();

            var rootObject = new TestEditableScreenObjectViewModel(mockMessageService, simpleModel);
            var windowManager = new FakeWindowManager();
            var window = windowManager.CreateWindow(rootObject);
            ScreenExtensions.TryActivate(rootObject);
            simpleModel.Name = DataGenerator.ValidName;
            window.Close();

            var wasCalled = mockMessageService.WasCalled;
            Assert.IsTrue(wasCalled);
        }

        [Test]
        [RequiresSTA]
        public void ModelIsNotChanged_WhenViewModelIsClosed_MessageBoxIsNotDisplayed()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();

            var rootObject = new TestEditableScreenObjectViewModel(mockMessageService, simpleModel);
            simpleModel.Name = DataGenerator.ValidName;
            rootObject.TryClose();

            var wasCalled = mockMessageService.WasCalled;
            Assert.IsFalse(wasCalled);
        }

        [Test]
        [RequiresSTA]
        public void ModelIsChanged_WhenViewModelIsClosedAndMessageResultIsYes_ModelIsSaved()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();
            mockMessageService.SetMessageResult(MessageResult.Yes);

            var rootObject = new TestEditableScreenObjectViewModel(mockMessageService, simpleModel);
            var windowManager = new FakeWindowManager();
            var window = windowManager.CreateWindow(rootObject);
            ScreenExtensions.TryActivate(rootObject);
            const string expectedValue = DataGenerator.ValidName;
            simpleModel.Name = expectedValue;
            window.Close();

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

            var rootObject = new TestEditableScreenObjectViewModel(mockMessageService, simpleModel);
            var windowManager = new FakeWindowManager();
            var window = windowManager.CreateWindow(rootObject);
            ScreenExtensions.TryActivate(rootObject);            
            simpleModel.Name = DataGenerator.ValidName;
            window.Close();

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

            var rootObject = new TestEditableScreenObjectViewModel(mockMessageService, simpleModel);
            var windowManager = new FakeWindowManager();
            var window = windowManager.CreateWindow(rootObject);
            ScreenExtensions.TryActivate(rootObject);
            simpleModel.Name = newValue;
            window.Close();

            var isDirty = simpleModel.IsDirty;
            Assert.True(isDirty);
            var actualValue = simpleModel.Name;
            Assert.AreEqual(DataGenerator.ValidName, actualValue);
        }
    }
}