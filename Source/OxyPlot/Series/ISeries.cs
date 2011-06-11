using System.Collections.ObjectModel;

namespace OxyPlot
{
    /// <summary>
    ///   The Series interface.
    /// </summary>
    public interface ISeries
    {
        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        /// <summary>
        ///   Renders the Series on the specified render context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        ///   Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The rect.</param>
        void RenderLegend(IRenderContext rc, OxyRect legendBox);

        /// <summary>
        /// Updates the data.
        /// </summary>
        void UpdateData();

        /// <summary>
        ///   Check if this data series requires X/Y axes. 
        ///   (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns></returns>
        bool AreAxesRequired();

        /// <summary>
        /// Ensures that the series has axes.
        /// </summary>
        /// <param name="axes">The axes collection of the parent PlotModel.</param>
        /// <param name="defaultXAxis">The default X axis of the parent PlotModel.</param>
        /// <param name="defaultYAxis">The default Y axis of the parent PlotModel.</param>
        void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis);

        /// <summary>
        /// Updates the maximum and minimum of the axes related to the series.
        /// </summary>
        void UpdateMaxMin();

        /// <summary>
        /// Sets default values from the plotmodel.
        /// </summary>
        void SetDefaultValues(PlotModel model);
    }
}