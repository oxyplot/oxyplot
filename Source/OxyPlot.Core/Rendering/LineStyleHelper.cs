// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineStyleHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to convert from <see cref="LineStyle" /> to a stroke dash array.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        public static double[] GetDashArray(this LineStyle style)
        {
            switch (style)
            {
                case LineStyle.Solid:
                    return null;
                case LineStyle.Dash:
                    return new double[] { 4, 1 };
                case LineStyle.Dot:
                    return new double[] { 1, 1 };
                case LineStyle.DashDot:
                    return new double[] { 4, 1, 1, 1 };
                case LineStyle.DashDashDot:
                    return new double[] { 4, 1, 4, 1, 1, 1 };
                case LineStyle.DashDotDot:
                    return new double[] { 4, 1, 1, 1, 1, 1 };
                case LineStyle.DashDashDotDot:
                    return new double[] { 4, 1, 4, 1, 1, 1, 1, 1 };
                case LineStyle.LongDash:
                    return new double[] { 10, 1 };
                case LineStyle.LongDashDot:
                    return new double[] { 10, 1, 1, 1 };
                case LineStyle.LongDashDotDot:
                    return new double[] { 10, 1, 1, 1, 1, 1 };
                default:
                    return null;
            }
        }
    }
}