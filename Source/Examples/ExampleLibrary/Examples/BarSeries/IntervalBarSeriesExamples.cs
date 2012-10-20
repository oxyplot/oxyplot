// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarSeriesExamples.cs" company="OxyPlot">
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
    using OxyPlot;

    [Examples("IntervalBarSeries")]
    public static class IntervalBarSeriesExamples
    {
        [Example("IntervalBarSeries")]
        public static PlotModel IntervalBarSeries()
        {
            var model = new PlotModel("IntervalBarSeries") { LegendPlacement = LegendPlacement.Outside };

            var s1 = new IntervalBarSeries { Title = "IntervalBarSeries 1" };
            s1.Items.Add(new IntervalBarItem { Start = 6, End = 8 });
            s1.Items.Add(new IntervalBarItem { Start = 4, End = 8 });
            s1.Items.Add(new IntervalBarItem { Start = 5, End = 11 });
            s1.Items.Add(new IntervalBarItem { Start = 4, End = 12 });
            model.Series.Add(s1);
            var s2 = new IntervalBarSeries { Title = "IntervalBarSeries 2" };
            s2.Items.Add(new IntervalBarItem { Start = 8, End = 9 });
            s2.Items.Add(new IntervalBarItem { Start = 8, End = 10 });
            s2.Items.Add(new IntervalBarItem { Start = 11, End = 12 });
            s2.Items.Add(new IntervalBarItem { Start = 12, End = 12.5 });
            model.Series.Add(s2);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Activity A");
            categoryAxis.Labels.Add("Activity B");
            categoryAxis.Labels.Add("Activity C");
            categoryAxis.Labels.Add("Activity D");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { MinimumPadding = 0.1, MaximumPadding = 0.1 };
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}