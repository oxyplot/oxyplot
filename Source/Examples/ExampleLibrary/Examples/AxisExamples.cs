// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisExamples.cs" company="OxyPlot">
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
using OxyPlot;

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    [Examples("Axis examples")]
    public class AxisExamples : ExamplesBase
    {
        [Example("TickStyle: None")]
        public static PlotModel TickStyleNone()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "None";
            var linearAxis1 = new LinearAxis();
            linearAxis1.TickStyle = TickStyle.None;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.TickStyle = TickStyle.None;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("TickStyle: Inside")]
        public static PlotModel TickStyleInside()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Inside";
            var linearAxis1 = new LinearAxis();
            linearAxis1.TickStyle = TickStyle.Inside;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.TickStyle = TickStyle.Inside;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("TickStyle: Crossing")]
        public static PlotModel TickStyleCrossing()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Crossing";
            var linearAxis1 = new LinearAxis();
            linearAxis1.TickStyle = TickStyle.Crossing;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.TickStyle = TickStyle.Crossing;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }
        [Example("TickStyle: Outside")]
        public static PlotModel TickStyleOutside()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Outside";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("GridLines: None")]
        public static PlotModel GridLinesNone()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "None";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("GridLines: Vertical")]
        public static PlotModel GridLinesVertical()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Vertical";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("GridLines: Horizontal")]
        public static PlotModel GridLinesHorizontal()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Horizontal";
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("GridLines: Both")]
        public static PlotModel GridLinesBoth()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Both";
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Axis position left/bottom")]
        public static PlotModel LeftAndBottom()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(40, 40, 40, 40);
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Title = "Left";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "Bottom";
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Axis position top/right")]
        public static PlotModel TopRight()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(40, 40, 40, 40);
            var linearAxis1 = new LinearAxis();
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Position = AxisPosition.Right;
            linearAxis1.Title = "Right";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Top;
            linearAxis2.Title = "Top";
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Axis label angle 45deg")]
        public static PlotModel Untitled()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(60, 40, 60, 30);
            var linearAxis1 = new LinearAxis();
            linearAxis1.Angle = 45;
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            linearAxis1.Title = "Left";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Angle = 45;
            linearAxis2.MajorGridlineStyle = LineStyle.Solid;
            linearAxis2.MinorGridlineStyle = LineStyle.Dot;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "Bottom";
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Zero crossing axis")]
        public static PlotModel ZeroCrossing()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotAreaBorderThickness = 0;
            plotModel1.PlotMargins = new OxyThickness(10, 10, 10, 10);
            var linearAxis1 = new LinearAxis();
            linearAxis1.Maximum = 50;
            linearAxis1.Minimum = -30;
            linearAxis1.PositionAtZeroCrossing = true;
            linearAxis1.TickStyle = TickStyle.Crossing;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Maximum = 70;
            linearAxis2.Minimum = -50;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.PositionAtZeroCrossing = true;
            linearAxis2.TickStyle = TickStyle.Crossing;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Reversed")]
        public static PlotModel Reversed()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(40, 10, 10, 30);
            var linearAxis1 = new LinearAxis();
            linearAxis1.EndPosition = 0;
            linearAxis1.Maximum = 50;
            linearAxis1.Minimum = -30;
            linearAxis1.StartPosition = 1;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.EndPosition = 0;
            linearAxis2.Maximum = 70;
            linearAxis2.Minimum = -50;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.StartPosition = 1;
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Four axes")]
        public static PlotModel FourAxes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(70, 40, 40, 40);
            var linearAxis1 = new LinearAxis();
            linearAxis1.Maximum = 36;
            linearAxis1.Minimum = 0;
            linearAxis1.Title = "km/h";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Maximum = 10;
            linearAxis2.Minimum = 0;
            linearAxis2.Position = AxisPosition.Right;
            linearAxis2.Title = "m/s";
            plotModel1.Axes.Add(linearAxis2);
            var linearAxis3 = new LinearAxis();
            linearAxis3.Maximum = 10;
            linearAxis3.Minimum = 0;
            linearAxis3.Position = AxisPosition.Bottom;
            linearAxis3.Title = "meter";
            plotModel1.Axes.Add(linearAxis3);
            var linearAxis4 = new LinearAxis();
            linearAxis4.Maximum = 10000;
            linearAxis4.Minimum = 0;
            linearAxis4.Position = AxisPosition.Top;
            linearAxis4.Title = "millimeter";
            plotModel1.Axes.Add(linearAxis4);
            return plotModel1;
        }

        [Example("Multiple panes")]
        public static PlotModel MultiplePanes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(40, 20, 40, 30);
            var linearAxis1 = new LinearAxis();
            linearAxis1.EndPosition = 0.25;
            linearAxis1.Maximum = 1;
            linearAxis1.Minimum = -1;
            linearAxis1.Title = "C1";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.EndPosition = 0.5;
            linearAxis2.Maximum = 1;
            linearAxis2.Minimum = -1;
            linearAxis2.Position = AxisPosition.Right;
            linearAxis2.StartPosition = 0.25;
            linearAxis2.Title = "C2";
            plotModel1.Axes.Add(linearAxis2);
            var linearAxis3 = new LinearAxis();
            linearAxis3.EndPosition = 0.75;
            linearAxis3.Maximum = 1;
            linearAxis3.Minimum = -1;
            linearAxis3.StartPosition = 0.5;
            linearAxis3.Title = "C3";
            plotModel1.Axes.Add(linearAxis3);
            var linearAxis4 = new LinearAxis();
            linearAxis4.Maximum = 1;
            linearAxis4.Minimum = -1;
            linearAxis4.Position = AxisPosition.Right;
            linearAxis4.StartPosition = 0.75;
            linearAxis4.Title = "C4";
            plotModel1.Axes.Add(linearAxis4);
            var linearAxis5 = new LinearAxis();
            linearAxis5.Maximum = 100;
            linearAxis5.Minimum = 0;
            linearAxis5.Position = AxisPosition.Bottom;
            linearAxis5.Title = "s";
            plotModel1.Axes.Add(linearAxis5);
            return plotModel1;
        }

        [Example("Logarithmic axes")]
        public static PlotModel LogarithmicAxes()
        {
            var plotModel1 = new PlotModel();
            plotModel1.PlotMargins = new OxyThickness(80, 10, 20, 30);
            var logarithmicAxis1 = new LogarithmicAxis();
            logarithmicAxis1.Maximum = 1000000;
            logarithmicAxis1.Minimum = 1;
            logarithmicAxis1.Title = "Log axis";
            logarithmicAxis1.UseSuperExponentialFormat = true;
            plotModel1.Axes.Add(logarithmicAxis1);
            var logarithmicAxis2 = new LogarithmicAxis();
            logarithmicAxis2.Maximum = 10000;
            logarithmicAxis2.Minimum = 0.001;
            logarithmicAxis2.Position = AxisPosition.Bottom;
            logarithmicAxis2.Title = "Log axis";
            logarithmicAxis2.UseSuperExponentialFormat = true;
            plotModel1.Axes.Add(logarithmicAxis2);
            return plotModel1;
        }

        [Example("Big numbers")]
        public static PlotModel BigNumbers()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            linearAxis1.Maximum = 6E+32;
            linearAxis1.Minimum = -1E+47;
            linearAxis1.Title = "big numbers";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Maximum = 3E+50;
            linearAxis2.Minimum = -1E+40;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "big numbers";
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Small numbers")]
        public static PlotModel SmallNumbers()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            linearAxis1.Maximum = 6E-20;
            linearAxis1.Minimum = -5E-20;
            linearAxis1.Title = "small numbers";
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Maximum = 3E-20;
            linearAxis2.Minimum = -4E-20;
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "small numbers";
            plotModel1.Axes.Add(linearAxis2);
            return plotModel1;
        }

        [Example("Default padding")]
        public static PlotModel Defaultpadding()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Default padding";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 78, 154, 6);
            lineSeries1.MarkerFill = OxyColor.FromArgb(255, 78, 154, 6);
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
            var plotModel1 = new PlotModel();
            plotModel1.Title = "No padding";
            var linearAxis1 = new LinearAxis();
            linearAxis1.MaximumPadding = 0;
            linearAxis1.MinimumPadding = 0;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MaximumPadding = 0;
            linearAxis2.MinimumPadding = 0;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 78, 154, 6);
            lineSeries1.MarkerFill = OxyColor.FromArgb(255, 78, 154, 6);
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
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Padding 10%";
            var linearAxis1 = new LinearAxis();
            linearAxis1.MaximumPadding = 0.1;
            linearAxis1.MinimumPadding = 0.1;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MaximumPadding = 0.1;
            linearAxis2.MinimumPadding = 0.1;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 78, 154, 6);
            lineSeries1.MarkerFill = OxyColor.FromArgb(255, 78, 154, 6);
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
            var plotModel1 = new PlotModel();
            plotModel1.Title = "X-axis MinimumPadding=0.1";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MinimumPadding = 0.1;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 78, 154, 6);
            lineSeries1.MarkerFill = OxyColor.FromArgb(255, 78, 154, 6);
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
            var plotModel1 = new PlotModel();
            plotModel1.Title = "X-axis MaximumPadding=0.1";
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.MaximumPadding = 0.1;
            linearAxis2.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis2);
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 78, 154, 6);
            lineSeries1.MarkerFill = OxyColor.FromArgb(255, 78, 154, 6);
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
            var model = new PlotModel("AbsoluteMinimum=-17, AbsoluteMaximum=63", "Zooming and panning is limited to these values.");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { Minimum = 0, Maximum = 50, AbsoluteMinimum = -17, AbsoluteMaximum = 63 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { Minimum = 0, Maximum = 50, AbsoluteMinimum = -17, AbsoluteMaximum = 63 });
            return model;
        }

        [Example("Title with unit")]
        public static PlotModel TitleWithUnit()
        {
            var model = new PlotModel("Axis titles with units");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { Title = "Speed", Unit = "km/h" });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { Title = "Temperature", Unit = "°C" });
            return model;
        }

        [Example("Invisible vertical axis")]
        public static PlotModel InvisibleVerticalAxis()
        {
            var model = new PlotModel("Invisible vertical axis");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsAxisVisible = false });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, -5, 5, 0.1));
            return model;
        }

        [Example("Invisible horizontal axis")]
        public static PlotModel InvisibleHorizontalAxis()
        {
            var model = new PlotModel("Invisible horizontal axis");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsAxisVisible = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            model.Series.Add(new FunctionSeries(x => Math.Sin(x) * x * x, -5, 5, 0.1));
            return model;
        }

        [Example("Zooming disabled")]
        public static PlotModel ZoomingDisabled()
        {
            var model = new PlotModel("Zooming disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsZoomEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsZoomEnabled = false });
            return model;
        }

        [Example("Panning disabled")]
        public static PlotModel PanningDisabled()
        {
            var model = new PlotModel("Panning disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsPanEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsPanEnabled = false });
            return model;
        }

        [Example("Dense intervals")]
        public static PlotModel DenseIntervals()
        {
            var model = new PlotModel("Dense intervals");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IntervalLength = 30 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IntervalLength = 20 });
            return model;
        }

        [Example("Graph Paper")]
        public static PlotModel GraphPaper()
        {
            var model = new PlotModel("Graph Paper");
            var c = OxyColors.DarkBlue;
            model.PlotType = PlotType.Cartesian;
            model.Axes.Add(
                new LinearAxis(AxisPosition.Bottom, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            return model;
        }

        [Example("Log-Log Paper")]
        public static PlotModel LogLogPaper()
        {
            var model = new PlotModel("Log-Log Paper");
            var c = OxyColors.DarkBlue;
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X") { Minimum = 0.1, Maximum = 1000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y") { Minimum = 0.1, Maximum = 1000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            return model;
        }

        [Example("Black background")]
        public static PlotModel OnBlack()
        {
            var model = new PlotModel("Black background");
            model.Background = OxyColors.Black;
            model.TextColor = OxyColors.White;
            model.PlotAreaBorderColor = OxyColors.White;
            var c = OxyColors.White;
            model.PlotType = PlotType.Cartesian;
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 1000, "f(x)=sin(x)"));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "x") { MajorStep = Math.PI / 2, FormatAsFractions = true, FractionUnit = Math.PI, FractionUnitSymbol = "π", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c), TicklineColor = OxyColors.White });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "f(x)") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c), TicklineColor = OxyColors.White });
            return model;
        }

        [Example("Background and PlotAreaBackground")]
        public static PlotModel Backgrounds()
        {
            var model = new PlotModel("Background and PlotAreaBackground");
            model.Background = OxyColors.Silver;
            model.PlotAreaBackground = OxyColors.Gray;
            model.PlotAreaBorderColor = OxyColors.Black;
            model.PlotAreaBorderThickness = 3;
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            return model;
        }

        [Example("Auto adjusting plot margins")]
        public static PlotModel AutoAdjustingMargins()
        {
            var model = new PlotModel("Auto adjusting plot margins");
            model.LegendPosition = LegendPosition.RightBottom;
            model.PlotMargins = new OxyThickness(0);
            model.AutoAdjustPlotMargins = true;
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X") { TickStyle = TickStyle.Outside });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Y") { TickStyle = TickStyle.Outside });
            model.Series.Add(new LineSeries("Butterfly curve") { ItemsSource = ButterflyCurve(0, Math.PI * 4, 1000) });
            return model;
        }

        [Example("Manual plot margins")]
        public static PlotModel ManualAdjustingMargins()
        {
            var model = new PlotModel("Manual plot margins");
            model.LegendPosition = LegendPosition.RightBottom;
            model.AutoAdjustPlotMargins = false;
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X") { TickStyle = TickStyle.Outside });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Y") { TickStyle = TickStyle.Outside });
            model.Series.Add(new LineSeries("Butterfly curve") { ItemsSource = ButterflyCurve(0, Math.PI * 4, 1000) });
            return model;
        }

        [Example("Current culture")]
        public static PlotModel CurrentCulture()
        {
            var model = new PlotModel("Current culture");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1));
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Invariant culture")]
        public static PlotModel InvariantCulture()
        {
            var model = new PlotModel("Invariant culture");
            model.Culture = CultureInfo.InvariantCulture;
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1));
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Custom culture")]
        public static PlotModel CustomCulture()
        {
            var model = new PlotModel("Custom culture");
            model.Culture = new CultureInfo("en-GB") { NumberFormat = { NumberDecimalSeparator = "·" } };
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1));
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
            var tooltip = "The tooltip is " + longTitle;
            var plotModel1 = new PlotModel("Long axis titles (clipped at 90%)");
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Left, longTitle) { ToolTip = tooltip });
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Bottom, longTitle) { ToolTip = tooltip });
            return plotModel1;
        }

        [Example("Long axis titles (clipped at 100%)")]
        public static PlotModel LongAxisTitlesClipped100()
        {
            var longTitle = "Long title 12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var tooltip = "The tooltip is " + longTitle;
            var plotModel1 = new PlotModel("Long axis titles (clipped at 100%)");
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Left, longTitle) { ToolTip = tooltip, TitleClippingLength = 1.0 });
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Bottom, longTitle) { ToolTip = tooltip, TitleClippingLength = 1.0 });
            return plotModel1;
        }

        [Example("Long axis titles (not clipped)")]
        public static PlotModel LongAxisTitlesNotClipped()
        {
            var longTitle = "Long title 12345678901234567890123456789012345678901234567890123456789012345678901234567890";
            var tooltip = "The tooltip is " + longTitle;
            var plotModel1 = new PlotModel("Long axis titles (not clipped)");
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Left, longTitle) { ToolTip = tooltip, ClipTitle = false });
            plotModel1.Axes.Add(new LinearAxis(AxisPosition.Bottom, longTitle) { ToolTip = tooltip, ClipTitle = false });
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
            var linearAxis3 = new LinearAxis { Maximum = 1, Minimum = -1, Position = AxisPosition.Right, Title = "PositionTier=0" };
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
            var linearAxis5 = new LinearAxis { Maximum = 1, Minimum = -1, Position = AxisPosition.Top, Title = "PositionTier=0" };
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
            var linearAxis8 = new LinearAxis { Maximum = 1, Minimum = -1, Position = AxisPosition.Bottom, Title = "PositionTier=0" };
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
            var model = new PlotModel("Custom axis title color");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1, "Bottom axis") { TitleColor = OxyColors.Red });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1, "Left axis") { TitleColor = OxyColors.Blue });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        [Example("Custom axis label color")]
        public static PlotModel LabelColor()
        {
            var model = new PlotModel("Custom axis label color");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1, "Bottom axis") { TextColor = OxyColors.Red });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1, "Left axis") { TextColor = OxyColors.Blue });
            model.Series.Add(new FunctionSeries(Math.Sin, -1, 1, 100));
            return model;
        }

        //[Example("Issue 9961: Round off error")]
        //public static PlotModel Issue9961RoundoffError()
        //{
        //    var model = new PlotModel();
        //    model.Axes.Add(new LinearAxis(AxisPosition.Left, -0.0182, 0.0012, 0.001, 0.0002));
        //    return model;
        //}

        [Example("Angled axis numbers")]
        public static PlotModel AngledAxisNumbers()
        {
            var model = new PlotModel("Angled axis numbers");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 1, "Bottom axis") { Angle = 45 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 1, "Left axis") { Angle = 45 });
            model.Axes.Add(new LinearAxis(AxisPosition.Top, -1, 1, "Top axis") { Angle = 45 });
            model.Axes.Add(new LinearAxis(AxisPosition.Right, -1, 1, "Right axis") { Angle = 45 });
            return model;
        }

        [Example("GridLines: Issue 9990")]
        public static PlotModel GridLinesBothDifferentColors()
        {
            var plotModel1 = new PlotModel { Title = "Don't stare at this plot too long...", Subtitle = "Minor gridlines should be below major gridlines (issue 9990)" };
            var leftAxis = new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineThickness = 6,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColors.Blue,
                MinorGridlineThickness = 6,
            };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis(AxisPosition.Bottom)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Black,
                MajorGridlineThickness = 6,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = OxyColors.Blue,
                MinorGridlineThickness = 6,
            };
            plotModel1.Axes.Add(bottomAxis);
            return plotModel1;
        }
    }
}