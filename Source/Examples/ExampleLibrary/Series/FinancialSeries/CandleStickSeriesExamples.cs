// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CandleStickSeries"), Tags("Series")]
    public static class CandleStickSeriesExamples
    {
        [Example("Large Data Set (wide window)")]
        public static Example LargeDataSetWide()
        {
            var pm = new PlotModel { Title = "Large Data Set (wide window)" };

            var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var n = 1000000;
            var items = HighLowItemGenerator.MRProcess(n).ToArray();
            var series = new CandleStickSeries
                             {
                                 Color = OxyColors.Black,
                                 IncreasingColor = OxyColors.DarkGreen,
                                 DecreasingColor = OxyColors.Red,
                                 DataFieldX = "Time",
                                 DataFieldHigh = "H",
                                 DataFieldLow = "L",
                                 DataFieldOpen = "O",
                                 DataFieldClose = "C",
                                 TrackerFormatString =
                                     "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                                 ItemsSource = items
                             };

            timeSpanAxis1.Minimum = items[n - 200].X;
            timeSpanAxis1.Maximum = items[n - 130].X;

            linearAxis1.Minimum = items.Skip(n - 200).Take(70).Select(x => x.Low).Min();
            linearAxis1.Maximum = items.Skip(n - 200).Take(70).Select(x => x.High).Max();

            pm.Series.Add(series);

            timeSpanAxis1.AxisChanged += (sender, e) => AdjustYExtent(series, timeSpanAxis1, linearAxis1);

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }

        [Example("Large Data Set (narrow window)")]
        public static Example LargeDataSetNarrow()
        {
            var pm = new PlotModel { Title = "Large Data Set (narrow window)" };

            var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var n = 1000000;
            var items = HighLowItemGenerator.MRProcess(n).ToArray();
            var series = new CandleStickSeries
                             {
                                 Color = OxyColors.Black,
                                 IncreasingColor = OxyColors.DarkGreen,
                                 DecreasingColor = OxyColors.Red,
                                 TrackerFormatString =
                                     "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                                 ItemsSource = items
                             };


            timeSpanAxis1.Minimum = items[0].X;
            timeSpanAxis1.Maximum = items[29].X;

            linearAxis1.Minimum = items.Take(30).Select(x => x.Low).Min();
            linearAxis1.Maximum = items.Take(30).Select(x => x.High).Max();

            pm.Series.Add(series);

            timeSpanAxis1.AxisChanged += (sender, e) => AdjustYExtent(series, timeSpanAxis1, linearAxis1);

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }

        [Example("Small Set")]
        public static Example SmallDataSet()
        {
            var pm = new PlotModel { Title = "Small Data Set" };

            var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
            pm.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            pm.Axes.Add(linearAxis1);
            var n = 100;
            var items = HighLowItemGenerator.MRProcess(n).ToArray();
            var series = new CandleStickSeries
                             {
                                 Color = OxyColors.Black,
                                 IncreasingColor = OxyColors.DarkGreen,
                                 DecreasingColor = OxyColors.Red,
                                 DataFieldX = "X",
                                 DataFieldHigh = "High",
                                 DataFieldLow = "Low",
                                 DataFieldOpen = "Open",
                                 DataFieldClose = "Close",
                                 TrackerFormatString =
                                     "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                                 ItemsSource = items
                             };

            pm.Series.Add(series);

            timeSpanAxis1.AxisChanged += (sender, e) => AdjustYExtent(series, timeSpanAxis1, linearAxis1);

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }

        [Example("Simple CandleStickSeries example")]
        public static PlotModel SimpleExample()
        {
            var startTimeValue = DateTimeAxis.ToDouble(new DateTime(2016, 1, 1));
            var pm = new PlotModel { Title = "Simple CandleStickSeries example" };
            pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = startTimeValue - 7, Maximum = startTimeValue + 7 });
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var series = new CandleStickSeries();
            series.Items.Add(new HighLowItem(startTimeValue, 100, 80, 92, 94));
            series.Items.Add(new HighLowItem(startTimeValue + 1, 102, 77, 94, 93));
            series.Items.Add(new HighLowItem(startTimeValue + 2, 99, 85, 93, 93));
            pm.Series.Add(series);
            return pm;
        }

        /// <summary>
        /// Adjusts the Y extent.
        /// </summary>
        /// <param name="series">Series.</param>
        /// <param name="xaxis">Xaxis.</param>
        /// <param name="yaxis">Yaxis.</param>
        private static void AdjustYExtent(CandleStickSeries series, DateTimeAxis xaxis, LinearAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }
}
