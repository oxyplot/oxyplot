// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenIssues.cs" company="OxyPlot">
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
//   Defines the OpenIssues type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z0 Issues (open)")]
    public class OpenIssues : ExamplesBase
    {
        [Example("#10018: Sub/superscript in vertical axis title")]
        public static PlotModel SubSuperScriptInAxisTitles()
        {
            var plotModel1 = new PlotModel { Title = "x_{i}^{j}", Subtitle = "x_{i}^{j}" };
            var leftAxis = new LinearAxis(AxisPosition.Left) { Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis(AxisPosition.Bottom) { Title = "x_{i}^{j}" };
            plotModel1.Axes.Add(bottomAxis);
            plotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 100, "x_{i}^{j}"));
            return plotModel1;
        }

        [Example("#10045: DateTimeAxis with IntervalType = Minutes")]
        public static PlotModel DateTimeAxisWithIntervalTypeMinutes()
        {
            var plotModel1 = new PlotModel();
            var linearAxis1 = new LinearAxis();
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);

            var dateTimeAxis1 = new DateTimeAxis();
            dateTimeAxis1.IntervalType = DateTimeIntervalType.Minutes;
            // dateTimeAxis1.MajorStep = 1.0 / 24 / 60;
            dateTimeAxis1.EndPosition = 0;
            dateTimeAxis1.StartPosition = 1;
            dateTimeAxis1.StringFormat = "hh:mm:ss";
            plotModel1.Axes.Add(dateTimeAxis1);
            var time0 = new DateTime(2013, 5, 6, 3, 24, 0);
            var time1 = new DateTime(2013, 5, 6, 3, 28, 0);
            //dateTimeAxis1.Minimum = DateTimeAxis.ToDouble(time0);
            //dateTimeAxis1.Maximum = DateTimeAxis.ToDouble(time1);
            var lineSeries1 = new LineSeries();
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time0), 36));
            lineSeries1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time1), 26));
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        [Example("10056: Tracker wrong for logarithmic y-axis")]
        public static PlotModel ValueTime()
        {
            var plotModel1 = new PlotModel
                                 {
                                     LegendBackground = OxyColor.FromArgb(200, 255, 255, 255),
                                     LegendBorder = OxyColors.Black,
                                     LegendPlacement = LegendPlacement.Outside,
                                     PlotAreaBackground = OxyColors.Gray,
                                     PlotAreaBorderColor = OxyColors.Gainsboro,
                                     PlotAreaBorderThickness = 2,
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

        [Example("10055: Hit testing LineSeries with smoothing")]
        public static PlotModel MouseDownEvent()
        {
            var model = new PlotModel("LineSeries with smoothing", "Tracker uses wrong points");
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
    }
}