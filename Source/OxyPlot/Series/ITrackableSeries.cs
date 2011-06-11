namespace OxyPlot
{
    /// <summary>
    /// Interface for Series that can be 'tracked'
    /// The plot control will show a tracker with the current value when moving the mouse over the data
    /// </summary>
    public interface ITrackableSeries
    {
        /// <summary>
        /// Gets the nearest interpolated point.
        /// </summary>
        /// <param name="pt">The point (in screen coordinates).</param>
        /// <param name="dp">The nearest interpolated point (in data coordinates).</param>
        /// <param name="sp">The nearest interpolated point (in screen coordinates).</param>
        /// <returns>true if a point was found. Return false if the series cannot be interpolated.</returns>
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
        /// Gets a value indicating whether the tracker can interpolate between the points.
        /// </summary>
        bool CanTrackerInterpolatePoints { get; }
    }
}