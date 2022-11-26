// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyListWrapper.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if NET40
namespace OxyPlot.Net40Compatibility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a wrapper around a <see cref="List{T}"/> which implements <see cref="IReadOnlyList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the list.</typeparam>
    internal class ReadOnlyListWrapper<T> : IReadOnlyList<T>, IList<T>
    {
        private readonly IList<T> source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyListWrapper{T}"/> class.
        /// </summary>
        /// <param name="source">The source list.</param>
        public ReadOnlyListWrapper(IList<T> source)
        {
            this.source = source;
        }

        /// <inheritdoc/>
        public T this[int index] => this.source[index];

        /// <inheritdoc/>
        T IList<T>.this[int index] { get => this[index]; set => throw new InvalidOperationException(); }

        /// <inheritdoc/>
        public int Count => this.source.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => true;

        /// <inheritdoc/>
        public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return this.source.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.source.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(T item)
        {
            return this.source.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.source.GetEnumerator();
        }
    }
}
#endif
