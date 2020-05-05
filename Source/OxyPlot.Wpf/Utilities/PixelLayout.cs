// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PixelLayout.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to support non-default dpi scaling
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Provides functionality to support non-default dpi scaling
    /// </summary>
    public static class PixelLayout
    {
        /// <summary>
        /// Snaps a screen point to a pixel grid.
        /// </summary>
        /// <remarks>
        /// Depending on the stroke thickness, the point is snapped either to the middle or the border of a pixel.
        /// </remarks>
        /// <param name="x">The x coordinate of the point.</param>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="visualOffset">A point structure which represents X and Y visual offsets relative to visual root</param>
        /// <param name="dpiScale">The DPI scale.</param>
        /// <returns>Snapped point</returns>
        public static Point Snap(double x, double y, double strokeThickness, Point visualOffset, double dpiScale)
        {
            var offsetX = visualOffset.X + GetPixelOffset(strokeThickness, dpiScale);
            var offsetY = visualOffset.Y + GetPixelOffset(strokeThickness, dpiScale);

            x = Snap(x, offsetX, dpiScale);
            y = Snap(y, offsetY, dpiScale);
            return new Point(x, y);
        }

        /// <summary>
        /// Snaps a rectangle structure to a pixel grid.
        /// </summary>
        /// <remarks>
        /// Depending on the stroke thickness, the rectangle bounds are snapped either to the middle or the border of pixels.
        /// </remarks>
        /// <param name="rect">Rectangle structure</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="visualOffset">A point structure which represents X and Y visual offsets relative to visual root</param>
        /// <param name="dpiScale">The DPI scale.</param>
        /// <returns>Snapped rectangle structure</returns>
        public static Rect Snap(Rect rect, double strokeThickness, Point visualOffset, double dpiScale)
        {
            var offsetX = visualOffset.X + GetPixelOffset(strokeThickness, dpiScale);
            var offsetY = visualOffset.Y + GetPixelOffset(strokeThickness, dpiScale);

            var l = Snap(rect.Left, offsetX, dpiScale);
            var t = Snap(rect.Top, offsetY, dpiScale);
            var r = Snap(rect.Right, offsetX, dpiScale);
            var b = Snap(rect.Bottom, offsetY, dpiScale);

            return new Rect(l, t, r - l, b - t);
        }

        /// <summary>
        /// Snaps a stroke thickness to an integer multiple of device pixels.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="dpiScale">The DPI scale.</param>
        /// <returns>The snapped stroke thickness.</returns>
        public static double SnapStrokeThickness(double strokeThickness, double dpiScale)
        {
            return Math.Round(strokeThickness * dpiScale, MidpointRounding.AwayFromZero) / dpiScale;
        }

        /// <summary>
        /// Snaps a screen coordinate to a pixel grid
        /// </summary>
        /// <param name="value">Screen coordinate</param>
        /// <param name="offset">Pixel grid offset</param>
        /// <param name="scale">Pixel grid scale</param>
        /// <returns>Snapped coordinate</returns>
        private static double Snap(double value, double offset, double scale)
        {
            return (Math.Round((value + offset) * scale, MidpointRounding.AwayFromZero)) / scale - offset;
        }

        /// <summary>
        /// Gets the pixel offset for the given scale and stroke thickness.
        /// </summary>
        /// <remarks>
        /// This takes into account that lines with even width should be rendered on the border between two pixels, while lines with odd width should be rendered
        /// in the middle of a pixel.
        /// </remarks>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="scale">Pixel grid scale</param>
        /// <returns>The pixel offset.</returns>
        private static double GetPixelOffset(double strokeThickness, double scale)
        {
            var mod = strokeThickness * scale % 2;
            var isOdd = mod >= 0.5 && mod < 1.5;
            return isOdd ? 0.5 / scale : 0;
        }
    }
}
