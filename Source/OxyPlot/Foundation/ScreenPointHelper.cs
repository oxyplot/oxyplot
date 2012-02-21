// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenPointHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// ScreenPoint helper class.
    /// </summary>
    public class ScreenPointHelper
    {
        #region Public Methods

        /// <summary>
        /// Resamples the points with the specified point distance limit.
        /// </summary>
        /// <param name="allPoints">
        /// All points.
        /// </param>
        /// <param name="minimumDistance">
        /// The minimum squared distance.
        /// </param>
        /// <returns>
        /// List of resampled points.
        /// </returns>
        public static IList<ScreenPoint> ResamplePoints(IList<ScreenPoint> allPoints, double minimumDistance)
        {
            double minimumSquaredDistance = minimumDistance * minimumDistance;
            int n = allPoints.Count;
            var result = new List<ScreenPoint>(n);
            if (n > 0)
            {
                result.Add(allPoints[0]);
                int i0 = 0;
                for (int i = 1; i < n; i++)
                {
                    double distSquared = allPoints[i0].DistanceToSquared(allPoints[i]);
                    if (distSquared < minimumSquaredDistance && i != n - 1)
                    {
                        continue;
                    }

                    i0 = i;
                    result.Add(allPoints[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the centroid of the specified polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The centroid.</returns>
        public static ScreenPoint GetCentroid(IList<ScreenPoint> points)
        {
            double cx = 0;
            double cy = 0;
            double a = 0;

            for (int i = 0; i < points.Count; i++)
            {
                int i1=(i+1)%points.Count;
                double da = points[i].x * points[i1].y - points[i1].x * points[i].y;
                cx += (points[i].x + points[i1].x) * da;
                cy += (points[i].y + points[i1].y) * da;
                a += da;
            }

            a *= 0.5;
            cx /= 6 * a;
            cy /= 6 * a;
            return new ScreenPoint(cx, cy);
        }

        #endregion
    }
}