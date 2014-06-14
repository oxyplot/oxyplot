// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxisExamples.cs" company="OxyPlot">
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
    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("CategoryAxis")]
    public static class CategoryAxisExamples
    {
        [Example("Standard")]
        public static PlotModel StandardCategoryAxis()
        {
            var plotModel1 = new PlotModel { Title = "Standard" };
            var catAxis = new CategoryAxis();
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

        [Example("MajorStep")]
        public static PlotModel MajorStepCategoryAxis()
        {
            var plotModel1 = new PlotModel { Title = "Major Step = 4, IsTickCentered = true" };
            var catAxis = new CategoryAxis { IsTickCentered = true, MajorStep = 4 };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }
    }
}