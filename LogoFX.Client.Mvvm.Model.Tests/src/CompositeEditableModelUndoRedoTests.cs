using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class CompositeEditableModelUndoRedoTests
    {
        [Test]
        public void InnerModelAddedThenUndoIsCalled_ModelDataIsRestoredAndIsDirtyIsFalse()
        {
            var expectedPhones = new[] { 546, 432 };
            var compositeModel = new CompositeEditableModelWithUndoRedo("Here", expectedPhones);

            compositeModel.AddPhone(647);
            compositeModel.Undo();

            var phones = ((ICompositeEditableModel)compositeModel).Phones.ToArray();
            CollectionAssert.AreEqual(expectedPhones, phones);
            var isCompositeDirty = compositeModel.IsDirty;
            Assert.IsFalse(isCompositeDirty);
        }

        [Test]
        public void InnerModelPropertyIsChanged_ThenCanUndoIsTrueAndIsDirtyIsTrue()
        {            
            var person = new SimpleEditableModel(DataGenerator.ValidName, 25);
            var name = "NewName";
            var compositeModel = new CompositeEditableModelWithUndoRedo("Here", new [] {person});

            person.Name = name;

            var canUndo = compositeModel.CanUndo;
            Assert.IsTrue(canUndo);
            var isCompositeDirty = compositeModel.IsDirty;
            Assert.IsTrue(isCompositeDirty);
        }
    }
}
