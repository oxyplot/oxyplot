// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayBuilder.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to build arrays.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides functionality to build arrays.
    /// </summary>
    public static class ArrayBuilder
    {
        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="x0">The first value.</param>
        /// <param name="x1">The last value.</param>
        /// <param name="n">The number of steps.</param>
        /// <returns>A vector.</returns>
        public static double[] CreateVector(double x0, double x1, int n)
        {
            var result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Math.Round(x0 + ((x1 - x0) * i / (n - 1)), 8);
            }

            return result;
        }

        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="x0">The first value.</param>
        /// <param name="x1">The last value.</param>
        /// <param name="dx">The step size.</param>
        /// <returns>A vector.</returns>
        public static double[] CreateVector(double x0, double x1, double dx)
        {
            var n = (int)Math.Round((x1 - x0) / dx);
            var result = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                result[i] = Math.Round(x0 + (i * dx), 8);
            }

            return result;
        }

        /// <summary>
        /// Evaluates the specified function.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="x">The x values.</param>
        /// <param name="y">The y values.</param>
        /// <returns>Array of evaluations. The value of f(x_i,y_j) will be placed at index [i, j].</returns>
        public static double[,] Evaluate(Func<double, double, double> f, double[] x, double[] y)
        {
            int m = x.Length;
            int n = y.Length;
            var result = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = f(x[i], y[j]);
                }
            }

            return result;
        }

        /// <summary>
        /// Fills the array with the specified value.
        /// </summary>
        /// <param name="array">The array to fill.</param>
        /// <param name="value">The value.</param>
        public static void Fill(this double[] array, double value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /// <summary>
        /// Fills the two-dimensional array with the specified value.
        /// </summary>
        /// <param name="array">The two-dimensional array.</param>
        /// <param name="value">The value.</param>
        public static void Fill2D(this double[,] array, double value)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = value;
                }
            }
        }
    }
}