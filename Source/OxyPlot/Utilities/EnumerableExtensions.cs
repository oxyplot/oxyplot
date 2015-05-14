// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides useful extension methods for enumerable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides useful extension methods for enumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
        {
            var list = source as IList<TSource>;
            return list != null ? CreateReverseIterator(list) : Enumerable.Reverse(source);
        }

        /// <summary>
        /// Creates a reverse iterator for a list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A list of values to reverse</param>
        /// <returns>A sequence whose elements correspond to those of the input list in reverse order.</returns>
        private static IEnumerable<TSource> CreateReverseIterator<TSource>(IList<TSource> source)
        {
            for (var i = source.Count - 1; i >= 0; --i)
            {
                yield return source[i];
            }
        }
    }
}
