using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeModelCancelChangesTests
    {
        [Test]
        public void InnerModelIsMadeDirtyThenCancelChangesIsCalled_ModelDataIsRestored()
        {
            var expectedPhones = new[] {546, 432};
            var compositeModel = new CompositeEditableModel("Here", expectedPhones);
            compositeModel.AddPhone(647);
            compositeModel.CancelChanges();

            var phones = ((ICompositeEditableModel)compositeModel).Phones.ToArray();
            CollectionAssert.AreEqual(expectedPhones, phones);
        }
    }
}