using System.Linq;
using NUnit.Framework;

namespace LogoFX.UI.ViewModels.Tests.WrappingCollectionTests
{
    [TestFixture]
    class InitializationTests : WrappingCollectionTestsBase
    {        
        [Test]
        public void AddingDataSource_DataSourceContainsModelsAndFactoryMethodIsSpecified_CollectionContainsConcreteTypeViewModelsWithDataSourceModels()
        {
            var dataSource = new[] {new TestModel(1), new TestModel(2), new TestModel(3)};

            var wrappingCollection = new WrappingCollection {FactoryMethod = o => new TestViewModel((TestModel)o)};
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<TestViewModel>().ToArray();
            var actualModels = viewModels.Select(t => t.Model).ToArray();
            CollectionAssert.AreEqual(dataSource,actualModels);
        }

        [Test]
        public void AddingDataSource_DataSourceContainsModelsAndFactoryMethodIsNotSpecified_CollectionContainsViewModelsWithDataSourceModels()
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection();
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<object>().ToArray();
            var viewModelType = viewModels.First().GetType();
            var modelPropertyInfo = viewModelType.GetProperty("Model");
            var actualModels = viewModels.Select(modelPropertyInfo.GetValue).ToArray();
            CollectionAssert.AreEqual(dataSource, actualModels);
        }

        [Test]
        public void AddingDataSource_DataSourceContainsModelsAndSelectionModeIsOne_FirstViewModelIsSelected()
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.One) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<TestViewModel>().ToArray();
            var firstViewModel = viewModels.First();
            var selectedViewModel = wrappingCollection.SelectedItem;
            Assert.AreSame(firstViewModel, selectedViewModel);
        }

        [Test]
        public void AddingDataSource_DataSourceContainsModelsAndSelectionModeIsZeroOrMore_NoViewModelIsSelected()
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);
            
            var selectedViewModel = wrappingCollection.SelectedItem;
            Assert.IsNull(selectedViewModel);
        }
    }
}
