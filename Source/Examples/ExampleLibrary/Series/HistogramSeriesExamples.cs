// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Creates example histograms
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System.Collections.Generic;

    [Examples("HistogramSeries"), Tags("Series")]
    public class HistogramSeriesExamples
    {
        [Example("Exponential Distribution")]
        public static PlotModel ExponentialDistribution()
        {
            return CreateExponentialDistribution();
        }

        [Example("Exponential Distribution (transposed)")]
        public static PlotModel ExponentialDistributionTransposed()
        {
            return ExponentialDistribution().Transpose();
        }

        [Example("Label Placement")]
        public static PlotModel HistogramLabelPlacement()
        {
            var model = new PlotModel { Title = "Label Placement" };

            var s1 = new HistogramSeries { LabelPlacement = LabelPlacement.Base, LabelFormatString = "Base", StrokeThickness = 1, LabelMargin = 5 }; 
            var s2 = new HistogramSeries { LabelPlacement = LabelPlacement.Inside, LabelFormatString = "Inside", StrokeThickness = 1, LabelMargin = 5 };
            var s3 = new HistogramSeries { LabelPlacement = LabelPlacement.Middle, LabelFormatString = "Middle", StrokeThickness = 1, LabelMargin = 5 };
            var s4 = new HistogramSeries { LabelPlacement = LabelPlacement.Outside, LabelFormatString = "Outside", StrokeThickness = 1, LabelMargin = 5 };

            s1.Items.Add(new HistogramItem(1, 2, 4));
            s1.Items.Add(new HistogramItem(2, 3, -4));
            s2.Items.Add(new HistogramItem(3, 4, 2));
            s2.Items.Add(new HistogramItem(4, 5, -2));
            s3.Items.Add(new HistogramItem(5, 6, 3));
            s3.Items.Add(new HistogramItem(6, 7, -3));
            s4.Items.Add(new HistogramItem(7, 8, 1));
            s4.Items.Add(new HistogramItem(8, 9, -1));

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Series.Add(s4);

            return model;
        }

        [Example("Label Placement (transposed)")]
        public static PlotModel LabelPlacementTransposed()
        {
            return HistogramLabelPlacement().Transpose();
        }

        [Example("Label Placement (reversed Y Axis)")]
        public static PlotModel LabelPlacementReversed()
        {
            return HistogramLabelPlacement().ReverseYAxis();
        }

        [Example("Label Placement (reversed Y Axis, transposed)")]
        public static PlotModel LabelPlacementReversedTransposed()
        {
            return LabelPlacementReversed().Transpose();
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

            Random rnd = new Random(1);

            HistogramSeries chs = new HistogramSeries();
            chs.Items.AddRange(HistogramHelpers.Collect(SampleExps(rnd, 1.0, n), 0, 5, 15, true));
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

            HistogramSeries chs = new HistogramSeries();

            chs.Items.AddRange(HistogramHelpers.Collect(SampleExps(rnd, 1.0, n), new double[] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1.0, 2.0, 3.0, 4.0, 5.0 }, true));
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

            HistogramSeries chs = new HistogramSeries();
            chs.Items.AddRange(new[] { new HistogramItem(0, 0.5, 10), new HistogramItem(0.75, 1.0, 10) });
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
