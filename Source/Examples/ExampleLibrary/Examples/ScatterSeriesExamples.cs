using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("ScatterSeries")]
    public static class ScatterSeriesExamples
    {

        [Example("Random points")]
        public static PlotModel RandomScatter()
        {
            return RandomScatter(31000, 8);
        }

        public static PlotModel RandomScatter(int n, int binsize)
        {
            var model = new PlotModel(string.Format("ScatterSeries (n={0})", n), "BinSize = " + binsize);

            var s1 = new ScatterSeries("Series 1")
            {
                MarkerType = MarkerType.Diamond,
                MarkerFill = OxyColors.Red,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 0,
                BinSize = binsize
            };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint(random.NextDouble(), random.NextDouble()));
            }
            model.Series.Add(s1);
            return model;
        }

        [Example("Random size")]
        public static PlotModel RandomSize()
        {
            return RandomSize(1000, 8);
        }

        public static PlotModel RandomSize(int n, int binsize)
        {
            var model = new PlotModel(string.Format("ScatterSeries with random MarkerSize (n={0})", n), "BinSize = " + binsize);

            var s1 = new ScatterSeries("Series 1")
            {
                MarkerFill = OxyColors.Red,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 0,
                BinSize = binsize
            };
            var random = new Random();
            double[] sizes = new double[n];
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint(random.NextDouble(), random.NextDouble()));
                sizes[i] = 4 + 10 * random.NextDouble();
            }
            s1.MarkerSizes = sizes;
            model.Series.Add(s1);
            return model;
        }
    }
}