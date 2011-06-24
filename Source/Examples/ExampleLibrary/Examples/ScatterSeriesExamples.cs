using System;
using System.Collections.Generic;
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

        [Example("Random points with random size")]
        public static PlotModel RandomSize()
        {
            return RandomSize(1000, 8);
        }

        public static PlotModel RandomSize(int n, int binsize)
        {
            var model = new PlotModel(string.Format("ScatterSeries with random MarkerSize (n={0})", n), "BinSize = " + binsize);

            var s1 = new ScatterSeries("Series 1")
            {
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

        [Example("Random points with least squares fit")]
        public static PlotModel RandomWithFit()
        {
            const int n = 20;
            var model = new PlotModel(string.Format("Random data (n={0})", n)) { LegendPosition = LegendPosition.LeftTop };

            var s1 = new ScatterSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble() * 10;
                y += 1 + random.NextDouble();
                var p = new DataPoint(x, y);
                s1.Points.Add(p);
            }
            model.Series.Add(s1);
            double a, b;
            LeastSquaresFit(s1.Points, out a, out b);
            model.Annotations.Add(new LineAnnotation { Slope = a, Intercept = b, Text = "Least squares fit" });
            return model;
        }

        /// <summary>
        /// Calculates the Least squares fit of a list of DataPoints.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="a">The slope.</param>
        /// <param name="b">The intercept.</param>
        public static void LeastSquaresFit(IEnumerable<DataPoint> points, out double a, out double b)
        {
            // http://en.wikipedia.org/wiki/Least_squares
            // http://mathworld.wolfram.com/LeastSquaresFitting.html
            // http://web.cecs.pdx.edu/~gerry/nmm/course/slides/ch09Slides4up.pdf
            
            double Sx = 0;
            double Sy = 0;
            double Sxy = 0;
            double Sxx = 0;
            int m = 0;
            foreach (var p in points)
            {
                Sx += p.X;
                Sy += p.Y;
                Sxy += p.X * p.Y;
                Sxx += p.X * p.X;
                m++;
            }
            double d = Sx * Sx - m * Sxx;
            a = 1 / d * (Sx * Sy - m * Sxy);
            b = 1 / d * (Sx * Sxy - Sxx * Sy);
        }

        [Example("Scatter plot using a LineSeries with markers only")]
        public static PlotModel MarkersOnly()
        {
            return MarkersOnly(31000);
        }

        public static PlotModel MarkersOnly(int n)
        {
            var model = new PlotModel(string.Format("LineSeries with markers only (n={0})", n));

            var s1 = new LineSeries("Series 1") { StrokeThickness = 0, MarkerType = MarkerType.Square, MarkerFill = OxyColors.Blue, MarkerStrokeThickness = 0 };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint(random.NextDouble(), random.NextDouble()));
            }
            model.Series.Add(s1);
            return model;
        }
    }
}