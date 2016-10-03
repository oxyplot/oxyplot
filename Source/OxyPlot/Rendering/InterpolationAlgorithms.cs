// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterpolationAlgorithms.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements a set of predefined interpolation algorithms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Implements a set of predefined interpolation algorithms.
    /// </summary>
    public static class InterpolationAlgorithms
    {
        /// <summary>
        /// Canonical spline, also known as Cardinal spline.
        /// </summary>
        public static IInterpolationAlgorithm CanonicalSpline { get; } = new CanonicalSpline(0.5);

        /// <summary>
        /// Centripetal Catmull–Rom spline.
        /// </summary>
        public static IInterpolationAlgorithm CatmullRomSpline { get; } = new CatmullRomSpline(0.5);

        /// <summary>
        /// Uniform Catmull–Rom spline.
        /// </summary>
        public static IInterpolationAlgorithm UniformCatmullRomSpline { get; } = new CatmullRomSpline(0.0);

        /// <summary>
        /// Chordal Catmull–Rom spline.
        /// </summary>
        public static IInterpolationAlgorithm ChordalCatmullRomSpline { get; } = new CatmullRomSpline(1.0);
    }
}