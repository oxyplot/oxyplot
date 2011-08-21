using System.Collections.Generic;

namespace OxyPlot
{
    /// <summary>
    /// ScreenPoint helper class.
    /// </summary>
    public class ScreenPointHelper
    {
        /// <summary>
        /// Resamples the points with the specified point distance limit.
        /// </summary>
        /// <param name="allPoints">All points.</param>
        /// <param name="minimumDistance">The minimum squared distance.</param>
        /// <returns>List of resampled points.</returns>
        public static IList<ScreenPoint> ResamplePoints(ScreenPoint[] allPoints, double minimumDistance)
        {
            double minimumSquaredDistance = minimumDistance*minimumDistance;
            var result = new List<ScreenPoint>(allPoints.Length);
            if (allPoints.Length > 0)
            {
                result.Add(allPoints[0]);
                int i0 = 0;
                for (int i = 1; i < allPoints.Length; i++)
                {
                    double distSquared = allPoints[i0].DistanceToSquared(allPoints[i]);
                    if (distSquared < minimumSquaredDistance && i != allPoints.Length - 1)
                        continue;
                    i0 = i;
                    result.Add(allPoints[i]);
                }
            }
            return result;
        }
    }
}