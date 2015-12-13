using NUnit.Framework;

namespace LogoFX.Core.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("5", "5")]
        [TestCase("y_s", "ys")]
        [TestCase("yS", "y S")]
        [TestCase("Y s", "Ys")]
        [TestCase("y s", "ys")]
        [TestCase("SomeEnum", "Some Enum")]
        public void StringBeautify_ProvidingSourceString_ResultIsCorrect(string source, string expectedResult)
        {
            var actualResult = source.Beautify();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
