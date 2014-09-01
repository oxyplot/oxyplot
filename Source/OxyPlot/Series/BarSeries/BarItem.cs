// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item used in the BarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item used in the BarSeries.
    /// </summary>
    public class BarItem : BarItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        public BarItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        public BarItem(double value, int categoryIndex = -1)
        {
            this.Value = value;
            this.CategoryIndex = categoryIndex;
        }
    }
}