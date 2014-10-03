// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashCodeBuilder.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to calculate hash codes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to calculate hash codes.
    /// </summary>
    public static class HashCodeBuilder
    {
        /// <summary>
        /// Calculates a hash code for the specified sequence of items.
        /// </summary>
        /// <param name="items">A sequence of items.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public static int GetHashCode(IEnumerable<object> items)
        {
            // See http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            // Overflow is fine, just wrap
            unchecked
            {
                return items.Where(item => item != null).Aggregate(17, (current, item) => (current * 23) + item.GetHashCode());
            }
        }
    }
}