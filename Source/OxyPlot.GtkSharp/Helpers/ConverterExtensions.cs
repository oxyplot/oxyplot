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

namespace OxyPlot.GtkSharp
{
    using System;
    using Cairo;

    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        public static LineJoin ToLineJoin(this OxyPenLineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case OxyPenLineJoin.Round:
                    return LineJoin.Round;
                case OxyPenLineJoin.Bevel:
                    return LineJoin.Bevel;
                default:
                    return LineJoin.Miter;
            }
        }
        public static void SetSourceColor(this Cairo.Context g, OxyColor c)
        {
            g.SetSourceRGBA(c.R / 256.0, c.G / 256.0, c.B / 256.0, c.A / 256.0);
        }
                
        /// <summary>
        /// Converts a HorizontalAlignment to a HorizontalTextAlign.
        /// </summary>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        /// <returns>
        /// A HorizontalTextAlign.
        /// </returns>
        public static OxyPlot.HorizontalAlignment ToHorizontalTextAlign(this HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return OxyPlot.HorizontalAlignment.Center;
                case HorizontalAlignment.Right:
                    return OxyPlot.HorizontalAlignment.Right;
                default:
                    return OxyPlot.HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Converts a Thickness to an OxyThickness.
        /// </summary>
        /// <returns>
        /// An OxyPlot thickness.
        /// </returns>
        /// <summary>
        /// Converts a ScreenPoint to a Point.
        /// </summary>
        /// <param name="pt">
        /// The screen point.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment conversion if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A point.
        /// </returns>
        public static Cairo.PointD ToPointD(this ScreenPoint pt, bool aliased)
        {
            if (aliased)
            {
                return new Cairo.PointD(pt.X, pt.Y);
            }

            return new Cairo.PointD((int)Math.Round(pt.X), (int)Math.Round(pt.Y));
        }

        /// <summary>
        /// Converts an OxyRect to a Rect.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A rect.
        /// </returns>
        public static Rectangle ToRect(this OxyRect r, bool aliased)
        {
            if (aliased)
            {
                var x = (int)r.Left;
                var y = (int)r.Top;
                var ri = (int)r.Right;
                var bo = (int)r.Bottom;
                return new Rectangle(x, y, ri - x, bo - y);
            }

            return new Rectangle(
                (int)Math.Round(r.Left), (int)Math.Round(r.Top), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
        }

        /// <summary>
        /// Converts a point to a ScreenPoint.
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <returns>
        /// A screen point.
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
    }
}