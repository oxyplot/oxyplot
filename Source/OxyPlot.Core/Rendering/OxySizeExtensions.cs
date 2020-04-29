// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySizeExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for <see cref="OxySize" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods for <see cref="OxySize"/>
    /// </summary>
    /// <remarks>These are pure methods. They could also be placed in the <see cref="OxySize" /> type with a PureAttribute.</remarks>
    public static class OxySizeExtensions
    {
        /// <summary>
        /// Calculates the bounds with respect to rotation angle and horizontal/vertical alignment.
        /// </summary>
        /// <param name="bounds">The size of the object to calculate bounds for.</param>
        /// <param name="angle">The rotation angle (degrees).</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <returns>A minimum bounding rectangle.</returns>
        public static OxyRect GetBounds(this OxySize bounds, double angle, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var u = horizontalAlignment == HorizontalAlignment.Left ? 0 : horizontalAlignment == HorizontalAlignment.Center ? 0.5 : 1;
            var v = verticalAlignment == VerticalAlignment.Top ? 0 : verticalAlignment == VerticalAlignment.Middle ? 0.5 : 1;

            var origin = new ScreenVector(u * bounds.Width, v * bounds.Height);

            if (angle == 0)
            {
                return new OxyRect(-origin.X, -origin.Y, bounds.Width, bounds.Height);
            }

            // the corners of the rectangle
            var p0 = new ScreenVector(0, 0) - origin;
            var p1 = new ScreenVector(bounds.Width, 0) - origin;
            var p2 = new ScreenVector(bounds.Width, bounds.Height) - origin;
            var p3 = new ScreenVector(0, bounds.Height) - origin;

            var theta = angle * Math.PI / 180.0;
            var costh = Math.Cos(theta);
            var sinth = Math.Sin(theta);
            Func<ScreenVector, ScreenVector> rotate = p => new ScreenVector((costh * p.X) - (sinth * p.Y), (sinth * p.X) + (costh * p.Y));

            var q0 = rotate(p0);
            var q1 = rotate(p1);
            var q2 = rotate(p2);
            var q3 = rotate(p3);

            var x = Math.Min(Math.Min(q0.X, q1.X), Math.Min(q2.X, q3.X));
            var y = Math.Min(Math.Min(q0.Y, q1.Y), Math.Min(q2.Y, q3.Y));
            var w = Math.Max(Math.Max(q0.X - x, q1.X - x), Math.Max(q2.X - x, q3.X - x));
            var h = Math.Max(Math.Max(q0.Y - y, q1.Y - y), Math.Max(q2.Y - y, q3.Y - y));

            return new OxyRect(x, y, w, h);
        }

        /// <summary>
        /// Gets the polygon outline of the specified rotated and aligned box.
        /// </summary>
        /// <param name="size">The size of the  box.</param>
        /// <param name="origin">The origin of the box.</param>
        /// <param name="angle">The rotation angle of the box.</param>
        /// <param name="horizontalAlignment">The horizontal alignment of the box.</param>
        /// <param name="verticalAlignment">The vertical alignment of the box.</param>
        /// <returns>A sequence of points defining the polygon outline of the box.</returns>
        public static IEnumerable<ScreenPoint> GetPolygon(this OxySize size, ScreenPoint origin, double angle, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var u = horizontalAlignment == HorizontalAlignment.Left ? 0 : horizontalAlignment == HorizontalAlignment.Center ? 0.5 : 1;
            var v = verticalAlignment == VerticalAlignment.Top ? 0 : verticalAlignment == VerticalAlignment.Middle ? 0.5 : 1;

            var offset = new ScreenVector(u * size.Width, v * size.Height);

            // the corners of the rectangle
            var p0 = new ScreenVector(0, 0) - offset;
            var p1 = new ScreenVector(size.Width, 0) - offset;
            var p2 = new ScreenVector(size.Width, size.Height) - offset;
            var p3 = new ScreenVector(0, size.Height) - offset;

            if (angle != 0)
            {
                var theta = angle * Math.PI / 180.0;
                var costh = Math.Cos(theta);
                var sinth = Math.Sin(theta);
                Func<ScreenVector, ScreenVector> rotate = p => new ScreenVector((costh * p.X) - (sinth * p.Y), (sinth * p.X) + (costh * p.Y));

                p0 = rotate(p0);
                p1 = rotate(p1);
                p2 = rotate(p2);
                p3 = rotate(p3);
            }

            yield return origin + p0;
            yield return origin + p1;
            yield return origin + p2;
            yield return origin + p3;
        }
    }
}
