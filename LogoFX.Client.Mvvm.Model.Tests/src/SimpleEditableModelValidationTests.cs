﻿using LogoFX.Client.Mvvm.Model.Contracts;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleEditableModelValidationTests
    {
        [Test]
        public void SimpleEditableModelIsValid_ErrorIsNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);

            AssertHelper.AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleEditableModelIsInvalid_ErrorIsNotNull()
        {            
            var model = new SimpleEditableModel(DataGenerator.InvalidName, 5);

            AssertHelper.AssertModelHasErrorIsTrue(model);
        }

        [Test]
        public void SimpleEditableModelIsValidExternalErrorIsSet_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.SetError("external error", "Name");

            AssertHelper.AssertModelHasErrorIsTrue(model);   
        }

        [Test]
        public void SimpleEditableModelIsValidExternalErrorIsSetAndErrorIsRemoved_ErrorIsNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.SetError("external error", "Name");
            model.ClearError("Name");

            AssertHelper.AssertModelHasErrorIsFalse(model);
        }

        [Test]
        public void SimpleEditableModelIsValidAndModelBecomesInvalid_ErrorIsNotNull()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);
            model.Name = DataGenerator.InvalidName;

            AssertHelper.AssertModelHasErrorIsTrue(model);
        }
    }
}
