using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleLibrary
{
    using OxyPlot;

    [Examples("BoxPlotSeries")]
    public class BoxPlotSeriesExamples
    {
        [Example("BoxPlot")]
        public static PlotModel BoxPlot()
        {
            const int boxes = 10;

            var model = new PlotModel(string.Format("BoxPlot (n={0})", boxes)) { LegendPlacement = LegendPlacement.Outside };

            var s1 = new BoxPlotSeries
                {
                    Title = "BoxPlotSeries",
                    Fill = OxyColors.Wheat,
                    Stroke = OxyColors.Blue,
                    StrokeThickness = 2,
                    OutlierSize = 2,
                    BoxWidth = 0.3
                };

            var random = new Random();
            for (var i = 0; i < boxes; i++)
            {
                double x = i;
                var points = 5 + random.Next(15);
                var yValues = new List<double>();
                for (var j = 0; j < points; j++)
                {
                    yValues.Add(random.Next(0, 20));
                }

                yValues.Sort();
                var median = GetMedian(yValues);
                int r = yValues.Count % 2;
                double firstQuartil = GetMedian(yValues.Take((yValues.Count + r) / 2));
                double thirdQuartil = GetMedian(yValues.Skip((yValues.Count - r) / 2));

                var iqr = thirdQuartil - firstQuartil;
                var step = iqr * 1.5;
                var upperWhisker = thirdQuartil + step;
                upperWhisker = yValues.Where(v => v <= upperWhisker).Max();
                var lowerWhisker = firstQuartil - step;
                lowerWhisker = yValues.Where(v => v >= lowerWhisker).Min();

                var outliers = yValues.Where(v => v > upperWhisker || v < lowerWhisker).ToList();

                s1.Items.Add(new BoxPlotItem(x, lowerWhisker, firstQuartil, median, thirdQuartil, upperWhisker, outliers));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { MinimumPadding = 0.1, MaximumPadding = 0.1 });
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

        [Example("Michelson-Morley experiment")]
        public static PlotModel MichelsonMorleyExperiment()
        {
            //// http://www.gutenberg.org/files/11753/11753-h/11753-h.htm
            //// http://en.wikipedia.org/wiki/Michelson%E2%80%93Morley_experiment
            //// http://stat.ethz.ch/R-manual/R-devel/library/datasets/html/morley.html
            
            var model = new PlotModel();

            var s1 = new BoxPlotSeries
            {
                Stroke = OxyColors.Black,
                StrokeThickness = 1,
                OutlierSize = 2,
                BoxWidth = 0.4
            };

            // note: approximated data values (not the original values)
            s1.Items.Add(new BoxPlotItem(0, 740, 850, 945, 980, 1070, new[] { 650.0 }));
            s1.Items.Add(new BoxPlotItem(1, 750, 805, 845, 890, 970, new double[] { }));
            s1.Items.Add(new BoxPlotItem(2, 845, 847, 855, 880, 910, new[] { 640.0, 950, 970 }));
            s1.Items.Add(new BoxPlotItem(3, 720, 760, 820, 870, 910, new double[] { }));
            s1.Items.Add(new BoxPlotItem(4, 730, 805, 807, 870, 950, new double[] { }));
            model.Series.Add(s1);
            model.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Horizontal, LineStyle = LineStyle.Solid, Y = 792.458, Text = "true speed" });
            model.Axes.Add(new CategoryAxis("Experiment No.", "1", "2", "3", "4", "5"));
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Speed of light (km/s minus 299,000)") { MajorStep = 100, MinorStep = 100 });
            return model;
        }
    }
}
