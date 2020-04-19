// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   A performance test program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PerformanceTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// A performance test program.
    /// </summary>
    /// <remarks>
    /// Build with the Release configuration.
    /// To be used with a profiler or as a standalone program (remember to run outside the Visual Studio IDE).
    /// </remarks>
    public class Program
    {
        /// <summary>
        /// The program entry point.
        /// </summary>
        public static void Main()
        {
            var testModels = new Dictionary<string, PlotModel>
            {
                { "LineSeries with 100000 points", CreateModel(100000) },
                { "LineSeries with 100000 points in ItemsSource", CreateModel2(100000) }
            };

            foreach (var kvp in testModels)
            {
                Console.WriteLine(kvp.Key);
                TestModelUpdate(kvp.Value);
                TestModelRender(kvp.Value);
                Console.WriteLine();
            }

            Console.WriteLine("DrawClippedLine test:");
            var t0 = TestDrawClippedLine(10000, 1000, false);
            var t1 = TestDrawClippedLine(10000, 1000, true);
            Console.WriteLine("{0:P1}", (t0 - t1) / t0);
            Console.ReadKey();
        }

        public static double TestModelUpdate(PlotModel model, int m = 1000)
        {
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < m; i++)
            {
                ((IPlotModel)model).Update(true);
            }

            stopwatch.Stop();
            Console.WriteLine("Update: {0}", (double)stopwatch.ElapsedMilliseconds);
            return stopwatch.ElapsedMilliseconds;
        }

        public static double TestModelRender(PlotModel model, int m = 100)
        {
            var rc = new EmptyRenderContext();
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < m; i++)
            {
                ((IPlotModel)model).Render(rc, new OxyRect(0, 0, 800, 600));
            }

            stopwatch.Stop();
            Console.WriteLine("Render: {0}", (double)stopwatch.ElapsedMilliseconds);
            return stopwatch.ElapsedMilliseconds;
        }

        private static PlotModel CreateModel(int n)
        {
            var model = new PlotModel();
            var series = new LineSeries();
            for (int i = 0; i < n; i++)
            {
                series.Points.Add(new DataPoint(i, Math.Sin(i)));
            }

            model.Series.Add(series);
            ((IPlotModel)model).Update(true);
            return model;
        }

        private static PlotModel CreateModel2(int n)
        {
            var points = new List<DataPoint>();
            for (int i = 0; i < n; i++)
            {
                points.Add(new DataPoint(i, Math.Sin(i)));
            }

            var model = new PlotModel();
            var series = new LineSeries();
            series.ItemsSource = points;

            model.Series.Add(series);
            ((IPlotModel)model).Update(true);
            return model;
        }

        /// <summary>
        /// Tests the <see cref="RenderingExtensions.DrawClippedLine" /> method.
        /// </summary>
        /// <param name="n">The number of points.</param>
        /// <param name="m">The number of repetitions.</param>
        /// <param name="useOutputBuffer"><c>true</c> to use an output buffer.</param>
        /// <returns>The elapsed time in milliseconds.</returns>
        public static double TestDrawClippedLine(int n, int m, bool useOutputBuffer)
        {
            var points = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                points[i] = new ScreenPoint((double)i / n, Math.Sin(40d * i / n));
            }

            var clippingRectangle = new OxyRect(0.3, -0.5, 0.5, 1);
            var rc = new EmptyRenderContext();
            var outputBuffer = useOutputBuffer ? new List<ScreenPoint>(n) : null;
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < m; i++)
            {
                rc.DrawClippedLine(
                    clippingRectangle,
                    points,
                    1,
                    OxyColors.Black,
                    1,
                    EdgeRenderingMode.Automatic,
                    null,
                    LineJoin.Miter,
                    outputBuffer);
            }

            stopwatch.Stop();
            Console.WriteLine((double)stopwatch.ElapsedMilliseconds);
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
