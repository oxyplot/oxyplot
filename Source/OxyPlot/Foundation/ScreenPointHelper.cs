//-----------------------------------------------------------------------
// <copyright file="ScreenPointHelper.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

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

        #endregion
    }
}
