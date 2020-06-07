// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementCollection{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a collection of <see cref="Element" /> objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of <see cref="Element" /> objects.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public class ElementCollection<T> : IList<T>, IReadOnlyList<T> where T : Element
    {
        /// <summary>
        /// The parent <see cref="Model" />.
        /// </summary>
        private readonly Model parent;

        /// <summary>
        /// The internal list.
        /// </summary>
        private readonly List<T> internalList = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollection{T}" /> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="PlotModel" />.</param>
        public ElementCollection(Model parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Raised when the collection changes.
        /// </summary>
        [Obsolete("May be removed in v4.0 (#111)")]
        public event EventHandler<ElementCollectionChangedEventArgs<T>> CollectionChanged;

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <returns>The number of elements contained in the collection.</returns>
        public int Count
        {
            get
            {
                return this.internalList.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <returns><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The element.</returns>
        public T this[int index]
        {
            get
            {
                return this.internalList[index];
            }

            set
            {
                value.Parent = this.parent;
                this.internalList[index] = value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        /// <exception cref="System.InvalidOperationException">The element cannot be added, it already belongs to a PlotModel.</exception>
        public void Add(T item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("The element cannot be added, it already belongs to a PlotModel.");
            }

            item.Parent = this.parent;
            this.internalList.Add(item);

            this.RaiseCollectionChanged(new[] { item });
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            var removedItems = new List<T>();

            foreach (var item in this.internalList)
            {
                item.Parent = null;
                removedItems.Add(item);
            }

            this.internalList.Clear();

            this.RaiseCollectionChanged(removedItems: removedItems);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><c>true</c> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <c>false</c>.</returns>
        public bool Contains(T item)
        {
            return this.internalList.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public bool Remove(T item)
        {
            item.Parent = null;
            var result = this.internalList.Remove(item);
            if (result)
            {
                this.RaiseCollectionChanged(removedItems: new[] { item });
            }

            return result;
        }

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return this.internalList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the collection.</param>
        /// <exception cref="System.InvalidOperationException">The element cannot be inserted, it already belongs to a PlotModel.</exception>
        public void Insert(int index, T item)
        {
            if (item.Parent != null)
            {
                throw new InvalidOperationException("The element cannot be inserted, it already belongs to a PlotModel.");
            }

            item.Parent = this.parent;
            this.internalList.Insert(index, item);

            this.RaiseCollectionChanged(new[] { item });
        }

        /// <summary>
        /// Removes the collection item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            var item = this[index];
            item.Parent = null;

            this.internalList.RemoveAt(index);

            this.RaiseCollectionChanged(removedItems: new[] { item });
        }

        /// <summary>
        /// Raises the collection changed event.
        /// </summary>
        /// <param name="addedItems">The added items.</param>
        /// <param name="removedItems">The removed items.</param>
        private void RaiseCollectionChanged(IEnumerable<T> addedItems = null, IEnumerable<T> removedItems = null)
        {
            var collectionChanged = this.CollectionChanged;
            if (collectionChanged != null)
            {
                collectionChanged(this, new ElementCollectionChangedEventArgs<T>(addedItems, removedItems));
            }
        }
    }
}
