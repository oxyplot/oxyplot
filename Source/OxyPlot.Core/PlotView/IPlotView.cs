// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for the plot views.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the plot views.
    /// </summary>
    public interface IPlotView : IView
    {
        /// <summary>
        /// Gets the actual <see cref="PlotModel" /> of the control.
        /// </summary>
        new PlotModel ActualModel { get; }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        void HideTracker();

        /// <summary>
        /// Invalidates the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data bindings will be updated.</param>
        void InvalidatePlot(bool updateData = true);

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        void ShowTracker(TrackerHitResult trackerHitResult);

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetClipboardText(string text);
    }
}