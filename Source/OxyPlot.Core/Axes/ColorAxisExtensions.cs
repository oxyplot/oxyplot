// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorAxisExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for <see cref="IColorAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Provides extension methods for <see cref="IColorAxis" />.
    /// </summary>
    public static class ColorAxisExtensions
    {
        /// <summary>
        /// Gets the color for the specified value.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="value">The value.</param>
        /// <returns>The color.</returns>
        public static OxyColor GetColor(this IColorAxis axis, double value)
        {
            return axis.GetColor(axis.GetPaletteIndex(value));
        }
    }
}