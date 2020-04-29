// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Selection.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a selection of items (by index) and features (by enumeration type).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a selection of items (by index) and features (by enumeration type).
    /// </summary>
    public class Selection
    {
        /// <summary>
        /// Static instance representing everything (all items and all features) selected.
        /// </summary>
        private static readonly Selection EverythingSelection = new Selection();

        /// <summary>
        /// The selection (cannot use HashSet{T} in PCL)
        /// </summary>
        private readonly Dictionary<SelectionItem, bool> selection = new Dictionary<SelectionItem, bool>();

        /// <summary>
        /// Gets the everything selected.
        /// </summary>
        /// <value>The everything.</value>
        public static Selection Everything
        {
            get
            {
                return EverythingSelection;
            }
        }

        /// <summary>
        /// Determines whether everything is selected.
        /// </summary>
        /// <returns><c>true</c> if everything is selected; otherwise, <c>false</c>.</returns>
        public bool IsEverythingSelected()
        {
            // ReSharper disable RedundantNameQualifier
            return object.ReferenceEquals(this, EverythingSelection);
            // ReSharper restore RedundantNameQualifier
        }

        /// <summary>
        /// Gets the indices of the selected items in this selection.
        /// </summary>
        /// <returns>Enumerator of indices.</returns>
        public IEnumerable<int> GetSelectedItems()
        {
            return this.selection.Keys.Select(si => si.Index);
        }

        /// <summary>
        /// Gets the selected items by the specified feature.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>Enumerator of indices.</returns>
        public IEnumerable<int> GetSelectedItems(Enum feature)
        {
            // ReSharper disable RedundantNameQualifier
            return this.selection.Keys.Where(si => object.Equals(si.Feature, feature)).Select(si => si.Index);
            // ReSharper restore RedundantNameQualifier
        }

        /// <summary>
        /// Clears the selected items.
        /// </summary>
        public void Clear()
        {
            this.selection.Clear();
        }

        /// <summary>
        /// Determines whether the specified item and feature is selected.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="feature">The feature.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public bool IsItemSelected(int index, Enum feature = null)
        {
            if (this.IsEverythingSelected())
            {
                return true;
            }

            var si = new SelectionItem(index, feature);
            return this.selection.ContainsKey(si);
        }

        /// <summary>
        /// Selects the specified item/feature.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="feature">The feature.</param>
        public void Select(int index, Enum feature = null)
        {
            var si = new SelectionItem(index, feature);
            this.selection[si] = true;
        }

        /// <summary>
        /// Unselects the specified item.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <param name="feature">The feature.</param>
        public void Unselect(int index, Enum feature = null)
        {
            var si = new SelectionItem(index, feature);
            if (!this.selection.ContainsKey(si))
            {
                throw new InvalidOperationException("Item " + index + " and feature " + feature + " is not selected. Cannot unselect.");
            }

            this.selection.Remove(si);
        }

        /// <summary>
        /// Represents an item in a <see cref="Selection" />.
        /// </summary>
        public struct SelectionItem : IEquatable<SelectionItem>
        {
            /// <summary>
            /// The index
            /// </summary>
            private readonly int index;

            /// <summary>
            /// The feature
            /// </summary>
            private readonly Enum feature;

            /// <summary>
            /// Initializes a new instance of the <see cref="SelectionItem" /> struct.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="feature">The feature.</param>
            public SelectionItem(int index, Enum feature)
            {
                this.index = index;
                this.feature = feature;
            }

            /// <summary>
            /// Gets the index.
            /// </summary>
            /// <value>The index.</value>
            public int Index
            {
                get
                {
                    return this.index;
                }
            }

            /// <summary>
            /// Gets the feature.
            /// </summary>
            /// <value>The feature.</value>
            public Enum Feature
            {
                get
                {
                    return this.feature;
                }
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
            public bool Equals(SelectionItem other)
            {
                // ReSharper disable RedundantNameQualifier
                return other.index == this.index && object.Equals(other.feature, this.feature);
                // ReSharper restore RedundantNameQualifier
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
            public override int GetHashCode()
            {
                if (this.feature == null)
                {
                    return this.index.GetHashCode();
                }

                // http://msdn.microsoft.com/en-us/library/system.object.gethashcode.aspx
                // http://stackoverflow.com/questions/2890040/implementing-gethashcode
                // http://stackoverflow.com/questions/508126/what-is-the-correct-implementation-for-gethashcode-for-entity-classes
                // http://stackoverflow.com/questions/70303/how-do-you-implement-gethashcode-for-structure-with-two-string
                return this.index.GetHashCode() ^ this.feature.GetHashCode();
            }
        }
    }
}