// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizedItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a CategorizedSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item in a CategorizedSeries.
    /// </summary>
    public abstract class CategorizedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategorizedItem" /> class. Initializes a new instance of the <see cref="CategorizedItem" /> class.
        /// </summary>
        protected CategorizedItem()
        {
            this.CategoryIndex = -1;
        }

        /// <summary>
        /// Gets or sets the index of the category.
        /// </summary>
        /// <value>The index of the category.</value>
        public int CategoryIndex { get; set; }

        /// <summary>
        /// Gets the index of the category.
        /// </summary>
        /// <param name="defaultIndex">The default index.</param>
        /// <returns>The index.</returns>
        internal int GetCategoryIndex(int defaultIndex)
        {
            if (this.CategoryIndex < 0)
            {
                return defaultIndex;
            }

            return this.CategoryIndex;
        }
    }
}