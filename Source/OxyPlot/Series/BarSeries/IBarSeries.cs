// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using OxyPlot.Axes;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the functionality of a bar series.
    /// </summary>
    public interface IBarSeries
    {
        /// <summary>
        /// Gets the bar width.
        /// </summary>
        double BarWidth { get; }

        /// <summary>
        /// Gets the <see cref="CategoryAxis"/> the bar series uses.
        /// </summary>
        CategoryAxis CategoryAxis { get; }

        /// <summary>
        /// Gets a value indicating whether the bar series is visible.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets or sets the manager of the bar series.
        /// </summary>
        BarSeriesManager Manager { get; set; }

        /// <summary>
        /// Gets the <see cref="PlotModel"/> the bar series belongs to.
        /// </summary>
        PlotModel PlotModel { get; }

        /// <summary>
        /// Gets the <see cref="ValueAxis"/> the bar series uses.
        /// </summary>
        Axis ValueAxis { get; }

        /// <summary>
        /// Updates the valid data.
        /// </summary>
        void UpdateValidData();

        /// <summary>
        /// Gets the actual bar items.
        /// </summary>
        IReadOnlyList<BarItemBase> ActualItems { get; }
    }
}
