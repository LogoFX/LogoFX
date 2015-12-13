using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LogoFX.Core.Tests
{
    [TestFixture]
    public class CollectionsExtensionsTests
    {
        [Test]
        public void ForEachByOne_CollectionIsValid_ActionIsAppliedForEachElement()
        {
            var collection = new[] {"t1", "t2", "t3"};
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);
            collection = collection.ForEachByOne(action).ToArray();

            const string expectedResult = "t1t2t3";
            var actualResult = collection.Aggregate(string.Empty,(t,r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
        }

        [Test]
        public void ForEachByOne_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEachByOne(action).ToArray());            
        }

        [Test]
        public void ForEachByOneWithIndex_CollectionIsValid_ActionIsAppliedForEachElementAndIndexIsIncreased()
        {
            var collection = new[] { "t1", "t2", "t3" };
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator += i*3;
            };
            collection = collection.ForEachByOne(action).ToArray();

            const string expectedResult = "t1t2t3";
            const int expectedIndexAggregator = 9;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
            Assert.AreEqual(expectedIndexAggregator,indexAggregator);
        }

        [Test]
        public void ForEachByOneWithIndex_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string,int> action = (r,t) => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEachByOne(action).ToArray());
        }

        [Test]
        public void ForEachByOneWithRange_CollectionIsValid_ActionIsAppliedForTheElementsWithinRange()
        {
            var collection = new[] { "t1", "t2", "t3" };
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator++;
            };
            collection = collection.ForEachByOne(1,2, action).ToArray();

            const string expectedResult = "t2t3";
            const int expectedAggregator = 2;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
            Assert.AreEqual(expectedAggregator, indexAggregator);
        }

        [Test]
        public void ForEachByOneWithRange_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEachByOne(0, 1, action).ToArray());
        }

        [Test]
        public void ForEach_CollectionIsValid_ActionIsAppliedForEachElement()
        {
            var collection = new[] { "t1", "t2", "t3" };
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);
            collection = collection.ForEach(action).ToArray();

            const string expectedResult = "t1t2t3";
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
        }

        [Test]
        public void ForEach_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string> action = r => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEach(action).ToArray());
        }

        [Test]
        public void ForEachWithIndex_CollectionIsValid_ActionIsAppliedForEachElementAndIndexIsIncreased()
        {
            var collection = new[] { "t1", "t2", "t3" };
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator += i * 3;
            };
            collection = collection.ForEach(action).ToArray();

            const string expectedResult = "t1t2t3";
            const int expectedIndexAggregator = 9;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
            Assert.AreEqual(expectedIndexAggregator, indexAggregator);
        }

        [Test]
        public void ForEachWithIndex_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEach(action).ToArray());
        }

        [Test]
        public void ForEachWithRange_CollectionIsValid_ActionIsAppliedForTheElementsWithinRange()
        {
            var collection = new[] { "t1", "t2", "t3" };
            var stringBuilder = new StringBuilder();
            var indexAggregator = 0;
            Action<string, int> action = (r, i) =>
            {
                stringBuilder.Append(r);
                indexAggregator++;
            };
            collection = collection.ForEach(1, 2, action).ToArray();

            const string expectedResult = "t2t3";
            const int expectedAggregator = 2;
            var actualResult = collection.Aggregate(string.Empty, (t, r) => t + r);
            StringAssert.AreEqualIgnoringCase(expectedResult, actualResult);
            Assert.AreEqual(expectedAggregator, indexAggregator);
        }

        [Test]
        public void ForEachWithRange_CollectionIsNull_ArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            var stringBuilder = new StringBuilder();
            Action<string, int> action = (r, t) => stringBuilder.Append(r);

            Assert.Throws<ArgumentNullException>(() => collection.ForEach(0, 1, action).ToArray());
        }
    }
}
