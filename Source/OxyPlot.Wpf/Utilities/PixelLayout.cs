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
        /// Snaps a screen point to a pixel grid
        /// </summary>
        /// <param name="point">Screen point</param>
        /// <param name="visualOffset">A point structure which represents X and Y visual offsets relative to visual root</param>
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>Snapped point</returns>
        public static Point Snap(Point point, ref Point visualOffset, ref Point dpiScales)
        {
            return new Point(Snap(point.X, visualOffset.X, dpiScales.X) + 0.5 / dpiScales.X, Snap(point.Y, visualOffset.Y, dpiScales.Y) + 0.5 / dpiScales.Y);
        }

        /// <summary>
        /// Snaps a size structure to a pixel grid
        /// </summary>
        /// <param name="size">Size structure</param>
        /// <param name="visualOffset">A point structure which represents X and Y visual offsets relative to visual root</param>
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>Snapped size structure</returns>
        public static Size Snap(Size size, ref Point visualOffset, ref Point dpiScales)
        {
            return new Size(Snap(size.Width, visualOffset.X, dpiScales.X), Snap(size.Height, visualOffset.Y, dpiScales.Y));
        }

        /// <summary>
        /// Snaps a rectangle structure to a pixel grid
        /// </summary>
        /// <param name="rect">Rectangle structure</param>
        /// <param name="visualOffset">A point structure which represents X and Y visual offsets relative to visual root</param>
        /// <param name="dpiScales">A point structure which represents X and Y pixel grid scales</param>
        /// <returns>Snapped rectangle structure</returns>
        public static Rect Snap(Rect rect, ref Point visualOffset, ref Point dpiScales)
        {
            var l = Snap(rect.Left, visualOffset.X, dpiScales.X);
            var t = Snap(rect.Top, visualOffset.Y, dpiScales.Y);
            var r = Snap(rect.Right, visualOffset.X, dpiScales.X);
            var b = Snap(rect.Bottom, visualOffset.Y, dpiScales.Y);

            return new Rect(l, t, r - l, b - t);
        }
    }
}
