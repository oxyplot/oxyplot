namespace OxyPlot.Wpf.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AxesTests
    {
        [Test]
        public void LinearAxis()
        {
            var s1 = new OxyPlot.LinearAxis();
            var s2 = new OxyPlot.Wpf.LinearAxis();
            Assert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void LogarithmicAxis()
        {
            var s1 = new OxyPlot.LogarithmicAxis();
            var s2 = new OxyPlot.Wpf.LogarithmicAxis();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void DateTimeAxis()
        {
            var s1 = new OxyPlot.DateTimeAxis();
            var s2 = new OxyPlot.Wpf.DateTimeAxis();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void TimeSpanAxis()
        {
            var s1 = new OxyPlot.TimeSpanAxis();
            var s2 = new OxyPlot.Wpf.TimeSpanAxis();
            Assert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void CategoryAxis()
        {
            var s1 = new OxyPlot.CategoryAxis();
            var s2 = new OxyPlot.Wpf.CategoryAxis();
            Assert.PropertiesAreEqual(s1, s2);
        }
    }
}
