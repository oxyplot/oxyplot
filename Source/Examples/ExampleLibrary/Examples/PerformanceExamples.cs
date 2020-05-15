// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ExampleLibrary.Utilities;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Performance")]
    public class PerformanceExamples
    {
        [Example("LineSeries, 1M points")]
        public static PlotModel LineSeries1M()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 1M points");
            var s1 = new LineSeries();
            AddPoints(s1.Points, 1000000);
            model.Series.Add(s1);
            return model;
        }        
        
        [Example("LineSeries, 1M points, EdgeRenderingMode==PreferSpeed")]
        public static PlotModel LineSeries1MSpeed()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 1M points");
            var s1 = new LineSeries() { EdgeRenderingMode = EdgeRenderingMode.PreferSpeed };
            AddPoints(s1.Points, 1000000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points")]
        public static PlotModel LineSeries()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points");
            var s1 = new LineSeries();
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points (dashed line)")]
        public static PlotModel LineSeriesDashedLines()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points", "LineStyle = Dash");
            var s1 = new LineSeries { LineStyle = LineStyle.Dash };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, markers")]
        public static PlotModel LineSeries1WithMarkers()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points", "MarkerType = Square");
            var s1 = new LineSeries { MarkerType = MarkerType.Square };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, markers, lower resolution")]
        public static PlotModel LineSeries1WithMarkersLowRes()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points, markers, lower resolution", "MarkerType = Square, MarkerResolution = 3");
            var s1 = new LineSeries { MarkerType = MarkerType.Square, MarkerResolution = 3 };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points, round line joins")]
        public static PlotModel LineSeriesRoundLineJoins()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points", "LineJoin = Round");
            var s1 = new LineSeries { LineJoin = LineJoin.Round };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);
            return model;
        }

        [Example("LineSeries, 100k points by ItemsSource set to a List<DataPoint>")]
        public static PlotModel LineSeriesItemsSourceList()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points by ItemsSource set to a List<DataPoint>");
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points by ItemsSource and Mapping", "Using the Mapping function");
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points, ItemsSource with reflection", "DataFieldX and DataFieldY");
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 100k points (thick)", "StrokeThickness = 10");
            var s1 = new LineSeries { StrokeThickness = 10 };
            AddPoints(s1.Points, 100000);
            model.Series.Add(s1);

            return model;
        }

        [Example("LineSeries, 3k points, miter line joins")]
        public static PlotModel LineSeries2MiterLineJoins()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 3k points, miter line joins", "LineJoin = Miter");
            var s1 = new LineSeries { LineJoin = LineJoin.Miter, StrokeThickness = 8.0 };
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 3k points, round line joins", "LineJoin = Round");
            var s1 = new LineSeries { LineJoin = LineJoin.Round, StrokeThickness = 8.0 };
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("LineSeries, 3k points, round line joins", "LineJoin = Bevel");
            var s1 = new LineSeries { LineJoin = LineJoin.Bevel, StrokeThickness = 8.0 };
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
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (squares)");
            var s1 = new ScatterSeries();
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares with outline)")]
        public static PlotModel ScatterSeriesSquaresOutline()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (squares with outline)", "MarkerStroke = Black");
            var s1 = new ScatterSeries { MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (squares without fill color)")]
        public static PlotModel ScatterSeriesSquaresOutlineOnly()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (squares with fill color)", "MarkerFill = Transparent, MarkerStroke = Black");
            var s1 = new ScatterSeries { MarkerFill = OxyColors.Transparent, MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries by ItemsSource and reflection")]
        public static PlotModel ScatterSeriesItemsSourceReflection()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (by ItemsSource)", "DataFieldX = 'X', DataFieldY = 'Y'");
            model.Series.Add(new ScatterSeries { ItemsSource = GetPoints(2000), DataFieldX = "X", DataFieldY = "Y" });
            return model;
        }

        [Example("ScatterSeries (circles)")]
        public static PlotModel ScatterSeriesCircles()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (circles)", "MarkerType = Circle");
            var s1 = new ScatterSeries { MarkerType = MarkerType.Circle };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (circles with outline)")]
        public static PlotModel ScatterSeriesCirclesOutline()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (circles with outline)", "MarkerType = Circle, MarkerStroke = Black");
            var s1 = new ScatterSeries { MarkerType = MarkerType.Circle, MarkerStroke = OxyColors.Black };
            AddPoints(s1.Points, 2000);
            model.Series.Add(s1);
            return model;
        }

        [Example("ScatterSeries (cross)")]
        public static PlotModel ScatterSeriesCrosses()
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("ScatterSeries (cross)", "MarkerType = Cross");
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

        [Example("int overflow (10k)")]
        public static PlotModel IntOverflow10k()
        {
            return IntOverflow(10000);
        }

        [Example("int overflow (50k)")]
        public static PlotModel IntOverflow50k()
        {
            return IntOverflow(50000);
        }

        [Example("int overflow (100k)")]
        public static PlotModel IntOverflow100k()
        {
            return IntOverflow(100000);
        }

        private static PlotModel IntOverflow(int n)
        {
            var model = PlotModelUtilities.CreateModelWithDefaultAxes("int overflow", "n = " + n);
            var ls = new LineSeries();
            int k = 0;
            for (int i = 0; i < n; i++)
            {
                ls.Points.Add(new DataPoint(i, k += i * i));
            }

            model.Series.Add(ls);
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
