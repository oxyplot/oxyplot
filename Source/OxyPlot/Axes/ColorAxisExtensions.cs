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

        /// <summary>
        /// Gets the high value of the specified palette index.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <returns>The value.</returns>
        public static double GetHighValue<T>(this T axis, int paletteIndex) where T: Axis, INumericColorAxis
        {
            return axis.GetLowValue(paletteIndex + 1);
        }

        /// <summary>
        /// Gets the low value of the specified palette index.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <returns>The value.</returns>
        public static double GetLowValue<T>(this T axis, int paletteIndex) where T : Axis, INumericColorAxis
        {
            return (double)paletteIndex / axis.Palette.Colors.Count * (axis.ClipMaximum - axis.ClipMinimum) + axis.ClipMinimum;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="paletteIndex">The color map index (less than NumberOfEntries).</param>
        /// <returns>The color.</returns>
        public static OxyColor GetColor<T>(this T axis, int paletteIndex) where T : Axis, INumericColorAxis
        {
            if (paletteIndex == int.MinValue)
            {
                return axis.InvalidNumberColor;
            }

            if (paletteIndex == 0)
            {
                return axis.LowColor;
            }

            if (paletteIndex == axis.Palette.Colors.Count + 1)
            {
                return axis.HighColor;
            }

            return axis.Palette.Colors[paletteIndex - 1];
        }
    }
}
