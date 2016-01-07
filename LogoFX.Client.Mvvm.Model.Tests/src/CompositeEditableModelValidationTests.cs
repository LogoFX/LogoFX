using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeEditableModelValidationTests
    {
        [Test]
        public void InnerModelIsValid_ErrorIsNull()
        {
            var compositeModel = new CompositeEditableModel("location");

            compositeModel.Person.Name = DataGenerator.ValidName;

            AssertHelper.AssertModelHasErrorIsFalse(compositeModel);
        }

        [Test]
        public void InnerModelIsInvalid_ErrorIsNotNull()
        {
            var compositeModel = new CompositeEditableModel("location");

            compositeModel.Person.Name = DataGenerator.InvalidName;

            AssertHelper.AssertModelHasErrorIsTrue(compositeModel);
        }

        [Test]
        public void InnerModelIsReset_ErrorNotificationIsRaised()
        {
            var compositeModel = new CompositeEditableModel("location");
            var isRaised = false;
            compositeModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Error")
                {
                    isRaised = true;    
                }                
            };

            compositeModel.Person = new SimpleEditableModel(DataGenerator.ValidName, 0);            

            Assert.IsTrue(isRaised);
        }

        [Test]
        [Ignore("This feature isn't supported yet")]
        public void InnerModelPropertyIsReset_ErrorNotificationIsRaised()
        {
            var compositeModel = new CompositeEditableModel("location");
            var isRaised = false;
            compositeModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Error")
                {
                    isRaised = true;
                }
            };

            compositeModel.Person.Name = DataGenerator.InvalidName;

            Assert.IsTrue(isRaised);
        }
    }
}
