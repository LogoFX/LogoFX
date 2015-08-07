using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    static class AssertHelper
    {
        internal static void AssertModelHasErrorIsFalse(EditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNullOrEmpty(error);
            Assert.IsFalse(hasErrors);
        }

        internal static void AssertModelHasErrorIsTrue(EditableModel model)
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNotNullOrEmpty(error);
            Assert.IsTrue(hasErrors);
        }
    }
}