using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    [TestFixture]
    class SelectionTests : WrappingCollectionTestsBase
    {
        [Test]
        public void Selection_ItemIsSelectedAndDeselected_SelectionIsEmpty()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection.WithSelection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Unselect(firstItem);

            Assert.IsNull(wrappingCollection.SelectedItem);
            CollectionAssert.IsEmpty(wrappingCollection.SelectedItems);
            Assert.AreEqual(0,wrappingCollection.SelectionCount);
        }

        [Test]
        public void Selection_SelectionModeIsMultipleItemIsSelectedAndAnotherItemIsSelected_BothItemsAreSelected()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Select(secondItem);

            Assert.AreSame(firstItem, wrappingCollection.SelectedItem);
            CollectionAssert.AreEqual(new [] {firstItem, secondItem}, wrappingCollection.SelectedItems);
            Assert.AreEqual(2, wrappingCollection.SelectionCount);
        }

        [Test]
        public void Selection_SelectionModeIsSingleItemIsSelectedAndAnotherItemIsSelected_OnlySecondItemIsSelected()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.One) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Select(secondItem);

            Assert.AreSame(secondItem, wrappingCollection.SelectedItem);
            CollectionAssert.AreEqual(new[] { secondItem }, wrappingCollection.SelectedItems);
            Assert.AreEqual(1, wrappingCollection.SelectionCount);
        }

        [Test]
        public void Selection_ItemIsSelectedThenItemIsRemoved_SelectionIsEmpty()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();            
            wrappingCollection.Select(firstItem);
            originalDataSource.RemoveAt(0);

            AssertEmptySelection(wrappingCollection);
        }        

        [Test]
        public void ClearSelection_CollectionContainsTwoSelectedItems_SelectionIsEmpty()
        {
            var originalDataSource =
                            new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(secondItem);
            wrappingCollection.ClearSelection();

            AssertEmptySelection(wrappingCollection);
        }

        private static void AssertEmptySelection(WrappingCollection.WithSelection wrappingCollection)
        {
            Assert.IsNull(wrappingCollection.SelectedItem);
            CollectionAssert.IsEmpty(wrappingCollection.SelectedItems);
            Assert.AreEqual(0, wrappingCollection.SelectionCount);
        }
    }
}
