// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotSeriesExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
// <summary>
//   Gets the median.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                    BoxWidth = 0.3
                };

            var random = new Random();
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
                int r = values.Count % 2;
                double firstQuartil = GetMedian(values.Take((values.Count + r) / 2));
                double thirdQuartil = GetMedian(values.Skip((values.Count - r) / 2));

                var iqr = thirdQuartil - firstQuartil;
                var step = iqr * 1.5;
                var upperWhisker = thirdQuartil + step;
                upperWhisker = values.Where(v => v <= upperWhisker).Max();
                var lowerWhisker = firstQuartil - step;
                lowerWhisker = values.Where(v => v >= lowerWhisker).Min();

                var outliers = values.Where(v => v > upperWhisker || v < lowerWhisker).ToList();

                s1.Items.Add(new BoxPlotItem(x, lowerWhisker, firstQuartil, median, thirdQuartil, upperWhisker, outliers));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis(AxisPosition.Left));
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

        [Example("BoxPlot (minimal data/ink ratio)")]
        public static PlotModel BoxPlot2()
        {
            var model = BoxPlot();
            var s0 = model.Series[0] as BoxPlotSeries;
            s0.ShowMedianAsDot = true;
            s0.OutlierType = MarkerType.Cross;
            s0.Fill = OxyColors.Black;
            s0.ShowBox = false;
            s0.WhiskerWidth = 0;
            return model;
        }

        [Example("BoxPlot (dashed line)")]
        public static PlotModel BoxPlot3()
        {
            var model = BoxPlot();
            var s0 = model.Series[0] as BoxPlotSeries;
            s0.LineStyle = LineStyle.Dash;
            return model;
        }

        [Example("BoxPlot (different outlier type)")]
        public static PlotModel BoxPlot4()
        {
            var model = BoxPlot();
            var s0 = model.Series[0] as BoxPlotSeries;
            s0.OutlierType = MarkerType.Cross;
            return model;
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
                Title = "Results",
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