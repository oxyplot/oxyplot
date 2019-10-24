// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IToolTipController.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   Controller for <see cref="IToolTipView"/>s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Controller for <see cref="IToolTipView"/>s.
    /// </summary>
    public interface IToolTipController
    {
        /// <summary>
        /// Gets or sets the associated tooltip view.
        /// </summary>
        IToolTipView ToolTipView { get; set; }

        /// <summary>
        /// Gets the associated plot model.
        /// </summary>
        PlotModel PlotModel { get; }

        /// <summary>
        /// Gets or sets the hit testing tolerance for usual <see cref="PlotElement"/>s (more precisely, excluding the plot title area).
        /// </summary>
        double UsualPlotElementHitTestingTolerance { get; set; }
    }
}
