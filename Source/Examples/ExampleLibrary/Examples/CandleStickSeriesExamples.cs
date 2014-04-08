// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickSeriesExamples.cs" company="OxyPlot">
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
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left,  MaximumPadding = 0.3, MinimumPadding = 0.3 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom,  MaximumPadding = 0.03, MinimumPadding = 0.03 });

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

        [Example("Minute data (DateTimeAxis)")]
        public static PlotModel MinuteData_DateTimeAxis()
        {
            var pm = new PlotModel("Minute Data (DateTimeAxis)");

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
                DataFieldX = "Time",
                DataFieldHigh = "H",
                DataFieldLow = "L",
                DataFieldOpen = "O",
                DataFieldClose = "C",
                TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                ItemsSource = lst
            };
            pm.Series.Add(candleStickSeries);
            return pm;
        }

        private static List<MinuteRec> lst = new List<MinuteRec>
            {
                new MinuteRec { QTime = TimeSpan.Parse("06:31:00"), O = 1672.5000, H = 1673.5000, L = 1671.7500, C = 1672.7500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:32:00"), O = 1672.5000, H = 1673.5000, L = 1672.5000, C = 1672.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:33:00"), O = 1672.5000, H = 1672.7500, L = 1670.7500, C = 1671.2500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:34:00"), O = 1671.2500, H = 1671.2500, L = 1670.2500, C = 1670.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:35:00"), O = 1670.7500, H = 1671.7500, L = 1670.5000, C = 1671.2500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:36:00"), O = 1671.0000, H = 1672.5000, L = 1671.0000, C = 1672.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:37:00"), O = 1672.5000, H = 1673.0000, L = 1672.0000, C = 1673.0000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:38:00"), O = 1672.7500, H = 1673.2500, L = 1672.5000, C = 1672.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:39:00"), O = 1672.5000, H = 1672.7500, L = 1671.2500, C = 1671.2500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:40:00"), O = 1671.2500, H = 1672.5000, L = 1671.0000, C = 1672.0000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:41:00"), O = 1672.2500, H = 1672.5000, L = 1671.2500, C = 1672.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:42:00"), O = 1672.2500, H = 1672.5000, L = 1671.5000, C = 1671.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:43:00"), O = 1671.5000, H = 1671.7500, L = 1670.5000, C = 1671.0000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:44:00"), O = 1670.7500, H = 1671.7500, L = 1670.7500, C = 1671.7500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:45:00"), O = 1672.0000, H = 1672.2500, L = 1671.5000, C = 1671.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:46:00"), O = 1671.7500, H = 1671.7500, L = 1671.0000, C = 1671.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:47:00"), O = 1671.7500, H = 1672.2500, L = 1671.5000, C = 1671.7500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:48:00"), O = 1671.7500, H = 1672.7500, L = 1671.7500, C = 1672.5000 },
                new MinuteRec { QTime = TimeSpan.Parse("06:49:00"), O = 1672.2500, H = 1673.7500, L = 1672.2500, C = 1673.7500 },
                new MinuteRec { QTime = TimeSpan.Parse("06:50:00"), O = 1673.7500, H = 1675.0000, L = 1673.5000, C = 1675.0000 }
            };

        [Example("Minute data (TimeSpanAxis)")]
        public static PlotModel MinuteData_TimeSpan()
        {

            var pm = new PlotModel("Minute Data (TimeSpanAxis)");

            var timeSpanAxis1 = new TimeSpanAxis { Position = AxisPosition.Bottom, StringFormat = "hh:mm" };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var candleStickSeries = new CandleStickSeries
            {
                CandleWidth = 5,
                Color = OxyColors.DarkGray,
                IncreasingFill = OxyColors.DarkGreen,
                DecreasingFill = OxyColors.Red,
                DataFieldX = "QTime",
                DataFieldHigh = "H",
                DataFieldLow = "L",
                DataFieldOpen = "O",
                DataFieldClose = "C",
                TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                ItemsSource = lst
            };
            pm.Series.Add(candleStickSeries);
            return pm;
        }

        public class MinuteRec
        {
            public DateTime Time { get { return new DateTime(2013, 10, 8) + this.QTime; } }
            public TimeSpan QTime { get; set; }
            public double H { get; set; }
            public double L { get; set; }
            public double O { get; set; }
            public double C { get; set; }
        }
    }
}