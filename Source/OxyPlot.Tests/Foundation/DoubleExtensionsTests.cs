namespace OxyPlot.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class DoubleExtensionsTests
    {
        [Test]
        public void RemoveNoise()
        {
            double d1 = 3 * 0.1;
            double d2 = d1.RemoveNoise();
            Assert.AreNotEqual(0.3, d1);
            Assert.AreEqual(0.3, d2);
        }
    }
}
