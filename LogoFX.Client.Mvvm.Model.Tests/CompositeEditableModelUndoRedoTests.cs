using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeEditableModelUndoRedoTests
    {
        [Test]
        public void InnerModelAddedThenCancelChangesIsCalled_ModelDataIsRestoredAndIsDirtyIsFalse()
        {
            var expectedPhones = new[] { 546, 432 };
            var compositeModel = new CompositeEditableModelWithUndoRedo("Here", expectedPhones);
            compositeModel.AddPhone(647);
            compositeModel.CancelChanges();

            var phones = ((ICompositeEditableModel)compositeModel).Phones.ToArray();
            CollectionAssert.AreEqual(expectedPhones, phones);
            var isCompositeDirty = compositeModel.IsDirty;
            Assert.IsFalse(isCompositeDirty);
        }
    }
}
