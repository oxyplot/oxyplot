// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Array helper methods.
    /// </summary>
    public static class ArrayHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="x0">
        /// The first value.
        /// </param>
        /// <param name="x1">
        /// The last value.
        /// </param>
        /// <param name="n">
        /// The number of steps.
        /// </param>
        /// <returns>
        /// A vector.
        /// </returns>
        public static double[] CreateVector(double x0, double x1, int n)
        {
            var result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = (x0 + ((x1 - x0) * i / (n - 1))).RemoveNoise();
            }

            return result;
        }

        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="x0">
        /// The first value.
        /// </param>
        /// <param name="x1">
        /// The last value.
        /// </param>
        /// <param name="dx">
        /// The step size.
        /// </param>
        /// <returns>
        /// A vector.
        /// </returns>
        public static double[] CreateVector(double x0, double x1, double dx)
        {
            var n = (int)Math.Round((x1 - x0) / dx);
            var result = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                result[i] = (x0 + (i * dx)).RemoveNoise();
            }

            return result;
        }

        /// <summary>
        /// Evaluates the specified function.
        /// </summary>
        /// <param name="f">
        /// The function.
        /// </param>
        /// <param name="x">
        /// The x values.
        /// </param>
        /// <param name="y">
        /// The y values.
        /// </param>
        /// <returns>
        /// Array of evaluations.
        /// </returns>
        public static double[,] Evaluate(Func<double, double, double> f, double[] x, double[] y)
        {
            int m = y.Length;
            int n = x.Length;
            var result = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = f(x[j], y[i]);
                }
            }

            return result;
        }

        #endregion
    }
}