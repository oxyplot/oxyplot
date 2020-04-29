// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementCollectionChangedEventArgs{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The element collection changed event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The element collection changed event args.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class ElementCollectionChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollectionChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="addedItems">The added items.</param>
        /// <param name="removedItems">The removed items.</param>
        public ElementCollectionChangedEventArgs(IEnumerable<T> addedItems, IEnumerable<T> removedItems)
        {
            this.AddedItems = new List<T>(addedItems ?? new T[] { });
            this.RemovedItems = new List<T>(removedItems ?? new T[] { });
        }

        /// <summary>
        /// Gets the added items.
        /// </summary>
        /// <value>The added items.</value>
        public List<T> AddedItems { get; private set; }

        /// <summary>
        /// Gets the removed items.
        /// </summary>
        /// <value>The removed items.</value>
        public List<T> RemovedItems { get; private set; }
    }
}