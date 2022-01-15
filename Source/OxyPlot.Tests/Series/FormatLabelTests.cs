using NUnit.Framework;
using OxyPlot.Series;
using System;
using System.Threading;

namespace OxyPlot.Tests.Series
{
    class BarSeriesForTest : BarSeries
    {
        public string GetFormattedLabel(BarItem item)
        {
            return base.GetFormattedLabel(item);
        }
    }

    class HistogramSeriesForTest : HistogramSeries
    {
        public string GetFormattedLabel(HistogramItem item)
        {
            return base.GetFormattedLabel(item);
        }
    }

    class IntervalBarSeriesForTest : IntervalBarSeries
    {
        public string GetFormattedLabel(IntervalBarItem item)
        {
            return base.GetFormattedLabel(item);
        }
    }
    

    /// <summary>
    /// Contains the unit tests for formatting series label <see cref="ItemsSeries"/> struct.
    /// </summary
    [TestFixture]
    internal class FormatLabelTests
    {
        [Test]
        public void TestEmptyLabelForUnsetLabelFormat()
        {
            var bs = new BarSeriesForTest();
            Assert.IsNull(bs.LabelFormatString);
            Assert.AreEqual(bs.GetFormattedLabel(new BarItem(1.0)), "");
        }

        [Test]
        [TestCase(1.0, "{0}", "1")]
        [TestCase(1.0, "{0:0}", "1")]
        public void TestBarItemLabelFormat(double value, string format, string expectedResult)
        {
            var bs = new BarSeriesForTest();
            bs.LabelFormatString = format;
            Assert.AreEqual(bs.GetFormattedLabel(new BarItem(value)), expectedResult);
        }

        [Test]
        public void TestHistogramItemLabelFormat()
        {
            var hs = new HistogramSeriesForTest();
            hs.LabelFormatString = "Start: {1:0.00}\nEnd: {2:0.00}\nValue: {0:0.00}\nArea: {3:0.00}\nCount: {4}";

            char sep = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            var expected = $"Start: 1{sep}00\nEnd: 2{sep}00\nValue: 4{sep}00\nArea: 4{sep}00\nCount: 4";
            Assert.AreEqual(hs.GetFormattedLabel(new HistogramItem(1, 2, 4, 4)) , expected);
        }

        [Test]
        public void TestIntervalBarItemLabelFormat()
        {
            var ibs = new IntervalBarSeriesForTest();
            ibs.LabelFormatString = "{0} - {1}";
            Assert.AreEqual(ibs.GetFormattedLabel(new IntervalBarItem(1.0, 5.0)), "1 - 5");
        }

        
    }
}
