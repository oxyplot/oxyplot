// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineStyleHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to convert from <see cref="LineStyle" /> to a stroke dash array.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#nullable enable

namespace OxyPlot
{
    /// <summary>
    /// Provides functionality to convert from <see cref="LineStyle" /> to a stroke dash array.
    /// </summary>
    public static class LineStyleHelper
    {
        /// <summary>
        /// Gets the stroke dash array for a given <see cref="LineStyle" />.
        /// </summary>
        /// <param name="style">The line style.</param>
        /// <returns>A dash array.</returns>
        public static double[]? GetDashArray(this LineStyle style)
        {
            return style switch
            {
                LineStyle.Solid => null,
                LineStyle.Dash => new double[] { 4, 1 },
                LineStyle.Dot => new double[] { 1, 1 },
                LineStyle.DashDot => new double[] { 4, 1, 1, 1 },
                LineStyle.DashDashDot => new double[] { 4, 1, 4, 1, 1, 1 },
                LineStyle.DashDotDot => new double[] { 4, 1, 1, 1, 1, 1 },
                LineStyle.DashDashDotDot => new double[] { 4, 1, 4, 1, 1, 1, 1, 1 },
                LineStyle.LongDash => new double[] { 10, 1 },
                LineStyle.LongDashDot => new double[] { 10, 1, 1, 1 },
                LineStyle.LongDashDotDot => new double[] { 10, 1, 1, 1, 1, 1 },
                _ => null,
            };
        }
    }
}
