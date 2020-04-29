// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizedSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Base class for series where the items are categorized.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    using OxyPlot.Axes;

    /// <summary>
    /// Base class for series where the items are categorized.
    /// </summary>
    public abstract class CategorizedSeries : XYAxisSeries
    {
        /// <summary>
        /// The default category axis title
        /// </summary>
        protected const string DefaultCategoryAxisTitle = "Category";

        /// <summary>
        /// The default value axis title
        /// </summary>
        protected const string DefaultValueAxisTitle = "Value";

        /// <summary>
        /// Gets or sets the width/height of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <value>The width of the bars.</value>
        /// <returns>The fractional width.</returns>
        /// <remarks>The available space will be determined by the GapWidth of the CategoryAxis used by this series.</remarks>
        internal abstract double GetBarWidth();

        /// <summary>
        /// Gets the items of this series.
        /// </summary>
        /// <returns>The items.</returns>
        protected internal abstract IList<CategorizedItem> GetItems();

        /// <summary>
        /// Gets the actual bar width/height of the items in this series.
        /// </summary>
        /// <returns>The width or height.</returns>
        /// <remarks>The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.</remarks>
        protected abstract double GetActualBarWidth();

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>The category axis.</returns>
        protected abstract CategoryAxis GetCategoryAxis();
    }
}