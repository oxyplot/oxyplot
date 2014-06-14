// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
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
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScatterErrorSeriesDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates ScatterErrorSeries.")]
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.Points = CreateExamplePoints(20).ToList();
            this.PointList = this.Points.Select(p => new ScatterErrorPoint(p.V1, p.V2, p.E1, p.E2)).ToList();
            this.PointArray = this.PointList.ToArray();

            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public List<ExamplePoint> Points { get; set; }

        /// <summary>
        /// Gets or sets the point list.
        /// </summary>
        public List<ScatterErrorPoint> PointList { get; set; }

        /// <summary>
        /// Gets or sets the point array.
        /// </summary>
        public ScatterErrorPoint[] PointArray { get; set; }

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