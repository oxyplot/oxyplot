// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceExamples.cs" company="OxyPlot">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using OxyPlot;

    [Examples("Performance")]
    public class PerformanceExamples : ExamplesBase
    {
        [Example("LineSeries, 100k points")]
        public static PlotModel LineSeries1()
        {
            var model = new PlotModel("LineSeries, 100k points");
#if !SILVERLIGHT && !MONO
            var watch = new Stopwatch();
            model.Updating += (sender, args) => watch.Restart();
            model.Updated += (sender, args) => Debug.WriteLine("Updated in " + watch.ElapsedMilliseconds + " ms");
#endif
            var s1 = new LineSeries();
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points, round line joins")]
        public static PlotModel LineSeries1Round()
        {
            var model = new PlotModel("LineSeries, 100k points, round line joins");
#if !SILVERLIGHT && !MONO
            var watch = new Stopwatch();
            model.Updating += (sender, args) => watch.Restart();
            model.Updated += (sender, args) => Debug.WriteLine("Updated in " + watch.ElapsedMilliseconds + " ms");
#endif
            var s1 = new LineSeries() { LineJoin = OxyPenLineJoin.Round };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points, ItemsSource, List<IDataPoint>")]
        public static PlotModel LineSeries10()
        {
            var model = new PlotModel("LineSeries, 100k points, ItemsSource, List<IDataPoint>");
            var s1 = new LineSeries();
            var points = new List<IDataPoint>();
            AddPoints(points, 100000);
            s1.ItemsSource = points;
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points, ItemsSource, List<OxyRect>")]
        public static PlotModel LineSeries10b()
        {
            var model = new PlotModel("LineSeries, 100k points, ItemsSource, List<OxyRect>");
            var s1 = new LineSeries();
            var points = new List<IDataPoint>();
            AddPoints(points, 100000);
            var rects = points.Select(pt => new OxyRect(pt.X, pt.Y, 0, 0)).ToList();
            s1.ItemsSource = rects;
            s1.DataFieldX = "Left";
            s1.DataFieldY = "Top";
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points (thick)")]
        public static PlotModel LineSeries1b()
        {
            var model = new PlotModel("LineSeries, 100k points (thick)");
            var s1 = new LineSeries();
            s1.StrokeThickness = 10;
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points (by ItemsSource)")]
        public static PlotModel LineSeries1c()
        {
            var model = new PlotModel("LineSeries, 100k points (by ItemsSource)");
            model.Series.Add(new LineSeries { ItemsSource = GetPoints(100000) });
            return model;
        }

        [Example("LineSeries 2, 3k points, miter line joins")]
        public static PlotModel LineSeries2miter()
        {
            var model = new PlotModel("LineSeries, 3k points, miter line joins");
            var s1 = new LineSeries() { LineJoin = OxyPenLineJoin.Miter, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
                s1.Points.Add(new DataPoint(i, i % 2));
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries 2, 3k points, round line joins")]
        public static PlotModel LineSeries2Round()
        {
            var model = new PlotModel("LineSeries, 3k points, round line joins");
            var s1 = new LineSeries() { LineJoin = OxyPenLineJoin.Round, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
                s1.Points.Add(new DataPoint(i, i % 2));
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries 2, 3k points, bevel line joins")]
        public static PlotModel LineSeries2Bevel()
        {
            var model = new PlotModel("LineSeries, 3k points, bevel line joins");
            var s1 = new LineSeries() { LineJoin = OxyPenLineJoin.Bevel, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
                s1.Points.Add(new DataPoint(i, i % 2));
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares)")]
        public static PlotModel ScatterSeries1()
        {
            var model = new PlotModel("ScatterSeries (squares)");
            var s1 = new ScatterSeries();
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares with outline)")]
        public static PlotModel ScatterSeries1b()
        {
            var model = new PlotModel("ScatterSeries (squares with outline)");
            var s1 = new ScatterSeries();
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares without fill color)")]
        public static PlotModel ScatterSeries1c()
        {
            var model = new PlotModel("ScatterSeries (squares without fill color)");
            var s1 = new ScatterSeries();
            s1.MarkerFill = OxyColors.Transparent;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (by ItemsSource)")]
        public static PlotModel ScatterSeries2()
        {
            var model = new PlotModel("ScatterSeries (by ItemsSource)");
            model.Series.Add(new ScatterSeries { ItemsSource = GetPoints(2000), DataFieldX = "X", DataFieldY = "Y" });
            return model;
        }

        [Example("ScatterSeries (circles)")]
        public static PlotModel ScatterSeries3()
        {
            var model = new PlotModel("ScatterSeries (circles)");
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Circle;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (circles with outline)")]
        public static PlotModel ScatterSeries4()
        {
            var model = new PlotModel("ScatterSeries (circles with outline)");
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Circle;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (cross)")]
        public static PlotModel ScatterSeries5()
        {
            var model = new PlotModel("ScatterSeries (cross)");
            var s1 = new ScatterSeries();
            s1.MarkerType = MarkerType.Cross;
            s1.MarkerFill = null;
            s1.MarkerStroke = OxyColors.Black;
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LinearAxis (no gridlines)")]
        public static PlotModel LinearAxis1()
        {
            var model = new PlotModel("LinearAxis (no gridlines)");
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1));
            return model;
        }

        [Example("LinearAxis (solid gridlines)")]
        public static PlotModel LinearAxis2()
        {
            var model = new PlotModel("LinearAxis (solid gridlines)");
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Solid });
            return model;
        }

        [Example("LinearAxis (dashed gridlines)")]
        public static PlotModel LinearAxis3()
        {
            var model = new PlotModel("LinearAxis (dashed gridlines)");
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dash });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dash });
            return model;
        }

        [Example("LinearAxis (dotted gridlines)")]
        public static PlotModel LinearAxis4()
        {
            var model = new PlotModel("LinearAxis (dotted gridlines)");
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dot });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, 1, 1) { MajorGridlineStyle = LineStyle.Dot });
            return model;
        }

        private static IList<IDataPoint> GetPoints(int n)
        {
            var points = new List<IDataPoint>();
            AddPoints(points, n);
            return points;
        }

        private static void AddPoints(IList<IDataPoint> points, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double x = Math.PI * 10 * i / (n - 1);
                points.Add(new DataPoint(x * Math.Cos(x), x * Math.Sin(x)));
            }
        }
    }
}