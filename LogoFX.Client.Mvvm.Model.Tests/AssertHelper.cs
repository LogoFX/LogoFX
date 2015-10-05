using System.ComponentModel;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    static class AssertHelper
    {
        internal static void AssertModelHasErrorIsFalse<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNullOrEmpty(error);
            Assert.IsFalse(hasErrors);
        }

        internal static void AssertModelHasErrorIsTrue<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var error = model.Error;
            var hasErrors = model.HasErrors;
            Assert.IsNotNullOrEmpty(error);
            Assert.IsTrue(hasErrors);
        }
    }
}