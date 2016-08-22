// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("LineSeries"), Tags("Series")]
    public class LineSeriesExamples
    {
        private static readonly Random Randomizer = new Random(13);

        [Example("Default style")]
        public static PlotModel DefaultStyle()
        {
            var model = new PlotModel { Title = "LineSeries with default style" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var lineSeries1 = CreateExampleLineSeries();
            lineSeries1.Title = "LineSeries 1";
            model.Series.Add(lineSeries1);

            return model;
        }

        [Example("Custom style")]
        public static PlotModel CustomStyle()
        {
            var model = new PlotModel { Title = "LineSeries with custom style", LegendSymbolLength = 24 };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var lineSeries1 = CreateExampleLineSeries();
            lineSeries1.Title = "LineSeries 1";
            lineSeries1.ToolTip = "This is a tooltip for a LineSeries 1";
            lineSeries1.Color = OxyColors.SkyBlue;
            lineSeries1.StrokeThickness = 3;
            lineSeries1.LineStyle = LineStyle.Dash;
            lineSeries1.MarkerType = MarkerType.Circle;
            lineSeries1.MarkerSize = 5;
            lineSeries1.MarkerStroke = OxyColors.White;
            lineSeries1.MarkerFill = OxyColors.SkyBlue;
            lineSeries1.MarkerStrokeThickness = 1.5;
            model.Series.Add(lineSeries1);

            return model;
        }

        [Example("Two LineSeries")]
        public static PlotModel TwoLineSeries()
        {
            var model = new PlotModel { Title = "Two LineSeries" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var lineSeries1 = CreateExampleLineSeries();
            lineSeries1.Title = "LineSeries 1";
            model.Series.Add(lineSeries1);

            var lineSeries2 = CreateExampleLineSeries(41);
            lineSeries2.Title = "LineSeries 2";
            model.Series.Add(lineSeries2);
            return model;
        }

        [Example("Visibility")]
        public static PlotModel IsVisibleFalse()
        {
            var model = new PlotModel { Title = "LineSeries with IsVisible = false", Subtitle = "Click to change the IsVisible property for LineSeries 2" };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 50 });

            var s1 = CreateExampleLineSeries(38);
            s1.Title = "LineSeries 1";
            model.Series.Add(s1);

            var s2 = CreateExampleLineSeries(39);
            s2.Title = "LineSeries 2";
            s2.IsVisible = false;
            model.Series.Add(s2);

            // handle mouse clicks to change visibility
            model.MouseDown += (s, e) => { s2.IsVisible = !s2.IsVisible; model.InvalidatePlot(true); };

            return model;
        }

        [Example("Custom TrackerFormatString")]
        public static PlotModel TrackerFormatString()
        {
            var model = new PlotModel { Title = "LineSeries with custom TrackerFormatString", Subtitle = "TrackerFormatString = \"X={2:0.0} Y={4:0.0}\"" };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var lineSeries1 = CreateExampleLineSeries();
            lineSeries1.TrackerFormatString = "X={2:0.0} Y={4:0.0}";
            model.Series.Add(lineSeries1);
            return model;
        }

        [Example("Custom markers")]
        public static PlotModel CustomMarkers()
        {
            var model = new PlotModel { Title = "LineSeries with custom markers", LegendSymbolLength = 30 };

            const int N = 6;
            var customMarkerOutline = new ScreenPoint[N];
            for (int i = 0; i < N; i++)
            {
                double th = Math.PI * ((4.0 * i / (N - 1)) - 0.5);
                const double R = 1;
                customMarkerOutline[i] = new ScreenPoint(Math.Cos(th) * R, Math.Sin(th) * R);
            }

            var s1 = CreateExampleLineSeries(39);
            s1.Title = "LineSeries 1";
            s1.MarkerType = MarkerType.Custom;
            s1.MarkerSize = 8;
            s1.MarkerOutline = customMarkerOutline;

            model.Series.Add(s1);

            return model;
        }

        [Example("Marker types")]
        public static PlotModel MarkerTypes()
        {
            var pm = CreateModel("LineSeries with different MarkerType", (int)MarkerType.Custom);
            pm.LegendBackground = OxyColor.FromAColor(220, OxyColors.White);
            pm.LegendBorder = OxyColors.Black;
            pm.LegendBorderThickness = 1.0;
            int i = 0;
            foreach (var ls in pm.Series.Cast<LineSeries>())
            {
                ls.Color = OxyColors.Red;
                ls.MarkerStroke = OxyColors.Black;
                ls.MarkerFill = OxyColors.Green;
                ls.MarkerType = (MarkerType)i++;
                ls.Title = ls.MarkerType.ToString();
            }

            return pm;
        }

        [Example("Labels")]
        public static PlotModel Labels()
        {
            var model = new PlotModel { Title = "LineSeries with labels", Subtitle = "Use the 'LabelFormatString' property", LegendSymbolLength = 24 };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MaximumPadding = 0.1 }); // increase the top padding to make sure the labels are visible
            var s1 = CreateExampleLineSeries();
            s1.LabelFormatString = "{1}";
            s1.MarkerType = MarkerType.Circle;
            model.Series.Add(s1);
            return model;
        }

        [Example("LineStyle")]
        public static PlotModel LineStyles()
        {
            var pm = CreateModel("LineSeries with LineStyle", (int)LineStyle.None);
            pm.LegendPlacement = LegendPlacement.Outside;
            pm.LegendSymbolLength = 50;
            int i = 0;
            foreach (var lineSeries in pm.Series.Cast<LineSeries>())
            {
                lineSeries.Color = OxyColors.Red;
                lineSeries.LineStyle = (LineStyle)i++;
                lineSeries.Title = lineSeries.LineStyle.ToString();
            }

            return pm;
        }

        [Example("Smooth")]
        public static PlotModel Smooth()
        {
            var model = new PlotModel { Title = "LineSeries with Smooth = true", LegendSymbolLength = 24 };

            var s1 = CreateExampleLineSeries();
            s1.MarkerType = MarkerType.Circle;
            s1.Smooth = true;
            model.Series.Add(s1);

            return model;
        }

        [Example("Smooth (complex curve)")]
        public static PlotModel ComplexSmoothLine()
        {
            var model = new PlotModel { Title = "LineSeries with Smooth = true", Subtitle = "complex curve" };

            var s1 = new LineSeries
            {
                Title = "Series 1",
                Smooth = true
            };
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

        [Example("LineLegendPosition")]
        public static PlotModel CustomLineLegendPosition()
        {
            var model = new PlotModel { Title = "LineSeries with LineLegendPosition" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var s1 = CreateExampleLineSeries();
            s1.Title = "Start";
            s1.MarkerType = MarkerType.Circle;
            s1.LineLegendPosition = LineLegendPosition.Start;
            model.Series.Add(s1);

            var s2 = CreateExampleLineSeries(41);
            s2.Title = "End";
            s2.MarkerType = MarkerType.Circle;
            s2.LineLegendPosition = LineLegendPosition.End;
            model.Series.Add(s2);

            return model;
        }

        [Example("Broken lines")]
        public static PlotModel BrokenLine()
        {
            var model = new PlotModel { Title = "LineSeries with broken lines" };

            var s1 = CreateExampleLineSeries();
            s1.Points[3] = DataPoint.Undefined;
            s1.Points[7] = DataPoint.Undefined;
            s1.BrokenLineColor = OxyColors.Gray;
            s1.BrokenLineThickness = 0.5;
            s1.BrokenLineStyle = LineStyle.Solid;
            model.Series.Add(s1);

            var s2 = CreateExampleLineSeries(49);
            s2.Points[3] = DataPoint.Undefined;
            s2.Points[7] = DataPoint.Undefined;
            s2.BrokenLineColor = OxyColors.Automatic;
            s2.BrokenLineThickness = 1;
            s2.BrokenLineStyle = LineStyle.Dot;
            model.Series.Add(s2);
            
            return model;
        }

        [Example("Without Decimator")]
        public static PlotModel WithoutDecimator()
        {
            var model = new PlotModel { Title = "LineSeries without Decimator" };
            var s1 = CreateSeriesSuitableForDecimation();
            model.Series.Add(s1);
            return model;
        }

        [Example("With X Decimator")]
        public static PlotModel WithXDecimator()
        {
            var model = new PlotModel { Title = "LineSeries with X Decimator" };
            var s1 = CreateSeriesSuitableForDecimation();
            s1.Decimator = Decimator.Decimate;
            model.Series.Add(s1);
            return model;
        }

        /// <summary>
        /// Creates an example line series.
        /// </summary>
        /// <returns>A line series containing random points.</returns>
        private static LineSeries CreateExampleLineSeries(int seed = 13)
        {
            var lineSeries1 = new LineSeries();
            var r = new Random(seed);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 100; x += 10)
            {
                lineSeries1.Points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }

            return lineSeries1;
        }

        private static PlotModel CreateModel(string title, int n = 20)
        {
            var model = new PlotModel { Title = title };
            for (int i = 1; i <= n; i++)
            {
                var s = new LineSeries { Title = "Series " + i };
                model.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                {
                    s.Points.Add(new DataPoint(x, (Math.Sin(x * i) / (i + 1)) + i));
                }
            }

            return model;
        }

        private static LineSeries CreateSeriesSuitableForDecimation()
        {
            var s1 = new LineSeries();

            int n = 20000;
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint((double)i / n, Math.Sin(i)));
            }

            return s1;
        }

        private static OxyPlot.Series.Series CreateRandomLineSeries(int n, string title, MarkerType markerType)
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
    }
}