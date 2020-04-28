// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlotModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for the plot model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the plot model.
    /// </summary>
    public interface IPlotModel
    {
        /// <summary>
        /// Gets the color of the background of the plot.
        /// </summary>
        /// <value>The color.</value>
        /// <remarks>If the background color is set to <see cref="OxyColors.Undefined" /> or is otherwise invisible then the background will be determined by the plot view or exporter.</remarks>
        OxyColor Background { get; }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void Update(bool updateData);

        /// <summary>
        /// Renders the plot with the specified rendering context within the given rectangle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        void Render(IRenderContext rc, OxyRect rect);

        /// <summary>
        /// Attaches this model to the specified plot view.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        /// <remarks>Only one plot view can be attached to the plot model.
        /// The plot model contains data (e.g. axis scaling) that is only relevant to the current plot view.</remarks>
        void AttachPlotView(IPlotView plotView);
    }
}
