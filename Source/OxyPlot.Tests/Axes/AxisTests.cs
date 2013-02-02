// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisTests.cs" company="OxyPlot">
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
namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class AxisTests
    {
        [Test]
        public void A00_NoAxesDefined()
        {
            var plot = new PlotModel("Simple plot without axes defined");

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(3, 13));
            ls.Points.Add(new DataPoint(10, 47));
            ls.Points.Add(new DataPoint(30, 23));
            ls.Points.Add(new DataPoint(40, 65));
            ls.Points.Add(new DataPoint(80, 10));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A00");
        }

        [Test]
        public void A01_SimpleAxes()
        {
            var plot = new PlotModel("Simple plot");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(3, 13));
            ls.Points.Add(new DataPoint(10, 47));
            ls.Points.Add(new DataPoint(30, 23));
            ls.Points.Add(new DataPoint(40, 65));
            ls.Points.Add(new DataPoint(80, 10));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A01");
        }

        [Test]
        public void A02_ReversedAxes()
        {
            var plot = new PlotModel("Reversed axes");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { StartPosition = 1, EndPosition = 0 });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { StartPosition = 1, EndPosition = 0 });

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(0, 13));
            ls.Points.Add(new DataPoint(10, 47));
            ls.Points.Add(new DataPoint(30, 23));
            ls.Points.Add(new DataPoint(40, 65));
            ls.Points.Add(new DataPoint(80, 10));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A02");
        }

        [Test]
        public void A11_SmallRangeAxis()
        {
            var plot = new PlotModel("Small range axis") { PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1e-40, 1e-38));
            ls.Points.Add(new DataPoint(1.2e-40, 1.9e-38));
            ls.Points.Add(new DataPoint(1.4e-40, 3.3e-38));
            ls.Points.Add(new DataPoint(1.6e-40, 2.5e-38));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A11");
        }

        [Test]
        public void A12_LargeRangeAxis()
        {
            var plot = new PlotModel("Large range axis") { PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1e40, 1e38));
            ls.Points.Add(new DataPoint(1.2e40, 1.9e38));
            ls.Points.Add(new DataPoint(1.4e40, 3.3e38));
            ls.Points.Add(new DataPoint(1.6e40, 2.5e38));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A12");
        }

        [Test]
        public void A13_BadConditionedAxis()
        {
            var plot = new PlotModel("Bad conditioned axis") { PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1.20000000001, 2400000001));
            ls.Points.Add(new DataPoint(1.20000000002, 2400000000));
            ls.Points.Add(new DataPoint(1.20000000004, 2400000004));
            ls.Points.Add(new DataPoint(1.20000000007, 2400000003));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A13");
        }

        [Test]
        public void A13B_BadConditionedAxis_SettingMinimumRange()
        {
            var plot = new PlotModel("Bad conditioned axis with MinimumRange")
                { PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { MinimumRange = 1e-3 });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { MinimumRange = 1e8, StringFormat = "0.00E00" });

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1.20000000001, 2400000001));
            ls.Points.Add(new DataPoint(1.20000000002, 2400000000));
            ls.Points.Add(new DataPoint(1.20000000004, 2400000004));
            ls.Points.Add(new DataPoint(1.20000000007, 2400000003));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A13B");
        }

        [Test]
        public void A14_ConstantValue()
        {
            var plot = new PlotModel("Constant value");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 2.4));
            ls.Points.Add(new DataPoint(2, 2.4));
            ls.Points.Add(new DataPoint(3, 2.4));
            ls.Points.Add(new DataPoint(4, 2.4));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A14");
        }

        [Test]
        public void A15_SinglePoint()
        {
            var plot = new PlotModel("Single point");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 2.4));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A15");
        }

        [Test]
        public void A16_TwoClosePoints()
        {
            var plot = new PlotModel("Two close points");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { MinimumRange = 1e-3 });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 2.4));
            ls.Points.Add(new DataPoint(1.000000000001, 2.4));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A16");
        }

        [Test]
        public void B01_LogarithmicAxis()
        {
            var plot = new PlotModel("Logarithmic axis");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1.2, 3.3));
            ls.Points.Add(new DataPoint(10, 30));
            ls.Points.Add(new DataPoint(100, 20));
            ls.Points.Add(new DataPoint(1000, 400));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "B01");
        }

        [Test]
        public void B02_LogarithmicAxis()
        {
            var plot = new PlotModel("Logarithmic axis");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 12));
            ls.Points.Add(new DataPoint(3, 14));
            ls.Points.Add(new DataPoint(24, 18));
            ls.Points.Add(new DataPoint(27, 19));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "B02");
        }

        [Test]
        public void B03_LogarithmicAxis()
        {
            var plot = new PlotModel("Logarithmic axis");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1e10, 1e18));
            ls.Points.Add(new DataPoint(1.2e20, 1.9e28));
            ls.Points.Add(new DataPoint(1.4e30, 3.3e30));
            ls.Points.Add(new DataPoint(1.6e40, 2.5e38));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "B03");
        }

        [Test]
        public void B04_LogarithmicAxis_Padding()
        {
            var plot = new PlotModel("Logarithmic axis with padding");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis") { MinimumPadding = 0.3, MaximumPadding = 0.3 });
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis") { MinimumPadding = 0.3, MaximumPadding = 0.3 });

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 12));
            ls.Points.Add(new DataPoint(3, 14));
            ls.Points.Add(new DataPoint(24, 18));
            ls.Points.Add(new DataPoint(27, 19));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "B04");
        }

        [Test]
        public void B05_LogarithmicAxis_SuperExponentialFormat()
        {
            var plot = new PlotModel("Logarithmic axis with SuperExponentialFormat");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis") { Minimum = 1.8e2, Maximum = 1e5, UseSuperExponentialFormat = true });
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis") { Minimum = 1e18, Maximum = 1e38, UseSuperExponentialFormat = true });
            OxyAssert.AreEqual(plot, "B05");
        }

        [Test]
        public void C01_DateTimeAxis()
        {
            var plot = new PlotModel("DateTime axis") { PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis(AxisPosition.Bottom, "DateTime X", null, DateTimeIntervalType.Days) { Angle = -46, MajorStep = 1 };
            var yaxis = new DateTimeAxis(AxisPosition.Left, "DateTime Y", null, DateTimeIntervalType.Days) { Angle = -45, MajorStep = 1 };
            plot.Axes.Add(xaxis);
            plot.Axes.Add(yaxis);

            var ls = new LineSeries();
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 1), new DateTime(2011, 3, 1)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 4), new DateTime(2011, 3, 8)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 6), new DateTime(2011, 3, 12)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 10), new DateTime(2011, 3, 13)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 19), new DateTime(2011, 3, 14)));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "C01");
        }

        [Test]
        public void C02_DateTimeAxis_WithSomeUndefinedPoints()
        {
            var plot = new PlotModel("DateTime axis") { PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis(AxisPosition.Bottom, "DateTime X", null, DateTimeIntervalType.Days) { Angle = -46, MajorStep = 1 };
            var yaxis = new DateTimeAxis(AxisPosition.Left, "DateTime Y", null, DateTimeIntervalType.Days) { Angle = -45, MajorStep = 1 };
            plot.Axes.Add(xaxis);
            plot.Axes.Add(yaxis);

            var ls = new LineSeries();
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 1), new DateTime(2011, 3, 1)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(double.NaN, new DateTime(2011, 3, 8)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 6), new DateTime(2011, 3, 12)));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 10), double.NaN));
            ls.Points.Add(DateTimeAxis.CreateDataPoint(new DateTime(2011, 1, 19), new DateTime(2011, 3, 14)));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "C02");
        }

        [Test]
        public void C03_DateTimeAxis_WithAllUndefinedPoints()
        {
            var plot = new PlotModel("DateTime axis") { PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis(AxisPosition.Bottom, "DateTime X", null, DateTimeIntervalType.Days) { Angle = -46, MajorStep = 1 };
            var yaxis = new DateTimeAxis(AxisPosition.Left, "DateTime Y", null, DateTimeIntervalType.Days) { Angle = -45, MajorStep = 1 };
            plot.Axes.Add(xaxis);
            plot.Axes.Add(yaxis);

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "C03");
        }

        [Test, ExpectedException]
        public void D01_InvalidAbsoluteMaxMin()
        {
            var plot = new PlotModel("Simple plot");
            plot.Axes.Add(new LinearAxis { AbsoluteMaximum = 0, AbsoluteMinimum = 0 });
            plot.Update();
        }

        [Test]
        public void D02_InvalidMaxMin()
        {
            var plot = new PlotModel("Simple plot");
            plot.Axes.Add(new LinearAxis { Maximum = 0, Minimum = 0 });
            plot.Update();
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(0, plot.Axes[0].ActualMinimum);
        }

        [Test]
        public void D03_InvalidMaxMin()
        {
            var plot = new PlotModel("Simple plot");
            plot.Axes.Add(new LogarithmicAxis { Maximum = 1, Minimum = 1 });
            plot.Update();
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(1, plot.Axes[0].ActualMinimum);
        }

        [Test]
        public void D04_InvalidLogAxis()
        {
            var plot = new PlotModel("Simple plot");
            plot.Axes.Add(new LogarithmicAxis { Maximum = 1, Minimum = 0 });
            plot.Update();
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(1, plot.Axes[0].ActualMinimum);
        }
    }
}