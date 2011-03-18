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
        /// Returns true if the data series requires X/Y axes.
        /// </summary>
        /// <returns></returns>
        bool AreAxesRequired();

        /// <summary>
        /// Ensures that the series has axes.
        /// </summary>
        /// <param name="axes">The axes collection.</param>
        /// <param name="defaultXAxis">The default X axis.</param>
        /// <param name="defaultYAxis">The default Y axis.</param>
        void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis);

        /// <summary>
        /// Updates the maximum and minimum of the axes related to the series.
        /// </summary>
        void UpdateMaxMin();

        /// <summary>
        /// Gets the nearest interpolated point.
        /// </summary>
        /// <param name="pt">The point (in screen coordinates).</param>
        /// <param name="dp">The nearest interpolated point (in data coordinates).</param>
        /// <param name="sp">The nearest interpolated point (in screen coordinates).</param>
        /// <returns>true if a point was found.</returns>
        bool GetNearestInterpolatedPoint(ScreenPoint pt, out DataPoint dp, out ScreenPoint sp);
        
        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="pt">The point (in screen coordinates).</param>
        /// <param name="dp">The nearest point (in data coordinates).</param>
        /// <param name="sp">The nearest point (in screen coordinates).</param>
        /// <returns>true if a point was found.</returns>
        bool GetNearestPoint(ScreenPoint pt, out DataPoint dp, out ScreenPoint sp);

        /// <summary>
        /// Sets default values from the plotmodel.
        /// </summary>
        void SetDefaultValues(PlotModel model);
    }

}