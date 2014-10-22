// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Issues.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Grids the lines both different colors.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z1 Issues")]
    public class Issues
    {
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
                Smooth = true
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
                LegendBackground = OxyColor.FromArgb(200, 255, 255, 255),
                LegendBorder = OxyColors.Black,
                LegendPlacement = LegendPlacement.Outside,
                PlotAreaBackground = OxyColors.Gray,
                PlotAreaBorderColor = OxyColors.Gainsboro,
                PlotAreaBorderThickness = new OxyThickness(2),
                Title = "Value / Time"
            };
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

        [Example("#79: LegendItemAlignment = Center")]
        public static PlotModel LegendItemAlignmentCenter()
        {
            var plotModel1 = new PlotModel { Title = "LegendItemAlignment = Center" };
            plotModel1.LegendItemAlignment = HorizontalAlignment.Center;
            plotModel1.LegendBorder = OxyColors.Black;
            plotModel1.LegendBorderThickness = 1;
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
            plotModel1.Series.Add(new LineSeries { Smooth = true });
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
                TextColor = OxyColors.Black,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPosition = LegendPosition.TopRight,
                LegendMargin = 0
            };
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
                Smooth = true,
                Color = OxyColor.FromArgb(255, 89, 128, 168)
            };
            var upperBoundary = new LineSeries
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.LightGray,
                Smooth = true,
                Title = string.Empty
            };

            var lowerBoundary = new LineSeries
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.LightGray,
                Smooth = true,
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
            plotModel.LegendPlacement = LegendPlacement.Inside;
            plotModel.LegendPosition = LegendPosition.RightMiddle;
            plotModel.LegendOrientation = LegendOrientation.Vertical;
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

        [Example("#220: Tracker strings not correctly showing date/times")]
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
    }
}