// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeriesExamples.cs" company="OxyPlot">
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
using System;
using OxyPlot;

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("LineSeries")]
    public class LineSeriesExamples : ExamplesBase
    {
        [Example("LineSeries")]
        public static PlotModel OneSeries()
        {
            var model = new PlotModel("LineSeries") { LegendSymbolLength = 24 };
            var s1 = new LineSeries("Series 1")
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);

            return model;
        }

        [Example("Two LineSeries")]
        public static PlotModel TwoLineSeries()
        {
            var model = new PlotModel("Two LineSeries") { LegendSymbolLength = 24 };
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 71, "Y-Axis"));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 61, "X-Axis"));
            var s1 = new LineSeries("Series 1")
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);

            var s2 = new LineSeries("Series 2")
            {
                Color = OxyColors.Teal,
                MarkerType = MarkerType.Diamond,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.Teal,
                MarkerStrokeThickness = 1.5
            };
            s2.Points.Add(new DataPoint(0, 4));
            s2.Points.Add(new DataPoint(10, 32));
            s2.Points.Add(new DataPoint(40, 14));
            s2.Points.Add(new DataPoint(60, 20));
            model.Series.Add(s2);
            return model;
        }

        [Example("Invisible LineSeries")]
        public static PlotModel InvisibleLineSeries()
        {
            var model = new PlotModel("Invisible LineSeries");
            var s1 = new LineSeries("Series 1");
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            var s2 = new LineSeries("Series 2") { IsVisible = false };
            s2.Points.Add(new DataPoint(40, 20));
            s2.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);
            model.Series.Add(s2);
            return model;
        }

        [Example("Marker types")]
        public static PlotModel MarkerTypes()
        {
            var model = new PlotModel("Marker types");
            model.Series.Add(CreateRandomLineSeries(10, "Circle", MarkerType.Circle));
            model.Series.Add(CreateRandomLineSeries(10, "Cross", MarkerType.Cross));
            model.Series.Add(CreateRandomLineSeries(10, "Diamond", MarkerType.Diamond));
            model.Series.Add(CreateRandomLineSeries(10, "Plus", MarkerType.Plus));
            model.Series.Add(CreateRandomLineSeries(10, "Square", MarkerType.Square));
            model.Series.Add(CreateRandomLineSeries(10, "Star", MarkerType.Star));
            model.Series.Add(CreateRandomLineSeries(10, "Triangle", MarkerType.Triangle));
            return model;
        }

        [Example("LineSeries with labels")]
        public static PlotModel LineSeriesWithLabels()
        {
            var model = new PlotModel("LineSeries with labels") { LegendSymbolLength = 24 };
            var s1 = new LineSeries("Series 1")
            {
                LabelFormatString = "{1}",
                // LabelFormatString = "{0} -> {1}",
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { MinimumPadding = 0.1, MaximumPadding = 0.1 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { MinimumPadding = 0.1, MaximumPadding = 0.1 });
            return model;
        }
        static readonly Random Randomizer = new Random();

        private static Series CreateRandomLineSeries(int n, string title, MarkerType markerType)
        {
            var s1 = new LineSeries { Title = title, MarkerType = markerType, MarkerStroke = OxyColors.Black, MarkerStrokeThickness = 1.0 };
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + Randomizer.NextDouble() * 10;
                y += 1 + Randomizer.NextDouble();
                var p = new DataPoint(x, y);
                s1.Points.Add(p);
            }
            return s1;
        }

        [Example("Custom markers")]
        public static PlotModel CustomMarkers()
        {
            var model = new PlotModel("LineSeries with custom markers") { LegendSymbolLength = 30, PlotType = PlotType.Cartesian };
            const int N = 6;
            var customMarkerOutline = new ScreenPoint[N];
            for (int i = 0; i < N; i++)
            {
                double th = Math.PI * (4.0 * i / (N - 1) - 0.5);
                const double R = 1;
                customMarkerOutline[i] = new ScreenPoint(Math.Cos(th) * R, Math.Sin(th) * R);
            }

            var s1 = new LineSeries("Series 1")
                         {
                             Color = OxyColors.Red,
                             StrokeThickness = 2,
                             MarkerType = MarkerType.Custom,
                             MarkerOutline = customMarkerOutline,
                             MarkerFill = OxyColors.DarkRed,
                             MarkerStroke = OxyColors.Black,
                             MarkerStrokeThickness = 0,
                             MarkerSize = 10
                         };

            foreach (var pt in customMarkerOutline)
            {
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));
            }

            model.Series.Add(s1);

            return model;
        }

        [Example("Normal distribution")]
        public static PlotModel CreateNormalDistributionModel()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function"
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
                Title = String.Format("μ={0}, σ²={1}", mean, variance)
            };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + (x1 - x0) * i / (n - 1);
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }
            return ls;
        }

        [Example("LineStyle")]
        public static PlotModel LineStyles()
        {
            var pm = CreateModel("LineStyle", (int)LineStyle.None);
            pm.LegendPlacement = LegendPlacement.Outside;
            pm.LegendSymbolLength = 50;
            int i = 0;
            foreach (LineSeries ls in pm.Series)
            {
                ls.Color = OxyColors.Red;
                ls.LineStyle = (LineStyle)i++;
                ls.Title = ls.LineStyle.ToString();
            }
            return pm;
        }

        [Example("MarkerType")]
        public static PlotModel MarkerTypes2()
        {
            var pm = CreateModel("MarkerType", (int)MarkerType.Custom);
            pm.LegendBackground = OxyColors.White.ChangeAlpha(220);
            pm.LegendBorder = OxyColors.Black;
            pm.LegendBorderThickness = 1.0;
            int i = 0;
            foreach (LineSeries ls in pm.Series)
            {
                ls.Color = OxyColors.Red;
                ls.MarkerStroke = OxyColors.Black;
                ls.MarkerFill = OxyColors.Green;
                ls.MarkerType = (MarkerType)i++;
                ls.Title = ls.MarkerType.ToString();
            }
            return pm;
        }

        [Example("Smooth Line")]
        public static PlotModel SmoothLine()
        {
            var model = new PlotModel("Smooth Line") { LegendSymbolLength = 24 };
            var s1 = new LineSeries("Series 1")
            {
                Color = OxyColors.Purple,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.Purple,
                MarkerStrokeThickness = 1.0,
                Smooth = true
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 5));
            s1.Points.Add(new DataPoint(20, 1));
            s1.Points.Add(new DataPoint(30, 20));
            model.Series.Add(s1);

            s1 = new LineSeries("Series 2 - tracker")
            {
                Color = OxyColors.OrangeRed,
                MarkerType = MarkerType.Diamond,
                MarkerSize = 4,
                MarkerStroke = OxyColors.WhiteSmoke,
                MarkerFill = OxyColors.OrangeRed,
                MarkerStrokeThickness = 1.0,
                Smooth = true
            };
            s1.Points.Add(new DataPoint(0, 15));
            s1.Points.Add(new DataPoint(3, 23));
            s1.Points.Add(new DataPoint(9, 30));
            s1.Points.Add(new DataPoint(20, 12));
            s1.Points.Add(new DataPoint(30, 10));
            model.Series.Add(s1);

            s1.CanTrackerInterpolatePoints = true;

            return model;
        }

        [Example("Complex Smooth Line")]
        public static PlotModel ComplexSmoothLine()
        {
            var model = new PlotModel("Complex Smooth Lines");

            var s1 = new LineSeries("Series 1") { Smooth = true };
            s1.Points.Add(new DataPoint(-0.03, 22695655));
            s1.Points.Add(new DataPoint(-0.02, 34005991));
            s1.Points.Add(new DataPoint(-0.01, 40209650));
            s1.Points.Add(new DataPoint(0, 41306630));
            s1.Points.Add(new DataPoint(0.01, 37296932));
            s1.Points.Add(new DataPoint(0.02, 28180557));
            s1.Points.Add(new DataPoint(0.03, 13957503));
            model.Series.Add(s1);
            return model;
        }

        private static PlotModel CreateModel(string title, int n = 20)
        {
            var model = new PlotModel(title);
            for (int i = 1; i <= n; i++)
            {
                var s = new LineSeries("Series " + i);
                model.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                    s.Points.Add(new DataPoint(x, Math.Sin(x * i) / (i + 1) + i));
            }
            return model;

        }

        [Example("Two-color LineSeries")]
        public static PlotModel TwoColorLineSeries()
        {
            var model = new PlotModel("TwoColorLineSeries") { LegendSymbolLength = 24 };
            var s1 = new TwoColorLineSeries()
            {
                Title = "Temperature at Eidesmoen, December 1986.",
                Color = OxyColors.Red,
                Color2 = OxyColors.LightBlue,
                StrokeThickness = 3,
                Limit = 0,
                Smooth = true,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 1.5
            };
            var temperature = new[]
                {
                    5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0,
                    -5
                };

            for (int i = 0; i < temperature.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperature[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { ExtraGridlines = new[] { 0.0 } });

            return model;
        }

        [Example("LineSeries with legend at the end of the line")]
        public static PlotModel LineLegendPositionAtEnd()
        {
            // http://www.perceptualedge.com/example2.php
            var model = new PlotModel("Average (Mean) monthly temperatures in 2003") { PlotMargins = new OxyThickness(60, 4, 60, 40), PlotAreaBorderThickness = 0, IsLegendVisible = false };
            var phoenix = new LineSeries("Phoenix") { LineLegendPosition = LineLegendPosition.End };
            var raleigh = new LineSeries("Raleigh") { LineLegendPosition = LineLegendPosition.End };
            var minneapolis = new LineSeries("Minneapolis") { LineLegendPosition = LineLegendPosition.End };

            var phoenixTemps = new[] { 52.1, 55.1, 59.7, 67.7, 76.3, 84.6, 91.2, 89.1, 83.8, 72.2, 59.8, 52.5 };
            var raleighTemps = new[] { 40.5, 42.2, 49.2, 59.5, 67.4, 74.4, 77.5, 76.5, 70.6, 60.2, 50.0, 41.2 };
            var minneapolisTemps = new[] { 12.2, 16.5, 28.3, 45.1, 57.1, 66.9, 71.9, 70.2, 60.0, 50.0, 32.4, 18.6 };

            for (int i = 0; i < 12; i++)
            {
                phoenix.Points.Add(new DataPoint(i, phoenixTemps[i]));
                raleigh.Points.Add(new DataPoint(i, raleighTemps[i]));
                minneapolis.Points.Add(new DataPoint(i, minneapolisTemps[i]));
            }

            model.Series.Add(phoenix);
            model.Series.Add(raleigh);
            model.Series.Add(minneapolis);

            model.Axes.Add(new CategoryAxis(null, "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec") { AxislineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Fahrenheit") { AxislineStyle = LineStyle.Solid });

            return model;
        }

        [Example("Broken line")]
        public static PlotModel BrokenLine()
        {
            var model = new PlotModel("Broken line");
            var s1 = new LineSeries
                {
                    BrokenLineColor = OxyColors.Gray,
                    BrokenLineThickness = 1,
                    BrokenLineStyle = LineStyle.Dash
                };
            s1.Points.Add(new DataPoint(0, 26));
            s1.Points.Add(new DataPoint(10, 30));
            s1.Points.Add(DataPoint.Undefined);
            s1.Points.Add(new DataPoint(10, 25));
            s1.Points.Add(new DataPoint(20, 26));
            s1.Points.Add(new DataPoint(25, 36));
            s1.Points.Add(new DataPoint(30, 40));
            s1.Points.Add(DataPoint.Undefined);
            s1.Points.Add(new DataPoint(30, 20));
            s1.Points.Add(new DataPoint(40, 10));
            model.Series.Add(s1);
            return model;
        }

    }
}