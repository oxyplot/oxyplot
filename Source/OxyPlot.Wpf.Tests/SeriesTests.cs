// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
            OxyAssert.PropertiesExist(typeof(OxyPlot.Series.Series), typeof(Series));
        }

        [Test]
        public void ItemsSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Series.ItemsSeries), typeof(ItemsSeries));
        }

        [Test]
        public void XYAxisSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Series.XYAxisSeries), typeof(XYAxisSeries));
        }

        [Test]
        public void DataPointSeries()
        {
            OxyAssert.PropertiesExist(typeof(OxyPlot.Series.DataPointSeries), typeof(DataPointSeries));
        }

        [Test]
        public void LineSeries()
        {
            var s1 = new OxyPlot.Series.LineSeries();
            var s2 = new LineSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void AreaSeries()
        {
            var s1 = new OxyPlot.Series.AreaSeries();
            var s2 = new AreaSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void BarSeries()
        {
            var s1 = new OxyPlot.Series.BarSeries();
            var s2 = new BarSeries();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void ScatterSeries()
        {
            var s1 = new OxyPlot.Series.ScatterSeries();
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