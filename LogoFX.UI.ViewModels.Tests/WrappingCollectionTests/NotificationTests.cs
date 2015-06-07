using LogoFX.Core;
using NUnit.Framework;

namespace LogoFX.UI.ViewModels.Tests.WrappingCollectionTests
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
            WhenCollectionIsCreatedWithRangeAndSourceIsUpdatedWithRemoveRange_ThenSingleNotificationIsRaisedWithAllWrappers
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
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                CollectionAssert.AreEquivalent(items, args.OldItems);
                numberOfTimes++;
                Assert.AreEqual(1, numberOfTimes);
            };
            source.RemoveRange(items);
        }
    }
}
