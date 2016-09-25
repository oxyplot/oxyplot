using System;
using System.Collections.Generic;

namespace OxyPlot
{
    /// <summary>
    /// Provides extension methods for <see cref="InterpolationAlgorithm" />.
    /// </summary>
    /// <remarks>These are pure methods.</remarks>
    public static class InterpolationAlgorithmExtensions {
        /// <summary>
        /// Creates a spline using specific interpolation algorithm.
        /// </summary>
        /// <param name="algorithm">Algorithm.</param>
        /// <param name="dataPoints">Data points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>Spline.</returns>
        public static List<DataPoint> CreateSpline(this InterpolationAlgorithm algorithm, List<DataPoint> dataPoints, bool isClosed, double tolerance) {
            switch (algorithm) {
                case InterpolationAlgorithm.Canonical:
                    return CanonicalSplineHelper.CreateSpline(dataPoints, 0.5, null, isClosed, tolerance);
                case InterpolationAlgorithm.CatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(dataPoints, 0.5, isClosed, tolerance);
                case InterpolationAlgorithm.UniformCatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(dataPoints, 0d, isClosed, tolerance);
                case InterpolationAlgorithm.ChordalCatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(dataPoints, 1d, isClosed, tolerance);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Creates a spline using specific interpolation algorithm.
        /// </summary>
        /// <param name="algorithm">Algorithm.</param>
        /// <param name="resampledPoints">Resampled points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>Spline.</returns>
        public static List<ScreenPoint> CreateSpline(this InterpolationAlgorithm algorithm, IList<ScreenPoint> resampledPoints, bool isClosed, double tolerance) {
            switch (algorithm) {
                case InterpolationAlgorithm.Canonical:
                    return CanonicalSplineHelper.CreateSpline(resampledPoints, 0.5, null, isClosed, tolerance);
                case InterpolationAlgorithm.CatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(resampledPoints, 0.5, isClosed, tolerance);
                case InterpolationAlgorithm.UniformCatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(resampledPoints, 0d, isClosed, tolerance);
                case InterpolationAlgorithm.ChordalCatmullRom:
                    return CatmullRomSplineHelper.CreateSpline(resampledPoints, 1d, isClosed, tolerance);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}