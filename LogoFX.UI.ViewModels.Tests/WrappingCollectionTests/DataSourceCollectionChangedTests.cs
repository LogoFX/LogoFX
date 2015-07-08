using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    [TestFixture]
    class DataSourceCollectionChangedTests : WrappingCollectionTestsBase
    {
        [Test]
        public void DataSourceCollectionChanged_ModelIsAdded_ViewModelIsAdded()
        {
            var dataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection = new WrappingCollection {FactoryMethod = o => new TestViewModel((TestModel)o)};
            wrappingCollection.AddSource(dataSource);
            dataSource.Add(new TestModel(4));

            var viewModels = wrappingCollection.OfType<TestViewModel>();
            var actualViewModel = viewModels.SingleOrDefault(t => t.Model.Id == 4);
            Assert.IsNotNull(actualViewModel);
        }

        [Test]
        public void DataSourceCollectionChanged_ModelIsRemoved_ViewModelIsRemoved()
        {
            var dataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);
            dataSource.Remove(dataSource.Last());

            var viewModels = wrappingCollection.OfType<TestViewModel>();
            var actualViewModel = viewModels.SingleOrDefault(t => t.Model.Id == 3);
            Assert.IsNull(actualViewModel);
        }

        [Test]
        public void DataSourceCollectionChanged_DataSourceIsCleared_ViewModelsAreCleared()
        {
            var dataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);
            dataSource.Clear();

            var viewModels = wrappingCollection.OfType<TestViewModel>();            
            CollectionAssert.IsEmpty(viewModels);
        }

        [Test]
        public void DataSourcesCollectionChanged_DataSourceIsAdded_ViewModelsAreAdded()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });
            var anotherDataSource = new ObservableCollection<TestModel>(new[] {new TestModel(5), new TestModel(6)});

            var wrappingCollection = new WrappingCollection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            wrappingCollection.AddSource(anotherDataSource);

            CollectionAssert.AllItemsAreInstancesOfType(wrappingCollection,typeof(TestViewModel));
            var expectedModels = originalDataSource.Concat(anotherDataSource);
            CollectionAssert.AreEqual(expectedModels,wrappingCollection.OfType<TestViewModel>().Select(t => t.Model));
        }

        [Test]
        public void DataSourcesCollectionChanged_DataSourceIsRemoved_ViewModelsAreRemoved()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });            

            var wrappingCollection = new WrappingCollection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            wrappingCollection.RemoveSource(originalDataSource);

            CollectionAssert.IsEmpty(wrappingCollection);            
        }

        [Test]
        public void DataSourcesCollectionChanged_DataSourcesAreCleared_ViewModelsAreRemoved()
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection = new WrappingCollection { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(originalDataSource);
            wrappingCollection.ClearSources();

            CollectionAssert.IsEmpty(wrappingCollection);
        }
    }
}
