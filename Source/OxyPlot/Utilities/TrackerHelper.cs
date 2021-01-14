namespace OxyPlot.Utilities
{
    internal static class TrackerHelper
    {
        /// <summary>
        /// Gets the nearest tracker hit.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="point">The point.</param>
        /// <param name="snap">Snap to points.</param>
        /// <param name="pointsOnly">Check points only (no interpolation).</param>
        /// <param name="firesDistance">The distance from the series at which the tracker fires</param>
        /// <returns>A tracker hit result.</returns>
        public static TrackerHitResult GetNearestHit(
            Series.Series series, ScreenPoint point, bool snap, bool pointsOnly, double firesDistance)
        {
            if (series == null)
            {
                return null;
            }

            // Check data points only
            if (snap || pointsOnly)
            {
                var result = series.GetNearestPoint(point, false);
                if (IsTrackerOpen(result, point, firesDistance))
                {
                    return result;
                }
            }

            // Check between data points (if possible)
            if (!pointsOnly)
            {
                var result = series.GetNearestPoint(point, true);
                if (IsTrackerOpen(result, point, firesDistance))
                {
                    return result;
                }
            }

            return null;
        }

        private static bool IsTrackerOpen(TrackerHitResult result, ScreenPoint point, double firesDistance) =>
            result?.Position.DistanceTo(point) < firesDistance;
    }
}
