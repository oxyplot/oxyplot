// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item used in the ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item used in the ColumnSeries.
    /// </summary>
    public class ColumnItem : BarItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnItem" /> class.
        /// </summary>
        public ColumnItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnItem" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        public ColumnItem(double value, int categoryIndex = -1)
        {
            this.Value = value;
            this.CategoryIndex = categoryIndex;
        }
    }
}