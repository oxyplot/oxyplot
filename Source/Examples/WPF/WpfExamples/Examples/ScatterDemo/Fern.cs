// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fern.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Barnesley fern point generator
//   http://en.wikipedia.org/wiki/Barnsley_fern
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScatterDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Barnesley fern point generator
    /// http://en.wikipedia.org/wiki/Barnsley_fern
    /// </summary>
    public static class Fern
    {
        public static List<Point> Generate(int n = 1000, double width = 1.0, double height = 1.0)
        {
            // Probabilities
            double[] p = {0.85, .92, .99, 1.00};

            // Transformations
            var a1 = new MatrixTransform(new Matrix(0.85, -0.04, 0.04, 0.85, 0, 1.6));
            var a2 = new MatrixTransform(new Matrix(0.20, 0.23, -0.26, 0.22, 0, 1.6));
            var a3 = new MatrixTransform(new Matrix(-0.15, 0.26, 0.28, 0.24, 0, 0.44));
            var a4 = new MatrixTransform(new Matrix(0, 0, 0, 0.16, 0, 0));
            var random = new Random(17);
            var point = new Point(0.5, 0.5);
            var points = new List<Point>();

            // Transformation for [-3,3,0,10] => output coordinates
            var T = new MatrixTransform(new Matrix(width/6.0, 0, 0, -height/10.1, width/2.0, height));

            for (int i = 0; i < n; i++)
            {
                var r = random.NextDouble();

                if (r < p[0])
                    point = a1.Transform(point);
                else if (r < p[1])
                    point = a2.Transform(point);
                else if (r < p[2])
                    point = a3.Transform(point);
                else
                    point = a4.Transform(point);

                points.Add(T.Transform(point));
            }

            return points;
        }
    }
}