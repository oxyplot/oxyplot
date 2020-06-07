// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompatibilityExtensions.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
#if NET40
    using OxyPlot.Net40Compatibility;
#endif
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods facilitating compatibility with .NET 4.0.
    /// </summary>
    public static class CompatibilityExtensions
    {
        /// <summary>
        /// Returns a <see cref="IReadOnlyList{T}"/> corresponding to the specified <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>
        /// On .NET4.0, this returns a wrapper around the <paramref name="list"/> which implements <see cref="IReadOnlyList{T}"/> and <see cref="IList{T}"/>.
        /// On .NET4.5 and later, this directly returns the <paramref name="list"/>.
        /// </returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this List<T> list)
        {
#if NET40
            return new ReadOnlyListWrapper<T>(list);
#else
            return list;
#endif
        }
        
        /// <summary>
        /// Returns a <see cref="IReadOnlyList{T}"/> corresponding to the specified array.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>
        /// On .NET4.0, this returns a wrapper around the <paramref name="array"/> which implements <see cref="IReadOnlyList{T}"/> and <see cref="IList{T}"/>.
        /// On .NET4.5 and later, this directly returns the <paramref name="array"/>.
        /// </returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this T[] array)
        {
#if NET40
            return new ReadOnlyListWrapper<T>(array);
#else
            return array;
#endif
        }
    }
}
