// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinancialSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
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