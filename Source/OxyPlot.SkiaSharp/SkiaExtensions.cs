// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkiaExtensions.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using global::SkiaSharp;

    /// <summary>
    /// Provides extension methods for conversion between SkiaSharp and oxyplot objects.
    /// </summary>
    public static class SkiaExtensions
    {
        /// <summary>
        /// Converts a <see cref="OxyColor"/> to a <see cref="SKColor"/>;
        /// </summary>
        /// <param name="color">The <see cref="OxyColor"/>.</param>
        /// <returns>The <see cref="SKColor"/>.</returns>
        public static OxyColor ToOxyColor(this SKColor color)
        {
            return OxyColor.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
        }

        /// <summary>
        /// Converts a <see cref="SKColor"/> to a <see cref="OxyColor"/>;
        /// </summary>
        /// <param name="color">The <see cref="SKColor"/>.</param>
        /// <returns>The <see cref="OxyColor"/>.</returns>
        public static SKColor ToSKColor(this OxyColor color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }
}
