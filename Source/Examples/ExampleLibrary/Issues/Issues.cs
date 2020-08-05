// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Issues.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Z1 Issues")]
    public class Issues
    {
        [Example("#1095: Issue 1095 Part 1")]
        public static PlotModel IssueHalfPolarReversedAxesPart1()
        {
            var plotModel = new PlotModel { Title = "", };
            plotModel.PlotType = OxyPlot.PlotType.Polar;
            plotModel.Axes.Add(
                new AngleAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorStep = 30,
                    MinorStep = 10,
                    CropGridlines = false,
                    StartAngle = 270 + 360,
                    EndAngle = 270,
                    Minimum = -180,
                    Maximum = +180,
                    LabelFormatter = (d) => d.ToString("F0")
                });
            plotModel.Axes.Add(
                new MagnitudeAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                });

            return plotModel;
        }

        [Example("#1095: Issue 1095 Part 2")]
        public static PlotModel IssueHalfPolarReversedAxesPart2()
        {
            var plotModel = new PlotModel { Title = "", };
            plotModel.PlotType = OxyPlot.PlotType.Polar;
            plotModel.Axes.Add(
                new AngleAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorStep = 30,
                    MinorStep = 10,
                    CropGridlines = false,
                    StartAngle = 180,
                    EndAngle = 0,
                    Minimum = -90,
                    Maximum = +90,
                    LabelFormatter = (d) => d.ToString("F0")
                });
            plotModel.Axes.Add(
                new MagnitudeAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                });

            return plotModel;
        }

        [Example("#91 AxisTitleDistance")]
        public static PlotModel AxisTitleDistance()
        {
            var plotModel = new PlotModel
            {
                Title = "AxisTitleDistance"
            };

            var l = new Legend
            {
                LegendFontSize = 12,
                LegendFontWeight = FontWeights.Bold
            };

            plotModel.Legends.Add(l);

            //var series = new LineSeries() { Title = "Push-Over Curve" };
            //series.Points.AddRange(pushOverPoints);
            //plotModel.Series.Add(series);

            plotModel.Axes.Add(new LinearAxis
            {
                Title = "Base Shear",
                Unit = "KN",
                TitleFontSize = 12,
                TitleFontWeight = FontWeights.Bold,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                AxisTitleDistance = 15
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Title = "Displacement",
                Unit = "mm",
                TitleFontSize = 12,
                TitleFontWeight = FontWeights.Bold,
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
                AxisTitleDistance = 10
            });
            return plotModel;
        }

        [Example("#1044 MinimumSegmentLength not working with AreaSeries")]
        public static PlotModel MinimumSegmentLengthInAreaSeries()
        {
            var model = new PlotModel() { Title = "MinimumSegmentLength in AreaSeries", Subtitle = "Three different areas should be visible" };
            for (var msl = 0; msl <= 200; msl += 100)
            {
                var series = new AreaSeries
                {
                    Title = $"MinimumSegmentLength = {msl}",
                    MinimumSegmentLength = msl
                };

                for (int i = 0; i < 1000; i++)
                {
                    series.Points.Add(new DataPoint(i, Math.Sin(i * 0.01) + 1));
                    series.Points2.Add(new DataPoint(i, Math.Sin(i * 0.01)));
                }

                model.Series.Add(series);
            }

            return model;
        }

        [Example("#1044 MinimumSegmentLength not working with LinesSeries")]
        public static PlotModel MinimumSegmentLengthInLineSeries()
        {
            var model = new PlotModel() { Title = "MinimumSegmentLength in LineSeries", Subtitle = "Three different curves should be visible" };
            for (var msl = 0; msl <= 200; msl += 100)
            {
                var series = new LineSeries
                {
                    Title = $"MinimumSegmentLength = {msl}",
                    MinimumSegmentLength = msl
                };

                for (int i = 0; i < 1000; i++)
                {
                    series.Points.Add(new DataPoint(i, Math.Sin(i * 0.01)));
                }

                model.Series.Add(series);
            }

            return model;
        }

        [Example("#1303 Problem with infinity size polyline")]
        public static PlotModel InfinitySizePolyline()
        {
            var model = new PlotModel();
            var series = new OxyPlot.Series.LineSeries();
            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(1, -1e40));
            model.Series.Add(series);
            return model;
        }

        [Example("#977 RectangleAnnotation Axis Clipping not configurable")]
        public static PlotModel RectangleAnnotationAxisClipping()
        {
            var model = new PlotModel
            {
                Title = "RectangleAnnotation Axis Clipping",
                PlotAreaBorderThickness = new OxyThickness(0),
                Axes =
                {
                    new LinearAxis
                    {
                        Position = AxisPosition.Bottom,
                        AxislineStyle = LineStyle.Solid,
                        EndPosition = 0.45
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Bottom,
                        AxislineStyle = LineStyle.Solid,
                        StartPosition = 0.55,
                        Key = "X2"
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        AxislineStyle = LineStyle.Solid,
                        EndPosition = 0.45,
                    },
                    new LinearAxis
                    {
                        Position = AxisPosition.Left,
                        AxislineStyle = LineStyle.Solid,
                        StartPosition = 0.55,
                        Key = "Y2"
                    }
                },
                Annotations =
                {
                    new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        Color = OxyColors.DarkCyan,
                        StrokeThickness = 2,
                        LineStyle = LineStyle.Solid,
                        X = 10,
                        Text = "LineAnnotation (default clipping)"
                    },
                    new LineAnnotation
                    {
                        Type = LineAnnotationType.Vertical,
                        Color = OxyColors.DarkGreen,
                        StrokeThickness = 2,
                        LineStyle = LineStyle.Solid,
                        X = 20,
                        ClipByYAxis = false,
                        Text = "LineAnnotation (ClipByYAxis = false)",
                        TextLinePosition = 0.5
                    },
                    new RectangleAnnotation
                    {
                        Fill = OxyColor.FromArgb(100, 255, 0, 0),
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1,
                        MinimumX = 40,
                        MaximumX = 60,
                        Text = "RectangleAnnotation (default clipping)",
                        TextRotation = -90,
                    },
                    new RectangleAnnotation
                    {
                        Fill = OxyColor.FromArgb(100, 0, 0, 255),
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1,
                        MinimumX = 70,
                        MaximumX = 90,
                        ClipByYAxis = false,
                        Text = "RectangleAnnotation (ClipByYAxis = false)",
                        TextRotation = -90
                    },
                    new RectangleAnnotation
                    {
                        Fill = OxyColor.FromArgb(100, 0, 255, 0),
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1,
                        MinimumY = 80,
                        MaximumY = 85,
                        Text = "RectangleAnnotation (default clipping)",
                        XAxisKey = "X2",
                        YAxisKey = "Y2"
                    },
                    new RectangleAnnotation
                    {
                        Fill = OxyColor.FromArgb(100, 0, 255, 0),
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1,
                        MinimumY = 90,
                        MaximumY = 95,
                        ClipByXAxis = false,
                        Text = "RectangleAnnotation (ClipByXAxis = false)",
                        XAxisKey = "X2",
                        YAxisKey = "Y2"
                    },
                    new RectangleAnnotation
                    {
                        Fill = OxyColor.FromArgb(50, 100, 100, 100),
                        Stroke = OxyColors.Black,
                        StrokeThickness = 1,
                        MinimumX = 92, MaximumX = 140,
                        MinimumY = 45, MaximumY = 140,
                        ClipByXAxis = false, ClipByYAxis = false,
                        Text = "no clipping at all"
                    }
                }
            };
            return model;
        }


        [Example("Support colour coding on scatter plots (Closed)")]
        public static PlotModel ColorCodingOnScatterPlots()
        {
            var model = new PlotModel { Title = "Colour coding on scatter plots" };
            var colorAxis = new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), Minimum = 0, Maximum = 5, HighColor = OxyColors.Gray, LowColor = OxyColors.Black };
            model.Axes.Add(colorAxis);

            var s4 = new ScatterSeries { MarkerType = MarkerType.Circle };
            s4.Points.Add(new ScatterPoint(3, 5, 5, 0));
            s4.Points.Add(new ScatterPoint(5, 5, 7, 0));
            s4.Points.Add(new ScatterPoint(2, 4, 5, 0.3));
            s4.Points.Add(new ScatterPoint(3, 3, 8, 0));
            s4.Points.Add(new ScatterPoint(3, 2, 5, 0));
            s4.Points.Add(new ScatterPoint(3, 5, 8, 1));
            s4.Points.Add(new ScatterPoint(2, 2, 3, 5));
            s4.Points.Add(new ScatterPoint(1, 4, 4, 1));
            s4.Points.Add(new ScatterPoint(4, 3, 5, 3));
            s4.Points.Add(new ScatterPoint(0, 0, 1, 1));
            s4.Points.Add(new ScatterPoint(8, 8, 1, 1));
            model.Series.Add(s4);
            return model;
        }

        [Example("Don't show minor ticks (Closed)")]
        public static PlotModel DontShowMinorTicks()
        {
            var model = new PlotModel { Title = "MinorTickSize = 0" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinorTickSize = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinorTickSize = 0, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid });
            return model;
        }

        /// <summary>
        /// Grids the lines both different colors.
        /// </summary>
        [Example("Major grid lines in front of minor (Closed)")]
        public static PlotModel GridLinesBothDifferentColors()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Major grid lines in front of minor",
                Subtitle = "Minor grid lines should be below major grid lines"
            };
            var leftAxis = new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineThickness = 4,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColors.LightBlue,
                MinorGridlineThickness = 4,
            };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineThickness = 4,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColors.LightBlue,
                MinorGridlineThickness = 4,
            };
            plotModel1.Axes.Add(bottomAxis);
            return plotModel1;
        }

        [Example("#50: Sub/superscript in vertical axis title")]
        public static PlotModel SubSuperScriptInAxisTitles()
        {
            var plotModel1 = new PlotModel { Title = "x_{i}^{j}", Subtitle = "x_{i}^{j}" };
            var leftAxis = new LinearAxis { Position = AxisPosition.Left, Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(bottomAxis);
            plotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 100, "x_{i}^{j}"));
            return plotModel1;
        }

        [Example("#50: Sub/superscript in rotated annotations")]
        public static PlotModel RotatedSubSuperScript()
        {
            var s = "x_{A}^{B}";
            var plotModel1 = new PlotModel { Title = s, Subtitle = s };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = s, Minimum = -1, Maximum = 1 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = s, Minimum = -1, Maximum = 11 });
            for (int rotation = 0; rotation < 360; rotation += 45)
            {
                plotModel1.Annotations.Add(new TextAnnotation { Text = s, TextPosition = new DataPoint(rotation / 360d * 10, 0), TextRotation = rotation });
            }

            return plotModel1;
        }

        [Example("#61: DateTimeAxis with IntervalType = Minutes")]
        public static PlotModel DateTimeAxisWithIntervalTypeMinutes()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);

            var dateTimeAxis1 = new DateTimeAxis
            {
                IntervalType = DateTimeIntervalType.Minutes,
                EndPosition = 0,
                StartPosition = 1,
                StringFormat = "hh:mm:ss"
            };
            plotModel1.Axes.Add(dateTimeAxis1);
            var time0 = new DateTime(2013, 5, 6, 3, 24, 0);
            var time1 = new DateTime(2013, 5, 6, 3, 28, 0);
            var lineSeries1 = new LineSeries();
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time0), 36));
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time1), 26));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("#67: Hit testing LineSeries with smoothing")]
        public static PlotModel MouseDownEvent()
        {
            var model = new PlotModel { Title = "LineSeries with smoothing", Subtitle = "Tracker uses wrong points" };
            var logarithmicAxis1 = new LogarithmicAxis { Position = AxisPosition.Bottom };
            model.Axes.Add(logarithmicAxis1);

            // Add a line series
            var s1 = new LineSeries
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
            };
            s1.Points.Add(new DataPoint(100, 100));
            s1.Points.Add(new DataPoint(400, 200));
            s1.Points.Add(new DataPoint(600, -300));
            s1.Points.Add(new DataPoint(1000, 400));
            s1.Points.Add(new DataPoint(1500, 500));
            s1.Points.Add(new DataPoint(2500, 600));
            s1.Points.Add(new DataPoint(3000, 700));
            model.Series.Add(s1);

            return model;
        }

        [Example("#68: Tracker wrong for logarithmic y-axis")]
        public static PlotModel ValueTime()
        {
            var plotModel1 = new PlotModel
            {
                PlotAreaBackground = OxyColors.Gray,
                PlotAreaBorderColor = OxyColors.Gainsboro,
                PlotAreaBorderThickness = new OxyThickness(2),
                Title = "Value / Time"
            };

            var l = new Legend
            {
                LegendBackground = OxyColor.FromArgb(200, 255, 255, 255),
                LegendBorder = OxyColors.Black,
                LegendPlacement = LegendPlacement.Outside
            };

            plotModel1.Legends.Add(l);

            var linearAxis1 = new LinearAxis
            {
                AbsoluteMaximum = 45,
                AbsoluteMinimum = 0,
                Key = "X-Axis",
                Maximum = 46,
                Minimum = -1,
                Position = AxisPosition.Bottom,
                Title = "Years",
                Unit = "yr"
            };
            plotModel1.Axes.Add(linearAxis1);
            var logarithmicAxis1 = new LogarithmicAxis { Key = "Y-Axis", Title = "Value for section" };
            plotModel1.Axes.Add(logarithmicAxis1);
            var lineSeries1 = new LineSeries
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                MarkerFill = OxyColors.Black,
                MarkerSize = 2,
                MarkerStroke = OxyColors.Black,
                MarkerType = MarkerType.Circle,
                DataFieldX = "X",
                DataFieldY = "Y",
                XAxisKey = "X-Axis",
                YAxisKey = "Y-Axis",
                Background = OxyColors.White,
                Title = "Section Value",
                TrackerKey = "ValueVersusTimeTracker"
            };
            lineSeries1.Points.Add(new DataPoint(0, 0));
            lineSeries1.Points.Add(new DataPoint(5, 0));
            lineSeries1.Points.Add(new DataPoint(10, 0));
            lineSeries1.Points.Add(new DataPoint(15, 0));
            lineSeries1.Points.Add(new DataPoint(20, 1));
            lineSeries1.Points.Add(new DataPoint(25, 1));
            lineSeries1.Points.Add(new DataPoint(30, 1));
            lineSeries1.Points.Add(new DataPoint(35, 1));
            lineSeries1.Points.Add(new DataPoint(40, 1));
            lineSeries1.Points.Add(new DataPoint(45, 1));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("AnnotationLayers (Closed)")]
        public static PlotModel AnnotationLayers()
        {
            var model = new PlotModel { Title = "AnnotationLayers" };

            var a1 = new RectangleAnnotation { MinimumX = 10, MaximumX = 20, MinimumY = -1, MaximumY = 1, Layer = AnnotationLayer.BelowAxes };
            var a2 = new RectangleAnnotation { MinimumX = 30, MaximumX = 40, MinimumY = -1, MaximumY = 1, Layer = AnnotationLayer.BelowSeries };
            var a3 = new RectangleAnnotation { MinimumX = 50, MaximumX = 60, MinimumY = -1, MaximumY = 1, Layer = AnnotationLayer.AboveSeries };
            model.Annotations.Add(a1);
            model.Annotations.Add(a2);
            model.Annotations.Add(a3);
            var s1 = new FunctionSeries(Math.Sin, 0, 100, 0.01);
            model.Series.Add(s1);
            a1.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked annotation below axes";
                model.InvalidatePlot(true);
                e.Handled = true;
            };
            a2.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked annotation below series";
                model.InvalidatePlot(true);
                e.Handled = true;
            };
            a3.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked annotation above series";
                model.InvalidatePlot(true);
                e.Handled = true;
            };
            s1.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked series";
                model.InvalidatePlot(true);
                e.Handled = true;
            };

            return model;
        }

        [Example("Argument out of range in OxyPlot mouse over (Closed)")]
        public static PlotModel ArgumentOutOfRangeInMouseOver()
        {
            var model = new PlotModel { Title = "Argument out of range in OxyPlot mouse over" };
            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(12, 10));
            model.Series.Add(ls);
            return model;
        }

        [Example("Slow redraws with noisy data (Closed)")]
        public static PlotModel NoisyData()
        {
            var model = new PlotModel { Title = "Noisy data" };

            var points = new List<DataPoint>();
            var rng = new Random(7);
            for (int i = 0; i < 500; i++)
            {
                points.Add(new DataPoint(i + 1, rng.NextDouble()));
            }

            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }

        [Example("Dashed line test (Closed)")]
        public static PlotModel DashedLineTest()
        {
            var model = new PlotModel { Title = "Dashed line test" };

            for (int y = 1; y <= 24; y++)
            {
                var line = new LineSeries
                {
                    StrokeThickness = y,
                    LineStyle = LineStyle.Dash,
                    Dashes = new double[] { 1, 2, 3 } // has no effect
                };
                for (int i = 0; i < 20; i++)
                {
                    line.Points.Add(new DataPoint(i + 1, y));
                }

                model.Series.Add(line);
            }

            return model;
        }

        [Example("Super exponential format (Closed)")]
        public static PlotModel SuperExponentialFormat()
        {
            var model = new PlotModel { Title = "UseSuperExponentialFormat=true and 0" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100, MajorStep = 10, MinorStep = 1, UseSuperExponentialFormat = true });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -100, Maximum = 100, MajorStep = 20, MinorStep = 10, UseSuperExponentialFormat = true });
            return model;
        }

        [Example("AreaSeries draws on top of other elements (Closed)")]
        public static PlotModel DefaultAnnotationLayer()
        {
            var plotModel1 = new PlotModel { Title = "Annotations should be drawn on top by default", Subtitle = "The line annotation should be on top!" };
            var areaSeries1 = new AreaSeries();
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 40));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.Points2.Add(new DataPoint(0, 60));
            areaSeries1.Points2.Add(new DataPoint(5, 80));
            areaSeries1.Points2.Add(new DataPoint(20, 70));
            areaSeries1.Color = OxyColors.Red;
            areaSeries1.Color2 = OxyColors.Blue;
            areaSeries1.Fill = OxyColors.Yellow;

            plotModel1.Series.Add(areaSeries1);
            var lineAnnotation = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                Layer = AnnotationLayer.AboveSeries,
                X = 6
            };

            plotModel1.Annotations.Add(lineAnnotation);
            return plotModel1;
        }

        [Example("#79: LegendItemAlignment = Center (closed)")]
        public static PlotModel LegendItemAlignmentCenter()
        {
            var plotModel1 = new PlotModel { Title = "LegendItemAlignment = Center" };
            var l = new Legend
            {
                LegendItemAlignment = HorizontalAlignment.Center,
                LegendBorder = OxyColors.Black,
                LegendBorderThickness = 1
            };

            plotModel1.Legends.Add(l);

            plotModel1.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, 0, 10, 100, "sin(x)/x"));
            plotModel1.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 100, "cos(x)"));
            return plotModel1;
        }

        [Example("AreaSeries should respect CanTrackerInterpolatePoints (Closed)")]
        public static PlotModel AreaSeries_CanTrackerInterpolatePointsFalse()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with CanTrackerInterpolatePoints=false" };
            var areaSeries1 = new AreaSeries { CanTrackerInterpolatePoints = false };
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 40));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.Points2.Add(new DataPoint(0, 60));
            areaSeries1.Points2.Add(new DataPoint(5, 80));
            areaSeries1.Points2.Add(new DataPoint(20, 70));
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("AreaSeries should respect CanTrackerInterpolatePoints=true (Closed)")]
        public static PlotModel AreaSeries_CanTrackerInterpolatePointsTrue()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries with CanTrackerInterpolatePoints=true" };
            var areaSeries1 = new AreaSeries { CanTrackerInterpolatePoints = true };
            areaSeries1.Points.Add(new DataPoint(0, 50));
            areaSeries1.Points.Add(new DataPoint(10, 40));
            areaSeries1.Points.Add(new DataPoint(20, 60));
            areaSeries1.Points2.Add(new DataPoint(0, 60));
            areaSeries1.Points2.Add(new DataPoint(5, 80));
            areaSeries1.Points2.Add(new DataPoint(20, 70));
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

        [Example("GetNearestPoint return DataPoint even when custom IDataPoint used (closed)")]
        public static PlotModel GetNearestPointReturnsDataPoint()
        {
            var plotModel1 = new PlotModel { Title = "GetNearestPoint" };
            //// TODO: add code to reproduce
            return plotModel1;
        }

        [Example("#102: Selecting points changes the legend colours")]
        public static PlotModel SelectingPointsChangesTheLegendColors()
        {
            var plotModel1 = new PlotModel { Title = "Selecting points changes the legend colours" };
            //// TODO: add code to reproduce
            return plotModel1;
        }

        [Example("Empty LineSeries with smoothing (Closed)")]
        public static PlotModel EmptyLineSeriesWithSmoothing_ThrowsException()
        {
            var plotModel1 = new PlotModel { Title = "Empty LineSeries with smoothing" };
            plotModel1.Series.Add(new LineSeries
            {
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
            });
            return plotModel1;
        }

        [Example("#119: Data points remain visible outside of bounds on panning")]
        public static PlotModel DataPointsRemainVisibleOutsideBoundsOnPanning()
        {
            var plotModel1 = new PlotModel();

            var masterAxis = new DateTimeAxis { Key = "MasterDateTimeAxis", Position = AxisPosition.Bottom };
            plotModel1.Axes.Add(masterAxis);

            var verticalAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Measurement",
                Key = "Measurement",
                AbsoluteMinimum = -100,
                Minimum = -100,
                AbsoluteMaximum = 100,
                Maximum = 100,
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            plotModel1.Axes.Add(verticalAxis);

            var line = new LineSeries { Title = "Measurement", XAxisKey = masterAxis.Key, YAxisKey = verticalAxis.Key };
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now), 10));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(1)), 10));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(2)), 45));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(3)), 17));

            line.Points.Add(DataPoint.Undefined);

            // this point should be visible
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(4)), 10));
            //// line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(4)), 10));

            line.Points.Add(DataPoint.Undefined);

            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(5)), 45));
            line.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(6)), 17));

            plotModel1.Series.Add(line);

            return plotModel1;
        }

        [Example("Floating-point inaccuracy (Closed)")]
        public static PlotModel FloatingPointInaccuracy()
        {
            var model = new PlotModel { Title = "Floating-point inaccuracy" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -0.0515724495834661, Maximum = 0.016609368598352, MajorStep = 0.02, MinorStep = 0.002 });
            return model;
        }

        [Example("LineSeries.Dashes property (Closed)")]
        public static PlotModel DashesTest()
        {
            var model = new PlotModel { Title = "Dashed line test" };

            for (int y = 1; y <= 10; y++)
            {
                var line = new LineSeries
                {
                    StrokeThickness = y,
                    Dashes = new double[] { 1, 2, 3 }
                };
                for (int i = 0; i < 20; i++)
                {
                    line.Points.Add(new DataPoint(i + 1, y));
                }

                model.Series.Add(line);
            }

            return model;
        }

        [Example("ScatterSeries and LinearColorAxis on the same plot (Closed)")]
        public static PlotModel ScatterSeriesAndLinearColorAxis()
        {
            var plotModel = new PlotModel { Title = "ScatterSeries and LinearColorAxis on the same plot" };
            int npoints = 100;
            var random = new Random();

            var scatterSeries = new ScatterSeries { ColorAxisKey = string.Empty };
            for (var i = 0; i < npoints; i++)
            {
                scatterSeries.Points.Add(new ScatterPoint((double)i / npoints, random.NextDouble()));
            }

            plotModel.Series.Add(scatterSeries);

            var lineSeries = new LineSeries();
            for (var i = 0; i < npoints; i++)
            {
                lineSeries.Points.Add(new DataPoint((double)i / npoints, random.NextDouble()));
            }

            plotModel.Series.Add(lineSeries);

            plotModel.Axes.Add(new LinearColorAxis());
            return plotModel;
        }

        [Example("#133: MinorStep should not be MajorStep/5 when MajorStep is 2")]
        public static PlotModel MinorTicks()
        {
            var plotModel1 = new PlotModel { Title = "Issue 10117" };
            plotModel1.Axes.Add(new LinearAxis { Minimum = 0, Maximum = 16 });
            return plotModel1;
        }

        [Example("Scatterseries not rendered at specific plot sizes (closed)")]
        public static PlotModel ScatterSeries()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Scatterseries not rendered at specific plot sizes",
                PlotMargins = new OxyThickness(50, 5, 5, 50),
                Padding = new OxyThickness(0),
                PlotAreaBorderThickness = new OxyThickness(1, 1, 1, 1),
                PlotAreaBorderColor = OxyColors.Black,
                TextColor = OxyColors.Black
            };

            var l = new Legend
            {
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPosition = LegendPosition.TopRight,
                LegendMargin = 0
            };

            plotModel1.Legends.Add(l);

            plotModel1.Axes.Add(new LinearAxis
            {
                IsAxisVisible = true,
                Title = "X",
                Position = AxisPosition.Bottom,
                TickStyle = TickStyle.Outside,
                TicklineColor = OxyColors.Black,
                Minimum = 0,
                MaximumPadding = 0.05
            });
            plotModel1.Axes.Add(new LogarithmicAxis
            {
                MinimumPadding = 0.05,
                MaximumPadding = 0.1,
                Title = "Y",
                Position = AxisPosition.Left,
                TickStyle = TickStyle.Outside,
                TicklineColor = OxyColors.Black,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineStyle = LineStyle.Solid
            });
            var referenceCurve = new LineSeries
            {
                Title = "Reference",
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Color = OxyColor.FromArgb(255, 89, 128, 168)
            };
            var upperBoundary = new LineSeries
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.LightGray,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Title = string.Empty
            };

            var lowerBoundary = new LineSeries
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.LightGray,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                Title = "+/- 15 %"
            };

            // Series that holds and formats points inside of the boundary
            var inBoundaryResultLine = new ScatterSeries
            {
                Title = "actual",
                MarkerFill = OxyColors.Black,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerType = MarkerType.Circle
            };

            // Series that holds and formats points outside of the boundary
            var outBoundaryResultLine = new ScatterSeries
            {
                Title = "not permissible deviation",
                MarkerFill = OxyColors.Red,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerType = MarkerType.Circle
            };

            // Just some random data to fill the series:
            var referenceValues = new[]
            {
                double.NaN, 0.985567558024852, 0.731704530257957, 0.591109071735532, 0.503627816316065, 0.444980686815776,
                0.403576666032678, 0.373234299823915, 0.350375591667333, 0.332795027566349, 0.319063666439909,
                0.30821748743148, 0.299583943726489, 0.292680371378706, 0.287151885046283, 0.282732008216725,
                0.279216923371711, 0.276557880999918
            };
            var actualValues = new[]
            {
                double.NaN, 0.33378346040897, 1.09868427497967, 0.970771068054048, 0.739778217457323, 0.582112938330166,
                0.456962500853806, 0.37488740614826, 0.330272509496142, 0.334461549522006, 0.30989175806678,
                0.286944862053553, 0.255895385950234, 0.231850970296068, 0.217579897050944, 0.217113227224437,
                0.164759946945322, 0.0459134254747994
            };

            for (var index = 0; index <= 17; index++)
            {
                var referenceValue = referenceValues[index];
                var lowerBound = referenceValue - (referenceValue * 0.15);
                var upperBound = referenceValue + (referenceValue * 0.15);
                referenceCurve.Points.Add(new DataPoint(index, referenceValue));
                lowerBoundary.Points.Add(new DataPoint(index, lowerBound));
                upperBoundary.Points.Add(new DataPoint(index, upperBound));

                var actualValue = actualValues[index];
                if (actualValue > lowerBound && actualValue < upperBound)
                {
                    inBoundaryResultLine.Points.Add(new ScatterPoint(index, actualValue));
                }
                else
                {
                    outBoundaryResultLine.Points.Add(new ScatterPoint(index, actualValue));
                }
            }

            plotModel1.Series.Add(referenceCurve);
            plotModel1.Series.Add(lowerBoundary);
            plotModel1.Series.Add(upperBoundary);
            plotModel1.Series.Add(outBoundaryResultLine);
            plotModel1.Series.Add(inBoundaryResultLine);

            return plotModel1;
        }

        [Example("ScatterSeries with invalid point and marker type circle (closed)")]
        public static PlotModel ScatterSeriesWithInvalidPointAndMarkerTypeCircle()
        {
            var plotModel1 = new PlotModel
            {
                Title = "ScatterSeries with invalid point and marker type circle",
            };
            plotModel1.Series.Add(new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                ItemsSource = new[] { new ScatterPoint(0, double.NaN), new ScatterPoint(0, 0) }
            });
            return plotModel1;
        }

        [Example("RectangleBarSeries rendered on top layer (rejected)")]
        public static PlotModel RectangleBarSeriesRenderedOnTopLayer()
        {
            var plotModel1 = new PlotModel
            {
                Title = "RectangleBarSeries rendered on top layer",
            };
            var lineSeries1 = new LineSeries();
            lineSeries1.Points.Add(new DataPoint(0, 1));
            lineSeries1.Points.Add(new DataPoint(1, 0));
            plotModel1.Series.Add(lineSeries1);
            var rectangleBarSeries1 = new RectangleBarSeries();
            rectangleBarSeries1.Items.Add(new RectangleBarItem(0.25, 0.25, 0.75, 0.75));
            plotModel1.Series.Add(rectangleBarSeries1);
            var lineSeries2 = new LineSeries();
            lineSeries2.Points.Add(new DataPoint(0, 0));
            lineSeries2.Points.Add(new DataPoint(1, 1));
            plotModel1.Series.Add(lineSeries2);
            return plotModel1;
        }

        [Example("Legend is not visible (closed)")]
        public static PlotModel LegendIsNotVisible()
        {
            var plotModel = new PlotModel
            {
                Title = "Legend is not visible",
            };
            plotModel.Series.Add(new LineSeries { Title = "LineSeries 1" });
            plotModel.Series.Add(new LineSeries { Title = "LineSeries 2" });
            plotModel.Series.Add(new LineSeries { Title = "LineSeries 3" });
            plotModel.IsLegendVisible = true;
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.RightMiddle,
                LegendOrientation = LegendOrientation.Vertical
            };

            plotModel.Legends.Add(l);
            return plotModel;
        }

        [Example("#189: Wrong position of titles when PositionAtZeroCrossing is true")]
        public static PlotModel PositionAtZeroCrossing()
        {
            var plotModel1 = new PlotModel { PlotType = PlotType.Cartesian, Title = "Zero Crossing Diagram", Subtitle = "The titles should be shown next to the axes" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, AxislineStyle = LineStyle.Solid, PositionAtZeroCrossing = true, Title = "horizontal axis" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, AxislineStyle = LineStyle.Solid, PositionAtZeroCrossing = true, Title = "vertical axis" });
            plotModel1.Series.Add(new FunctionSeries(x => Math.Cos(x * Math.PI / 180.0) * 2, x => Math.Sin(x * Math.PI / 180.0) * 2, 0.0, 180.0, 1.0)
            {
                Color = OxyColors.Red
            });

            return plotModel1;
        }

        [Example("#189: PositionAtZeroCrossing and no plot area border")]
        public static PlotModel PositionAtZeroCrossingNoPlotBorder()
        {
            var pm = PositionAtZeroCrossing();
            pm.PlotAreaBorderThickness = new OxyThickness(0);
            pm.Subtitle = "The axis lines should be drawn when the origin is outside the plot area.";
            return pm;
        }

        [Example("#185: Wrong plot margins when Angle = 90 (LinearAxis)")]
        public static PlotModel PlotMarginsLinearAxisWhenAxisAngleIs90()
        {
            var plotModel1 = new PlotModel { Title = "Plot margins not adjusted correctly when Angle = 90", Subtitle = "The numbers should not be clipped" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Angle = 90, Minimum = 1e8, Maximum = 1e9 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Angle = 90, Minimum = 1e8, Maximum = 1e9 });
            return plotModel1;
        }

        [Example("#185: Wrong plot margins when Angle = 90 (DateTimeAxis)")]
        public static PlotModel PlotMarginsDateTimeAxisWhenAxisAngleIs90()
        {
            var plotModel1 = new PlotModel { Title = "Plot margins not adjusted correctly when Angle = 90", Subtitle = "The numbers should not be clipped" };
            plotModel1.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Angle = 90 });
            plotModel1.Axes.Add(new DateTimeAxis { Position = AxisPosition.Left, Angle = 90 });
            return plotModel1;
        }

        [Example("#180: Two vertical axes on the same position")]
        public static PlotModel TwoVerticalAxisOnTheSamePosition()
        {
            var plotModel1 = new PlotModel { Title = "Two vertical axes on the same position", Subtitle = "The titles should overlap here!" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "First axis" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Second axis" });
            return plotModel1;
        }

        [Example("#180: Two vertical axis on the same position (Start/EndPosition)")]
        public static PlotModel TwoVerticalAxisOnTheSamePositionStartEndPosition()
        {
            var plotModel1 = new PlotModel { Title = "Two vertical axes on the same position with different StartPosition/EndPosition", Subtitle = "The titles should be centered on the axes" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 0, EndPosition = 0.4, Title = "First axis" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 0.6, EndPosition = 1, Title = "Second axis" });
            return plotModel1;
        }

        [Example("#180: Two vertical axis on the same position (PositionTier)")]
        public static PlotModel TwoVerticalAxisOnTheSamePositionStartEndPositionPositionTier()
        {
            var plotModel1 = new PlotModel { Title = "Two vertical axes on the same position with different PositionTier", Subtitle = "The titles should be centered and not overlapping" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, PositionTier = 0, Title = "First axis" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, PositionTier = 1, Title = "Second axis", AxislineStyle = LineStyle.Solid });
            return plotModel1;
        }

        [Example("#220: Tracker strings not correctly showing date/times (closed)")]
        public static PlotModel TrackerStringsNotCorrectlySHowingDateTimes()
        {
            var plotModel1 = new PlotModel { Title = "Tracker strings not correctly showing date/times" };
            plotModel1.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Title = "Date" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Quantity" });
            var ls = new LineSeries { TrackerFormatString = "{1}: {2:d-M-yyyy}\n{3}: {4:0.00}", MarkerType = MarkerType.Circle };
            var t0 = new DateTime(2014, 10, 16);
            for (int i = 0; i < 20; i++)
            {
                ls.Points.Add(new DataPoint(DateTimeAxis.ToDouble(t0.AddDays(i)), 13 + Math.IEEERemainder(i, 7)));
            }

            plotModel1.Series.Add(ls);
            return plotModel1;
        }

        [Example("#226: LineSeries exception when smoothing")]
        public static PlotModel LineSeriesExceptionWhenSmoothing()
        {
            var plotModel1 = new PlotModel
            {
                Title = "LineSeries null reference exception when smoothing is enabled and all datapoints have the same y value",
                Subtitle = "Click on the plot to reproduce the issue."
            };
            var ls = new LineSeries
            {
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
            };
            ls.Points.Add(new DataPoint(0, 0));
            ls.Points.Add(new DataPoint(1, 0));
            ls.Points.Add(new DataPoint(10, 0));
            plotModel1.Series.Add(ls);
            return plotModel1;
        }

        [Example("#79: Center aligned legends (closed)")]
        public static PlotModel CenterAlignedLegends()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Center aligned legends"
            };

            var l = new Legend
            {
                LegendPosition = LegendPosition.BottomCenter,
                LegendItemAlignment = HorizontalAlignment.Center
            };

            plotModel1.Legends.Add(l);

            plotModel1.Series.Add(new LineSeries { Title = "LineSeries 1" });
            plotModel1.Series.Add(new LineSeries { Title = "LS2" });
            return plotModel1;
        }

        [Example("#356: Draw legend line with custom pattern")]
        public static PlotModel LegendWithCustomPattern()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Draw legend line with custom pattern",
            };
            var solid = new LineSeries
            {
                Title = "Solid",
                LineStyle = LineStyle.Solid
                // without dashes
            };
            var custom = new LineSeries
            {
                Title = "Custom",
                LineStyle = LineStyle.Solid,
                // dashd-dot pattern
                Dashes = new[] { 10.0, 2.0, 4.0, 2.0 },
            };
            solid.Points.Add(new DataPoint(0, 2));
            solid.Points.Add(new DataPoint(100, 1));
            custom.Points.Add(new DataPoint(0, 3));
            custom.Points.Add(new DataPoint(100, 2));
            plotModel1.Series.Add(solid);
            plotModel1.Series.Add(custom);

            var l = new Legend
            {
                LegendSymbolLength = 100 // wide enough to see pattern
            };

            plotModel1.Legends.Add(l);
            return plotModel1;
        }

        [Example("#409: ImageAnnotation width width/height crashes")]
        public static PlotModel ImageAnnotationWithWidthHeightCrashes()
        {
            var myModel = new PlotModel { Title = "Example 1" };
            myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            var rng = new Random();
            var buf = new byte[100, 100];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    buf[i, j] = (byte)rng.Next();
                }
            }

            var palette = new OxyColor[256];
            for (int i = 0; i < palette.Length; i++)
            {
                palette[i] = OxyColor.FromArgb(128, (byte)i, 0, 0);
            }

            var image = OxyImage.Create(buf, palette, ImageFormat.Bmp);
            myModel.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,

                X = new PlotLength(1, PlotLengthUnit.Data),
                Y = new PlotLength(0, PlotLengthUnit.Data),
                Width = new PlotLength(1, PlotLengthUnit.Data),
                Height = new PlotLength(1, PlotLengthUnit.Data)
            });

            myModel.Annotations.Add(new ImageAnnotation
            {
                ImageSource = image,

                X = new PlotLength(5, PlotLengthUnit.Data),
                Y = new PlotLength(0, PlotLengthUnit.Data),
            });

            return myModel;
        }

        [Example("#413: HeatMap tracker format string")]
        public static PlotModel HeatMapTrackerFormatString()
        {
            var plotModel1 = new PlotModel
            {
                Title = "HeatMap tracker format string",
            };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StringFormat = "0.000", Minimum = 0, Maximum = 1 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StringFormat = "0.000", Minimum = 0, Maximum = 1 });
            plotModel1.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Minimum = 0, Maximum = 5 });
            var data = new double[,] { { 1, 2 }, { 3, 4 } };
            plotModel1.Series.Add(new HeatMapSeries
            {
                Data = data,
                CoordinateDefinition = HeatMapCoordinateDefinition.Edge,
                X0 = 0.1,
                X1 = 0.9,
                Y0 = 0.1,
                Y1 = 0.9,
                TrackerFormatString = "{0}\n{1}: {2:0.000}\n{3}: {4:0.000}\n{5}: {6:0.0000}"
            });
            return plotModel1;
        }

        [Example("#413: Using axis format strings in tracker")]
        public static PlotModel AxisFormatStringInTracker()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Using axis format strings in tracker",
            };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StringFormat = "0.000", Minimum = 0, Maximum = 1 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StringFormat = "0.000", Minimum = 0, Maximum = 1 });
            plotModel1.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Minimum = 0, Maximum = 5 });
            var data = new double[,] { { 1, 2 }, { 3, 4 } };
            plotModel1.Series.Add(new HeatMapSeries
            {
                Data = data,
                CoordinateDefinition = HeatMapCoordinateDefinition.Edge,
                X0 = 0.1,
                X1 = 0.9,
                Y0 = 0.1,
                Y1 = 0.9,

                // IDEA: add new arguments for axis formatted values
                // TODO: this will throw an exception, argument 7 and 8 is not implemented
                TrackerFormatString = "{0}\n{1}: {7}\n{3}: {8}\n{5}: {6:0.0000}"
            });
            return plotModel1;
        }

        [Example("#408: CategoryAxis label clipped on left margin")]
        public static PlotModel CategoryAxisLabelClipped()
        {
            var plotModel1 = new PlotModel
            {
                Title = "CategoryAxis label clipped on left margin",
            };
            var axis = new CategoryAxis { Position = AxisPosition.Left, Angle = -52 };
            axis.Labels.Add("Very very very very long label");
            axis.Labels.Add("Short label");
            axis.Labels.Add("Short label");
            axis.Labels.Add("Short label");
            plotModel1.Axes.Add(axis);
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            return plotModel1;
        }

        private class TimeValue
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        [Example("#474: Vertical Axis Title Font Bug")]
        public static PlotModel VerticalAxisTitleFontBug()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Vertical Axis Title Font Bug",
            };

            plotModel1.Axes.Add(new LinearAxis
            {
                Title = "X_Axe",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                TitleFont = "Times New Roman"
            });

            plotModel1.Axes.Add(new LinearAxis
            {
                Title = "Y_Axe",
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                TitleFont = "Times New Roman"
            });

            return plotModel1;
        }


        [Example("#535: Transposed HeatMap")]
        public static PlotModel TransposedHeatMap()
        {
            int n = 100;

            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayBuilder.CreateVector(x0, x1, n);
            var yvalues = ArrayBuilder.CreateVector(y0, y1, n);
            var peaksData = ArrayBuilder.Evaluate(peaks, xvalues, yvalues);

            var model = new PlotModel { Title = "Normal Heatmap" };

            model.Axes.Add(
                new LinearAxis() { Key = "x_axis", AbsoluteMinimum = x0, AbsoluteMaximum = x1, Position = AxisPosition.Left });

            model.Axes.Add(
                new LinearAxis() { Key = "y_axis", AbsoluteMinimum = y0, AbsoluteMaximum = y1, Position = AxisPosition.Top });

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries
            {
                X0 = x0,
                X1 = x1,
                Y0 = y0,
                Y1 = y1,
                Data = peaksData,
                XAxisKey = "x_axis",
                YAxisKey = "y_axis"
            };
            model.Series.Add(hms);

            return model;
        }

        [Example("#535: Normal HeatMap")]
        public static PlotModel NormalHeatMap()
        {
            int n = 100;

            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayBuilder.CreateVector(x0, x1, n);
            var yvalues = ArrayBuilder.CreateVector(y0, y1, n);
            var peaksData = ArrayBuilder.Evaluate(peaks, xvalues, yvalues);

            var model = new PlotModel { Title = "Peaks" };

            model.Axes.Add(
    new LinearAxis() { Key = "x_axis", AbsoluteMinimum = x0, AbsoluteMaximum = x1, Position = AxisPosition.Top });

            model.Axes.Add(
                new LinearAxis() { Key = "y_axis", AbsoluteMinimum = y0, AbsoluteMaximum = y1, Position = AxisPosition.Left });

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries
            {
                X0 = x0,
                X1 = x1,
                Y0 = y0,
                Y1 = y1,
                Data = peaksData,
                XAxisKey = "x_axis",
                YAxisKey = "y_axis"
            };
            model.Series.Add(hms);

            return model;
        }

        [Example("#1065: LinearColorAxis Title")]
        public static PlotModel ColorAxisTitle()
        {
            int n = 100;

            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayBuilder.CreateVector(x0, x1, n);
            var yvalues = ArrayBuilder.CreateVector(y0, y1, n);
            var peaksData = ArrayBuilder.Evaluate(peaks, xvalues, yvalues);

            var model = new PlotModel { Title = "Peaks" };

            model.Axes.Add(new LinearAxis() { Key = "x_axis", AbsoluteMinimum = x0, AbsoluteMaximum = x1, Position = AxisPosition.Top });

            model.Axes.Add(new LinearAxis() { Key = "y_axis", AbsoluteMinimum = y0, AbsoluteMaximum = y1, Position = AxisPosition.Left });

            model.Axes.Add(new LinearColorAxis { Title = "wrong Placement", Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries
            {
                X0 = x0,
                X1 = x1,
                Y0 = y0,
                Y1 = y1,
                Data = peaksData,
                XAxisKey = "x_axis",
                YAxisKey = "y_axis"
            };
            model.Series.Add(hms);

            return model;
        }

        /// <summary>
        /// Contains example code for https://github.com/oxyplot/oxyplot/issues/42
        /// </summary>
        /// <returns>The plot model.</returns>
        [Example("#42: ContourSeries not working for not square data array")]
        public static PlotModel IndexOutOfRangeContour()
        {
            var model = new PlotModel { Title = "Issue #42" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(5) });

            var x = ArrayBuilder.CreateVector(0, 1, 20);
            var y = ArrayBuilder.CreateVector(-1, 1, 2);
            var data = ArrayBuilder.Evaluate((a, b) => a * b, x, y);

            var contour = new ContourSeries
            {
                ColumnCoordinates = y,
                RowCoordinates = x,
                Data = data
            };
            model.Series.Add(contour);

            return model;
        }

        [Example("#624: Rendering math text with syntax error gets stuck in an endless loop")]
        public static PlotModel MathTextWithSyntaxError()
        {
            var model = new PlotModel { Title = "Math text syntax errors" };
            model.Series.Add(new LineSeries { Title = "x_{1" });
            model.Series.Add(new LineSeries { Title = "x^{2" });
            model.Series.Add(new LineSeries { Title = "x^{2_{1" });
            model.Series.Add(new LineSeries { Title = "x^{ x^" });
            model.Series.Add(new LineSeries { Title = "x_{ x_" });
            model.Series.Add(new LineSeries { Title = "" });
            model.Series.Add(new LineSeries { Title = "x^{ x_{ x^_" });
            return model;
        }

        [Example("#19: The minimum value is not mentioned on the axis I")]
        public static PlotModel MinimumValueOnAxis()
        {
            var model = new PlotModel { Title = "Show minimum and maximum values on axis" };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                //ShowMinimumValue = true,
                //ShowMaximumValue = true,
                //MinimumValueStringFormat = "0.###",
                //MaximumValueStringFormat = "0.###",
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                //ShowMinimumValue = true,
                //ShowMaximumValue = true,
                //MinimumValueStringFormat = "0.###",
                //MaximumValueStringFormat = "0.###",
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(0.14645, 0.14645));
            ls.Points.Add(new DataPoint(9.85745, 9.85745));
            model.Series.Add(ls);
            return model;
        }

        [Example("#19: The minimum value is not mentioned on the axis II")]
        public static PlotModel MinimumValueOnAxis2()
        {
            var model = new PlotModel { Title = "Show minimum and maximum values on axis" };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                //ShowMinimumValue = true,
                //ShowMaximumValue = true,
                //MinimumValueStringFormat = "0.###",
                //MaximumValueStringFormat = "0.###",
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                //ShowMinimumValue = true,
                //ShowMaximumValue = true,
                //MinimumValueStringFormat = "0.###",
                //MaximumValueStringFormat = "0.###",
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(-0.14645, -0.14645));
            ls.Points.Add(new DataPoint(10.15745, 10.15745));
            model.Series.Add(ls);
            return model;
        }

        [Example("#550: MinimumRange with Minimum")]
        public static PlotModel MinimumRangeWithMinimum()
        {
            var model = new PlotModel { Title = "MinimumRange of 500 with a Minimum of 50", Subtitle = "Should initially show a range from 50 to 550." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 50,
                    MinimumRange = 500
                });

            return model;
        }


        [Example("#710: MinimumRange and MaximumRange with Minimum")]
        public static PlotModel MinimumRangeAndMaximumRangeWithMinimum()
        {
            var model = new PlotModel { Title = "MinimumRange of 5 and MaximumRange of 200 with a Minimum of 0", Subtitle = "Should show a range from 0 to 5 minimum and a range of 200 maximum." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    MinimumRange = 5,
                    MaximumRange = 200
                });

            return model;
        }

        [Example("#711: MinimumRange with AbsoluteMinimum")]
        public static PlotModel MinimumRangeWithAbsoluteMinimum()
        {
            var model = new PlotModel { Title = "MinimumRange of 500 with a AbsoluteMinimum of 50", Subtitle = "Should initially show a range from 50 to 550. It should not be possible to pan below 50." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    AbsoluteMinimum = 50,
                    MinimumRange = 500
                });

            return model;
        }

        [Example("#711: MinimumRange with AbsoluteMaximum")]
        public static PlotModel MinimumRangeWithAbsoluteMaximum()
        {
            var model = new PlotModel { Title = "MinimumRange of 500 with a AbsoluteMaximum of 200", Subtitle = "Should initially show a range from -300 to 200. It should not be possible to pan above 200." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    AbsoluteMaximum = 200,
                    MinimumRange = 500
                });

            return model;
        }

        [Example("#711: MaximumRange with AbsoluteMinimum")]
        public static PlotModel MaximumRangeWithAbsoluteMinimum()
        {
            var model = new PlotModel { Title = "MaximumRange of 50 with a AbsoluteMinimum of 20", Subtitle = "Should initially show a range from 20 to 70. It should not be possible to pan below 20." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    AbsoluteMinimum = 20,
                    MaximumRange = 50
                });

            return model;
        }

        [Example("#711: MaximumRange with AbsoluteMaximum")]
        public static PlotModel MaximumRangeWithAbsoluteMaximum()
        {
            var model = new PlotModel { Title = "MaximumRange of 25 with a AbsoluteMaximum of -20", Subtitle = "Should initially show a range from -45 to -20. It should not be possible to pan above -20." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    AbsoluteMaximum = -20,
                    MaximumRange = 25
                });

            return model;
        }

        [Example("#745: HeatMap not working in Windows Universal")]
        public static PlotModel PlotHeatMap()
        {
            var model = new PlotModel { Title = "FOOBAR" };
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(500),
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black
            });

            var data = new double[,] { { 1, 2 }, { 1, 1 }, { 2, 1 }, { 2, 2 } };

            var hs = new HeatMapSeries
            {
                Background = OxyColors.Red,
                X0 = 0,
                X1 = 2,
                Y0 = 0,
                Y1 = 3,
                Data = data,
            };
            model.Series.Add(hs);
            return model;
        }

        [Example("#758: IntervalLength = 0")]
        public static PlotModel IntervalLength0()
        {
            var model = new PlotModel
            {
                Title = "IntervalLength = 0",
                Subtitle = "An exception should be thrown. Should not go into infinite loop."
            };
            model.Axes.Add(new LinearAxis { IntervalLength = 0 });
            return model;
        }

        [Example("#737: Wrong axis line when PositionAtZeroCrossing = true")]
        public static PlotModel WrongAxisLineWhenPositionAtZeroCrossingIsSet()
        {
            var model = new PlotModel { Title = "PositionAtZeroCrossing" };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left
                });
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    PositionAtZeroCrossing = true,
                    AxislineStyle = LineStyle.Solid,
                    AxislineThickness = 1
                });
            var lineSeries = new LineSeries();
            lineSeries.Points.Add(new DataPoint(-10, 10));
            lineSeries.Points.Add(new DataPoint(0, -10));
            lineSeries.Points.Add(new DataPoint(10, 10));
            model.Series.Add(lineSeries);
            return model;
        }

        [Example("#727: Axis Min/Max ignored")]
        public static PlotModel AxisMinMaxIgnored()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Axes min/max ignored",
                PlotType = PlotType.Cartesian,
            };
            var ls = new LineSeries();
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 866, Key = "Horizontal" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 103, Maximum = 37141, Key = "Vertical" });
            ls.XAxisKey = "Horizontal";
            ls.YAxisKey = "Vertical";
            plotModel1.Series.Add(ls);

            return plotModel1;
        }

        [Example("#727: Axis Min/Max")]
        public static PlotModel AxisMinMax()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Axes min/max",
            };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 866 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 103, Maximum = 37141 });

            return plotModel1;
        }

        /// <summary>
        /// Creates a demo PlotModel with MinimumRange defined
        /// and with series with values which are within this range.
        /// </summary>
        /// <returns>The created PlotModel</returns>
        [Example("#794: Axis alignment when MinimumRange is set")]
        public static PlotModel MinimumRangeTest()
        {
            var model = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
            };

            model.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));
            series.Points.Add(new DataPoint(4, 10.1));

            model.Series.Add(series);

            return model;
        }

        [Example("#72: Smooth")]
        public static PlotModel Smooth()
        {
            var model = new PlotModel { Title = "LineSeries with Smooth = true (zoomed in)" };
            var l = new Legend
            {
                LegendSymbolLength = 24 
            };

            model.Legends.Add(l);

            var s1 = new LineSeries { InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline };
            s1.Points.Add(new DataPoint(0, 0));
            s1.Points.Add(new DataPoint(10, 2));
            s1.Points.Add(new DataPoint(40, 1));
            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 10.066564180257437, Maximum = 10.081628088306001 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 2.0013430243084067, Maximum = 2.00209808854281 });
            return model;
        }

        [Example("#880: Too much padding")]
        public static PlotModel TooMuchPadding()
        {
            return new PlotModel { Title = "Too much padding", Padding = new OxyThickness(0, 0, 0, 10000) };
        }

        [Example("#880: Too much padding with legend outside")]
        public static PlotModel TooMuchPaddingWithLegend()
        {
            var model = new PlotModel
            {
                Title = "Too much padding with legend outside",
                Padding = new OxyThickness(500)
            };

            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            model.Legends.Add(l);
            model.Series.Add(new LineSeries { Title = "Series 1" });
            model.Series.Add(new LineSeries { Title = "Series 2" });
            return model;
        }

        [Example("#880: Too much title padding")]
        public static PlotModel TooMuchTitlePadding()
        {
            var model = new PlotModel { Title = "Too much title padding", TitlePadding = 10000 };
            return model;
        }

        /// <summary>
        /// Creates a demo PlotModel with MinimumRange defined
        /// and with series with values which are within this range.
        /// </summary>
        /// <returns>The created PlotModel</returns>
        [Example("#794: Axis alignment when MinimumRange is set with AbsoluteMaximum")]
        public static PlotModel MinimumRangeAbsoluteMaximumTest()
        {
            var model = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
                AbsoluteMaximum = 10.5
            };

            model.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));
            series.Points.Add(new DataPoint(4, 10.1));

            model.Series.Add(series);

            return model;
        }

        /// <summary>
        /// Creates a demo PlotModel with MinimumRange defined
        /// and with series with values which are within this range.
        /// </summary>
        /// <returns>The created PlotModel</returns>
        [Example("#794: Axis alignment when MinimumRange is set with AbsoluteMinimum")]
        public static PlotModel MinimumRangeAbsoluteMinimumTest()
        {
            var model = new PlotModel();
            var yaxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MinimumRange = 1,
                AbsoluteMinimum = 10,
            };

            model.Axes.Add(yaxis);

            var series = new LineSeries();
            series.Points.Add(new DataPoint(0, 10.1));
            series.Points.Add(new DataPoint(1, 10.15));
            series.Points.Add(new DataPoint(2, 10.3));
            series.Points.Add(new DataPoint(3, 10.25));
            series.Points.Add(new DataPoint(4, 10.1));

            model.Series.Add(series);

            return model;
        }

        /// <summary>
        /// Creates a demo PlotModel with the data from the issue.
        /// </summary>
        /// <returns>The created PlotModel</returns>
        [Example("#589: LogarithmicAxis glitches with multiple series containing small data")]
        public static PlotModel LogaritmicAxesSuperExponentialFormatTest()
        {
            var model = new PlotModel();
            model.Axes.Add(new LogarithmicAxis
            {
                UseSuperExponentialFormat = true,
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Dot,
                PowerPadding = true
            });

            model.Axes.Add(new LogarithmicAxis
            {
                UseSuperExponentialFormat = true,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dot,
                PowerPadding = true
            });

            var series1 = new LineSeries();
            series1.Points.Add(new DataPoint(1e5, 1e-14));
            series1.Points.Add(new DataPoint(4e7, 1e-12));
            model.Series.Add(series1);

            return model;
        }

        /// <summary>
        /// Attempts to create a logarithmic axis starting at 1 and going to 0.
        /// </summary>
        /// <returns>The plot model.</returns>
        [Example("#925: WPF app freezes when LogarithmicAxis is reversed.")]
        public static PlotModel LogarithmicAxisReversed()
        {
            var model = new PlotModel();
            model.Axes.Add(new LogarithmicAxis { StartPosition = 1, EndPosition = 0 });

            return model;
        }

        [Example("#1029: LineAnnotation (loglin axes)")]
        public static PlotModel Issue1029LogLin()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Possible Infinite Loop in LineAnnotation.GetPoints() when Minimum=Maximum",
            };
            plotModel1.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 10 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 8 });
            plotModel1.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MinimumY = 2, MaximumY = 2 });
            plotModel1.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MinimumX = 2, MaximumX = 2 });
            return plotModel1;
        }

        [Example("#1029: LineAnnotation (linlin axes)")]
        public static PlotModel Issue1029LinLin()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Possible Infinite Loop in LineAnnotation.GetPoints() when Minimum=Maximum",
            };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 10 });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 8 });
            plotModel1.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, X = 4, MinimumY = 2, MaximumY = 2 });
            plotModel1.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, Y = 2, MinimumX = 2, MaximumX = 2 });
            return plotModel1;
        }

        /// <summary>
        /// Creates a plot model as described in issue 1090.
        /// </summary>
        /// <returns>The plot model.</returns>
        [Example("#1090: Overflow when zoomed in on logarithmic scale")]
        public static PlotModel Issue1090()
        {
            var plotModel = new PlotModel();
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, AbsoluteMinimum = 0 });
            plotModel.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left });

            return plotModel;
        }

        [Example("#1132: ScatterSeries with TimeSpanAxis")]
        public static PlotModel ScatterSeriesWithTimeSpanAxis()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new TimeSpanAxis { Position = AxisPosition.Bottom });
            plotModel1.Axes.Add(new TimeSpanAxis { Position = AxisPosition.Left });

            var points = new[]
                             {
                                 new TimeSpanPoint { X = TimeSpan.FromSeconds(0), Y = TimeSpan.FromHours(1) },
                                 new TimeSpanPoint { X = TimeSpan.FromSeconds(0), Y = TimeSpan.FromHours(1) }
                             };

            plotModel1.Series.Add(new ScatterSeries { ItemsSource = points, DataFieldX = "X", DataFieldY = "Y" });

            return plotModel1;
        }

        [Example("#1132: ScatterSeries with DateTimeAxis")]
        public static PlotModel ScatterSeriesWithDateTimeAxis()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });
            plotModel1.Axes.Add(new DateTimeAxis { Position = AxisPosition.Left });

            var points = new[]
                             {
                                 new DateTimePoint { X = new DateTime(2017,10,10), Y = new DateTime(2017,10,11) },
                                 new DateTimePoint { X = new DateTime(2017,1,1), Y = new DateTime(2018,1,1) }
                             };

            plotModel1.Series.Add(new ScatterSeries { ItemsSource = points, DataFieldX = "X", DataFieldY = "Y" });

            return plotModel1;
        }

        [Example("#1160: Exporting TextAnnotation with transparent TextColor to SVG produces opaque text")]
        public static PlotModel ExportTransparentTextAnnotationToSvg()
        {
            var plot = new PlotModel();
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plot.Background = OxyColors.Black;
            plot.Annotations.Add(new TextAnnotation
            {
                TextPosition = new DataPoint(25, 0),
                Text = "Opaque",
                TextColor = OxyColor.FromRgb(255, 0, 0),
                FontSize = 10,
            });
            plot.Annotations.Add(new TextAnnotation
            {
                TextPosition = new DataPoint(25, 20),
                Text = "Semi transparent",
                TextColor = OxyColor.FromArgb(125, 255, 0, 0),
                FontSize = 10,
            });
            plot.Annotations.Add(new TextAnnotation
            {
                TextPosition = new DataPoint(25, 40),
                Text = "Transparent1",
                TextColor = OxyColor.FromArgb(0, 255, 0, 0),
                FontSize = 10,
            });
            plot.Annotations.Add(new TextAnnotation
            {
                TextPosition = new DataPoint(25, 60),
                Text = "Transparent2",
                TextColor = OxyColors.Transparent,
                FontSize = 10,
            });

            return plot;
        }

        [Example("#1312: Annotations ignore LineStyle.None and draw as if Solid")]
        public static PlotModel DrawArrowAnnotationsWithDifferentLineStyles()
        {
            LineStyle[] lineStyles = new[]
            {
                LineStyle.Solid,
                LineStyle.Dash,
                LineStyle.Dot,
                LineStyle.DashDot,
                LineStyle.DashDashDot,
                LineStyle.DashDotDot,
                LineStyle.DashDashDotDot,
                LineStyle.LongDash,
                LineStyle.LongDashDotDot,
                LineStyle.None,
                LineStyle.Automatic
            };

            var plot = new PlotModel() { Title = "Annotation Line Styles", Subtitle = "'None' should produce nothing" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = lineStyles.Length * 10 + 10 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });

            double y = 10;
            foreach (var lineStyle in lineStyles)
            {
                plot.Annotations.Add(new LineAnnotation()
                {
                    LineStyle = lineStyle,
                    Type = LineAnnotationType.Horizontal,
                    Y = y,
                    MinimumX = 10,
                    MaximumX = 45
                });

                plot.Annotations.Add(new ArrowAnnotation()
                {
                    LineStyle = lineStyle,
                    Text = lineStyle.ToString(),
                    TextHorizontalAlignment = HorizontalAlignment.Center,
                    TextVerticalAlignment = VerticalAlignment.Bottom,
                    TextPosition = new DataPoint(50, y),
                    StartPoint = new DataPoint(55, y),
                    EndPoint = new DataPoint(90, y)
                });

                y += 10;
            }

            return plot;
        }

        [Example("#1385: GraphicsRenderContext sometimes clips last line of text")]
        public static PlotModel DrawMultilineText()
        {
            var plot = new PlotModel() { Title = "GraphicsRenderContext\nsometimes clips last\nline of text" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });

            double y = 0.0;
            for (int i = 2; i < 50; i++)
            {
                plot.Annotations.Add(new TextAnnotation() { Text = "GraphicsRenderContext\nsometimes clips last\nline of text", TextPosition = new DataPoint(50, y += i), FontSize = i });
            }

            return plot;
        }

        [Example("#1441: Zero crossing quadrant axes disappear on zoom and pan (Closed)")]
        public static PlotModel ZeroCrossingQuadrantAxes()
        {
            var plot = new PlotModel() { Title = "Zoom or Pan axes to make them disappear" };

            var xaxis = new LinearAxis
            {
                Position = AxisPosition.Top,
                PositionTier = 0,
                AxislineStyle = LineStyle.Solid,
                Minimum = 0,
                AxislineColor = OxyColors.Red,
                StartPosition = 0.5,
                EndPosition = 1,
                PositionAtZeroCrossing = true,
                AbsoluteMinimum = 0,
                Title = "ABC",

            };
            plot.Axes.Add(xaxis);

            var xaxis2 = new LinearAxis
            {
                Position = AxisPosition.Top,
                PositionTier = 0,
                AxislineStyle = LineStyle.Solid,
                Minimum = 0,
                AxislineColor = OxyColors.Gold,
                StartPosition = 0.5,
                EndPosition = 0,
                PositionAtZeroCrossing = true,
                IsAxisVisible = true,
                AbsoluteMinimum = 0,
                Title = "DCS",

            };
            plot.Axes.Add(xaxis2);

            var yaxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.Solid,
                Minimum = 0,
                AxislineColor = OxyColors.Green,
                PositionTier = 0,
                StartPosition = 0.5,
                EndPosition = 1,
                PositionAtZeroCrossing = true,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 1000,
                Title = "DSDC",

            };
            plot.Axes.Add(yaxis);

            var yaxis2 = new LinearAxis
            {
                Position = AxisPosition.Right,
                AxislineStyle = LineStyle.Solid,
                Minimum = 0,
                AxislineColor = OxyColors.Violet,
                PositionTier = 0,
                StartPosition = 0.5,
                EndPosition = 0,
                PositionAtZeroCrossing = true,
                AbsoluteMinimum = 0,

            };
            plot.Axes.Add(yaxis2);

            plot.PlotAreaBorderThickness = new OxyThickness(1, 1, 1, 1);

            return plot;
        }

        [Example("#1524: HitTracker IndexOutOfRangeException with HeatMapSeries")]
        public static PlotModel HitTrackerIndexOutOfRangeExceptionWithHeatMapSeries()
        {
            List<string> xAxisLabels = new List<string>() { "1", "2", "3", "4", "5", "1", "2", "3", "4", "5" };
            List<string> yAxisLabels = new List<string>() { "1", "2", "3", "4", "5", "1", "2", "3", "4", "5" };

            var plot = new PlotModel() { Title = "Place the tracker to the far top or right of the series\nwith a small window size." };

            List<OxyColor> jetTransparent = new List<OxyColor>();
            foreach (var item in OxyPalettes.Jet(1000).Colors)
            {
                jetTransparent.Add(OxyColor.FromAColor(220, item));
            }
            OxyPalette myPalette = new OxyPalette(jetTransparent);

            plot.PlotAreaBackground = OxyColors.White;
            plot.DefaultFontSize = 14;
            //plotModel.DefaultColors = SeriesColors;
            plot.Padding = new OxyThickness(0);
            plot.TitlePadding = 0;


            plot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "HorizontalAxis",
                ItemsSource = xAxisLabels,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                Angle = -45,
                FontSize = 14,
                TitleFontSize = 14,
                AxisTitleDistance = 0
            });

            plot.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "VerticalAxis",
                ItemsSource = yAxisLabels,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineStyle = LineStyle.Solid,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                FontSize = 14,
                TitleFontSize = 14
            });

            // Color axis
            plot.Axes.Add(new LinearColorAxis()
            {
                Palette = myPalette,
            });

            var rand = new Random();
            var data = new double[yAxisLabels.Count, xAxisLabels.Count];
            for (int x = 0; x < xAxisLabels.Count; ++x)
            {
                for (int y = 0; y < yAxisLabels.Count; ++y)
                {
                    data[y, x] = rand.Next(0, 200) * (0.13876876848794587508758879 * (y + 1));
                }
            }

            var heatMapSeries = new HeatMapSeries
            {
                X0 = 0,
                X1 = xAxisLabels.Count - 1,
                Y0 = 0,
                Y1 = yAxisLabels.Count - 1,
                XAxisKey = "HorizontalAxis",
                YAxisKey = "VerticalAxis",
                RenderMethod = HeatMapRenderMethod.Bitmap,
                Interpolate = true,
                Data = data,
                TrackerFormatString = "{3}: {4}\n{1}: {2}\n{5}: {6:N1}%",
            };

            plot.Series.Add(heatMapSeries);

            return plot;
        }

        [Example("#1545: Some render contexts do not support unix line endings.")]
        public static PlotModel UnixLineEndings()
        {
            var plot = new PlotModel() { Title = "Some render contexts\r\ndo not support\nunix line endings." };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });

            plot.Annotations.Add(new TextAnnotation() { Text = "CRLF\r\nLine\r\nEndings", TextPosition = new DataPoint(16, 50), FontSize = 12 });
            plot.Annotations.Add(new TextAnnotation() { Text = "LF\nLine\nEndings", TextPosition = new DataPoint(50, 50), FontSize = 12 });
            plot.Annotations.Add(new TextAnnotation() { Text = "Mixed\r\nLine\nEndings", TextPosition = new DataPoint(84, 50), FontSize = 12 });

            return plot;
        }

        [Example("#1481: Emoji text.")]
        public static PlotModel EmojiText()
        {
            var plot = new PlotModel() { Title = "🖊 Emoji plot 📈" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100 });

            plot.Annotations.Add(new TextAnnotation() { Text = "0 ❗ 1 📊 2 ❗ 3 📊 4 ❗ 5 📊 6 ❗ 7 📊 8 ❗ 9 📊", TextPosition = new DataPoint(50, 80), FontSize = 12 });
            plot.Annotations.Add(new TextAnnotation() { Text = "0 ❗ 1 📊 2 ❗ 3 📊 4 ❗ 5 📊 6 ❗ 7 📊 8 ❗ 9 📊", TextPosition = new DataPoint(50, 50), FontSize = 12, TextRotation = 45 });

            return plot;
        }

        [Example("#1512: FindWindowStartIndex.")]
        public static PlotModel FindWindowsStartIndex()
        {
            var plotModel1 = new PlotModel { Title = "AreaSeries broken in time" };
            var axis = new LinearAxis {Position = AxisPosition.Left, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0};
            var xAxis = new LinearAxis() {Position = AxisPosition.Bottom, Minimum = 4};
            
            plotModel1.Axes.Add(axis);
            plotModel1.Axes.Add(xAxis);

            var N = 15;
            var random = new Random(6);
            var currentValue = random.NextDouble() - 0.5;
            var areaSeries = new AreaSeries();
            for (int i = 0; i < N; ++i)
            {
                if (random.Next(4) == 0)
                {
                    areaSeries.Points.Add(DataPoint.Undefined);
                    areaSeries.Points2.Add(DataPoint.Undefined);
                }
                    
                currentValue += random.NextDouble();
                areaSeries.Points.Add(new DataPoint(currentValue, currentValue));
                areaSeries.Points2.Add(new DataPoint(currentValue, currentValue));
            }
            
            plotModel1.Series.Add(areaSeries);
          

            return plotModel1;
        }

        [Example("#1441: Near-border axis line clipping (Closed)")]
        public static PlotModel ZeroCrossingWithInsertHorizontalAxisAndTransparentBorder()
        {
            var plotModel1 = new PlotModel
            {
                Title = "PositionAtZeroCrossing = true",
                Subtitle = "Horizontal axis StartPosition = 0.1 End Position = 0.9",
                PlotAreaBorderThickness = new OxyThickness(5),
                PlotAreaBorderColor = OxyColor.FromArgb(127, 127, 127, 127),
                PlotMargins = new OxyThickness(10, 10, 10, 10)
            };
            plotModel1.Axes.Add(new LinearAxis
            {
                Minimum = -100,
                Maximum = 100,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.Crossing
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Minimum = -100,
                Maximum = 100,
                Position = AxisPosition.Bottom,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.Crossing,
                StartPosition = 0.9,
                EndPosition = 0.1
            });

            var scatter = new ScatterSeries();
            var rnd = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                scatter.Points.Add(new ScatterPoint(rnd.NextDouble() * 100 - 50, rnd.NextDouble() * 100 - 50));
            }
            plotModel1.Series.Add(scatter);

            return plotModel1;
        }

        private class TimeSpanPoint
        {
            public TimeSpan X { get; set; }
            public TimeSpan Y { get; set; }
        }

        private class DateTimePoint
        {
            public DateTime X { get; set; }
            public DateTime Y { get; set; }
        }
        /* NEW ISSUE TEMPLATE
           [Example("#123: Issue Description")]
           public static PlotModel IssueDescription()
           {
               var plotModel1 = new PlotModel
               {
                   Title = "",
               };

               return plotModel1;
           }
           */
    }
}
