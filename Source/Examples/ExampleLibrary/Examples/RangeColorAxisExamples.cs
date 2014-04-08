// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxisExamples.cs" company="OxyPlot">
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
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RangeColorAxis")]
    public class RangeColorAxisExamples : ExamplesBase
    {
        [Example("ScatterSeries with RangeColorAxis")]
        public static PlotModel RangeColorAxis()
        {
            int n = 1000;
            var model = new PlotModel
                {
                    Title = string.Format("ScatterSeries and RangeColorAxis (n={0})", n),
                    Background = OxyColors.LightGray
                };
            var rca = new RangeColorAxis { Position = AxisPosition.Right, Maximum = 2, Minimum = -2 };
            rca.AddRange(0, 0.5, OxyColors.Blue);
            rca.AddRange(-0.2, -0.1, OxyColors.Red);
            model.Axes.Add(rca);

            var s1 = new ScatterSeries { MarkerType = MarkerType.Square, MarkerSize = 6, };

            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble() * 2.2 - 1.1;
                s1.Points.Add(new ScatterPoint(x, random.NextDouble()) { Value = x });
            }

            model.Series.Add(s1);
            return model;
        }
    }
}