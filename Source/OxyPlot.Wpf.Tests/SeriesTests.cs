// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesTests.cs" company="OxyPlot">
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
    public class SeriesTests
    {
        [Test]
        public void PlotElement()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.PlotElement), typeof(Series));
        }

        [Test]
        public void SelectablePlotElement()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.SelectablePlotElement), typeof(Series));
        }

        [Test]
        public void Series()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Series), typeof(Series));
        }

        [Test]
        public void ItemsSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.ItemsSeries), typeof(ItemsSeries));
        }

        [Test]
        public void XYAxisSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.XYAxisSeries), typeof(XYAxisSeries));
        }

        [Test]
        public void DataPointSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.DataPointSeries), typeof(DataPointSeries));
        }

        [Test]
        public void LineSeries()
        {
            var s1 = new OxyPlot.LineSeries();
            var s2 = new LineSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void AreaSeries()
        {
            var s1 = new OxyPlot.AreaSeries();
            var s2 = new AreaSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void BarSeries()
        {
            var s1 = new OxyPlot.BarSeries();
            var s2 = new BarSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void ScatterSeries()
        {
            var s1 = new OxyPlot.ScatterSeries();
            var s2 = new ScatterSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        /*
        [Test]
        public void CandleStickSeries()
        {
            var s1 = new OxyPlot.CandleStickSeries();
            var s2 = new OxyPlot.Wpf.CandleStickSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void FunctionSeries()
        {
            var s1 = new OxyPlot.FunctionSeries();
            var s2 = new OxyPlot.Wpf.FunctionSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void HighLowSeries()
        {
            var s1 = new OxyPlot.HighLowSeries();
            var s2 = new OxyPlot.Wpf.HighLowSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void PieSeries()
        {
            var s1 = new OxyPlot.PieSeries();
            var s2 = new OxyPlot.Wpf.PieSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void StairStepSeries()
        {
            var s1 = new OxyPlot.StairStepSeries();
            var s2 = new OxyPlot.Wpf.StairStepSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }
        [Test]
        public void StemSeries()
        {
            var s1 = new OxyPlot.StemSeries();
            var s2 = new OxyPlot.Wpf.StemSeries();
            Assert.PropertiesAreEqual(s1, s2);
        }*/
    }
}