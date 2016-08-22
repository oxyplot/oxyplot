// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparerHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to create custom comparers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides functionality to create custom comparers.
    /// </summary>
    public static class ComparerHelper
    {
        /// <summary>
        /// Creates a <see cref="IComparer{T}"/> based on the specified comparison.
        /// </summary>
        /// <typeparam name="T">The type of the elements to compare.</typeparam>
        /// <param name="comparison">The delegate used to compare elements.</param>
        /// <returns>The created comparer.</returns>
        public static IComparer<T> CreateComparer<T>(Comparison<T> comparison)
        {
            return new ComparisonComparer<T>(comparison);
        }

        /// <summary>
        /// A comparer that uses a delegate to compare elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements to compare.</typeparam>
        private class ComparisonComparer<T> : IComparer<T>
        {
            /// <summary>
            /// The delegate used to compare elements.
            /// </summary>
            private readonly Comparison<T> comparison;

            /// <summary>
            /// Initializes a new instance of the <see cref="ComparisonComparer{T}" /> class.
            /// </summary>
            /// <param name="comparison">The delegate used to compare elements.</param>
            public ComparisonComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            /// <summary>
            /// Compares two elements.
            /// </summary>
            /// <param name="x">The first element to compare.</param>
            /// <param name="y">The second element to compare.</param>
            /// <returns>A value indicating whether <paramref name="x"/> is less than, equal to, or greater than <paramref name="y"/>.</returns>
            public int Compare(T x, T y)
            {
                return this.comparison.Invoke(x, y);
            }
        }
    }
}