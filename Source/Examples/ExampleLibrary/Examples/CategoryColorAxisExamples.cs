// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryColorAxisExamples.cs" company="OxyPlot">
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
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CategoryColorAxis")]
    public class CategoryColorAxisExamples : ExamplesBase
    {
        [Example("CategoryColorAxis")]
        public static PlotModel StandardCategoryColorAxis()
        {
            var plotModel1 = new PlotModel { Title = "CategoryColorAxis" };
            var catAxis = new CategoryColorAxis { Key = "ccc", Palette = OxyPalettes.BlackWhiteRed(12) };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            var ss = new ScatterSeries { ColorAxisKey = catAxis.Key };
            ss.Points.Add(new ScatterPoint(0, 0) { Value = 0 });
            ss.Points.Add(new ScatterPoint(3, 0) { Value = 3 });
            plotModel1.Series.Add(ss);
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

        [Example("Centered ticks, MajorStep = 4")]
        public static PlotModel MajorStep4()
        {
            var plotModel1 = new PlotModel { Title = "Major Step = 4, IsTickCentered = true" };
            var catAxis = new CategoryColorAxis
            {
                Palette = OxyPalettes.BlackWhiteRed(3),
                IsTickCentered = true,
                MajorStep = 4
            };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }
    }
}