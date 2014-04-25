// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeriesExamples.cs" company="OxyPlot">
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
// <summary>
//   Provides examples for the <see cref="TwoColorLineSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Provides examples for the <see cref="TwoColorLineSeries" />.
    /// </summary>
    [Examples("TwoColorLineSeries")]
    public class TwoColorLineSeriesExamples : ExamplesBase
    {
        /// <summary>
        /// Creates an example showing temperatures by a red/blue line.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures")]
        public static PlotModel TwoColorLineSeries()
        {
            var model = new PlotModel { Title = "TwoColorLineSeries", LegendSymbolLength = 24 };
            var s1 = new TwoColorLineSeries
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
            var temperatures = new[] { 5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0, -5 };

            for (int i = 0; i < temperatures.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, ExtraGridlines = new[] { 0.0 } });

            return model;
        }
    }
}