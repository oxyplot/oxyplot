// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxesTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void LogarithmicAxis()
        {
            var s1 = new OxyPlot.LogarithmicAxis();
            var s2 = new OxyPlot.Wpf.LogarithmicAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void DateTimeAxis()
        {
            var s1 = new OxyPlot.DateTimeAxis();
            var s2 = new OxyPlot.Wpf.DateTimeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void TimeSpanAxis()
        {
            var s1 = new OxyPlot.TimeSpanAxis();
            var s2 = new OxyPlot.Wpf.TimeSpanAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void CategoryAxis()
        {
            var s1 = new OxyPlot.CategoryAxis();
            var s2 = new OxyPlot.Wpf.CategoryAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void AngleAxis()
        {
            var s1 = new OxyPlot.AngleAxis();
            var s2 = new OxyPlot.Wpf.AngleAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void MagnitudeAxis()
        {
            var s1 = new OxyPlot.MagnitudeAxis();
            var s2 = new OxyPlot.Wpf.MagnitudeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
    }
}