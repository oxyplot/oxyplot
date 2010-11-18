namespace OxyPlot
{
    public interface ISeries
    {
        /// <summary>
        /// Renders the Series on the specified rc.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The model.</param>
        void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified rc.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The rect.</param>
        void RenderLegend(IRenderContext rc, OxyRect rect);

        /// <summary>
        /// Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }
    }
}