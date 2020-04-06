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
    using System.Windows.Media;

    /// <summary>
    /// Provides functionality to support non-default dpi scaling
    /// </summary>
    public static class PixelLayout
    {
        /// <summary>
        /// Determines a visual offset of one <see cref="Visual"/> relative to another
        /// </summary>
        /// <param name="visual">The <see cref="Visual"/> which offset is calculated</param>
        /// <param name="root">The reference <see cref="Visual"/></param>
        /// <returns>A point structure which contains X and Y offsets</returns>
        public static Point GetVisualOffset(Visual visual, Visual root)
        {
            if (visual == null)
                throw new ArgumentNullException(nameof(visual));

            if (root == null)
                return default;

            return visual.TransformToAncestor(root).Transform(default);
        }

        /// <summary>
        /// Determines visual dpi scales of a <see cref="Visual"/>
        /// </summary>
        /// <param name="visual">The <see cref="Visual"/> which dpi scales are determined</param>
        /// <returns>A point structure which contains X and Y dpi scales</returns>
        public static Point GetDpiScales(Visual visual)
        {
            if (visual == null)
                throw new ArgumentNullException(nameof(visual));

            var transformMatrix = PresentationSource.FromVisual(visual)?
                .CompositionTarget?
                .TransformToDevice;

            if (!transformMatrix.HasValue)
                return new Point(1, 1);

            return new Point(
                transformMatrix.Value.M11,
                transformMatrix.Value.M22
            );
        }

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
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>Snapped point</returns>
        public static Point Snap(double x, double y, double strokeThickness, Point visualOffset, Point dpiScales)
        {
            var offsetX = visualOffset.X + GetPixelOffset(strokeThickness, dpiScales.X);
            var offsetY = visualOffset.Y + GetPixelOffset(strokeThickness, dpiScales.Y);

            x = Snap(x, offsetX, dpiScales.X);
            y = Snap(y, offsetY, dpiScales.Y);
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
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>Snapped rectangle structure</returns>
        public static Rect Snap(Rect rect, double strokeThickness, Point visualOffset, Point dpiScales)
        {
            var offsetX = visualOffset.X + GetPixelOffset(strokeThickness, dpiScales.X);
            var offsetY = visualOffset.Y + GetPixelOffset(strokeThickness, dpiScales.Y);

            var l = Snap(rect.Left, offsetX, dpiScales.X);
            var t = Snap(rect.Top, offsetY, dpiScales.Y);
            var r = Snap(rect.Right, offsetX, dpiScales.X);
            var b = Snap(rect.Bottom, offsetY, dpiScales.Y);

            return new Rect(l, t, r - l, b - t);
        }

        /// <summary>
        /// Snaps a stroke thickness to an integer multiple of device pixels.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>The snapped stroke thickness.</returns>
        public static double SnapStrokeThickness(double strokeThickness, Point dpiScales)
        {
            var scale = (dpiScales.X + dpiScales.Y) / 2;
            return Math.Round(strokeThickness * scale, MidpointRounding.AwayFromZero) / scale;
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
