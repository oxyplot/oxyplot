// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;

namespace OxyPlot.Utilities
{
    /// <summary>
    /// Provides general helper functions.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Switches the values of two specified variables.
        /// </summary>
        /// <typeparam name="T">The type of the variables.</typeparam>
        /// <param name="value">The first value.</param>
        /// <param name="other">The second value.</param>
        public static void Swap<T>(ref T value, ref T other)
        {
            var tmp = value;
            value = other;
            other = tmp;
        }

        /// <summary>
        /// Gets the first element with a minimal projected value.
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <typeparam name="TComparable">The comparable type.</typeparam>
        /// <param name="sequence">The enumerable of elements.</param>
        /// <param name="projection">The projection from elements to a comparable.</param>
        /// <returns>The first element with a minimal projected value.</returns>
        public static T ArgMin<T, TComparable>(IEnumerable<T> sequence, Func<T, TComparable> projection) where TComparable : IComparable<TComparable>
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            if (projection is null)
            {
                throw new ArgumentNullException(nameof(projection));
            }

            T? best = default;
            TComparable? bestComparable = default;

            bool first = true;
            foreach (T t in sequence)
            {
                var comparable = projection(t);
                if (first || comparable.CompareTo(bestComparable!) < 0)
                {
                    best = t;
                    bestComparable = comparable;
                    first = false;
                }
            }

            if (first)
            {
                throw new ArgumentException("Sequence must be non-empty.", nameof(sequence));
            }

            return best!;
        }

        /// <summary>
        /// Performs a linear interpolation between two points.
        /// </summary>
        /// <param name="x0">The x coordinate of the first point.</param>
        /// <param name="y0">The y coordinate of the first point.</param>
        /// <param name="x1">The x coordinate of the second point.</param>
        /// <param name="y1">The y coordinate of the second point.</param>
        /// <param name="value">The x value where the interpolation should be evaluated.</param>
        /// <returns>The y coordinate of a point with x=<paramref name="value"/> on the line between the first and second point.</returns>
        public static double LinearInterpolation(double x0, double y0, double x1, double y1, double value)
        {
            return (value - x0) / (x1 - x0) * (y1 - y0) + y0;
        }
    }
}
