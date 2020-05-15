// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="Axis" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Provides unit tests for the <see cref="Axis" /> class.
    /// </summary>
    [TestFixture]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    public class AxisTests
    {
        [Test]
        public void A00_NoAxesDefined()
        {
            var plot = new PlotModel { Title = "Simple plot without axes defined" };

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
            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Reversed axes" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", StartPosition = 1, EndPosition = 0 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", StartPosition = 1, EndPosition = 0 });

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
            var plot = new PlotModel { Title = "Small range axis", PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Large range axis", PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Bad conditioned axis", PlotMargins = new OxyThickness(80, 60, 50, 50) };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel
            {
                Title = "Bad conditioned axis with MinimumRange",
                PlotMargins = new OxyThickness(80, 60, 50, 50)
            };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", MinimumRange = 1e-3 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", MinimumRange = 1e8, StringFormat = "0.00E00" });

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
            var plot = new PlotModel { Title = "Constant value" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Single point" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 2.4));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A15");
        }

        [Test]
        public void A16_TwoClosePoints()
        {
            var plot = new PlotModel { Title = "Two close points" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", MinimumRange = 1e-3 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" });

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(1, 2.4));
            ls.Points.Add(new DataPoint(1.000000000001, 2.4));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "A16");
        }

        [Test]
        public void B01_LogarithmicAxis()
        {
            var plot = new PlotModel { Title = "Logarithmic axis" };
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Logarithmic axis" };
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Logarithmic axis" };
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            plot.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Title = "Y-axis" });

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
            var plot = new PlotModel { Title = "Logarithmic axis with padding" };
            plot.Axes.Add(
                new LogarithmicAxis { Position = AxisPosition.Bottom, Title = "X-axis", MinimumPadding = 0.3, MaximumPadding = 0.3 });
            plot.Axes.Add(
                new LogarithmicAxis { Position = AxisPosition.Left, Title = "Y-axis", MinimumPadding = 0.3, MaximumPadding = 0.3 });

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
            var plot = new PlotModel { Title = "Logarithmic axis with SuperExponentialFormat" };
            plot.Axes.Add(
                new LogarithmicAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "X-axis",
                    Minimum = 1.8e2,
                    Maximum = 1e5,
                    UseSuperExponentialFormat = true
                });
            plot.Axes.Add(
                new LogarithmicAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Y-axis",
                    Minimum = 1e18,
                    Maximum = 1e38,
                    UseSuperExponentialFormat = true
                });
            OxyAssert.AreEqual(plot, "B05");
        }

        [Test]
        public void C01_DateTimeAxis()
        {
            var plot = new PlotModel { Title = "DateTime axis", PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "DateTime X",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -46,
                MajorStep = 1
            };
            var yaxis = new DateTimeAxis
            {
                Position = AxisPosition.Left,
                Title = "DateTime Y",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -45,
                MajorStep = 1
            };
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
            var plot = new PlotModel { Title = "DateTime axis", PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "DateTime X",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -46,
                MajorStep = 1
            };
            var yaxis = new DateTimeAxis
            {
                Position = AxisPosition.Left,
                Title = "DateTime Y",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -45,
                MajorStep = 1
            };
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
            var plot = new PlotModel { Title = "DateTime axis", PlotMargins = new OxyThickness(100, 40, 20, 100) };
            var xaxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "DateTime X",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -46,
                MajorStep = 1
            };
            var yaxis = new DateTimeAxis
            {
                Position = AxisPosition.Left,
                Title = "DateTime Y",
                IntervalType = DateTimeIntervalType.Days,
                Angle = -45,
                MajorStep = 1
            };
            plot.Axes.Add(xaxis);
            plot.Axes.Add(yaxis);

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(ls);

            OxyAssert.AreEqual(plot, "C03");
        }

        [Test]
        public void D01_InvalidAbsoluteMaxMin()
        {
            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(new LinearAxis { AbsoluteMaximum = 0, AbsoluteMinimum = 0 });
            ((IPlotModel)plot).Update(true);
            Assert.IsNotNull(plot.GetLastPlotException() as InvalidOperationException);
        }

        [Test]
        public void D02_InvalidMaxMin()
        {
            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(new LinearAxis { Maximum = 0, Minimum = 0 });
            ((IPlotModel)plot).Update(true);
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(0, plot.Axes[0].ActualMinimum);
        }

        [Test]
        public void D03_InvalidMaxMin()
        {
            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(new LogarithmicAxis { Maximum = 1, Minimum = 1 });
            ((IPlotModel)plot).Update(true);
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(1, plot.Axes[0].ActualMinimum);
        }

        [Test]
        public void D04_InvalidLogAxis()
        {
            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(new LogarithmicAxis { Maximum = 1, Minimum = 0 });
            ((IPlotModel)plot).Update(true);
            Assert.AreEqual(100, plot.Axes[0].ActualMaximum);
            Assert.AreEqual(1, plot.Axes[0].ActualMinimum);
        }

        /// <summary>
        /// Test DesiredSize property to see if working property
        /// </summary>
        [Test]
        public void Axis_DesiredMargin()
        {
            var xaxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" };
            var yaxis = new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis" };

            var plot = new PlotModel { Title = "Simple plot" };
            plot.Axes.Add(xaxis);
            plot.Axes.Add(yaxis);

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(3, 13));
            ls.Points.Add(new DataPoint(10, 47));
            ls.Points.Add(new DataPoint(30, 23));
            ls.Points.Add(new DataPoint(40, 65));
            ls.Points.Add(new DataPoint(80, 10));
            plot.Series.Add(ls);

            // initial setting
            plot.UpdateAndRenderToNull(800, 600);
            Assert.That(yaxis.DesiredMargin.Left, Is.EqualTo(35.0).Within(0.5), "y-axis left");
            Assert.That(yaxis.DesiredMargin.Top, Is.EqualTo(5).Within(0.5), "y-axis top");
            Assert.That(yaxis.DesiredMargin.Right, Is.EqualTo(0.0).Within(1e-6), "y-axis right");
            Assert.That(yaxis.DesiredMargin.Bottom, Is.EqualTo(5).Within(0.5), "y-axis bottom");

            Assert.That(xaxis.DesiredMargin.Left, Is.EqualTo(5).Within(0.5), "x-axis left");
            Assert.That(xaxis.DesiredMargin.Top, Is.EqualTo(0.0).Within(1e-6), "x-axis top");
            Assert.That(xaxis.DesiredMargin.Right, Is.EqualTo(5).Within(0.5), "x-axis right");
            Assert.That(xaxis.DesiredMargin.Bottom, Is.EqualTo(35.0).Within(0.5), "x-axis bottom");

            // larger numbers on axis -> larger desired size
            yaxis.Zoom(10000, 11000);
            xaxis.Zoom(10000, 11000);
            plot.UpdateAndRenderToNull(800, 600);

            Assert.That(yaxis.DesiredMargin.Left, Is.EqualTo(50.0).Within(0.5), "y-axis left");
            Assert.That(yaxis.DesiredMargin.Top, Is.EqualTo(5).Within(0.5), "y-axis top");
            Assert.That(yaxis.DesiredMargin.Right, Is.EqualTo(0.0).Within(1e-6), "y-axis right");
            Assert.That(yaxis.DesiredMargin.Bottom, Is.EqualTo(5).Within(0.5), "y-axis bottom");

            Assert.That(xaxis.DesiredMargin.Left, Is.EqualTo(12.5).Within(0.5), "x-axis left");
            Assert.That(xaxis.DesiredMargin.Top, Is.EqualTo(0.0).Within(1e-6), "x-axis top");
            Assert.That(xaxis.DesiredMargin.Right, Is.EqualTo(12.5).Within(0.5), "x-axis right");
            Assert.That(xaxis.DesiredMargin.Bottom, Is.EqualTo(35.0).Within(0.5), "x-axis bottom");
        }

        /// <summary>
        /// Test DesiredSize property with axis start and end position
        /// </summary>
        [Test]
        public void Axis_DesiredMargin_WithPosition()
        {
            var plot = new PlotModel();
            var axis1 = new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 0, EndPosition = 0.5, Title = "X-axis 1" };
            var axis2 = new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0.5, Title = "X-axis 2" };
            plot.Axes.Add(axis1);
            plot.Axes.Add(axis2);

            axis1.Zoom(0, 80);
            axis2.Zoom(0, 80);
            plot.UpdateAndRenderToNull(800, 600);
            Assert.That(axis1.DesiredMargin.Left, Is.EqualTo(5).Within(0.5), "axis1 left");
            Assert.That(axis1.DesiredMargin.Top, Is.EqualTo(0).Within(1e-6), "axis1 top");
            Assert.That(axis1.DesiredMargin.Right, Is.EqualTo(0).Within(1e-6), "axis1 right");
            Assert.That(axis1.DesiredMargin.Bottom, Is.EqualTo(35).Within(0.5), "axis1 bottom");

            Assert.That(axis2.DesiredMargin.Left, Is.EqualTo(0d).Within(1e-6), "axis2 left");
            Assert.That(axis2.DesiredMargin.Top, Is.EqualTo(0d).Within(1e-6), "axis2 top");
            Assert.That(axis2.DesiredMargin.Right, Is.EqualTo(5).Within(0.5), "axis2 right");
            Assert.That(axis2.DesiredMargin.Bottom, Is.EqualTo(35).Within(0.5), "axis2 bottom");

            // larger numbers on axis -> larger desired size
            axis1.Zoom(10000, 11000);
            axis2.Zoom(10000, 11000);
            plot.UpdateAndRenderToNull(800, 600);
            Assert.That(axis1.DesiredMargin.Left, Is.EqualTo(12.5).Within(0.5), "axis1 left");
            Assert.That(axis1.DesiredMargin.Top, Is.EqualTo(0).Within(1e-6), "axis1 top");
            Assert.That(axis1.DesiredMargin.Right, Is.EqualTo(0).Within(1e-6), "axis1 right");
            Assert.That(axis1.DesiredMargin.Bottom, Is.EqualTo(35).Within(0.5), "axis1 bottom");

            Assert.That(axis2.DesiredMargin.Left, Is.EqualTo(0d).Within(1e-6), "axis2 left");
            Assert.That(axis2.DesiredMargin.Top, Is.EqualTo(0d).Within(1e-6), "axis2 top");
            Assert.That(axis2.DesiredMargin.Right, Is.EqualTo(12.5).Within(0.5), "axis2 right");
            Assert.That(axis2.DesiredMargin.Bottom, Is.EqualTo(35).Within(0.5), "axis2 bottom");
        }

        /// <summary>
        /// Tests the alignment of the series, if minimum range is set
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MinimumRange()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            double dataMin = 10.1;
            double dataMax = 10.3;
            double dataCenter = (dataMax + dataMin) / 2;

            // Center should be the between data min and max
            Assert.AreEqual(dataCenter, (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-5, "center");
            Assert.AreEqual(dataCenter - 0.5, plot.Axes[0].ActualMinimum, 1e-5, "minimum");
            Assert.AreEqual(dataCenter + 0.5, plot.Axes[0].ActualMaximum, 1e-5, "maximum");
        }

        /// <summary>
        /// Tests the alignment of the series, if MinimumRange and the AbsoluteMaximum are set
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MinimumRange_AbsoluteMaximum()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
                AbsoluteMaximum = 10.5,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            // Center should be the between AbsoluteMaximum and the (AboluteMaximum - MinimumRange)
            Assert.AreEqual(yaxis.AbsoluteMaximum, plot.Axes[0].ActualMaximum, 0, "absolute maximum");
            Assert.AreEqual(yaxis.AbsoluteMaximum - (yaxis.MinimumRange / 2), (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-5, "center");
            Assert.AreEqual(yaxis.AbsoluteMaximum - yaxis.MinimumRange, plot.Axes[0].ActualMinimum, 1e-5, "minimum");
        }

        /// <summary>
        /// Tests the alignment of the series, if MinimumRange and the AbsoluteMaximum are set
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MinimumRange_AbsoluteMinimum()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
                AbsoluteMinimum = 10,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            // Center should be the between AbsoluteMinimum and the (AboluteMinimum + MinimumRange)
            Assert.AreEqual(yaxis.AbsoluteMinimum, plot.Axes[0].ActualMinimum, 0, "absolute minimum");
            Assert.AreEqual(yaxis.AbsoluteMinimum + (yaxis.MinimumRange / 2), (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-5, "center");
            Assert.AreEqual(yaxis.AbsoluteMinimum + yaxis.MinimumRange, plot.Axes[0].ActualMaximum, 1e-5, "maximum");
        }

        /// <summary>
        /// Tests the alignment of the series, if maximum range is set.
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MaximumRange()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MaximumRange = 0.1,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            double dataMin = 10.1;
            double dataMax = 10.3;
            double dataCenter = (dataMax + dataMin) / 2;

            // Center should be the between data min and max
            Assert.AreEqual(dataCenter, (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-5, "center");
            Assert.AreEqual(dataCenter - 0.05, plot.Axes[0].ActualMinimum, 1e-5, "minimum");
            Assert.AreEqual(dataCenter + 0.05, plot.Axes[0].ActualMaximum, 1e-5, "maximum");
        }

        /// <summary>
        /// Tests the alignment of the series, if MaximumRange and the AbsoluteMaximum are set.
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MaximumRange_AbsoluteMaximum()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MaximumRange = 0.1,
                AbsoluteMaximum = 10.22,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            // Range is between AbsoluteMaximum and the (AboluteMaximum - MaximumRange)
            Assert.AreEqual(yaxis.AbsoluteMaximum, plot.Axes[0].ActualMaximum, 0, "absolute maximum");
            Assert.AreEqual(yaxis.AbsoluteMaximum - (yaxis.MaximumRange / 2), (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-6, "center");
            Assert.AreEqual(yaxis.AbsoluteMaximum - yaxis.MaximumRange, plot.Axes[0].ActualMinimum, 1e-6, "minimum");
        }

        /// <summary>
        /// Tests the alignment of the series, if MaximumRange and the AbsoluteMinimum are set.
        /// </summary>
        [Test]
        public void Axis_VerticalAlignment_MaximumRange_AbsoluteMinimum()
        {
            var plot = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MaximumRange = 0.1,
                AbsoluteMinimum = 10.16,
            };

            plot.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            // Range is between AbsoluteMinimum and the (AboluteMinimum + MaximumRange)
            Assert.AreEqual(yaxis.AbsoluteMinimum, plot.Axes[0].ActualMinimum, 0, "absolute minimum");
            Assert.AreEqual(yaxis.AbsoluteMinimum + (yaxis.MaximumRange / 2), (plot.Axes[0].ActualMaximum + plot.Axes[0].ActualMinimum) / 2, 1e-6, "center");
            Assert.AreEqual(yaxis.AbsoluteMinimum + yaxis.MaximumRange, plot.Axes[0].ActualMaximum, 1e-6, "maximum");
        }

        [Test]
        public void Axis_Toggle_Between_Linear_And_Log_Axis()
        {
            // Setting up test data per repro steps for issue 1067.
            // https://github.com/oxyplot/oxyplot/issues/1067

            var plot = new PlotModel();
            var linearAxis = new LinearAxis()
            {
                Position = AxisPosition.Left
            };

            plot.Axes.Add(linearAxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0.005, 0.004));
            series.Points.Add(new DataPoint(0.99, 0.98));

            plot.Series.Add(series);

            ((IPlotModel)plot).Update(true);

            // Now toggle from linear to log axis.
            plot.Axes[0] = new LogarithmicAxis { Position = AxisPosition.Left };

            ((IPlotModel)plot).Update(false);

            // Changing the axis type should not cause the data minimum to be invalid.
            Assert.AreEqual(0.004, plot.Axes[0].DataMinimum);
        }
    }
}
