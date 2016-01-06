using System.ComponentModel;
using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    static class AssertHelper
    {
        internal static void AssertModelHasErrorIsFalse<T>(T model) where T : INotifyDataErrorInfo
#if NET45
                      , IDataErrorInfo
#endif  
        {            
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
#if NET45
            Assert.IsNullOrEmpty(model.Error);
#endif
            Assert.IsFalse(hasErrors);
            Assert.IsTrue(collectionOfErrorsIsEmpty);
        }

        internal static void AssertModelHasErrorIsTrue<T>(T model) where T : INotifyDataErrorInfo
#if NET45
                      , IDataErrorInfo
#endif
        {            
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
#if NET45
            Assert.IsNotNullOrEmpty(model.Error);
#endif
            Assert.IsTrue(hasErrors);
            Assert.IsFalse(collectionOfErrorsIsEmpty);
        }
    }
}