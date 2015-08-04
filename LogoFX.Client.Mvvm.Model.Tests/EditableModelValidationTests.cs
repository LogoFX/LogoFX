using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class EditableModelValidationTests
    {
        [Test]
        public void ModelIsValid_ErrorIsNull()
        {
            var model = new SimpleEditableModel("valid name", 5);

            AssertModelErrorIsFalse(model);
        }

        [Test]
        public void ModelIsInvalid_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel("in$valid name", 5);

            AssertModelErrorIsTrue(model);
        }

        [Test]
        public void ModelIsValidExternalErrorIsSet_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel("valid name", 5);
            model.SetError("external error", "Name");

            AssertModelErrorIsTrue(model);   
        }

        [Test]
        public void ModelIsValidExternalErrorIsSetAndErrorIsRemoved_ErrorIsNull()
        {
            var model = new SimpleEditableModel("valid name", 5);
            model.SetError("external error", "Name");
            model.ClearError("Name");

            AssertModelErrorIsFalse(model);
        }

        private static void AssertModelErrorIsTrue(SimpleEditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNotNullOrEmpty(error);
            Assert.IsTrue(hasErrors);
        }

        private static void AssertModelErrorIsFalse(SimpleEditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNullOrEmpty(error);
            Assert.IsFalse(hasErrors);
        }
    }
}
