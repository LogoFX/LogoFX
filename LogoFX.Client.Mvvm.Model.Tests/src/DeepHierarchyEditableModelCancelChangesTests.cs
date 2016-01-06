using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class DeepHierarchyEditableModelCancelChangesTests
    {
        [Test]
        public void InnerModelInsideCollectionIsRemovedAndCancelChangesIsCalled_ModelIsRestored()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location");
            compositeModel.AddSimpleModelImpl(simpleEditableModel);
            var deepHierarchyModel = new DeepHierarchyEditableModel();
            deepHierarchyModel.AddCompositeItemImpl(compositeModel);

            compositeModel.RemoveSimpleItem(simpleEditableModel);
            deepHierarchyModel.CancelChanges();

            Assert.IsFalse(deepHierarchyModel.CanCancelChanges);
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            CollectionAssert.AreEquivalent(new[] { compositeModel }, deepHierarchyModel.CompositeModels);
            CollectionAssert.AreEquivalent(new[] { simpleEditableModel }, deepHierarchyModel.CompositeModels.First().SimpleCollection);
        }

        [Test]
        public void InnerCollectionItemIsRemovedAndCancelChangesIsCalledAndInnerModelInsideCollectionIsRemovedAndCancelChangesIsCalled_ModelIsRestored()
        {
            var simpleEditableModelOne = new SimpleEditableModel();
            var simpleEditableModelTwo = new SimpleEditableModel();
            var compositeModelOne = new CompositeEditableModel("location");
            var compositeModelTwo = new CompositeEditableModel("location");
            var deepHierarchyModel = new DeepHierarchyEditableModel();
            compositeModelOne.AddSimpleModelImpl(simpleEditableModelOne);
            compositeModelOne.AddSimpleModelImpl(simpleEditableModelTwo);
            deepHierarchyModel.AddCompositeItemImpl(compositeModelOne);
            deepHierarchyModel.AddCompositeItemImpl(compositeModelTwo);

            deepHierarchyModel.RemoveCompositeModel(compositeModelOne);
            deepHierarchyModel.CancelChanges();
            ((CompositeEditableModel)deepHierarchyModel.CompositeModels.First()).RemoveSimpleItem(simpleEditableModelOne);
            deepHierarchyModel.CancelChanges();

            Assert.IsFalse(deepHierarchyModel.CanCancelChanges);
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            CollectionAssert.AreEquivalent(new[] { compositeModelOne, compositeModelTwo }, deepHierarchyModel.CompositeModels);
            CollectionAssert.AreEquivalent(new[] { simpleEditableModelOne, simpleEditableModelTwo },
                deepHierarchyModel.CompositeModels.First().SimpleCollection);
        }
    }

    [TestFixture]
    class DeepHierarchyEditableModelSaveChangesTests
    {
        [Test]
        public void InnerModelInsideCollectionIsRemovedAndSaveChangesIsCalled_ModelIsChangedAndDirtyStatusIsCleared()
        {
            var simpleEditableModel = new SimpleEditableModel();
            var compositeModel = new CompositeEditableModel("location");
            var deepHierarchyModel = new DeepHierarchyEditableModel();
            compositeModel.AddSimpleModelImpl(simpleEditableModel);
            deepHierarchyModel.AddCompositeItemImpl(compositeModel);
            compositeModel.RemoveSimpleItem(simpleEditableModel);
            deepHierarchyModel.ClearDirty(forceClearChildren:true);

            Assert.IsFalse(deepHierarchyModel.CanCancelChanges);
            Assert.IsFalse(deepHierarchyModel.IsDirty);
            CollectionAssert.AreEquivalent(new[] { compositeModel }, deepHierarchyModel.CompositeModels);
            CollectionAssert.AreEquivalent(new ISimpleEditableModel[] { }, deepHierarchyModel.CompositeModels.First().SimpleCollection);
        }
    }
}
