
namespace OxyPlot.Tests
{
    using NUnit.Framework;
    using Axes;

    /// <summary>
    /// Provides unit tests for the <see cref="TimeSpanAxis" /> class.
    /// </summary>
    [TestFixture]
    public class TimeSpanAxisTests
    {
        [Test]
        [TestCase("mm:ss:f", "01:02:3")]
        [TestCase("mm:ss:ff", "01:02:34")]
        [TestCase("mm:ss:fff", "01:02:345")]
        public void SupportMillisecondsInFormatStrings(string format, string expected)
        {
            var axis = new TimeSpanAxis { StringFormat = format };
            var formattedValue = axis.FormatValue(TimeSpanAxis.ToDouble(new System.TimeSpan(1, 1, 1, 2, 345)));
            Assert.That(formattedValue, Is.EqualTo(expected));
        }
    }
}