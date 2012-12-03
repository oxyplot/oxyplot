// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinancialSeriesExamples.cs" company="OxyPlot">
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
using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Financial Series")]
    public static class FinancialSeriesExamples
    {
        [Example("HighLowSeries")]
        public static PlotModel HighLowSeries()
        {
            var model = new PlotModel("HighLowSeries") { LegendSymbolLength = 24 };
            var s1 = new HighLowSeries("random values")
                         {
                             Color = OxyColors.Black,
                         };
            var r = new Random();
            var price = 100.0;
            for (int x = 0; x < 24; x++)
            {
                price = price + r.NextDouble() + 0.1;
                var high = price + 10 + r.NextDouble() * 10;
                var low = price - (10 + r.NextDouble() * 10);
                var open = low + r.NextDouble() * (high - low);
                var close = low + r.NextDouble() * (high - low);
                s1.Items.Add(new HighLowItem(x, high, low, open, close));
            }
            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { MaximumPadding = 0.3, MinimumPadding = 0.3 });

            return model;
        }

        [Example("CandleStickSeries")]
        public static PlotModel CandleStickSeries()
        {
            var model = new PlotModel("CandleStickSeries") { LegendSymbolLength = 24 };
            var s1 = new CandleStickSeries("random values")
                         {
                             Color = OxyColors.Black,
                         };
            var r = new Random();
            var price = 100.0;
            for (int x = 0; x < 16; x++)
            {
                price = price + r.NextDouble() + 0.1;
                var high = price + 10 + r.NextDouble() * 10;
                var low = price - (10 + r.NextDouble() * 10);
                var open = low + r.NextDouble() * (high - low);
                var close = low + r.NextDouble() * (high - low);
                s1.Items.Add(new HighLowItem(x, high, low, open, close));
            }
            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { MaximumPadding = 0.3, MinimumPadding = 0.3 });

            return model;
        }
    }
}