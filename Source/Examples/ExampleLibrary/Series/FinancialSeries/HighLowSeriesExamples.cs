// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("HighLowSeries"), Tags("Series")]
    public static class HighLowSeriesExamples
    {
        [Example("HighLowSeries")]
        public static PlotModel HighLowSeries()
        {
            var model = new PlotModel { Title = "HighLowSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new HighLowSeries { Title = "HighLowSeries 1", Color = OxyColors.Black, };
            var r = new Random(314);
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
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MaximumPadding = 0.3, MinimumPadding = 0.3 });

            return model;
        }

        [Example("HighLowSeries (reversed X Axis)")]
        public static PlotModel HighLowSeriesReversedXAxis()
        {
            return HighLowSeries().ReverseXAxis();
        }

        [Example("HighLowSeries (DateTime axis)")]
        public static PlotModel HighLowSeriesDateTimeAxis()
        {
            var m = new PlotModel();
            var x0 = DateTimeAxis.ToDouble(new DateTime(2013, 05, 04));
            var a = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = x0 - 0.9,
                Maximum = x0 + 1.9,
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MinorStep = 1,
                StringFormat = "yyyy-MM-dd"
            };
            m.Axes.Add(a);
            var s = new HighLowSeries
            {
                TrackerFormatString =
                    "X: {1:yyyy-MM-dd}\nHigh: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}"
            };

            s.Items.Add(new HighLowItem(x0, 14, 10, 13, 12.4));
            s.Items.Add(new HighLowItem(x0 + 1, 17, 8, 12.4, 16.3));
            m.Series.Add(s);

            return m;
        }
    }
}
