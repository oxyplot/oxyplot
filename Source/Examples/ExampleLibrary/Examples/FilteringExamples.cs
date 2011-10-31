// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilteringExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Filtering data points")]
    public static class FilteringExamples
    {
        [Example("Filtering invalid points")]
        public static PlotModel FilteringInvalidPoints()
        {
            var plot = new PlotModel("Filtering invalid points");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var lsNaN = new LineSeries("NaN");
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            lsNaN.Points.Add(new DataPoint(1, 0));
            lsNaN.Points.Add(new DataPoint(2, 10));
            lsNaN.Points.Add(new DataPoint(double.NaN, 20));
            lsNaN.Points.Add(new DataPoint(3, 10));
            lsNaN.Points.Add(new DataPoint(4, 0));
            lsNaN.Points.Add(new DataPoint(4.5, double.NaN));
            lsNaN.Points.Add(new DataPoint(5, 0));
            lsNaN.Points.Add(new DataPoint(6, 10));
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            lsNaN.Points.Add(new DataPoint(7, 0));
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(lsNaN);
            var lsPosInf = new LineSeries("PositiveInfinity");
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(1, 1));
            lsPosInf.Points.Add(new DataPoint(2, 11));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, 20));
            lsPosInf.Points.Add(new DataPoint(3, 11));
            lsPosInf.Points.Add(new DataPoint(4, 1));
            lsPosInf.Points.Add(new DataPoint(4.5, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(5, 1));
            lsPosInf.Points.Add(new DataPoint(6, 11));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(7, 1));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            plot.Series.Add(lsPosInf);
            var lsNegInf = new LineSeries("NegativeInfinity");
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(1, 2));
            lsNegInf.Points.Add(new DataPoint(2, 12));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, 20));
            lsNegInf.Points.Add(new DataPoint(3, 12));
            lsNegInf.Points.Add(new DataPoint(4, 2));
            lsNegInf.Points.Add(new DataPoint(4.5, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(5, 2));
            lsNegInf.Points.Add(new DataPoint(6, 12));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(7, 2));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            plot.Series.Add(lsNegInf);
            return plot;
        }

        [Example("Filtering invalid points (log axis)")]
        public static PlotModel FilteringInvalidPointsLog()
        {
            var plot = new PlotModel("Filtering invalid points on logarithmic axes");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(1, 1));
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(0, 20));
            ls.Points.Add(new DataPoint(100, 2));
            ls.Points.Add(new DataPoint(1000, 12));
            ls.Points.Add(new DataPoint(4.5, 0));
            ls.Points.Add(new DataPoint(10000, 4));
            ls.Points.Add(new DataPoint(100000, 14));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(1000000, 5));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(ls);
            return plot;
        }
    
        [Example("Filtering points outside (-1,1)")]
        public static PlotModel FilteringPointsOutsideRange()
        {
            var plot = new PlotModel("Filtering points outside (-1,1)");
          plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { FilterMinValue=-1, FilterMaxValue=1});
          plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { FilterMinValue = -1, FilterMaxValue = 1 });

            var ls = new LineSeries();
            for (double i = 0; i < 200;i+=0.01)
                ls.Points.Add(new DataPoint(0.01*i*Math.Sin(i), 0.01*i*Math.Cos(i)));
            plot.Series.Add(ls);
            return plot;
        }

    }
}