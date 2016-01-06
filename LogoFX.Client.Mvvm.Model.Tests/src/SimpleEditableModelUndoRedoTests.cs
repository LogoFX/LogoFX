using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleEditableModelUndoRedoTests
    {
        [Test]
        public void SimpleModelIsChangedTwiceThenUndoIsCalledOnce_ChangedPropertyValueIsCorrectAndIsDirtyIsTrue()
        {
            var nameOne = "NameOne";
            var nameTwo = "NameTwo";
            var model = new SimpleEditableModelWithUndoRedo(DataGenerator.ValidName, 5);

            model.Name = nameOne;
            model.Name = nameTwo;
            model.Undo();

            Assert.AreEqual(nameOne, model.Name);
            Assert.IsTrue(model.IsDirty);
        }

        [Test]
        public void SimpleModelIsChangedTwiceThenUndoIsCalledTwice_ChangedPropertyValueIsCorrectAndIsDirtyIsFalse()
        {
            var nameOne = "NameOne";
            var nameTwo = "NameTwo";
            var initialName = DataGenerator.ValidName;
            var model = new SimpleEditableModelWithUndoRedo(initialName, 5);

            model.Name = nameOne;
            model.Name = nameTwo;
            model.Undo();
            model.Undo();

            Assert.AreEqual(initialName, model.Name);
            Assert.IsFalse(model.IsDirty);
        }

        [Test]
        public void SimpleModelIsChangedTwiceThenUndoIsCalledOnceThenRedoIsCalledOnce_ChangedPropertyValueIsCorrectAndIsDirtyIsTrue()
        {
            var nameOne = "NameOne";
            var nameTwo = "NameTwo";
            var model = new SimpleEditableModelWithUndoRedo(DataGenerator.ValidName, 5);

            model.Name = nameOne;
            model.Name = nameTwo;
            model.Undo();
            model.Redo();

            Assert.AreEqual(nameTwo, model.Name);
            Assert.IsTrue(model.IsDirty);
        }        
    }
}