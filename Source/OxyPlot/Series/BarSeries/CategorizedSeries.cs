// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizedSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Base class for series where the items are categorized.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class for series where the items are categorized.
    /// </summary>
    public abstract class CategorizedSeries : XYAxisSeries
    {
        #region Methods

        /// <summary>
        /// Gets or sets the width/height of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <returns>
        /// The fractional width. 
        /// </returns>
        /// <value>
        /// The width of the bars. 
        /// </value>
        /// <remarks>
        /// The available space will be determined by the GapWidth of the CategoryAxis used by this series.
        /// </remarks>
        internal abstract double GetBarWidth();

        /// <summary>
        /// Gets the items of this series.
        /// </summary>
        /// <returns>
        /// The items. 
        /// </returns>
        protected internal abstract IList<CategorizedItem> GetItems();

        /// <summary>
        /// Gets the actual bar width/height of the items in this series.
        /// </summary>
        /// <returns>
        /// The width or height. 
        /// </returns>
        /// <remarks>
        /// The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.
        /// </remarks>
        protected abstract double GetActualBarWidth();

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>
        /// The category axis. 
        /// </returns>
        protected abstract CategoryAxis GetCategoryAxis();

        #endregion
    }
}