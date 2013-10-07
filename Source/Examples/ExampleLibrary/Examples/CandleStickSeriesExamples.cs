namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CandleStickSeries")]
    public static class CandleStickSeriesExamples
    {
        [Example("CandleStickSeries")]
        public static PlotModel CandleStickSeries()
        {
            var model = new PlotModel("CandleStickSeries") { LegendSymbolLength = 24 };
            var s1 = new CandleStickSeries("random values")
                {
                    Color = OxyColors.Black,
                };
            var r = new Random(314);
            var price = 100.0;
            for (int x = 0; x < 16; x++)
            {
                price = price + r.NextDouble() + 0.1;
                var high = price + 10 + (r.NextDouble() * 10);
                var low = price - (10 + (r.NextDouble() * 10));
                var open = low + (r.NextDouble() * (high - low));
                var close = low + (r.NextDouble() * (high - low));
                s1.Items.Add(new HighLowItem(x, high, low, open, close));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { MaximumPadding = 0.3, MinimumPadding = 0.3 });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { MaximumPadding = 0.03, MinimumPadding = 0.03 });

            return model;
        }

        [Example("CandleStickSeries (red/green)")]
        public static PlotModel CandleStickSeriesRedGreen()
        {
            var model = CandleStickSeries();
            model.Title = "CandleStickSeries (red/green)";
            var s1 = (CandleStickSeries)model.Series[0];
            s1.IncreasingFill = OxyColors.DarkGreen;
            s1.DecreasingFill = OxyColors.Red;
            s1.ShadowEndColor = OxyColors.Gray;
            s1.Color = OxyColors.Black;
            return model;
        }
    }
}