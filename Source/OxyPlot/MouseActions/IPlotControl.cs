namespace OxyPlot
{
    public interface IPlotControl
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        PlotModel Model { get; set; }

        /// <summary>
        /// Gets the axes from a point.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="xaxis">The x-axis.</param>
        /// <param name="yaxis">The y-axis.</param>
        void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis);

        /// <summary>
        /// Gets the series from point.
        /// </summary>
        /// <param name="pt">The point (screen coordinates).</param>
        /// <param name="limit">The maximum allowed distance.</param>
        /// <returns></returns>
        ISeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100);

        /// <summary>
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        void RefreshPlot();

        /// <summary>
        /// Invalidate the plot (not blocking the UI thread)
        /// </summary>
        void InvalidatePlot();

        /// <summary>
        /// Resets the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        void Reset(IAxis axis);

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="x0">The previous point coordinate.</param>
        /// <param name="x1">The last point coordinate.</param>
        void Pan(IAxis axis, double x0, double x1);

        /// <summary>
        /// Zooms the specified axis to the specified values.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="p1">The new minimum value.</param>
        /// <param name="p2">The new maximum value.</param>
        void Zoom(IAxis axis, double p1, double p2);

        /// <summary>
        /// Zooms at.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="factor">The zoom factor.</param>
        /// <param name="x">The current position.</param>
        void ZoomAt(IAxis axis, double factor, double x);

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="s">The selected series.</param>
        /// <param name="dp">The point.</param>
        void ShowTracker(ISeries s, DataPoint dp);

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        void HideTracker();

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        void ShowZoomRectangle(OxyRect r);

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        void HideZoomRectangle();
    }
}