using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleModelValidationTests
    {
        [Test]
        public void SimpleModelIsValid_ErrorIsNull()
        {
            var model = new SimpleModel(DataGenerator.ValidName, 5);

            AssertHelper.AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleModelIsInvalid_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(DataGenerator.InvalidName, 5);

            AssertHelper.AssertModelHasErrorIsTrue(model);
        }

        [Test]
        public void SimpleModelIsValidExternalErrorIsSet_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.SetError("external error", "Name");

            AssertHelper.AssertModelHasErrorIsTrue(model);
        }

        [Test]
        public void SimpleModelIsValidExternalErrorIsSetAndErrorIsRemoved_ErrorIsNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.SetError("external error", "Name");
            model.ClearError("Name");

            AssertHelper.AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleModelIsValidAndModelBecomesInvalid_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.Name = DataGenerator.InvalidName;

            AssertHelper.AssertModelHasErrorIsTrue(model);
        }
    }
}