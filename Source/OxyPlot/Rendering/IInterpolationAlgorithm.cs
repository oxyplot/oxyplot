// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInterpolationAlgorithm.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines an interpolation algorithm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interpolation algorithm.
    /// </summary>
    public interface IInterpolationAlgorithm {
        /// <summary>
        /// Creates a spline from data points.
        /// </summary>
        /// <param name="points">Data points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>Spline.</returns>
        List<DataPoint> CreateSpline(List<DataPoint> points, bool isClosed, double tolerance);

        /// <summary>
        /// Creates a spline from points in screen space.
        /// </summary>
        /// <param name="points">Resampled points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>Spline.</returns>
        List<ScreenPoint> CreateSpline(IList<ScreenPoint> points, bool isClosed, double tolerance);
    }
}