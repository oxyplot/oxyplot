// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxesTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class AxesTests
    {
        [Test]
        public void Axis()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Axis), typeof(Axis));
        }

        [Test]
        public void LinearAxis()
        {
            var s1 = new OxyPlot.LinearAxis();
            var s2 = new LinearAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void LogarithmicAxis()
        {
            var s1 = new OxyPlot.LogarithmicAxis();
            var s2 = new LogarithmicAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void DateTimeAxis()
        {
            var s1 = new OxyPlot.DateTimeAxis();
            var s2 = new DateTimeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void TimeSpanAxis()
        {
            var s1 = new OxyPlot.TimeSpanAxis();
            var s2 = new TimeSpanAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void CategoryAxis()
        {
            var s1 = new OxyPlot.CategoryAxis();
            var s2 = new CategoryAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void AngleAxis()
        {
            var s1 = new OxyPlot.AngleAxis();
            var s2 = new AngleAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void MagnitudeAxis()
        {
            var s1 = new OxyPlot.MagnitudeAxis();
            var s2 = new MagnitudeAxis();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
    }
}