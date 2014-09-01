// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Arrays.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides utilities for <see cref="Array" />s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides utilities for <see cref="Array" />s.
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Copies a range of the specified <see cref="Array" />.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="from">The start index.</param>
        /// <param name="to">The end index.</param>
        /// <returns>An <see cref="Array" /> containing the items from index <paramref name="from" /> to index <paramref name="to" />.</returns>
        public static T[] CopyOfRange<T>(T[] source, int from, int to)
        {
            var result = new T[to - from];
            for (int i = from; i < Math.Min(source.Length, to); i++)
            {
                result[i - from] = source[i];
            }

            return result;
        }

        /// <summary>
        /// Copies the first items of the specified <see cref="Array" />.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="newLength">The number of items to copy.</param>
        /// <returns>An <see cref="Array" /> containing the items from index 0 to index <paramref name="newLength" />.</returns>
        public static T[] CopyOf<T>(T[] source, int newLength)
        {
            var result = new T[newLength];
            for (int i = 0; i < Math.Min(source.Length, newLength); i++)
            {
                result[i] = source[i];
            }

            return result;
        }

        /// <summary>
        /// Fills the specified array with values in the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the array items.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="i0">The start index.</param>
        /// <param name="i1">The end index.</param>
        /// <param name="v">The value to fill.</param>
        public static void Fill<T>(T[] source, int i0, int i1, T v)
        {
            for (int i = i0; i < i1; i++)
            {
                source[i] = v;
            }
        }
    }
}