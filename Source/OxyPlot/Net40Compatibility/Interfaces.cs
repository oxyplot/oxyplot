// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interfaces.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if NET40
namespace System.Collections.Generic
{
    /// <summary>
    /// Defines a read-only collection of elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    {
        /// <summary>
        /// The number of elements in the collection.
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// Defines a read-only list of elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Gets the element at the specified index in the list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The element at the specified index in the list.</returns>
        T this[int index] { get; }
    }
}
#endif
