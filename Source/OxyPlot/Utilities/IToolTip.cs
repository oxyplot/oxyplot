// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IToolTip.cs" company="OxyPlot">
//   Copyright (c) 2019 OxyPlot contributors
// </copyright>
// <summary>
//   A wrapper around native ToolTip objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// A wrapper around native ToolTip objects.
    /// </summary>
    public interface IToolTip
    {
        /// <summary>
        /// Hit testing tolerance for usual <see cref="PlotElement"/>s (more precisely, excluding the plot title area).
        /// </summary>
        double UsualPlotElementHitTestingTolerance { get; set; }

        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        void Hide();

        /// <summary>
        /// The string representation of the tooltip.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Disposes the tooltip if possible.
        /// </summary>
        void Dispose();
    }
}
