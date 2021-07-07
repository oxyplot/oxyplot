// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections;
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

            T best = default;
            TComparable bestComparable = default;

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
    }
}
