// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterExtensions.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Extension method used to convert to/from Windows/Windows.Media classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        /// <param name="p1">
        /// The first point.
        /// </param>
        /// <param name="p2">
        /// The second point.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Converts an <see cref="OxyColor"/> to a <see cref="Brush"/>.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// A <see cref="SolidColorBrush"/>.
        /// </returns>
        public static Brush ToBrush(this OxyColor c)
        {
            return c != null ? new SolidColorBrush(c.ToColor()) : null;
        }

        /// <summary>
        /// Converts an <see cref="OxyColor"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// A Color.
        /// </returns>
        public static Color ToColor(this OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts an OxyThickness to a Thickness.
        /// </summary>
        /// <param name="c">The thickness.</param>
        /// <returns>A <see cref="Thickness"/> instance.</returns>
        public static Thickness ToThickness(this OxyThickness c)
        {
            return new Thickness(c.Left, c.Top, c.Right, c.Bottom);
        }

        /// <summary>
        /// Converts a ScreenVector to a Vector.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>A <see cref="Vector"/> instance.</returns>
        public static Vector ToVector(this ScreenVector c)
        {
            return new Vector(c.X, c.Y);
        }

        /// <summary>
        /// Converts a HorizontalAlignment to a HorizontalAlignment.
        /// </summary>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        /// <returns>
        /// A HorizontalAlignment.
        /// </returns>
        public static HorizontalAlignment ToHorizontalTextAlign(this System.Windows.HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case System.Windows.HorizontalAlignment.Center:
                    return HorizontalAlignment.Center;
                case System.Windows.HorizontalAlignment.Right:
                    return HorizontalAlignment.Right;
                default:
                    return HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Converts a HorizontalAlignment to a VerticalAlignment.
        /// </summary>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        /// <returns>
        /// A VerticalAlignment.
        /// </returns>
        public static VerticalAlignment ToVerticalTextAlign(this System.Windows.VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case System.Windows.VerticalAlignment.Center:
                    return VerticalAlignment.Middle;
                case System.Windows.VerticalAlignment.Top:
                    return VerticalAlignment.Top;
                default:
                    return VerticalAlignment.Bottom;
            }
        }

        /// <summary>
        /// Converts a Color to an OxyColor.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// An OxyColor.
        /// </returns>
        public static OxyColor ToOxyColor(this Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a nullable <see cref="Color"/> to an OxyColor.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// An OxyColor.
        /// </returns>
        public static OxyColor ToOxyColor(this Color? color)
        {
            return color.HasValue ? color.Value.ToOxyColor() : null;
        }

        /// <summary>
        /// Converts a <see cref="Brush"/> to an <see cref="OxyColor"/>.
        /// </summary>
        /// <param name="brush">
        /// The brush.
        /// </param>
        /// <returns>
        /// An <see cref="OxyColor"/>.
        /// </returns>
        public static OxyColor ToOxyColor(this Brush brush)
        {
            var scb = brush as SolidColorBrush;
            return scb != null ? scb.Color.ToOxyColor() : null;
        }

        /// <summary>
        /// Converts a Thickness to an <see cref="OxyThickness"/>.
        /// </summary>
        /// <param name="t">
        /// The thickness.
        /// </param>
        /// <returns>
        /// An <see cref="OxyThickness"/>.
        /// </returns>
        public static OxyThickness ToOxyThickness(this Thickness t)
        {
            return new OxyThickness(t.Left, t.Top, t.Right, t.Bottom);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint"/> to a <see cref="Point"/>.
        /// </summary>
        /// <param name="pt">
        /// The screen point.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment conversion if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A <see cref="Point"/>.
        /// </returns>
        public static Point ToPoint(this ScreenPoint pt, bool aliased)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new Point(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
            }

            return new Point(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect"/> to a <see cref="Rect"/>.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A <see cref="Rect"/>.
        /// </returns>
        public static Rect ToRect(this OxyRect r, bool aliased)
        {
            if (aliased)
            {
                double x = 0.5 + (int)r.Left;
                double y = 0.5 + (int)r.Top;
                double ri = 0.5 + (int)r.Right;
                double bo = 0.5 + (int)r.Bottom;
                return new Rect(x, y, ri - x, bo - y);
            }

            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts a <see cref="Point"/> to a <see cref="ScreenPoint"/>.
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <returns>
        /// A <see cref="ScreenPoint"/>.
        /// </returns>
        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a Point array to a ScreenPoint array.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// A ScreenPoint array.
        /// </returns>
        public static ScreenPoint[] ToScreenPointArray(this Point[] points)
        {
            if (points == null)
            {
                return null;
            }

            var pts = new ScreenPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pts[i] = points[i].ToScreenPoint();
            }

            return pts;
        }

        /// <summary>
        /// Converts the specified vector to a ScreenVector.
        /// </summary>
        /// <param name="vector">
        /// The vector.
        /// </param>
        /// <returns>
        /// A <see cref="ScreenVector"/>.
        /// </returns>
        public static ScreenVector ToScreenVector(this Vector vector)
        {
            return new ScreenVector(vector.X, vector.Y);
        }
    }
}