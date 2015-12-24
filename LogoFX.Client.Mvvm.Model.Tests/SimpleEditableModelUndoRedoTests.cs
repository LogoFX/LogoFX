using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleEditableModelUndoRedoTests
    {
        [Test]
        public void SimpleModelIsChangedTwiceThenUndoIsCalledOnce_ChangedPropertyValueIsCorrect()
        {
            var nameOne = "NameOne";
            var nameTwo = "NameTwo";
            var model = new SimpleEditableModelWithUndoRedo(DataGenerator.ValidName, 5);

            model.Name = nameOne;
            model.Name = nameTwo;
            model.Undo();

            Assert.AreEqual(nameOne, model.Name);
        }

        [Test]
        public void SimpleModelIsChangedTwiceThenUndoIsCalledOnceThenRedoIsCalledOnce_ChangedPropertyValueIsCorrect()
        {
            var nameOne = "NameOne";
            var nameTwo = "NameTwo";
            var model = new SimpleEditableModelWithUndoRedo(DataGenerator.ValidName, 5);

            model.Name = nameOne;
            model.Name = nameTwo;
            model.Undo();
            model.Redo();

            Assert.AreEqual(nameTwo, model.Name);
        }        
    }
}