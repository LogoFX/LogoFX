using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeModelCancelChangesTests
    {
        [Test]
        public void InnerModelIsMadeDirtyThenCancelChangesIsCalled_ModelDataIsRestoredAndIsDirtyIsFalse()
        {
            var expectedPhones = new[] {546, 432};
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

        [Test]
        public void InnerModelInsideCollectionIsRemovedAndCancelChangesIsCalled_ModelIsRestored()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location");
            var deepHierarchyModel = new DeepHierarchyEditableModel();
            compositeModel.AddSimpleModelImpl(simpleEditableModel);
            deepHierarchyModel.AddCompositeItemImpl(compositeModel);
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            deepHierarchyModel.CancelChanges();

            Assert.IsFalse(deepHierarchyModel.CanCancelChanges);
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            CollectionAssert.AreEquivalent(new[] {compositeModel}, deepHierarchyModel.CompositeModels);
            CollectionAssert.AreEquivalent(new[] { simpleEditableModel }, deepHierarchyModel.CompositeModels.First().SimpleCollection);
        }

        [Test]
        public void InnerCollectionItemIsRemovedAndCancelChangesIsCalledAndInnerModelInsideCollectionIsRemovedAndCancelChangesIsCalled_ModelIsRestored()
        {
            var simpleEditableModelOne = new SimpleEditableModel(); 
            var simpleEditableModelTwo = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location");
            var deepHierarchyModel = new DeepHierarchyEditableModel();
            compositeModel.AddSimpleModelImpl(simpleEditableModelOne);
            compositeModel.AddSimpleModelImpl(simpleEditableModelTwo);
            deepHierarchyModel.AddCompositeItemImpl(compositeModel);

            deepHierarchyModel.RemoveCompositeModel(compositeModel);
            deepHierarchyModel.CancelChanges();
            ((CompositeEditableModel)deepHierarchyModel.CompositeModels.First()).RemoveSimpleItem(simpleEditableModelOne);
            deepHierarchyModel.CancelChanges();

            Assert.IsFalse(deepHierarchyModel.CanCancelChanges);
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            CollectionAssert.AreEquivalent(new[] { compositeModel }, deepHierarchyModel.CompositeModels);
            CollectionAssert.AreEquivalent(new[] {simpleEditableModelOne, simpleEditableModelTwo},
                deepHierarchyModel.CompositeModels.First().SimpleCollection);
        }
    }
}