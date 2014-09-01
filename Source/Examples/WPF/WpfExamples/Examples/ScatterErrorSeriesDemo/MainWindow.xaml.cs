// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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