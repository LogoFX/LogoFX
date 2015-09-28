using System.Collections.Specialized;
using LogoFX.Core;
using NUnit.Framework;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    [TestFixture]
    class NotificationTests : WrappingCollectionTestsBase
    {
        [Test]
        public void
            WhenCollectionIsCreatedWithRangeAndSourceIsUpdatedWithAddRange_ThenSingleNotificationIsRaisedWithAllWrappers
            ()
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(isBulk: true)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            collection.CollectionChanged += (sender, args) =>
            {
                CollectionAssert.AreEquivalent(items, args.NewItems);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.AddRange(items);
        }

        [Test]
        public void
            WhenCollectionIsCreatedWithRangeAndSingleItemAndSourceIsUpdatedWithRemoveRange_ThenSingleNotificationIsRaisedWithAllWrappers
            ()
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(isBulk: true)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);            
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                CollectionAssert.AreEquivalent(items, args.OldItems);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.RemoveRange(items);
        }

        [Test]
        public void
            WhenCollectionIsCreatedWithRangeAndMultipleItemsAndSourceIsUpdatedWithRemoveRange_ThenSingleNotificationIsRaisedWithAllWrappersAndActionIsReset
            ()
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(isBulk: true)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.RemoveRange(items);
        }

        [Test]
        public void
            WhenCollectionIsCreatedWithRangeAndSingleItemAndSourceIsCleared_ThenSingleNotificationIsRaisedWithAllWrappers
            ()
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(isBulk: true)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);            
            source.AddRange(items);            
            collection.CollectionChanged += (sender, args) =>
            {
                CollectionAssert.AreEquivalent(items, args.OldItems);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.Clear();
        }

        [Test]
        public void
            WhenCollectionIsCreatedWithRangeAndMultipleItemsAndSourceIsCleared_ThenSingleNotificationIsRaisedWithAllWrappersAndActionisReset
            ()
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(isBulk: true)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, args.Action);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.Clear();
        }
    }
}
