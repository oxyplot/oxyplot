// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContinuousHistogramSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Creates example histograms
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System.Collections.Generic;

    [Examples("ContinuousHistogramSeries"), Tags("Series")]
    public class ContinuousHistogramSeriesExamples
    {
        [Example("Exponential Distribution")]
        public static PlotModel ExponentialDistribution()
        {
            return CreateExponentialDistribution();
        }

        [Example("Custom Bins")]
        public static PlotModel CustomBins()
        {
            return CreateExponentialDistributionCustomBins();
        }

        [Example("Disconnected Bins")]
        public static PlotModel DisconnectedBins()
        {
            return CreateDisconnectedBins();
        }

        public static PlotModel CreateExponentialDistribution(double mean = 1, int n = 10000)
        {
            var model = new PlotModel { Title = "Exponential Distribution", Subtitle = "Uniformly distributed bins (" + n + " samples)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random();

            ContinuousHistogramSeries chs = new ContinuousHistogramSeries();
            chs.Items.AddRange(ContinuousHistogramHelpers.Collect(SampleExps(rnd, 1.0, n), 0, 5, 15, true));
            chs.StrokeThickness = 1;
            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateExponentialDistributionCustomBins(double mean = 1, int n = 50000)
        {
            var model = new PlotModel { Title = "Exponential Distribution", Subtitle = "Custom bins (" + n + " samples)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random();

            ContinuousHistogramSeries chs = new ContinuousHistogramSeries();

            chs.Items.AddRange(ContinuousHistogramHelpers.Collect(SampleExps(rnd, 1.0, n), new double[] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1.0, 2.0, 3.0, 4.0, 5.0 }, true));
            chs.StrokeThickness = 1;
            chs.FillColor = OxyColors.Purple;
            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateDisconnectedBins()
        {
            var model = new PlotModel { Title = "Disconnected Bins" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Representation" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random();

            ContinuousHistogramSeries chs = new ContinuousHistogramSeries();
            chs.Items.AddRange(new[] { new ContinuousHistogramItem(0, 0.5, 10), new ContinuousHistogramItem(0.75, 1.0, 10) });
            chs.LabelFormatString = "{0:0.00}";
            chs.LabelPlacement = LabelPlacement.Middle;
            model.Series.Add(chs);

            return model;
        }
        
        private static IEnumerable<double> SampleExps(Random rnd, double mean, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return SampleExp(rnd, mean);
            }
        }

        private static double SampleExp(Random rnd, double mean)
        {
            return Math.Log(1.0 - rnd.NextDouble()) / -mean;
        }
    }
}
