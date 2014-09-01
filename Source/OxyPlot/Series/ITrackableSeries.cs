// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITrackableSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to return data for a tracker control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Provides functionality to return data for a tracker control.
    /// </summary>
    /// <remarks>The plot view will show a tracker with the current value when moving the mouse over the data.</remarks>
    public interface ITrackableSeries
    {
        /// <summary>
        /// Gets a format string used for the tracker.
        /// </summary>
        /// <remarks>The fields that can be used in the format string depends on the series.</remarks>
        string TrackerFormatString { get; }

        /// <summary>
        /// Gets the tracker key.
        /// </summary>
        string TrackerKey { get; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);
    }
}