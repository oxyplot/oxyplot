//-----------------------------------------------------------------------
// <copyright file="ITrackableSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Interface for Series that can be 'tracked'
    /// The plot control will show a tracker with the current value when moving the mouse over the data.
    /// </summary>
    public interface ITrackableSeries : ISeries
    {
        #region Public Properties

        /// <summary>
        /// Gets a format string used for the tracker.
        /// </summary>
        string TrackerFormatString { get; }

        /// <summary>
        /// Gets the tracker key.
        /// </summary>
        string TrackerKey { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// interpolate if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        #endregion
    }
}
