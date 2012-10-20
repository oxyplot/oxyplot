// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartesianAxesExamples.cs" company="OxyPlot">
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
namespace ExampleLibrary
{
    using System;

    using OxyPlot;

    [Examples("Cartesian axes")]
    public class CartesianAxesExamples : ExamplesBase
    {
        [Example("Trigonometric functions")]
        public static PlotModel FunctionSeries()
        {
            var pm = new PlotModel("Trigonometric functions") { PlotType = PlotType.Cartesian };
            pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
            pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 1000, "cos(t),sin(t)"));
            return pm;
        }

        [Example("Clover")]
        public static PlotModel Clover()
        {
            var plot = new PlotModel { Title = "Parametric function", PlotType = PlotType.Cartesian };
            plot.Series.Add(new FunctionSeries(t => 2 * Math.Cos(2 * t) * Math.Cos(t), t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                0, Math.PI * 2, 1000, "2cos(2t)cos(t) , 2cos(2t)sin(t)"));
            return plot;
        }

        [Example("AbsoluteMinimum Y")]
        public static PlotModel AbsoluteYmin()
        {
            var plot = new PlotModel("Y: AbsoluteMinimum = 0") { PlotType = PlotType.Cartesian };
            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum Y, manual plotmargins")]
        public static PlotModel AbsoluteYmin2()
        {
            var plot = new PlotModel("Y: AbsoluteMinimum = 0", "AutoAdjustPlotMargins = false") { PlotType = PlotType.Cartesian, AutoAdjustPlotMargins = false };
            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum X/Y")]
        public static PlotModel AbsoluteYminXmin()
        {
            var plot = new PlotModel("X: AbsoluteMinimum = -10, Y: AbsoluteMinimum = 0") { PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { AbsoluteMinimum = -10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum/Maximum Y")]
        public static PlotModel AbsoluteYminYmax()
        {
            var plot = new PlotModel("Y: AbsoluteMinimum = 0, AbsoluteMaximum = 2") { PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, AbsoluteMaximum = 2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum Y, AbsoluteMinimum/Maximum X")]
        public static PlotModel AbsoluteYminXminXmax()
        {
            var plot = new PlotModel("Y: AbsoluteMinimum = 0, X: AbsoluteMinimum = -10, AbsoluteMaximum = 10") { PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { AbsoluteMinimum = -10, AbsoluteMaximum = 10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());

            return plot;
        }

        [Example("AbsoluteMinimum/Maximum X/Y")]
        public static PlotModel AbsoluteYminYmaxXminXmax()
        {
            var plot = new PlotModel("Y: AbsoluteMinimum = 0, AbsoluteMaximum = 2, X: AbsoluteMinimum = -10, AbsoluteMaximum = 10") { PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { AbsoluteMinimum = -10, AbsoluteMaximum = 10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { AbsoluteMinimum = 0, Minimum = 0, AbsoluteMaximum = 2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());

            return plot;
        }

        private static Series CreateTestSeries()
        {
            var absSerie = new LineSeries();

            absSerie.Points.Add(new DataPoint(-8.0, 0.0));
            absSerie.Points.Add(new DataPoint(-7.5, 0.1));
            absSerie.Points.Add(new DataPoint(-7.0, 0.2));
            absSerie.Points.Add(new DataPoint(-6.0, 0.4));
            absSerie.Points.Add(new DataPoint(-5.0, 0.5));
            absSerie.Points.Add(new DataPoint(-4.0, 0.6));
            absSerie.Points.Add(new DataPoint(-3.0, 0.7));
            absSerie.Points.Add(new DataPoint(-2.0, 0.8));
            absSerie.Points.Add(new DataPoint(-1.0, 0.9));
            absSerie.Points.Add(new DataPoint(0.0, 1.0));
            absSerie.Points.Add(new DataPoint(1.0, 0.9));
            absSerie.Points.Add(new DataPoint(2.0, 0.8));
            absSerie.Points.Add(new DataPoint(3.0, 0.7));
            absSerie.Points.Add(new DataPoint(4.0, 0.6));
            absSerie.Points.Add(new DataPoint(5.0, 0.5));
            absSerie.Points.Add(new DataPoint(6.0, 0.4));
            absSerie.Points.Add(new DataPoint(7.0, 0.2));
            absSerie.Points.Add(new DataPoint(7.5, 0.1));
            absSerie.Points.Add(new DataPoint(8.0, 0.0));

            absSerie.Points.Add(DataPoint.Undefined);

            // Plot a square
            absSerie.Points.Add(new DataPoint(-0.5, 0.5));
            absSerie.Points.Add(new DataPoint(-0.5, 1.5));
            absSerie.Points.Add(new DataPoint(0.5, 1.5));
            absSerie.Points.Add(new DataPoint(0.5, 0.5));
            absSerie.Points.Add(new DataPoint(-0.5, 0.5));

            return absSerie;
        }
    }
}