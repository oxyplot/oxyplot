// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

namespace ExampleGenerator
{
    using System;
    using System.Globalization;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class SeriesExamples
    {
        public static PlotModel Example1()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function",
                DefaultFont = "Arial",
                Culture = CultureInfo.InvariantCulture
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.05,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -5.25,
                Maximum = 5.25,
                MajorStep = 1,
                MinorStep = 0.25,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }

        private static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance,
                                                                 int n = 1001)
        {
            var ls = new LineSeries
            {
                Title = string.Format(CultureInfo.InvariantCulture, "N({0},{1})", mean, variance)
            };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + (x1 - x0) * i / (n - 1);
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }

            return ls;
        }

        public static PlotModel BarSeries()
        {
            var model = new PlotModel("BarSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new BarSeries { Title = "Series 1", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new BarItem { Value = 25 });
            s1.Items.Add(new BarItem { Value = 137 });
            s1.Items.Add(new BarItem { Value = 18 });
            s1.Items.Add(new BarItem { Value = 40 });

            var s2 = new BarSeries { Title = "Series 2", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new BarItem { Value = 12 });
            s2.Items.Add(new BarItem { Value = 14 });
            s2.Items.Add(new BarItem { Value = 120 });
            s2.Items.Add(new BarItem { Value = 26 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        public static PlotModel ColumnSeries()
        {
            var model = new PlotModel("ColumnSeries")
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new ColumnSeries { Title = "Series 1", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new ColumnItem { Value = 25 });
            s1.Items.Add(new ColumnItem { Value = 137 });
            s1.Items.Add(new ColumnItem { Value = 18 });
            s1.Items.Add(new ColumnItem { Value = 40 });

            var s2 = new ColumnSeries { Title = "Series 2", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new ColumnItem { Value = 12 });
            s2.Items.Add(new ColumnItem { Value = 14 });
            s2.Items.Add(new ColumnItem { Value = 120 });
            s2.Items.Add(new ColumnItem { Value = 26 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        public static PlotModel TwoColorLineSeries()
        {
            var model = new PlotModel { Title = "TwoColorLineSeries" };
            var twoColorLineSeries = new TwoColorLineSeries
                             {
                                 Color = OxyColors.Red,
                                 Color2 = OxyColors.Blue,
                                 LineStyle = LineStyle.Solid,
                                 LineStyle2 = LineStyle.Dot
                             };
            twoColorLineSeries.Points.Add(new DataPoint(0, -6));
            twoColorLineSeries.Points.Add(new DataPoint(10, 4));
            twoColorLineSeries.Points.Add(new DataPoint(30, -2));
            twoColorLineSeries.Points.Add(new DataPoint(40, 8));
            model.Series.Add(twoColorLineSeries);
            return model;
        }

        public static PlotModel ScatterSeries()
        {
            var model = new PlotModel { Title = "ScatterSeries" };
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };
            var r = new Random(314);
            for (int i = 0; i < 100; i++)
            {
                var x = r.NextDouble();
                var y = r.NextDouble();
                var size = r.Next(5, 15);
                var colorValue = r.Next(100, 1000);
                scatterSeries.Points.Add(new ScatterPoint(x, y, size, colorValue));
            }

            model.Series.Add(scatterSeries);
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(200) });
            return model;
        }

        public static PlotModel HeatMapSeries()
        {
            var model = new PlotModel { Title = "HeatMapSeries" };
            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayHelper.CreateVector(x0, x1, 100);
            var yvalues = ArrayHelper.CreateVector(y0, y1, 100);
            var peaksData = ArrayHelper.Evaluate(peaks, xvalues, yvalues);

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData };
            model.Series.Add(hms);

            return model;
        }

        public static PlotModel ContourSeries()
        {
            var model = new PlotModel { Title = "ContourSeries" };
            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayHelper.CreateVector(x0, x1, 100);
            var yvalues = ArrayHelper.CreateVector(y0, y1, 100);
            var peaksData = ArrayHelper.Evaluate(peaks, xvalues, yvalues);

            var cs = new ContourSeries
            {
                Color = OxyColors.Black,
                LabelBackground = OxyColors.White,
                ColumnCoordinates = yvalues,
                RowCoordinates = xvalues,
                Data = peaksData
            };
            model.Series.Add(cs);

            return model;
        }

        public static PlotModel LineSeries()
        {
            var model = new PlotModel { Title = "LineSeries" };
            var lineSeries = new LineSeries();
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 4));
            lineSeries.Points.Add(new DataPoint(30, 2));
            lineSeries.Points.Add(new DataPoint(40, 8));
            model.Series.Add(lineSeries);
            return model;
        }

        public static PlotModel LineSeriesSmoothed()
        {
            var model = new PlotModel { Title = "LineSeries", Subtitle = "Smooth = true" };
            var lineSeries = new LineSeries { Smooth = true };
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 4));
            lineSeries.Points.Add(new DataPoint(30, 2));
            lineSeries.Points.Add(new DataPoint(40, 8));
            model.Series.Add(lineSeries);
            return model;
        }
    }
}