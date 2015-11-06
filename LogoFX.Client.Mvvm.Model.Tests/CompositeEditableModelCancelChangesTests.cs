using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeEditableModelCancelChangesTests
    {
        [Test]
        public void InnerModelIsMadeDirtyThenCancelChangesIsCalled_ModelDataIsRestoredAndIsDirtyIsFalse()
        {
            var simpleEditableModel = new SimpleEditableModel("Old Value", 10);
            var compositeModel = new CompositeEditableModel("location", new[] { simpleEditableModel });
            var deepHierarchyModel = new DeepHierarchyEditableModel(new[] { compositeModel });
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            simpleEditableModel.Name = "New Value";
            Assert.IsTrue(deepHierarchyModel.IsDirty);
            Assert.AreEqual(simpleEditableModel.Name, "New Value");
            deepHierarchyModel.CancelChanges();
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            Assert.AreEqual(simpleEditableModel.Name, "Old Value");
        }

        [Test]
        public void InnerModelAddedThenCancelChangesIsCalled_ModelDataIsRestoredAndIsDirtyIsFalse()
        {
            var expectedPhones = new[] { 546, 432 };
            var compositeModel = new CompositeEditableModel("Here", expectedPhones);
            compositeModel.AddPhone(647);
            compositeModel.CancelChanges();

            var phones = ((ICompositeEditableModel)compositeModel).Phones.ToArray();
            CollectionAssert.AreEqual(expectedPhones, phones);
            var isCompositeDirty = compositeModel.IsDirty;
            Assert.IsFalse(isCompositeDirty);
        }

        [Test]
        public void InnerModelInsideCollectionIsRemoved_CanCancelChangesIsTrue()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location", new[] { simpleEditableModel });
            var deepHierarchyModel = new DeepHierarchyEditableModel(new[] {compositeModel});
            compositeModel.RemoveSimpleItem(simpleEditableModel);

            Assert.IsTrue(deepHierarchyModel.CanCancelChanges);
        }        
    }
}