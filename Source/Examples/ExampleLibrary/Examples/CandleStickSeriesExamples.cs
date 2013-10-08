namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

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

        [Example("Minute data")]
        public static PlotModel Test()
        {
            var recordings = new List<Recording>
                {
                    new Recording { QTime = new DateTime(2013, 10, 8, 9, 0, 0), O = 100, C = 103, L = 97, H = 104 },
                    new Recording { QTime = new DateTime(2013, 10, 8, 9, 1, 0), O = 103, C = 102, L = 97, H = 107 },
                    new Recording { QTime = new DateTime(2013, 10, 8, 9, 2, 0), O = 102, C = 97, L = 93, H = 104 },
                    new Recording { QTime = new DateTime(2013, 10, 8, 10, 0, 0), O = 102, C = 97, L = 93, H = 104 }
                };

            var pm = new PlotModel("Minute Data");

            var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "hh:mm" };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var candleStickSeries = new CandleStickSeries
            {
                CandleWidth = 6,
                Color = OxyColors.Black,
                IncreasingFill = OxyColors.DarkGreen,
                DecreasingFill = OxyColors.Red,
                DataFieldX = "QTime",
                DataFieldHigh = "H",
                DataFieldLow = "L",
                DataFieldOpen = "O",
                DataFieldClose = "C",
                TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                ItemsSource = recordings
            };
            pm.Series.Add(candleStickSeries);
            return pm;
        }

        public class Recording
        {
            public DateTime QTime { get; set; }
            public double H { get; set; }
            public double L { get; set; }
            public double O { get; set; }
            public double C { get; set; }
        }
    }
}