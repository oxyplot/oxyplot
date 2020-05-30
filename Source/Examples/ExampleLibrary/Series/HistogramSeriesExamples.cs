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
    using System.Linq;

    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System.Collections.Generic;

    [Examples("HistogramSeries"), Tags("Series")]
    public class HistogramSeriesExamples
    {
        [Example("Exponential Distribution")]
        [DocumentationExample("Series/HistogramSeries")]
        public static PlotModel ExponentialDistribution()
        {
            return CreateExponentialDistribution();
        }

        [Example("Label Placement")]
        public static PlotModel HistogramLabelPlacement()
        {
            var model = new PlotModel { Title = "Label Placement" };

            var s1 = new HistogramSeries { LabelPlacement = LabelPlacement.Base, LabelFormatString = "Base", StrokeThickness = 1, LabelMargin = 5 };
            var s2 = new HistogramSeries { LabelPlacement = LabelPlacement.Inside, LabelFormatString = "Inside", StrokeThickness = 1, LabelMargin = 5 };
            var s3 = new HistogramSeries { LabelPlacement = LabelPlacement.Middle, LabelFormatString = "Middle", StrokeThickness = 1, LabelMargin = 5 };
            var s4 = new HistogramSeries { LabelPlacement = LabelPlacement.Outside, LabelFormatString = "Outside", StrokeThickness = 1, LabelMargin = 5 };

            s1.Items.Add(new HistogramItem(1, 2, 4, 4));
            s1.Items.Add(new HistogramItem(2, 3, -4, 4));
            s2.Items.Add(new HistogramItem(3, 4, 2, 2));
            s2.Items.Add(new HistogramItem(4, 5, -2, 2));
            s3.Items.Add(new HistogramItem(5, 6, 3, 3));
            s3.Items.Add(new HistogramItem(6, 7, -3, 3));
            s4.Items.Add(new HistogramItem(7, 8, 1, 1));
            s4.Items.Add(new HistogramItem(8, 9, -1, -1));

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);
            model.Series.Add(s4);

            return model;
        }

        [Example("Label Placement (reversed Y Axis)")]
        public static PlotModel LabelPlacementReversed()
        {
            return HistogramLabelPlacement().ReverseYAxis();
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

        [Example("Normal Distribution Three Colors")]
        public static PlotModel NormalDistribution()
        {
            return CreateNormalDistribution();
        }

        [Example("Individual Bin Colors")]
        public static PlotModel IndividualBinColors()
        {
            return CreateIndividualBinColors();
        }

        [Example("Custom Item Mapping")]
        public static PlotModel CustomItemMapping()
        {
            var model = new PlotModel { Title = "Custom Item Mapping" };

            var s = new HistogramSeries { Mapping = obj => (HistogramItem)obj, TrackerFormatString = "{Description}"};
            s.Items.Add(new CustomHistogramItem(1, 2, 4, 4, "Item 1"));
            s.Items.Add(new CustomHistogramItem(2, 3, -4, 4, "Item 2"));
            s.Items.Add(new CustomHistogramItem(3, 4, 2, 2, "Item 3"));
            s.Items.Add(new CustomHistogramItem(4, 5, -2, 2, "Item 4"));
            model.Series.Add(s);

            return model;
        }

        public class CustomHistogramItem : HistogramItem
        {
            public CustomHistogramItem(double rangeStart, double rangeEnd, double area, int count, string description)
                : base(rangeStart, rangeEnd, area, count)
            {
                this.Description = description;
            }

            public string Description { get; }
        }

        public static PlotModel CreateExponentialDistribution(double mean = 1, int n = 10000)
        {
            var model = new PlotModel { Title = "Exponential Distribution", Subtitle = "Uniformly distributed bins (" + n + " samples)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random(1);

            HistogramSeries chs = new HistogramSeries();

            var binningOptions = new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
            var binBreaks = HistogramHelpers.CreateUniformBins(0, 5, 15);
            chs.Items.AddRange(HistogramHelpers.Collect(SampleExps(rnd, mean, n), binBreaks, binningOptions));
            chs.StrokeThickness = 1;
            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateExponentialDistributionCustomBins(double mean = 1, int n = 50000)
        {
            var model = new PlotModel { Title = "Exponential Distribution", Subtitle = "Custom bins (" + n + " samples)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random(1);

            HistogramSeries chs = new HistogramSeries();

            var binningOptions = new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
            chs.Items.AddRange(HistogramHelpers.Collect(SampleExps(rnd, mean, n), new double[] { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1.0, 2.0, 3.0, 4.0, 5.0 }, binningOptions));
            chs.StrokeThickness = 1;
            chs.FillColor = OxyColors.Purple;
            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateNormalDistribution(double mean = 0, double std = 1, int n = 1000000)
        {
            var model = new PlotModel { Title = $"Normal Distribution (μ={mean}, σ={std})", Subtitle = "95% of the distribution (" + n + " samples)" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            Random rnd = new Random(1);

            HistogramSeries chs = new HistogramSeries();
            var binningOptions = new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
            var binBreaks = HistogramHelpers.CreateUniformBins(-std * 4, std * 4, 100);
            chs.Items.AddRange(HistogramHelpers.Collect(SampleNormal(rnd, mean, std, n), binBreaks, binningOptions));
            chs.StrokeThickness = 1;

            double LimitHi = mean + 1.96 * std;
            double LimitLo = mean - 1.96 * std;
            OxyColor ColorHi = OxyColors.DarkRed;
            OxyColor ColorLo = OxyColors.DarkRed;

            chs.ColorMapping = (item) =>
            {
                if (item.RangeCenter > LimitHi)
                {
                    return ColorHi;
                }
                else if (item.RangeCenter < LimitLo)
                {
                    return ColorLo;
                }
                return chs.ActualFillColor;
            };

            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateDisconnectedBins()
        {
            var model = new PlotModel { Title = "Disconnected Bins" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Representation" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x" });

            HistogramSeries chs = new HistogramSeries();
            chs.Items.AddRange(new[] { new HistogramItem(0, 0.5, 10, 7), new HistogramItem(0.75, 1.0, 10, 7) });
            chs.LabelFormatString = "{0:0.00}";
            chs.LabelPlacement = LabelPlacement.Middle;
            model.Series.Add(chs);

            return model;
        }

        public static PlotModel CreateIndividualBinColors(double mean = 1, int n = 10000)
        {
            var model = new PlotModel { Title = "Individual Bin Colors", Subtitle = "Minimum is Red, Maximum is Green" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Observation" });

            Random rnd = new Random(1);

            HistogramSeries chs = new HistogramSeries() { FillColor = OxyColors.Gray, RenderInLegend = true, Title = "Measurements" };

            var binningOptions = new BinningOptions(BinningOutlierMode.CountOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.ExcludeExtremeValues);
            var binBreaks = HistogramHelpers.CreateUniformBins(0, 10, 20);
            var bins = HistogramHelpers.Collect(SampleUniform(rnd, 0, 10, 1000), binBreaks, binningOptions).OrderBy(b => b.Count).ToArray();
            bins.First().Color = OxyColors.Red;
            bins.Last().Color = OxyColors.Green;
            chs.Items.AddRange(bins);
            chs.StrokeThickness = 1;
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

        private static IEnumerable<double> SampleNormal(Random rnd, double mean, double std, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return SampleNormal(rnd, mean, std);
            }
        }

        private static double SampleNormal(Random rnd, double mean, double std)
        {
            // http://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
            var u1 = 1.0 - rnd.NextDouble();
            var u2 = rnd.NextDouble();
            return Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2) * std + mean;
        }

        private static IEnumerable<double> SampleUniform(Random rnd, double min, double max, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return rnd.NextDouble() * (max - min) + min;
            }
        }
    }
}
