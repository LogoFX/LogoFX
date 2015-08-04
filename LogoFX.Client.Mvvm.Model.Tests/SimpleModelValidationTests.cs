using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleModelValidationTests
    {
        private const string InvalidName = "in$valid name";
        private const string ValidName = "valid name";

        [Test]
        public void SimpleModelIsValid_ErrorIsNull()
        {
            var model = new SimpleEditableModel(ValidName, 5);

            AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleModelIsInvalid_ErrorIsNotNull()
        {            
            var model = new SimpleEditableModel(InvalidName, 5);

            AssertModelHasErrorIsTrue(model);
        }

        [Test]
        public void SimpleModelIsValidExternalErrorIsSet_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(ValidName, 5);
            model.SetError("external error", "Name");

            AssertModelHasErrorIsTrue(model);   
        }

        [Test]
        public void SimpleModelIsValidExternalErrorIsSetAndErrorIsRemoved_ErrorIsNull()
        {
            var model = new SimpleEditableModel(ValidName, 5);
            model.SetError("external error", "Name");
            model.ClearError("Name");

            AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleModelIsValidAndModelBecomesInvalid_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(ValidName, 5);
            model.Name = InvalidName;

            AssertModelHasErrorIsTrue(model);
        }

        private static void AssertModelHasErrorIsTrue(EditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNotNullOrEmpty(error);
            Assert.IsTrue(hasErrors);
        }

        private static void AssertModelHasErrorIsFalse(EditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNullOrEmpty(error);
            Assert.IsFalse(hasErrors);
        }
    }
}
