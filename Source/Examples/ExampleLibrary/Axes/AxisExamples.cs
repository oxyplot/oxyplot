// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for general axis properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Provides examples for general axis properties.
    /// </summary>
    [Examples("Axis examples"), Tags("Axes")]
    public class AxisExamples
    {
        /// <summary>
        /// Creates an example for the <see cref="Axis.TickStyle" /> property using <see cref="TickStyle.None" />.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("TickStyle: None")]
        public static PlotModel TickStyleNone()
        {
            return CreateTickStyleExample(TickStyle.None);
        }

        [Example("TickStyle: Inside")]
        public static PlotModel TickStyleInside()
        {
            return CreateTickStyleExample(TickStyle.Inside);
        }

        [Example("TickStyle: Crossing")]
        public static PlotModel TickStyleCrossing()
        {
            return CreateTickStyleExample(TickStyle.Crossing);
        }

        [Example("TickStyle: Outside")]
        public static PlotModel TickStyleOutside()
        {
            return CreateTickStyleExample(TickStyle.Outside);
        }

        [Example("TickStyle: Color major and minor ticks differently")]
        public static PlotModel TickLineColor()
        {
            var plotModel1 = new PlotModel { Title = "Color major and minor ticks differently" };
            plotModel1.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                TicklineColor = OxyColors.Blue,
                MinorTicklineColor = OxyColors.Gray,
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineThickness = 3,
                MinorGridlineThickness = 3,
                TicklineColor = OxyColors.Blue,
                MinorTicklineColor = OxyColors.Gray,
            });
            return plotModel1;
        }

        [Example("GridLinestyle: None (default)")]
        public static PlotModel GridlineStyleNone()
        {
            var plotModel1 = new PlotModel { Title = "No gridlines" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            return plotModel1;
        }

        [Example("GridLinestyle: Vertical")]
        public static PlotModel GridLinestyleVertical()
        {
            var plotModel1 = new PlotModel { Title = "Vertical gridlines" };
            plotModel1.Axes.Add(new LinearAxis());
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom
            });
            return plotModel1;
        }

        [Example("GridLinestyle: Horizontal")]
        public static PlotModel GridLinestyleHorizontal()
        {
            var plotModel1 = new PlotModel { Title = "Horizontal gridlines" };
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            return plotModel1;
        }

        [Example("GridLinestyle: Horizontal and vertical")]
        public static PlotModel GridLinestyleBoth()
        {
            var plotModel1 = new PlotModel { Title = "Horizontal and vertical gridlines" };
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom
            });
            return plotModel1;
        }

        [Example("Axis position left/bottom")]
        public static PlotModel AxisPositionLeftAndBottom()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Left"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom,
                Title = "Bottom"
            });
            return plotModel1;
        }

        [Example("Axis position top/right")]
        public static PlotModel AxisPositionTopRight()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Right,
                Title = "Right"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Top,
                Title = "Top"
            });
            return plotModel1;
        }

        [Example("Axis label angle 45deg")]
        public static PlotModel AxisAngle45()
        {
            var plotModel1 = new PlotModel { PlotMargins = new OxyThickness(60, 40, 60, 30) };
            plotModel1.Axes.Add(new LinearAxis
            {
                Angle = 45,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Left"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Angle = 45,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom,
                Title = "Bottom"
            });
            return plotModel1;
        }

        [Example("Zero crossing axis")]
        public static PlotModel ZeroCrossing()
        {
            var plotModel1 = new PlotModel
            {
                Title = "PositionAtZeroCrossing = true",
                PlotAreaBorderThickness = new OxyThickness(0),
                PlotMargins = new OxyThickness(10, 10, 10, 10)
            };
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 50,
                Minimum = -30,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.Crossing
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 70,
                Minimum = -50,
                Position = AxisPosition.Bottom,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                TickStyle = TickStyle.Crossing
            });
            return plotModel1;
        }

        [Example("Horizontal zero crossing axis")]
        public static PlotModel HorizontalZeroCrossing()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Bottom axis: PositionAtZeroCrossing = true"
            };
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 50,
                Minimum = -30,
                Position = AxisPosition.Left,
                PositionAtZeroCrossing = false,
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 70,
                Minimum = -50,
                Position = AxisPosition.Bottom,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
            });
            return plotModel1;
        }

        [Example("Vertical zero crossing axis")]
        public static PlotModel VerticalZeroCrossing()
        {
            var plotModel1 = new PlotModel
            {
                Title = "Left axis: PositionAtZeroCrossing = true"
            };
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 50,
                Minimum = -30,
                Position = AxisPosition.Left,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 70,
                Minimum = -50,
                Position = AxisPosition.Bottom,
                PositionAtZeroCrossing = false,
            });
            return plotModel1;
        }

        [Example("Reversed")]
        public static PlotModel Reversed()
        {
            var plotModel1 = new PlotModel { Title = "EndPosition = 0, StartPosition = 1" };
            plotModel1.Axes.Add(new LinearAxis
            {
                EndPosition = 0,
                StartPosition = 1,
                Maximum = 50,
                Minimum = -30,
                Position = AxisPosition.Left
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                EndPosition = 0,
                StartPosition = 1,
                Maximum = 70,
                Minimum = -50,
                Position = AxisPosition.Bottom
            });
            return plotModel1;
        }

        [Example("Sharing Y axis")]
        public static PlotModel SharingY()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis
            {
                EndPosition = 0,
                StartPosition = 1,
                Maximum = 1.5,
                Minimum = -1.5,
                Position = AxisPosition.Left
            });

            var x1 = new LinearAxis
            {
                StartPosition = 0,
                EndPosition = 0.45,
                Maximum = 7,
                Minimum = 0,
                Position = AxisPosition.Bottom,
                Key = "x1"
            };
            plotModel1.Axes.Add(x1);

            var x2 = new LinearAxis
            {
                StartPosition = 0.55,
                EndPosition = 1,
                Maximum = 10,
                Minimum = 0,
                Position = AxisPosition.Bottom,
                Key = "x2"
            };
            plotModel1.Axes.Add(x2);

            plotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 1000) { XAxisKey = x1.Key });
            plotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 1000) { XAxisKey = x2.Key });

            return plotModel1;
        }

        [Example("Four axes")]
        public static PlotModel FourAxes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis { Maximum = 36, Minimum = 0, Title = "km/h" });
            plotModel1.Axes.Add(new LinearAxis { Maximum = 10, Minimum = 0, Position = AxisPosition.Right, Title = "m/s" });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 10,
                Minimum = 0,
                Position = AxisPosition.Bottom,
                Title = "meter"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 10000,
                Minimum = 0,
                Position = AxisPosition.Top,
                Title = "millimeter"
            });
            return plotModel1;
        }

        [Example("Five axes")]
        public static PlotModel FiveAxes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis { EndPosition = 0.25, Maximum = 1, Minimum = -1, Title = "C1" });
            plotModel1.Axes.Add(new LinearAxis
            {
                EndPosition = 0.5,
                Maximum = 1,
                Minimum = -1,
                Position = AxisPosition.Right,
                StartPosition = 0.25,
                Title = "C2"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                EndPosition = 0.75,
                Maximum = 1,
                Minimum = -1,
                StartPosition = 0.5,
                Title = "C3"
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 1,
                Minimum = -1,
                Position = AxisPosition.Right,
                StartPosition = 0.75,
                Title = "C4"
            });
            plotModel1.Axes.Add(new LinearAxis { Maximum = 100, Minimum = 0, Position = AxisPosition.Bottom, Title = "s" });
            return plotModel1;
        }

        [Example("Logarithmic axes")]
        public static PlotModel LogarithmicAxes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LogarithmicAxis
            {
                Maximum = 1000000,
                Minimum = 1,
                Title = "Log axis",
                UseSuperExponentialFormat = true
            });
            plotModel1.Axes.Add(new LogarithmicAxis
            {
                Maximum = 10000,
                Minimum = 0.001,
                Position = AxisPosition.Bottom,
                Title = "Log axis",
                UseSuperExponentialFormat = true
            });
            return plotModel1;
        }

        [Example("Big numbers")]
        public static PlotModel BigNumbers()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis { Maximum = 6E+32, Minimum = -1E+47, Title = "big numbers" });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 3E+50,
                Minimum = -1E+40,
                Position = AxisPosition.Bottom,
                Title = "big numbers"
            });
            return plotModel1;
        }

        [Example("Big numbers with super exponential format")]
        public static PlotModel BigNumbersSuperExponentialFormat()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 6E+32,
                Minimum = -1E+47,
                Title = "big numbers",
                UseSuperExponentialFormat = true
            });

            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 3E+50,
                Minimum = -1E+40,
                Position = AxisPosition.Bottom,
                Title = "big numbers",
                UseSuperExponentialFormat = true
            });
            return plotModel1;
        }

        [Example("Small numbers")]
        public static PlotModel SmallNumbers()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Axes.Add(new LinearAxis { Maximum = 6E-20, Minimum = -5E-20, Title = "small numbers" });
            plotModel1.Axes.Add(new LinearAxis
            {
                Maximum = 3E-20,
                Minimum = -4E-20,
                Position = AxisPosition.Bottom,
                Title = "small numbers"
            });
            return plotModel1;
        }

        [Example("Default padding")]
        public static PlotModel Defaultpadding()
        {
            var plotModel1 = new PlotModel { Title = "Default padding" };
            plotModel1.Axes.Add(new LinearAxis());
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6)
            };
            lineSeries1.Points.Add(new DataPoint(10, 4));
            lineSeries1.Points.Add(new DataPoint(12, 7));
            lineSeries1.Points.Add(new DataPoint(16, 3));
            lineSeries1.Points.Add(new DataPoint(20, 9));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("No padding")]
        public static PlotModel Nopadding()
        {
            var plotModel1 = new PlotModel { Title = "No padding" };
            plotModel1.Axes.Add(new LinearAxis { MaximumPadding = 0, MinimumPadding = 0 });
            plotModel1.Axes.Add(new LinearAxis { MaximumPadding = 0, MinimumPadding = 0, Position = AxisPosition.Bottom });
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6)
            };
            lineSeries1.Points.Add(new DataPoint(10, 4));
            lineSeries1.Points.Add(new DataPoint(12, 7));
            lineSeries1.Points.Add(new DataPoint(16, 3));
            lineSeries1.Points.Add(new DataPoint(20, 9));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("Padding 10%")]
        public static PlotModel Padding()
        {
            var plotModel1 = new PlotModel { Title = "Padding 10%" };
            plotModel1.Axes.Add(new LinearAxis { MaximumPadding = 0.1, MinimumPadding = 0.1 });
            plotModel1.Axes.Add(new LinearAxis
            {
                MaximumPadding = 0.1,
                MinimumPadding = 0.1,
                Position = AxisPosition.Bottom
            });
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6)
            };
            lineSeries1.Points.Add(new DataPoint(10, 4));
            lineSeries1.Points.Add(new DataPoint(12, 7));
            lineSeries1.Points.Add(new DataPoint(16, 3));
            lineSeries1.Points.Add(new DataPoint(20, 9));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("X-axis MinimumPadding=0.1")]
        public static PlotModel XaxisMinimumPadding()
        {
            var plotModel1 = new PlotModel { Title = "X-axis MinimumPadding=0.1" };
            plotModel1.Axes.Add(new LinearAxis());
            plotModel1.Axes.Add(new LinearAxis { MinimumPadding = 0.1, Position = AxisPosition.Bottom });
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6)
            };
            lineSeries1.Points.Add(new DataPoint(10, 4));
            lineSeries1.Points.Add(new DataPoint(12, 7));
            lineSeries1.Points.Add(new DataPoint(16, 3));
            lineSeries1.Points.Add(new DataPoint(20, 9));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("X-axis MaximumPadding=0.1")]
        public static PlotModel XaxisMaximumPadding()
        {
            var plotModel1 = new PlotModel { Title = "X-axis MaximumPadding=0.1" };
            plotModel1.Axes.Add(new LinearAxis());
            plotModel1.Axes.Add(new LinearAxis { MaximumPadding = 0.1, Position = AxisPosition.Bottom });
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6)
            };
            lineSeries1.Points.Add(new DataPoint(10, 4));
            lineSeries1.Points.Add(new DataPoint(12, 7));
            lineSeries1.Points.Add(new DataPoint(16, 3));
            lineSeries1.Points.Add(new DataPoint(20, 9));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("AbsoluteMinimum and AbsoluteMaximum")]
        public static PlotModel AbsoluteMinimumAndMaximum()
        {
            var model = new PlotModel { Title = "AbsoluteMinimum=-17, AbsoluteMaximum=63", Subtitle = "Zooming and panning is limited to these values." };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Minimum = 0,
                    Maximum = 50,
                    AbsoluteMinimum = -17,
                    AbsoluteMaximum = 63
                });
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = 0,
                    Maximum = 50,
                    AbsoluteMinimum = -17,
                    AbsoluteMaximum = 63
                });
            return model;
        }

        [Example("MinimumRange")]
        public static PlotModel MinimumRange()
        {
            var model = new PlotModel { Title = "MinimumRange = 400" };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    MinimumRange = 400
                });

            return model;
        }

        [Example("MaximumRange")]
        public static PlotModel MaximumRange()
        {
            var model = new PlotModel { Title = "MaximumRange = 40" };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    MaximumRange = 40
                });

            return model;
        }

        [Example("Title with unit")]
        public static PlotModel TitleWithUnit()
        {
            var model = new PlotModel { Title = "Axis titles with units" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Speed", Unit = "km/h" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature", Unit = "°C" });
            return model;
        }

        [Example("Invisible vertical axis")]
        public static PlotModel InvisibleVerticalAxis()
        {
            var model = new PlotModel { Title = "Invisible vertical axis" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, IsAxisVisible = false });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, -5, 5, 0.1));
            return model;
        }

        [Example("Invisible horizontal axis")]
        public static PlotModel InvisibleHorizontalAxis()
        {
            var model = new PlotModel { Title = "Invisible horizontal axis" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, IsAxisVisible = false });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x) * x * x, -5, 5, 0.1));
            return model;
        }

        [Example("Zooming disabled")]
        public static PlotModel ZoomingDisabled()
        {
            var model = new PlotModel { Title = "Zooming disabled" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, IsZoomEnabled = false });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, IsZoomEnabled = false });
            return model;
        }

        [Example("Panning disabled")]
        public static PlotModel PanningDisabled()
        {
            var model = new PlotModel { Title = "Panning disabled" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, IsPanEnabled = false });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, IsPanEnabled = false });
            return model;
        }

        [Example("Dense intervals")]
        public static PlotModel DenseIntervals()
        {
            var model = new PlotModel { Title = "Dense intervals" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, IntervalLength = 30 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, IntervalLength = 20 });
            return model;
        }

        [Example("Graph Paper")]
        public static PlotModel GraphPaper()
        {
            var model = new PlotModel { Title = "Graph Paper" };
            var c = OxyColors.DarkBlue;
            model.PlotType = PlotType.Cartesian;
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "X",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c)
                });
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Y",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c)
                });
            return model;
        }

        [Example("Log-Log Paper")]
        public static PlotModel LogLogPaper()
        {
            var model = new PlotModel { Title = "Log-Log Paper" };
            var c = OxyColors.DarkBlue;
            model.Axes.Add(
                new LogarithmicAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "X",
                    Minimum = 0.1,
                    Maximum = 1000,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c)
                });
            model.Axes.Add(
                new LogarithmicAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Y",
                    Minimum = 0.1,
                    Maximum = 1000,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c)
                });
            return model;
        }

        [Example("Black background")]
        public static PlotModel OnBlack()
        {
            var model = new PlotModel
            {
                Title = "Black background",
                Background = OxyColors.Black,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White
            };
            var c = OxyColors.White;
            model.PlotType = PlotType.Cartesian;
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 1000, "f(x)=sin(x)"));
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "x",
                    MajorStep = Math.PI / 2,
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c),
                    TicklineColor = OxyColors.White
                });
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "f(x)",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColor.FromAColor(40, c),
                    MinorGridlineColor = OxyColor.FromAColor(20, c),
                    TicklineColor = OxyColors.White
                });
            return model;
        }

        [Example("Background and PlotAreaBackground")]
        public static PlotModel Backgrounds()
        {
            var model = new PlotModel
            {
                Title = "Background and PlotAreaBackground",
                Background = OxyColors.Silver,
                PlotAreaBackground = OxyColors.Gray,
                PlotAreaBorderColor = OxyColors.Black,
                PlotAreaBorderThickness = new OxyThickness(3)
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Current culture")]
        public static PlotModel CurrentCulture()
        {
            var model = new PlotModel { Title = "Current culture" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1 });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Invariant culture")]
        public static PlotModel InvariantCulture()
        {
            var model = new PlotModel { Title = "Invariant culture", Culture = CultureInfo.InvariantCulture };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, MaximumPadding = 1 });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Custom culture")]
        public static PlotModel CustomCulture()
        {
            var model = new PlotModel
            {
                Title = "Custom culture",
                Culture = new CultureInfo("en-GB") { NumberFormat = { NumberDecimalSeparator = "·" } }
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1 });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        private static IEnumerable<DataPoint> ButterflyCurve(double t0, double t1, int n)
        {
            // http://en.wikipedia.org/wiki/Butterfly_curve_(transcendental)
            double dt = (t1 - t0) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double t = t0 + dt * i;
                double r = (Math.Exp(Math.Cos(t)) - 2 * Math.Cos(4 * t) - Math.Pow(Math.Sin(t / 12), 5));
                double x = Math.Sin(t) * r;
                double y = Math.Cos(t) * r;
                yield return new DataPoint(x, y);
            }
        }

        [Example("Long axis titles (clipped at 90%)")]
        public static PlotModel LongAxisTitlesClipped90()
        {
            var longTitle = "Long title 12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var tooltip = "The tool tip is " + longTitle;
            var plotModel1 = new PlotModel { Title = "Long axis titles (clipped at 90%)" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = longTitle, ToolTip = tooltip });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = longTitle, ToolTip = tooltip });
            return plotModel1;
        }

        [Example("Long axis titles (clipped at 100%)")]
        public static PlotModel LongAxisTitlesClipped100()
        {
            var longTitle = "Long title 12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var tooltip = "The tool tip is " + longTitle;
            var plotModel1 = new PlotModel { Title = "Long axis titles (clipped at 100%)" };
            plotModel1.Axes.Add(
                new LinearAxis { Position = AxisPosition.Left, Title = longTitle, ToolTip = tooltip, TitleClippingLength = 1.0 });
            plotModel1.Axes.Add(
                new LinearAxis { Position = AxisPosition.Bottom, Title = longTitle, ToolTip = tooltip, TitleClippingLength = 1.0 });
            return plotModel1;
        }

        [Example("Long axis titles (not clipped)")]
        public static PlotModel LongAxisTitlesNotClipped()
        {
            var longTitle = "Long title 12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var tooltip = "The tool tip is " + longTitle;
            var plotModel1 = new PlotModel { Title = "Long axis titles (not clipped)" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = longTitle, ToolTip = tooltip, ClipTitle = false });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = longTitle, ToolTip = tooltip, ClipTitle = false });
            return plotModel1;
        }

        [Example("PositionTier")]
        public static PlotModel PositionTier()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis { Maximum = 1, Minimum = -1, Title = "PositionTier=0" };
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 2,
                Minimum = -2,
                PositionTier = 1,
                Title = "PositionTier=1"
            };
            plotModel1.Axes.Add(linearAxis2);
            var linearAxis3 = new LinearAxis
            {
                Maximum = 1,
                Minimum = -1,
                Position = AxisPosition.Right,
                Title = "PositionTier=0"
            };
            plotModel1.Axes.Add(linearAxis3);
            var linearAxis4 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 2,
                Minimum = -2,
                Position = AxisPosition.Right,
                PositionTier = 1,
                Title = "PositionTier=1"
            };
            plotModel1.Axes.Add(linearAxis4);
            var linearAxis5 = new LinearAxis
            {
                Maximum = 1,
                Minimum = -1,
                Position = AxisPosition.Top,
                Title = "PositionTier=0"
            };
            plotModel1.Axes.Add(linearAxis5);
            var linearAxis6 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 2,
                Minimum = -2,
                Position = AxisPosition.Top,
                PositionTier = 1,
                Title = "PositionTier=1"
            };
            plotModel1.Axes.Add(linearAxis6);
            var linearAxis7 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 10,
                Minimum = -10,
                Position = AxisPosition.Top,
                PositionTier = 2,
                Title = "PositionTier=2"
            };
            plotModel1.Axes.Add(linearAxis7);
            var linearAxis8 = new LinearAxis
            {
                Maximum = 1,
                Minimum = -1,
                Position = AxisPosition.Bottom,
                Title = "PositionTier=0"
            };
            plotModel1.Axes.Add(linearAxis8);
            var linearAxis9 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 2,
                Minimum = -2,
                Position = AxisPosition.Bottom,
                PositionTier = 1,
                Title = "PositionTier=1"
            };
            plotModel1.Axes.Add(linearAxis9);
            var linearAxis10 = new LinearAxis
            {
                AxislineStyle = LineStyle.Solid,
                Maximum = 10,
                Minimum = -10,
                Position = AxisPosition.Bottom,
                PositionTier = 2,
                Title = "PositionTier=2"
            };
            plotModel1.Axes.Add(linearAxis10);
            return plotModel1;
        }

        [Example("Custom axis title color")]
        public static PlotModel TitleColor()
        {
            var model = new PlotModel { Title = "Custom axis title color" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1, Title = "Bottom axis", TitleColor = OxyColors.Red });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1, Title = "Left axis", TitleColor = OxyColors.Blue });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Custom axis label color")]
        public static PlotModel LabelColor()
        {
            var model = new PlotModel { Title = "Custom axis label color" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1, Title = "Bottom axis", TextColor = OxyColors.Red });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1, Title = "Left axis", TextColor = OxyColors.Blue });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Angled axis numbers")]
        public static PlotModel AngledAxisNumbers()
        {
            var model = new PlotModel { Title = "Angled axis numbers" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -1, Maximum = 1, Title = "Bottom axis", Angle = 45 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1, Title = "Left axis", Angle = 45 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Top, Minimum = -1, Maximum = 1, Title = "Top axis", Angle = 45 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Minimum = -1, Maximum = 1, Title = "Right axis", Angle = 45 });
            return model;
        }

        [Example("Axis distance")]
        public static PlotModel AxisDistance()
        {
            var plotModel = new PlotModel { Title = "AxisDistance = 20" };
            plotModel.Axes.Add(new LinearAxis { AxislineStyle = LineStyle.Solid, AxisDistance = 20, Position = AxisPosition.Bottom });
            plotModel.Axes.Add(new LinearAxis { AxislineStyle = LineStyle.Solid, AxisDistance = 20, Position = AxisPosition.Left });
            plotModel.Axes.Add(new LinearAxis { AxislineStyle = LineStyle.Solid, AxisDistance = 20, Position = AxisPosition.Right });
            plotModel.Axes.Add(new LinearAxis { AxislineStyle = LineStyle.Solid, AxisDistance = 20, Position = AxisPosition.Top });
            return plotModel;
        }

        [Example("No axes defined")]
        public static PlotModel NoAxesDefined()
        {
            var plotModel = new PlotModel { Title = "No axes defined", Subtitle = "Bottom and left axes are auto-generated." };
            plotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 400));
            return plotModel;
        }

        /// <summary>
        /// Shows usage of the <see cref="Axis.LabelFormatter" /> property.
        /// </summary>
        /// <returns>The <see cref="PlotModel" /> for the example.</returns>
        [Example("LabelFormatter")]
        public static PlotModel LabelFormatter()
        {
            var plotModel = new PlotModel { Title = "LabelFormatter" };
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -10,
                Maximum = 10,
                LabelFormatter = x => Math.Abs(x) < double.Epsilon ? "ZERO" : x.ToString()
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 25,
                MajorStep = 1,
                MinorStep = 1,
                MaximumPadding = 0,
                MinimumPadding = 0,
                LabelFormatter = y => ((char)(y + 'A')).ToString()
            });
            return plotModel;
        }

        [Example("Tool tips")]
        public static PlotModel ToolTips()
        {
            var plotModel1 = new PlotModel { Title = "Tool tips" };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Left axis", ToolTip = "Tool tip for the left axis" });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Bottom axis", ToolTip = "Tool tip for the bottom axis" });
            return plotModel1;
        }

        [Example("Sub- and superscript in axis titles")]
        public static PlotModel SubSuperscriptInAxisTitles()
        {
            var plotModel1 = new PlotModel { Title = "Sub- and superscript in axis titles" };
            plotModel1.Axes.Add(new LinearAxis { Title = "Title with^{super}_{sub}script" });
            plotModel1.Axes.Add(new LinearAxis { Title = "Title with^{super}_{sub}script", Position = AxisPosition.Bottom });
            return plotModel1;
        }

        [Example("MinimumMajorStep")]
        public static PlotModel MinimumMajorStep()
        {
            var model = new PlotModel
            {
                Title = "Axes with MinimumMajorStep"
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "MinimuMajorStep = 1", Minimum = 0, Maximum = 2, MinimumMajorStep = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "MinimuMajorStep = 10", Minimum = 0, Maximum = 15, MinimumMajorStep = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Top, Title = "MinimuMajorStep = 0 (default)", Minimum = 0, Maximum = 2 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "MinimuMajorStep = 0 (default)", Minimum = 0, Maximum = 15 });
            return model;
        }

        [Example("MinimumMinorStep")]
        public static PlotModel MinimumMinorStep()
        {
            var model = new PlotModel
            {
                Title = "Axes with MinimumMinorStep"
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "MinimumMinorStep = 1", Minimum = 0, Maximum = 20, MinimumMinorStep = 1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "MinimumMinorStep = 10", Minimum = 0, Maximum = 150, MinimumMinorStep = 10 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Top, Title = "MinimumMinorStep = 0 (default)", Minimum = 0, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "MinimumMinorStep = 0 (default)", Minimum = 0, Maximum = 150 });
            return model;
        }

        [Example("Default AxisTitleDistance")]
        public static PlotModel DefaultAxisTitleDistance()
        {
            var model = new PlotModel
            {
                Title = "AxisTitleDistance = 4 (default)"
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Bottom", Minimum = 0, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Left", Minimum = 0, Maximum = 150 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Top, Title = "Top", Minimum = 0, Maximum = 20 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "Right", Minimum = 0, Maximum = 150 });
            return model;
        }

        [Example("Custom AxisTitleDistance")]
        public static PlotModel CustomAxisTitleDistance()
        {
            var model = new PlotModel
            {
                Title = "AxisTitleDistance = 40"
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Bottom", Minimum = 0, Maximum = 20, AxisTitleDistance = 40 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Left", Minimum = 0, Maximum = 150, AxisTitleDistance = 40 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Top, Title = "Top", Minimum = 0, Maximum = 20, AxisTitleDistance = 40 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Right, Title = "Right", Minimum = 0, Maximum = 150, AxisTitleDistance = 40 });
            return model;
        }

        [Example("MajorGridlineStyle")]
        public static PlotModel MajorGridlineStyle()
        {
            var pm = new PlotModel { Title = "MajorGridlineStyle and MajorGridlineThickness" };
            pm.Axes.Add(new LinearAxis { MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 10 });
            pm.Axes.Add(new LinearAxis { MajorGridlineStyle = LineStyle.Solid, MajorGridlineThickness = 10, Position = AxisPosition.Bottom });
            return pm;
        }

        /// <summary>
        /// Creates an example with the specified <see cref="TickStyle" />.
        /// </summary>
        /// <param name="tickStyle">The tick style.</param>
        /// <returns>A <see cref="PlotModel" />.</returns>
        private static PlotModel CreateTickStyleExample(TickStyle tickStyle)
        {
            var plotModel1 = new PlotModel { Title = "TickStyle = " + tickStyle };
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TickStyle = tickStyle });
            plotModel1.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TickStyle = tickStyle });
            return plotModel1;
        }

        [Example("Gridlines Cropping: Horizontal and vertical")]
        public static PlotModel GridlineCroppingBoth()
        {
            var plotModel1 = new PlotModel { Title = "Gridline cropping" };
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                ExtraGridlines = new double[] { 46d },
                ExtraGridlineColor = OxyColors.Red,
                StartPosition = 0.1,
                EndPosition = 0.4,
                CropGridlines = true
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                ExtraGridlines = new double[] { 46d },
                ExtraGridlineColor = OxyColors.Red,
                StartPosition = 0.6,
                EndPosition = 0.9,
                CropGridlines = true
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom,
                ExtraGridlines = new double[] { 46d },
                ExtraGridlineColor = OxyColors.Red,
                StartPosition = 0.1,
                EndPosition = 0.4,
                CropGridlines = true
            });
            plotModel1.Axes.Add(new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Position = AxisPosition.Bottom,
                ExtraGridlines = new double[] { 46d },
                ExtraGridlineColor = OxyColors.Red,
                StartPosition = 0.6,
                EndPosition = 0.9,
                CropGridlines = true
            });
            return plotModel1;
        }

        [Example("Multi vertical axes with lineSeries")]
        public static PlotModel MultiVerticalAxes()
        {
            const string keyAxisY_Temperature = "axisY_Temperature";
            const string keyAxisY_Pressure = "axisY_Pressure";
            const string keyAxisY_Humidity = "axisY_Humidity";

            var plotModel = new PlotModel()
            {
                Title = "Multi vertical axes with lineSeries",
            };

            Legend l = new Legend
            {
                LegendBackground = OxyColors.White
            };
            plotModel.Legends.Add(l);

            var axisX_Time = new DateTimeAxis()
            {
                Title = "Time",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                FontSize = 13,
            };
            plotModel.Axes.Add(axisX_Time);

            LineSeries lineSeriesTemperature = null;
            LineSeries lineSeriesPressure = null;
            LineSeries lineSeriesHumidity = null;
            LinearAxis axisY_Temperature = null;
            LinearAxis axisY_Pressure = null;
            LinearAxis axisY_Humidity = null;

            //Initialization lineSeries temperature
            {
                axisY_Temperature = new LinearAxis()
                {
                    Title = "Temperature",
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.None,
                    PositionTier = 1,
                    Key = keyAxisY_Temperature,
                    IsAxisVisible = true,
                };

                lineSeriesTemperature = new LineSeries()
                {
                    Title = "Temperature",
                    Color = OxyColors.Tomato,
                    LineStyle = LineStyle.Solid,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
                    MarkerFill = OxyColors.Red,
                    YAxisKey = keyAxisY_Temperature,
                    IsVisible = true,
                };

                plotModel.Axes.Add(axisY_Temperature);
                plotModel.Series.Add(lineSeriesTemperature);
            }

            // Initialization lineSeries pressure
            {
                axisY_Pressure = new LinearAxis()
                {
                    Title = "Pressure",
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.None,
                    PositionTier = 2,
                    Key = keyAxisY_Pressure,
                    IsAxisVisible = true,
                };

                lineSeriesPressure = new LineSeries()
                {
                    Title = "Pressure",
                    Color = OxyColors.Peru,
                    LineStyle = LineStyle.Solid,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
                    MarkerFill = OxyColors.Sienna,
                    YAxisKey = keyAxisY_Pressure,
                    IsVisible = true,
                };

                plotModel.Axes.Add(axisY_Pressure);
                plotModel.Series.Add(lineSeriesPressure);
            }

            // Initialization lineSeries humidity
            {
                axisY_Humidity = new LinearAxis()
                {
                    Title = "Humidity",
                    Position = AxisPosition.Left,
                    MajorGridlineStyle = LineStyle.None,
                    PositionTier = 3,
                    Key = keyAxisY_Humidity,
                    IsAxisVisible = true,
                };

                lineSeriesHumidity = new LineSeries()
                {
                    Title = "Humidity",
                    Color = OxyColors.LightSkyBlue,
                    LineStyle = LineStyle.Solid,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
                    MarkerFill = OxyColors.DeepSkyBlue,
                    YAxisKey = keyAxisY_Humidity,
                    IsVisible = true,
                };

                plotModel.Axes.Add(axisY_Humidity);
                plotModel.Series.Add(lineSeriesHumidity);
            }

            // Add points
            {
                lineSeriesTemperature.Points.Clear();
                lineSeriesPressure.Points.Clear();
                lineSeriesHumidity.Points.Clear();

                var timeSpan = TimeSpan.FromSeconds(1);
                var time = new DateTime(2018, 09, 10);
                int countPoints = 100;
                for (int i = 1; i <= countPoints; i++)
                {
                    double temperature = 20 + Math.Sin(i);
                    double pressure = 760 + 1.5 * Math.Cos(1.5 * i);
                    double humidity = 50 + 2.0 * Math.Sin(2.0 * i);

                    lineSeriesTemperature.Points.Add(DateTimeAxis.CreateDataPoint(time, temperature));
                    lineSeriesPressure.Points.Add(DateTimeAxis.CreateDataPoint(time, pressure));
                    lineSeriesHumidity.Points.Add(DateTimeAxis.CreateDataPoint(time, humidity));

                    time += timeSpan;
                }

                axisY_Temperature.Minimum = 10;
                axisY_Temperature.Maximum = 23;

                axisY_Pressure.Minimum = 750;
                axisY_Pressure.Maximum = 770;

                axisY_Humidity.Minimum = 47;
                axisY_Humidity.Maximum = 60;
            }

            return plotModel;
        }

        [Example("Auto Margins")]
        public static PlotModel AutoMargin()
        {
            var plotModel = new PlotModel() { Title = "Auto-adjusting plot margins", Subtitle = "When zooming in and out the plot margins should adjust accordingly" };
            plotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "X Axis", TitleFontSize = 16 });
            return plotModel;
        }

        [Example("Manual Margins")]
        public static PlotModel ManualMargins()
        {
            var plotModel = new PlotModel() { Title = "Manual Margins", Subtitle = "PlotMargins = 40", PlotMargins = new OxyThickness(40) };
            plotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom });
            return plotModel;
        }

        [Example("Manual Left Margin")]
        public static PlotModel ManualLeftMargin()
        {
            var plotModel = new PlotModel() { Title = "Manual Left Margin", Subtitle = "PlotMargins = 40,NaN,NaN,NaN", PlotMargins = new OxyThickness(40, double.NaN, double.NaN, double.NaN) };
            plotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom });
            return plotModel;
        }

        [Example("Auto Margins - Wide Labels")]
        public static PlotModel AutoMarginWideLabels()
        {
            var plotModel = new PlotModel() { Title = "Auto-adjusting plot margins - wide axis labels", Subtitle = "There should be enough space reserved such that the axis labels always fit in the viewport" };
            plotModel.Axes.Add(GetLongLabelSeries());
            return plotModel;
        }

        [Example("Auto Margins - Wide Labels, rotated")]
        public static PlotModel AutoMarginWideLabelsRotated()
        {
            var plotModel = new PlotModel() { Title = "Auto-adjusting plot margins - wide rotated axis labels", Subtitle = "There should be enough space reserved such that the axis labels always fit in the viewport" };
            var axis = GetLongLabelSeries();
            axis.Angle = -90;
            plotModel.Axes.Add(axis);
            return plotModel;
        }

        [Example("Auto Margins - Wide Labels, fixed Range")]
        public static PlotModel AutoMarginWideLabelsFixedRange()
        {
            var plotModel = new PlotModel() { Title = "Auto-adjusting plot margins - wide axis labels, fixed range", Subtitle = "When the axis range is fixed there should be no unnecessary space reserved for axis labels" };
            var axis = GetLongLabelSeries();
            axis.IsPanEnabled = false;
            axis.IsZoomEnabled = false;
            plotModel.Axes.Add(axis);
            return plotModel;
        }

        [Example("Auto Margins - Wide Labels, fixed Range 2")]
        public static PlotModel AutoMarginWideLabelsFixedRange2()
        {
            var plotModel = new PlotModel() { Title = "Auto-adjusting plot margins - wide axis labels, fixed range", Subtitle = "The axis labels should exactly fit in the viewport" };
            var axis = GetLongLabelSeries();
            axis.IsPanEnabled = false;
            axis.IsZoomEnabled = false;
            axis.Minimum = -0.01;
            axis.Maximum = 3.01;
            plotModel.Axes.Add(axis);
            return plotModel;
        }

        private static CategoryAxis GetLongLabelSeries()
        {
            var axis = new CategoryAxis() { Position = AxisPosition.Bottom };
            axis.Labels.Add("Label");
            axis.Labels.Add("Long Label");
            axis.Labels.Add("Longer Label");
            axis.Labels.Add("Even Longer Label");
            return axis;
        }
    }
}
