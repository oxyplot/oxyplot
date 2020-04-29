// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides useful extension methods for arrays.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides useful extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Finds the maximum value in the specified 2D array (NaN values not included).
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>The maximum value.</returns>
        public static double Max2D(this double[,] array)
        {
            var max = double.MinValue;
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j].CompareTo(max) > 0)
                    {
                        max = array[i, j];
                    }
                }
            }

            return max;
        }

        /// <summary>
        /// Finds the minimum value in the specified 2D array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="excludeNaN">Exclude NaN values if set to <c>true</c>.</param>
        /// <returns>The minimum value.</returns>
        public static double Min2D(this double[,] array, bool excludeNaN = false)
        {
            var min = double.MaxValue;
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (excludeNaN && double.IsNaN(array[i, j]))
                    {
                        continue;
                    }

                    if (array[i, j].CompareTo(min) < 0)
                    {
                        min = array[i, j];
                    }
                }
            }

            return min;
        }
    }
}