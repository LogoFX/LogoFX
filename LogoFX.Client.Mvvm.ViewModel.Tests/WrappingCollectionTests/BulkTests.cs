using System.Linq;
using LogoFX.Client.Mvvm.Model;
using LogoFX.Client.Mvvm.Model.Contracts;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    [TestFixture]
    class BulkTests : WrappingCollectionTestsBase
    {
        [Test]
        public void
            GivenCollectionIsBulkAndSourceWithTwoItemsIsAdded_WhenSecondItemIsRemoved_ThenCollectionContainsOneItem()
        {
            var source = new RangeModelsCollection<TestModel>();
            var modelOne = new TestModel(4);
            var modelTwo = new TestModel(5);            

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrOne, isBulk: true)
                {
                    FactoryMethod = o => o
                }.WithSource(((IReadModelsCollection<TestModel>) source).Items);
            source.AddRange(new[] { modelOne, modelTwo });
            source.Remove(modelTwo);

            CollectionAssert.AreEqual(new [] {modelOne}, wrappingCollection.OfType<TestModel>().ToArray());
        }
    }
}
