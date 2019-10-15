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
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A wrapper around native ToolTip objects.
    /// </summary>
    public interface IToolTip
    {
        /// <summary>
        /// Shows the tooltip if it is the case.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the tooltip if it is the case.
        /// </summary>
        void Hide();

        /// <summary>
        /// The string representation of the ToolTip. In its setter there isn't any check of the value to be different than the previous value, and in the setter, if the value is null or empty string, the ToolTip is removed from the PlotView. The ToolTip shows up naturally if the mouse is over the PlotView, using the configuration in the PlotView's c-tor.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Disposes the tool tip if possible.
        /// </summary>
        void Dispose();
    }
}
