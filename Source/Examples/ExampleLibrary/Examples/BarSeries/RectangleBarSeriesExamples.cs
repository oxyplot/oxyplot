// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleBarSeriesExamples.cs" company="OxyPlot">
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

    [Examples("RectangleBarSeries")]
    public static class RectangleBarSeriesExamples
    {
        [Example("RectangleBarSeries")]
        public static PlotModel RectangleBarSeries()
        {
            var model = new PlotModel("RectangleBarSeries") { LegendPlacement = LegendPlacement.Outside };

            var s1 = new RectangleBarSeries { Title = "RectangleBarSeries 1" };
            s1.Items.Add(new RectangleBarItem { X0 = 2, X1 = 8, Y0 = 1, Y1 = 4 });
            s1.Items.Add(new RectangleBarItem { X0 = 6, X1 = 12, Y0 = 6, Y1 = 7 });
            model.Series.Add(s1);

            var s2 = new RectangleBarSeries { Title = "RectangleBarSeries 2" };
            s2.Items.Add(new RectangleBarItem { X0 = 2, X1 = 8, Y0 = -4, Y1 = -1 });
            s2.Items.Add(new RectangleBarItem { X0 = 6, X1 = 12, Y0 = -7, Y1 = -6 });
            model.Series.Add(s2);

            return model;
        }
    }
}