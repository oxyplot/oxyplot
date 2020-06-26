// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Abstract base class for series that can contain items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Abstract base class for series that can contain items.
    /// </summary>
    public abstract class ItemsSeries : Series
    {
        /// <summary>
        /// Gets or sets the items source. The default is <c>null</c>.
        /// </summary>
        /// <value>The items source.</value>
        [CodeGeneration(false)]
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets the item for the specified index.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        /// <param name="index">The index.</param>
        /// <returns>The get item.</returns>
        /// <remarks>Returns <c>null</c> if ItemsSource is not set, or the index is outside the boundaries.</remarks>
        protected static object GetItem(IEnumerable itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }

                return null;
            }

            var i = 0;
            return itemsSource.Cast<object>().FirstOrDefault(item => i++ == index);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected virtual object GetItem(int i)
        {
            return GetItem(this.ItemsSource, i);
        }
    }
}
