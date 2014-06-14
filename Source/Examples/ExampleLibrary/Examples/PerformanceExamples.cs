// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceExamples.cs" company="OxyPlot">
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

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Performance")]
    public class PerformanceExamples : ExamplesBase
    {
        [Example("LineSeries, 1M points")]
        public static PlotModel LineSeries1M()
        {
            var model = new PlotModel { Title = "LineSeries, 1M points" };
            var s1 = new LineSeries();
            AddPoints(s1.Points, 1000000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points")]
        public static PlotModel LineSeries()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points" };
            var s1 = new LineSeries();
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points (dashed line)")]
        public static PlotModel LineSeriesDashedLines()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points", Subtitle = "LineStyle = Dash" };
            var s1 = new LineSeries { LineStyle = LineStyle.Dash };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, markers")]
        public static PlotModel LineSeries1WithMarkers()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points", Subtitle = "MarkerType = Square" };
            var s1 = new LineSeries { MarkerType = MarkerType.Square };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, markers, lower resolution")]
        public static PlotModel LineSeries1WithMarkersLowRes()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points, markers, lower resolution", Subtitle = "MarkerType = Square, MarkerResolution = 3" };
            var s1 = new LineSeries { MarkerType = MarkerType.Square, MarkerResolution = 3 };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, round line joins")]
        public static PlotModel LineSeriesRoundLineJoins()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points", Subtitle = "LineJoin = Round" };
            var s1 = new LineSeries { LineJoin = OxyPenLineJoin.Round };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points by ItemsSource set to a List<DataPoint>")]
        public static PlotModel LineSeriesItemsSourceList()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points by ItemsSource set to a List<DataPoint>" };
            var s1 = new LineSeries();
            var points = new List<DataPoint>();
            AddPoints(points, 100000);
            s1.ItemsSource = points;
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points by ItemsSource and Mapping")]
        public static PlotModel LineSeriesItemsSourceMapping()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points by ItemsSource and Mapping", Subtitle = "Using the Mapping function" };
            var s1 = new LineSeries();
            var points = new List<DataPoint>();
            AddPoints(points, 100000);
            var rects = points.Select(pt => new OxyRect(pt.X, pt.Y, 0, 0)).ToList();
            s1.ItemsSource = rects;
            s1.Mapping = r => new DataPoint(((OxyRect)r).Left, ((OxyRect)r).Top);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points by ItemsSource and reflection")]
        public static PlotModel LineSeriesItemsSourceReflection()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points, ItemsSource with reflection", Subtitle = "DataFieldX and DataFieldY" };
            var s1 = new LineSeries();
            var points = new List<DataPoint>();
            AddPoints(points, 100000);
            var rects = points.Select(pt => new OxyRect(pt.X, pt.Y, 0, 0)).ToList();
            s1.ItemsSource = rects;
            s1.DataFieldX = "Left";
            s1.DataFieldY = "Top";
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 100k points (thick)")]
        public static PlotModel LineSeriesThick()
        {
            var model = new PlotModel { Title = "LineSeries, 100k points (thick)", Subtitle = "StrokeThickness = 10" };
            var s1 = new LineSeries { StrokeThickness = 10 };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 3k points, miter line joins")]
        public static PlotModel LineSeries2MiterLineJoins()
        {
            var model = new PlotModel { Title = "LineSeries, 3k points, miter line joins", Subtitle = "LineJoin = Miter" };
            var s1 = new LineSeries { LineJoin = OxyPenLineJoin.Miter, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
            {
                s1.Points.Add(new DataPoint(i, i % 2));
            }

            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 3k points, round line joins")]
        public static PlotModel LineSeries2RoundLineJoins()
        {
            var model = new PlotModel { Title = "LineSeries, 3k points, round line joins", Subtitle = "LineJoin = Round" };
            var s1 = new LineSeries { LineJoin = OxyPenLineJoin.Round, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
            {
                s1.Points.Add(new DataPoint(i, i % 2));
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 3k points, bevel line joins")]
        public static PlotModel LineSeries2BevelLineJoins()
        {
            var model = new PlotModel { Title = "LineSeries, 3k points, bevel line joins", Subtitle = "LineJoin = Bevel" };
            var s1 = new LineSeries { LineJoin = OxyPenLineJoin.Bevel, StrokeThickness = 8.0 };
            for (int i = 0; i < 3000; i++)
            {
                s1.Points.Add(new DataPoint(i, i % 2));
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares)")]
        public static PlotModel ScatterSeriesSquares()
        {
            var model = new PlotModel { Title = "ScatterSeries (squares)" };
            var s1 = new ScatterSeries();
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares with outline)")]
        public static PlotModel ScatterSeriesSquaresOutline()
        {
            var model = new PlotModel { Title = "ScatterSeries (squares with outline)", Subtitle = "MarkerStroke = Black" };
            var s1 = new ScatterSeries { MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares without fill color)")]
        public static PlotModel ScatterSeriesSquaresOutlineOnly()
        {
            var model = new PlotModel { Title = "ScatterSeries (squares without fill color)", Subtitle = ";arkerFill = Transparent, MarkerStroke = Black" };
            var s1 = new ScatterSeries { MarkerFill = OxyColors.Transparent, MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries by ItemsSource and reflection")]
        public static PlotModel ScatterSeriesItemsSourceReflection()
        {
            var model = new PlotModel { Title = "ScatterSeries (by ItemsSource)", Subtitle = "DataFieldX = 'X', DataFieldY = 'Y'" };
            model.Series.Add(new ScatterSeries { ItemsSource = GetPoints(2000), DataFieldX = "X", DataFieldY = "Y" });
            return model;
        }

        [Example("ScatterSeries (circles)")]
        public static PlotModel ScatterSeriesCircles()
        {
            var model = new PlotModel { Title = "ScatterSeries (circles)", Subtitle = "MarkerType = Circle" };
            var s1 = new ScatterSeries { MarkerType = MarkerType.Circle };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (circles with outline)")]
        public static PlotModel ScatterSeriesCirclesOutline()
        {
            var model = new PlotModel { Title = "ScatterSeries (circles with outline)", Subtitle = "MarkerType = Circle, MarkerStroke = Black" };
            var s1 = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (cross)")]
        public static PlotModel ScatterSeriesCrosses()
        {
            var model = new PlotModel { Title = "ScatterSeries (cross)", Subtitle = "MarkerType = Cross" };
            var s1 = new ScatterSeries
            {
                MarkerType = MarkerType.Cross,
                MarkerFill = OxyColors.Undefined,
                MarkerStroke = OxyColors.Black
            };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LinearAxis (no gridlines)")]
        public static PlotModel LinearAxisNoGridlines()
        {
            var model = new PlotModel { Title = "LinearAxis (no gridlines)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1 });
            return model;
        }

        [Example("LinearAxis (solid gridlines)")]
        public static PlotModel LinearAxisSolidGridlines()
        {
            var model = new PlotModel { Title = "LinearAxis (solid gridlines)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Solid });
            return model;
        }

        [Example("LinearAxis (dashed gridlines)")]
        public static PlotModel LinearAxisDashedGridlines()
        {
            var model = new PlotModel { Title = "LinearAxis (dashed gridlines)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Dash });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Dash });
            return model;
        }

        [Example("LinearAxis (dotted gridlines)")]
        public static PlotModel LinearAxisDottedGridlines()
        {
            var model = new PlotModel { Title = "LinearAxis (dotted gridlines)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Dot });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, MajorStep = 1, MinorStep = 1, MajorGridlineStyle = LineStyle.Dot });
            return model;
        }

        private static List<DataPoint> GetPoints(int n)
        {
            var points = new List<DataPoint>();
            AddPoints(points, n);
            return points;
        }

        private static void AddPoints(ICollection<DataPoint> points, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double x = Math.PI * 10 * i / (n - 1);
                points.Add(new DataPoint(x * Math.Cos(x), x * Math.Sin(x)));
            }
        }

        private static void AddPoints(ICollection<ScatterPoint> points, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double x = Math.PI * 10 * i / (n - 1);
                points.Add(new ScatterPoint(x * Math.Cos(x), x * Math.Sin(x)));
            }
        }
    }
}