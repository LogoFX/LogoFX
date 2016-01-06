using NUnit.Framework;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    [TestFixture]
    class SimpleEditableModelDirtyTests
    {
        [Test]
        public void SimpleModelIsNotMadeDirty_IsDirtyIsFalse()
        {
            var model = new SimpleEditableModel(DataGenerator.ValidName, 5);

            Assert.IsFalse(model.IsDirty);
        }

        [Test]
        public void SimpleModelIsMadeDirty_IsDirtyIsTrue()
        {
            var model = new SimpleEditableModel(DataGenerator.InvalidName, 5);
            model.MakeDirty();

            Assert.IsTrue(model.IsDirty);
        }
    }
}