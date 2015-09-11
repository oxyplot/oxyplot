// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartesianAxesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Cartesian axes"), Tags("Axes")]
    public class CartesianAxesExamples
    {
        [Example("Trigonometric functions")]
        public static PlotModel FunctionSeries()
        {
            var pm = new PlotModel { Title = "Trigonometric functions", PlotType = PlotType.Cartesian };
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
            var plot = new PlotModel { Title = "Y: AbsoluteMinimum = 0", PlotType = PlotType.Cartesian };
            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum Y, manual plotmargins")]
        public static PlotModel AbsoluteYmin2()
        {
            var plot = new PlotModel
            {
                Title = "Y: AbsoluteMinimum = 0",
                Subtitle = "AutoAdjustPlotMargins = false",
                PlotType = PlotType.Cartesian,
                PlotMargins = new OxyThickness(60, 4, 4, 40)
            };
            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum X/Y")]
        public static PlotModel AbsoluteYminXmin()
        {
            var plot = new PlotModel { Title = "X: AbsoluteMinimum = -10, Y: AbsoluteMinimum = 0", PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", AbsoluteMinimum = -10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum/Maximum Y")]
        public static PlotModel AbsoluteYminYmax()
        {
            var plot = new PlotModel { Title = "Y: AbsoluteMinimum = 0, AbsoluteMaximum = 2", PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, AbsoluteMaximum = 2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());
            return plot;
        }

        [Example("AbsoluteMinimum Y, AbsoluteMinimum/Maximum X")]
        public static PlotModel AbsoluteYminXminXmax()
        {
            var plot = new PlotModel { Title = "Y: AbsoluteMinimum = 0, X: AbsoluteMinimum = -10, AbsoluteMaximum = 10", PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", AbsoluteMinimum = -10, AbsoluteMaximum = 10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());

            return plot;
        }

        [Example("AbsoluteMinimum/Maximum X/Y")]
        public static PlotModel AbsoluteYminYmaxXminXmax()
        {
            var plot = new PlotModel { Title = "Y: AbsoluteMinimum = 0, AbsoluteMaximum = 2, X: AbsoluteMinimum = -10, AbsoluteMaximum = 10", PlotType = PlotType.Cartesian };

            var c = OxyColors.DarkBlue;
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis", AbsoluteMinimum = -10, AbsoluteMaximum = 10, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y-axis", AbsoluteMinimum = 0, Minimum = 0, AbsoluteMaximum = 2, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            plot.Series.Add(CreateTestSeries());

            return plot;
        }

        private static OxyPlot.Series.Series CreateTestSeries()
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