// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeriesExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Provides examples for the <see cref="StairStepSeries" />.
    /// </summary>
    [Examples("StairStepSeries")]
    public static class StairStepSeriesExamples
    {
        [Example("StairStepSeries")]
        public static PlotModel StairStepSeries()
        {
            return CreateExampleModel(new StairStepSeries());
        }

        [Example("StairStepSeries with labels")]
        public static PlotModel StairStepSeriesWithLabels()
        {
            return CreateExampleModel(new StairStepSeries { LabelFormatString = "{1:0.00}" });
        }

        [Example("StairStepSeries with markers")]
        public static PlotModel StairStepSeriesWithMarkers()
        {
            return CreateExampleModel(new StairStepSeries
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         });
        }

        [Example("StairStepSeries with thin vertical lines")]
        public static PlotModel StairStepSeriesThinVertical()
        {
            return CreateExampleModel(new StairStepSeries
            {
                StrokeThickness = 3,
                VerticalStrokeThickness = 0.4,
                MarkerType = MarkerType.None
            });
        }

        [Example("StairStepSeries with dashed vertical lines")]
        public static PlotModel StairStepSeriesDashedVertical()
        {
            return CreateExampleModel(new StairStepSeries
            {
                VerticalLineStyle = LineStyle.Dash,
                MarkerType = MarkerType.None
            });
        }

        /// <summary>
        /// Creates an example model and fills the specified series with points.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <returns>A plot model.</returns>
        private static PlotModel CreateExampleModel(DataPointSeries series)
        {
            var model = new PlotModel { Title = "StairStepSeries", LegendSymbolLength = 24 };
            series.Title = "sin(x)";
            for (double x = 0; x < Math.PI * 2; x += 0.5)
            {
                series.Points.Add(new DataPoint(x, Math.Sin(x)));
            }

            model.Series.Add(series);
            return model;
        }
    }
}