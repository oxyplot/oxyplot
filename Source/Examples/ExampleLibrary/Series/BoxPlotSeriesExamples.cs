// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Gets the median.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("BoxPlotSeries"), Tags("Series")]
    public class BoxPlotSeriesExamples
    {
        [Example("BoxPlot")]
        public static PlotModel BoxPlot()
        {
            const int boxes = 10;

            var model = new PlotModel { Title = string.Format("BoxPlot (n={0})", boxes) };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            model.Legends.Add(l);

            var s1 = new BoxPlotSeries
            {
                Title = "BoxPlotSeries",
                BoxWidth = 0.3
            };

            var random = new Random(31);
            for (var i = 0; i < boxes; i++)
            {
                double x = i;
                var points = 5 + random.Next(15);
                var values = new List<double>();
                for (var j = 0; j < points; j++)
                {
                    values.Add(random.Next(0, 20));
                }

                values.Sort();
                var median = GetMedian(values);
                var mean = values.Average();
                int r = values.Count % 2;
                double firstQuartil = GetMedian(values.Take((values.Count + r) / 2));
                double thirdQuartil = GetMedian(values.Skip((values.Count - r) / 2));

                var iqr = thirdQuartil - firstQuartil;
                var step = iqr * 1.5;
                var upperWhisker = thirdQuartil + step;
                upperWhisker = values.Where(v => v <= upperWhisker).Max();
                var lowerWhisker = firstQuartil - step;
                lowerWhisker = values.Where(v => v >= lowerWhisker).Min();

                var outliers = new[] { upperWhisker + random.Next(1, 10), lowerWhisker - random.Next(1, 10) };

                s1.Items.Add(new BoxPlotItem(x, lowerWhisker, firstQuartil, median, thirdQuartil, upperWhisker) { Mean = mean, Outliers =  outliers });
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            return model;
        }

        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static double GetMedian(IEnumerable<double> values)
        {
            var sortedInterval = new List<double>(values);
            sortedInterval.Sort();
            var count = sortedInterval.Count;
            if (count % 2 == 1)
            {
                return sortedInterval[(count - 1) / 2];
            }

            return 0.5 * sortedInterval[count / 2] + 0.5 * sortedInterval[(count / 2) - 1];
        }

        [Example("BoxPlot (minimal data/ink ratio)")]
        public static PlotModel BoxPlot2()
        {
            var model = BoxPlot();
            var boxPlotSeries = (BoxPlotSeries)model.Series[0];
            boxPlotSeries.ShowMedianAsDot = true;
            boxPlotSeries.OutlierType = MarkerType.Cross;
            boxPlotSeries.Fill = OxyColors.Black;
            boxPlotSeries.ShowBox = false;
            boxPlotSeries.WhiskerWidth = 0;
            return model;
        }

        [Example("BoxPlot (dashed line)")]
        public static PlotModel BoxPlot3()
        {
            var model = BoxPlot();
            var boxPlotSeries = (BoxPlotSeries)model.Series[0];
            boxPlotSeries.LineStyle = LineStyle.Dash;
            return model;
        }

        [Example("Outlier type = Cross")]
        public static PlotModel OutlierTypeCross()
        {
            var model = BoxPlot();
            var boxPlotSeries = (BoxPlotSeries)model.Series[0];
            boxPlotSeries.OutlierType = MarkerType.Cross;
            return model;
        }

        [Example("Outlier type = Custom")]
        public static PlotModel OutlierTypeCustom()
        {
            var model = BoxPlot();
            var boxPlotSeries = (BoxPlotSeries)model.Series[0];
            boxPlotSeries.OutlierType = MarkerType.Custom;
            boxPlotSeries.OutlierOutline = new[] { new ScreenPoint(-1, -1), new ScreenPoint(1, 1), new ScreenPoint(-1, 1), new ScreenPoint(1, -1) };
            return model;
        }

        [Example("Michelson-Morley experiment")]
        public static PlotModel MichelsonMorleyExperiment()
        {
            //// http://www.gutenberg.org/files/11753/11753-h/11753-h.htm
            //// http://en.wikipedia.org/wiki/Michelson%E2%80%93Morley_experiment
            //// http://stat.ethz.ch/R-manual/R-devel/library/datasets/html/morley.html

            var model = new PlotModel();

            var boxPlotSeries = new BoxPlotSeries
            {
                Title = "Results",
                Stroke = OxyColors.Black,
                StrokeThickness = 1,
                OutlierSize = 2,
                BoxWidth = 0.4
            };

            // note: approximated data values (not the original values)
            boxPlotSeries.Items.Add(new BoxPlotItem(0, 740, 850, 945, 980, 1070) { Outliers = new[] { 650.0 }});
            boxPlotSeries.Items.Add(new BoxPlotItem(1, 750, 805, 845, 890, 970) { Outliers = new double[] { }});
            boxPlotSeries.Items.Add(new BoxPlotItem(2, 845, 847, 855, 880, 910) { Outliers = new[] { 640.0, 950, 970 }});
            boxPlotSeries.Items.Add(new BoxPlotItem(3, 720, 760, 820, 870, 910) { Outliers = new double[] { }});
            boxPlotSeries.Items.Add(new BoxPlotItem(4, 730, 805, 807, 870, 950) { Outliers = new double[] { }});
            model.Series.Add(boxPlotSeries);
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, LineStyle = LineStyle.Solid, Y = 792.458, Text = "true speed" });
            var categoryAxis = new CategoryAxis
            {
                Title = "Experiment No.",
            };
            categoryAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5" });
            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Speed of light (km/s minus 299,000)", MajorStep = 100, MinorStep = 100 });
            return model;
        }

        [Example("BoxPlot (DateTime axis)")]
        [DocumentationExample("Series/BoxPlotSeries")]
        public static PlotModel BoxPlotSeries_DateTimeAxis()
        {
            var m = new PlotModel();
            var x0 = DateTimeAxis.ToDouble(new DateTime(2013, 05, 04));
            m.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = x0 - 0.9,
                Maximum = x0 + 1.9,
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MinorStep = 1,
                StringFormat = "yyyy-MM-dd"
            });
            var boxPlotSeries = new BoxPlotSeries
            {
                TrackerFormatString = "X: {1:yyyy-MM-dd}\nUpper Whisker: {2:0.00}\nThird Quartil: {3:0.00}\nMedian: {4:0.00}\nFirst Quartil: {5:0.00}\nLower Whisker: {6:0.00}\nMean: {7:0.00}"
            };
            boxPlotSeries.Items.Add(new BoxPlotItem(x0, 10, 14, 16, 20, 22) { Mean = 17, Outliers = new[] { 23.5 }});
            boxPlotSeries.Items.Add(new BoxPlotItem(x0 + 1, 11, 13, 14, 15, 18) { Outliers = new[] { 23.4 }});
            m.Series.Add(boxPlotSeries);
            return m;
        }
    }
}
