// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscussionExamples.cs" company="OxyPlot">
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
    using System.Diagnostics.CodeAnalysis;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z0 Discussions")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    public class DiscussionExamples : ExamplesBase
    {
        [Example("#445576: Invisible contour series")]
        public static PlotModel InvisibleContourSeries()
        {
            var model = new PlotModel("Invisible contour series");
            var cs = new ContourSeries
            {
                IsVisible = false,
                ColumnCoordinates = ArrayHelper.CreateVector(-1, 1, 0.05),
                RowCoordinates = ArrayHelper.CreateVector(-1, 1, 0.05)
            };
            cs.Data = ArrayHelper.Evaluate((x, y) => x + y, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Series.Add(cs);
            return model;
        }

        [Example("#461507: StairStepSeries NullReferenceException")]
        public static PlotModel StairStepSeries_NullReferenceException()
        {
            var plotModel1 = new PlotModel("StairStepSeries NullReferenceException");
            plotModel1.Series.Add(new StairStepSeries());
            return plotModel1;
        }

        [Example("#501409: Heatmap interpolation color")]
        public static PlotModel HeatMapSeriesInterpolationColor()
        {
            var data = new double[2, 3];
            data[0, 0] = 10;
            data[0, 1] = 0;
            data[0, 2] = -10;

            var model = new PlotModel("HeatMapSeries");
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = new OxyPalette(OxyColors.Red, OxyColors.Green, OxyColors.Blue) });

            var hms = new HeatMapSeries
            {
                CoordinateDefinition = HeatMapCoordinateDefinition.Center,
                X0 = 0,
                X1 = 3,
                Y0 = 0,
                Y1 = 2,
                Data = data,
                Interpolate = false,
                LabelFontSize = 0.2
            };
            model.Series.Add(hms);
            return model;
        }

        [Example("#522598: Peaks 400x400")]
        public static PlotModel Peaks400()
        {
            return HeatMapSeriesExamples.CreatePeaks(null, true, 400);
        }
    }
}