// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XkcdExamples.cs" company="OxyPlot">
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
//   Plot examples in XKCD style.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Plot examples in XKCD style.
    /// </summary>
    [Examples("XKCD")]
    public static class XkcdExamples
    {
        /// <summary>
        /// Xkcd style example #1.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Test #1")]
        public static PlotModel Test1()
        {
            var model = new PlotModel
            {
                Title = "XKCD style plot",
                Subtitle = "Install the 'Humor Sans' font for the best experience",
                RenderingDecorator = rc => new XkcdRenderingDecorator(rc)
            };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 50, "sin(x)"));
            return model;
        }


        /// <summary>
        /// Xkcd style example #2.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Test #2")]
        public static PlotModel Test2()
        {
            var model = new PlotModel
            {
                Title = "Test #2",
                RenderingDecorator = rc => new XkcdRenderingDecorator(rc)
            };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 8, Title = "INTENSITY" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "TIME" });

            var s1 = new LineSeries
            {
                Color = OxyColors.Cyan,
                StrokeThickness = 4,
            };

            var s2 = new LineSeries
            {
                Color = OxyColors.White,
                StrokeThickness = 14,
            };

            var s3 = new LineSeries
            {
                Color = OxyColors.Red,
                StrokeThickness = 4,
            };

            int n = 257;
            double x0 = 1;
            double x1 = 9;
            for (int i = 0; i < n; i++)
            {
                var x = x0 + ((x1 - x0) * i / (n - 1));
                var y1 = 1.5 + (10.0 * (Math.Sin(x) * Math.Sin(x) / Math.Sqrt(x)) * Math.Exp(-0.5 * (x - 5.0) * (x - 5.0)));
                var y2 = 3.0 + (10.0 * (Math.Sin(x) * Math.Sin(x) / Math.Sqrt(x)) * Math.Exp(-0.5 * (x - 7.0) * (x - 7.0)));
                s1.Points.Add(new DataPoint(x, y1));
                s2.Points.Add(new DataPoint(x, y2));
                s3.Points.Add(new DataPoint(x, y2));
            }

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);

            return model;
        }
    }
}