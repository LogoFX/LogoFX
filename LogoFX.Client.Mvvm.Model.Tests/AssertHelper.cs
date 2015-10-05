using System.ComponentModel;
using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    static class AssertHelper
    {
        internal static void AssertModelHasErrorIsFalse<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
            Assert.IsNullOrEmpty(error);
            Assert.IsFalse(hasErrors);
            Assert.IsTrue(collectionOfErrorsIsEmpty);
        }

        internal static void AssertModelHasErrorIsTrue<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
            Assert.IsNotNullOrEmpty(error);
            Assert.IsTrue(hasErrors);
            Assert.IsFalse(collectionOfErrorsIsEmpty);
        }
    }
}