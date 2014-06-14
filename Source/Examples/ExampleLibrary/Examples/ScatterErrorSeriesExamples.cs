// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorSeriesExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Defines the ScatterErrorSeriesExamples examples.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Series;

    [Examples("ScatterErrorSeries")]
    public class ScatterErrorSeriesExamples : ExamplesBase
    {
        [Example("Random points and errors (n=20)")]
        public static PlotModel RandomPointsAndError20()
        {
            return RandomPointsAndError(20);
        }

        [Example("Random points and errors (n=2000)")]
        public static PlotModel RandomPointsAndError2000()
        {
            return RandomPointsAndError(2000);
        }

        [Example("Definining points by ItemsSource and Mapping")]
        public static PlotModel ItemsSourceMapping()
        {
            const int N = 20;
            var model = new PlotModel { Title = "ScatterErrorSeries, points defined by ItemsSource and Mapping", Subtitle = string.Format("Random data (n={0})", N), LegendPosition = LegendPosition.LeftTop };
            model.Series.Add(new ScatterErrorSeries
            {
                Title = "Measurements",
                ItemsSource = CreateExamplePoints(N).ToArray(),
                Mapping =
                    obj =>
                    {
                        var p = (ExamplePoint)obj;
                        return new ScatterErrorPoint(p.V1, p.V2, p.E1, p.E2);
                    }
            });
            return model;
        }

        [Example("Defining points by ItemsSource (List)")]
        public static PlotModel ItemsSourceList()
        {
            const int N = 20;
            var model = new PlotModel { Title = "ScatterErrorSeries, points defined by ItemsSource (List)", Subtitle = string.Format("Random data (n={0})", N), LegendPosition = LegendPosition.LeftTop };
            model.Series.Add(new ScatterErrorSeries { Title = "Measurements", ItemsSource = CreateScatterErrorPoints(N).ToList() });
            return model;
        }

        [Example("Defining points by ItemsSource (IEnumerable)")]
        public static PlotModel ItemsSourceEnumerable()
        {
            const int N = 20;
            var model = new PlotModel { Title = "ScatterErrorSeries, points defined by ItemsSource (IEnumerable)", Subtitle = string.Format("Random data (n={0})", N), LegendPosition = LegendPosition.LeftTop };
            model.Series.Add(new ScatterErrorSeries { Title = "Measurements", ItemsSource = CreateScatterErrorPoints(N).ToArray() });
            return model;
        }

        [Example("Definining points by ItemsSource and reflection")]
        public static PlotModel ItemsSourceReflection()
        {
            const int N = 20;
            var model = new PlotModel { Title = "ScatterErrorSeries, points defined by ItemsSource (reflection)", Subtitle = string.Format("Random data (n={0})", N), LegendPosition = LegendPosition.LeftTop };
            model.Series.Add(new ScatterErrorSeries
            {
                Title = "Measurements",
                ItemsSource = CreateExamplePoints(N).ToArray(),
                DataFieldX = "V1",
                DataFieldY = "V2",
                DataFieldErrorX = "E1",
                DataFieldErrorY = "E2"
            });
            return model;
        }

        /// <summary>
        /// Creates an example model with the specified number of points.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns>A plot model.</returns>
        private static PlotModel RandomPointsAndError(int n)
        {
            var model = new PlotModel { Title = "ScatterErrorSeries", Subtitle = string.Format("Random data (n={0})", n), LegendPosition = LegendPosition.LeftTop };

            var s1 = new ScatterErrorSeries { Title = "Measurements" };
            s1.Points.AddRange(CreateScatterErrorPoints(n));
            model.Series.Add(s1);
            return model;
        }

        /// <summary>
        /// Creates random example data.
        /// </summary>
        /// <param name="n">The number of points to generate.</param>
        /// <returns>A sequence of points.</returns>
        private static IEnumerable<ScatterErrorPoint> CreateScatterErrorPoints(int n)
        {
            var random = new Random(27);
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble();
                y += 1 + random.NextDouble();

                yield return new ScatterErrorPoint(x, y, random.NextDouble(), random.NextDouble());
            }
        }

        /// <summary>
        /// Creates random example data.
        /// </summary>
        /// <param name="n">The number of points to generate.</param>
        /// <returns>A sequence of points.</returns>
        private static IEnumerable<ExamplePoint> CreateExamplePoints(int n)
        {
            var random = new Random(27);
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble();
                y += 1 + random.NextDouble();

                yield return new ExamplePoint { V1 = x, V2 = y, E1 = random.NextDouble(), E2 = random.NextDouble() };
            }
        }

        /// <summary>
        /// Represents a point with errors.
        /// </summary>
        public class ExamplePoint
        {
            /// <summary>
            /// Gets or sets the first value.
            /// </summary>
            public double V1 { get; set; }

            /// <summary>
            /// Gets or sets the second value.
            /// </summary>
            public double V2 { get; set; }

            /// <summary>
            /// Gets or sets the first error.
            /// </summary>
            public double E1 { get; set; }

            /// <summary>
            /// Gets or sets the second error.
            /// </summary>
            public double E2 { get; set; }
        }
    }
}
