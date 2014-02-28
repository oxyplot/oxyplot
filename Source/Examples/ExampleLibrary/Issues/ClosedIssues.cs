// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClosedIssues.cs" company="OxyPlot">
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

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z9 Issues (closed)")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    public class ClosedIssues : ExamplesBase
    {
        [Example("#9971: Don't show minor ticks")]
        public static PlotModel DontShowMinorTicks()
        {
            var model = new PlotModel("ShowMinorTicks = false");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { ShowMinorTicks = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { ShowMinorTicks = false });
            return model;
        }

        [Example("#9990: Major grid lines in front of minor")]
        public static PlotModel GridLinesBothDifferentColors()
        {
            var plotModel1 = new PlotModel
                {
                    Title = "Issue #9990",
                    Subtitle = "Minor gridlines should be below major gridlines"
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
            var bottomAxis = new LinearAxis(AxisPosition.Bottom)
                {
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

        [Example("#10060: AnnotationLayers")]
        public static PlotModel AnnotationLayers()
        {
            var model = new PlotModel("AnnotationLayers");

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
                model.RefreshPlot(true);
                e.Handled = true;
            };
            a2.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked annotation below series";
                model.RefreshPlot(true);
                e.Handled = true;
            };
            a3.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked annotation above series";
                model.RefreshPlot(true);
                e.Handled = true;
            };
            s1.MouseDown += (s, e) =>
            {
                model.Subtitle = "Clicked series";
                model.RefreshPlot(true);
                e.Handled = true;
            };

            return model;
        }

        [Example("#10061: Argument out of range in OxyPlot mouse over")]
        public static PlotModel ArgumentOutOfRangeInMouseOver()
        {
            var model = new PlotModel("Argument out of range in OxyPlot mouse over");
            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(12, 10));
            model.Series.Add(ls);
            return model;
        }

        [Example("#10076: Slow redraws with noisy data")]
        public static PlotModel NoisyData()
        {
            var model = new PlotModel("Noisy data");

            var points = new List<IDataPoint>();
            var rng = new Random();
            for (int i = 0; i < 500; i++)
            {
                points.Add(new DataPoint(i + 1, rng.NextDouble()));
            }

            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }

        [Example("#10076: Dashed line test")]
        public static PlotModel DashedLineTest()
        {
            var model = new PlotModel("Dashed line test");

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

        [Example("#10076: Super exponential format")]
        public static PlotModel SuperExponentialFormat()
        {
            var model = new PlotModel("UseSuperExponentialFormat=true and 0");
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 100, 10, 1) { UseSuperExponentialFormat = true });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -100, 100, 20, 10) { UseSuperExponentialFormat = true });
            return model;
        }

        [Example("#10079: AreaSeries draws on top of other elements")]
        public static PlotModel DefaultAnnotationLayer()
        {
            var plotModel1 = new PlotModel("Annotations should be drawn on top by default", "The line annotation should be on top!");
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

        [Example("#10084: AreaSeries should respect CanTrackerInterpolatePoints")]
        public static PlotModel AreaSeries_CanTrackerInterpolatePointsFalse()
        {
            var plotModel1 = new PlotModel("AreaSeries with CanTrackerInterpolatePoints=false");
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

        [Example("#10084: AreaSeries should respect CanTrackerInterpolatePoints")]
        public static PlotModel AreaSeries_CanTrackerInterpolatePointsTrue()
        {
            var plotModel1 = new PlotModel("AreaSeries with CanTrackerInterpolatePoints=true");
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

        [Example("#10118: Empty LineSeries with smoothing")]
        public static PlotModel EmptyLineSeriesWithSmoothing_ThrowsException()
        {
            var plotModel1 = new PlotModel("Empty LineSeries with smoothing");
            plotModel1.Series.Add(new LineSeries { Smooth = true });
            return plotModel1;
        }
    }
}