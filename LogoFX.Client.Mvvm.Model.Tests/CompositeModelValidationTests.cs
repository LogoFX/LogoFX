﻿using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeModelValidationTests
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
    }

    [TestFixture]
    class CompositeModelDirtyTests
    {
        [Test]
        public void InnerModelIsNotMadeDirty_IsDirtyIsFalse()
        {
            var compositeModel = new CompositeEditableModel("location");            

            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsMadeDirty_IsDirtyIsTrue()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;

            Assert.IsTrue(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsReset_DirtyNotificationIsRaised()
        {
            var compositeModel = new CompositeEditableModel("location");
            var isRaised = false;
            compositeModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsDirty")
                {
                    isRaised = true;
                }
            };
            compositeModel.Person = new SimpleEditableModel(DataGenerator.ValidName, 0);

            Assert.IsTrue(isRaised);
        }

        [Test]
        public void InnerModelIsMadeDirtyThenClearDirtyIsCalledWithChildrenEnforcement_IsDirtyIsFalse()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;
            compositeModel.ClearDirty(forceClearChildren:true);

            Assert.IsFalse(compositeModel.IsDirty);
        }

        [Test]
        public void InnerModelIsMadeDirtyThenClearDirtyIsCalledWithoutChildrenEnforcement_IsDirtyIsTrue()
        {
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.Person.Name = DataGenerator.InvalidName;
            compositeModel.ClearDirty(forceClearChildren: false);

            Assert.IsTrue(compositeModel.IsDirty);
        }
    }
}