// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeriesExamples.cs" company="OxyPlot">
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
// <summary>
//   Calculates the Least squares fit of a list of DataPoints.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("ScatterSeries")]
    public class ScatterSeriesExamples : ExamplesBase
    {
        [Example("Random points")]
        public static PlotModel RandomScatter()
        {
            return RandomScatter(31000, 8);
        }

        public static PlotModel RandomScatter(int n, int binsize)
        {
            var model = new PlotModel(string.Format("ScatterSeries (n={0})", n), "BinSize = " + binsize);

            var s1 = new ScatterSeries("Series 1")
            {
                MarkerType = MarkerType.Diamond,
                MarkerStrokeThickness = 0,
                BinSize = binsize
            };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new ScatterPoint(random.NextDouble(), random.NextDouble()));
            }

            model.Series.Add(s1);
            return model;
        }

        public static PlotModel CreateRandomScatterSeriesWithColorAxisPlotModel(int n, OxyPalette palette, MarkerType markerType = MarkerType.Square, AxisPosition colorAxisPosition = AxisPosition.Right, OxyColor highColor = null, OxyColor lowColor = null)
        {
            var model = new PlotModel(string.Format("ScatterSeries (n={0})", n)) { Background = OxyColors.LightGray };
            model.Axes.Add(new ColorAxis { Position = colorAxisPosition, Palette = palette, Minimum = -1, Maximum = 1, HighColor = highColor, LowColor = lowColor });

            var s1 = new ScatterSeries
            {
                MarkerType = markerType,
                MarkerSize = 6,
            };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble() * 2.2 - 1.1;
                s1.Points.Add(new ScatterPoint(x, random.NextDouble()) { Value = x });
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("Random points with random size")]
        public static PlotModel RandomSize()
        {
            return RandomSize(1000, 8);
        }

        public static PlotModel RandomSize(int n, int binsize)
        {
            var model = new PlotModel(string.Format("ScatterSeries with random MarkerSize (n={0})", n), "BinSize = " + binsize);

            var s1 = new ScatterSeries("Series 1")
            {
                MarkerStrokeThickness = 0,
                BinSize = binsize
            };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new ScatterPoint(random.NextDouble(), random.NextDouble(), 4 + 10 * random.NextDouble()));
            }
            model.Series.Add(s1);
            return model;
        }

        [Example("Random points with least squares fit")]
        public static PlotModel RandomWithFit()
        {
            const int n = 20;
            var model = new PlotModel(string.Format("Random data (n={0})", n)) { LegendPosition = LegendPosition.LeftTop };

            var s1 = new ScatterSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble() * 10;
                y += 1 + random.NextDouble();
                var p = new ScatterPoint(x, y);
                s1.Points.Add(p);
            }
            model.Series.Add(s1);
            double a, b;
            LeastSquaresFit(s1.Points, out a, out b);
            model.Annotations.Add(new LineAnnotation { Slope = a, Intercept = b, Text = "Least squares fit" });
            return model;
        }

        /// <summary>
        /// Calculates the Least squares fit of a list of DataPoints.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="a">The slope.</param>
        /// <param name="b">The intercept.</param>
        public static void LeastSquaresFit(IEnumerable<IDataPoint> points, out double a, out double b)
        {
            // http://en.wikipedia.org/wiki/Least_squares
            // http://mathworld.wolfram.com/LeastSquaresFitting.html
            // http://web.cecs.pdx.edu/~gerry/nmm/course/slides/ch09Slides4up.pdf

            double Sx = 0;
            double Sy = 0;
            double Sxy = 0;
            double Sxx = 0;
            int m = 0;
            foreach (var p in points)
            {
                Sx += p.X;
                Sy += p.Y;
                Sxy += p.X * p.Y;
                Sxx += p.X * p.X;
                m++;
            }
            double d = Sx * Sx - m * Sxx;
            a = 1 / d * (Sx * Sy - m * Sxy);
            b = 1 / d * (Sx * Sxy - Sxx * Sy);
        }

        [Example("Scatter plot using a LineSeries with markers only")]
        public static PlotModel MarkersOnly()
        {
            return MarkersOnly(31000);
        }

        public static PlotModel MarkersOnly(int n)
        {
            var model = new PlotModel(string.Format("LineSeries with markers only (n={0})", n));

            var s1 = new LineSeries("Series 1") { StrokeThickness = 0, MarkerType = MarkerType.Square, MarkerFill = OxyColors.Blue, MarkerStrokeThickness = 0 };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint(random.NextDouble(), random.NextDouble()));
            }
            model.Series.Add(s1);
            return model;
        }

        [Example("Marker types")]
        public static PlotModel MarkerTypes()
        {
            var model = new PlotModel("Marker types");
            model.Series.Add(CreateRandomScatterSeries(10, "Circle", MarkerType.Circle));
            model.Series.Add(CreateRandomScatterSeries(10, "Cross", MarkerType.Cross));
            model.Series.Add(CreateRandomScatterSeries(10, "Diamond", MarkerType.Diamond));
            model.Series.Add(CreateRandomScatterSeries(10, "Plus", MarkerType.Plus));
            model.Series.Add(CreateRandomScatterSeries(10, "Square", MarkerType.Square));
            model.Series.Add(CreateRandomScatterSeries(10, "Star", MarkerType.Star));
            model.Series.Add(CreateRandomScatterSeries(10, "Triangle", MarkerType.Triangle));
            return model;
        }

        [Example("ScatterSeries defined by Points collection")]
        public static PlotModel DataPoints()
        {
            var model = new PlotModel("Scatter plot of DataPoints");
            model.Series.Add(new ScatterSeries
            {
                Points = CreateRandomPoints(100)
            });
            return model;
        }

        [Example("ScatterSeries defined by ItemsSource")]
        public static PlotModel FromItemsSource()
        {
            var model = new PlotModel("Scatter plot from ItemsSource");
            model.Series.Add(new ScatterSeries
            {
                ItemsSource = CreateRandomPoints(100),
                DataFieldX = "X",
                DataFieldY = "Y"
            });
            return model;
        }

        [Example("ScatterSeries defined by Mapping")]
        public static PlotModel FromMapping()
        {
            var model = new PlotModel("Scatter plot from Mapping");
            model.Series.Add(new ScatterSeries
            {
                ItemsSource = CreateRandomPoints(100),
                Mapping = item => new ScatterPoint(((IDataPoint)item).X, ((IDataPoint)item).Y)
            });
            return model;
        }

        [Example("ScatterSeries with ColorAxis Rainbow(16)")]
        public static PlotModel ColorMapRainbow16()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Rainbow(16));
        }

        [Example("ScatterSeries with ColorAxis Hue(30) Star")]
        public static PlotModel ColorMapHue30()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Hue(30), MarkerType.Star);
        }

        [Example("ScatterSeries with ColorAxis Hot(64)")]
        public static PlotModel ColorMapHot64()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Hot(64), MarkerType.Triangle);
        }

        [Example("ScatterSeries with ColorAxis Cool(32)")]
        public static PlotModel ColorMapCool32()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Cool(32), MarkerType.Circle);
        }

        [Example("ScatterSeries with ColorAxis Gray(32)")]
        public static PlotModel ColorMapGray32()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Gray(32), MarkerType.Diamond);
        }

        [Example("ScatterSeries with ColorAxis Jet(32)")]
        public static PlotModel ColorMapJet32()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Jet(32), MarkerType.Plus);
        }

        [Example("ScatterSeries with ColorAxis Hot with extreme colors")]
        public static PlotModel ColorMapHot64Extreme()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Hot(64), MarkerType.Square, AxisPosition.Right, OxyColors.Magenta, OxyColors.Green);
        }

        [Example("ScatterSeries with ColorAxis Hot (top legend)")]
        public static PlotModel ColorMapHot64ExtremeTopLegend()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.Hot(64), MarkerType.Cross, AxisPosition.Top, OxyColors.Magenta, OxyColors.Green);
        }
        [Example("ScatterSeries with ColorAxis Hot(16) N=31000")]
        public static PlotModel ColorMapHot16Big()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(31000, OxyPalettes.Hot(16));
        }

        [Example("ScatterSeries with ColorAxis BlueWhiteRed (3)")]
        public static PlotModel ColorMapBlueWhiteRed3()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.BlueWhiteRed(3));
        }

        [Example("ScatterSeries with ColorAxis BlueWhiteRed (9)")]
        public static PlotModel ColorMapBlueWhiteRed9()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.BlueWhiteRed(9));
        }

        [Example("ScatterSeries with ColorAxis BlueWhiteRed (256)")]
        public static PlotModel ColorMapBlueWhiteRed256()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.BlueWhiteRed(256));
        }

        [Example("ScatterSeries with ColorAxis BlackWhiteRed (9)")]
        public static PlotModel ColorMapBlackWhiteRed9()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.BlackWhiteRed(9));
        }

        [Example("ScatterSeries with ColorAxis BlackWhiteRed (9) top legend")]
        public static PlotModel ColorMapBlackWhiteRed9TopLegend()
        {
            return CreateRandomScatterSeriesWithColorAxisPlotModel(2500, OxyPalettes.BlackWhiteRed(9), MarkerType.Square, AxisPosition.Top);
        }

        static readonly Random Randomizer = new Random();

        private static ScatterSeries CreateRandomScatterSeries(int n, string title, MarkerType markerType)
        {
            var s1 = new ScatterSeries { Title = title, MarkerType = markerType, MarkerStroke = OxyColors.Black, MarkerStrokeThickness = 1.0 };
            for (int i = 0; i < n; i++)
            {
                double x = Randomizer.NextDouble() * 10;
                double y = Randomizer.NextDouble() * 10;
                var p = new ScatterPoint(x, y);
                s1.Points.Add(p);
            }
            return s1;
        }

        private static ScatterSeries CreateRandomScatterSeries2(int n, string title, MarkerType markerType)
        {
            return new ScatterSeries
            {
                Title = title,
                MarkerType = markerType,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 1.0,
                Points = CreateRandomPoints(n)
            };
        }

        private static IList<IDataPoint> CreateRandomPoints(int n)
        {
            var points = new List<IDataPoint>();
            for (int i = 0; i < n; i++)
            {
                double x = Randomizer.NextDouble() * 10;
                double y = Randomizer.NextDouble() * 10;
                var p = new DataPoint(x, y);
                points.Add(p);
            }

            return points;
        }
    }
}