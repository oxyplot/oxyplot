// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITrackableSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Interface for Series that can be 'tracked'
    /// </summary>
    /// <remarks>
    /// The plot control will show a tracker with the current value when moving the mouse over the data.
    /// </remarks>
    public interface ITrackableSeries 
    {
        #region Public Properties

        /// <summary>
        ///   Gets a format string used for the tracker.
        /// </summary>
        /// <remarks>
        ///   The fields that can be used in the format string depends on the series.
        /// </remarks>
        string TrackerFormatString { get; }

        /// <summary>
        ///   Gets the tracker key.
        /// </summary>
        string TrackerKey { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// Interpolate the series if this flag is set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        #endregion
    }
}