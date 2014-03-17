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
    using Cairo;

    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts an <see cref="OxyPenLineJoin"/> to a <see cref="LineJoin"/>.
        /// </summary>
        /// <param name="lineJoin">The <see cref="OxyPenLineJoin"/> to convert.</param>
        /// <returns>The converted value.</returns>
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

        /// <summary>
        /// Sets the source color for the Cairo context.
        /// </summary>
        /// <param name="g">The Cairo context.</param>
        /// <param name="c">The color.</param>
        public static void SetSourceColor(this Context g, OxyColor c)
        {
            g.SetSourceRGBA(c.R / 256.0, c.G / 256.0, c.B / 256.0, c.A / 256.0);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint"/> to a Cairo <see cref="PointD"/>.
        /// </summary>
        /// <param name="pt">The point to convert.</param>
        /// <param name="aliased">Alias if set to <c>true</c>.</param>
        /// <returns>The converted point.</returns>
        public static PointD ToPointD(this ScreenPoint pt, bool aliased)
        {
            if (aliased)
            {
                return new PointD(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
            }

            return new PointD(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect"/> to a <see cref="Cairo.Rectangle"/>.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        /// <param name="aliased">
        /// Use pixel alignment if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// The converted rectangle.
        /// </returns>
        public static Rectangle ToRect(this OxyRect r, bool aliased)
        {
            if (aliased)
            {
                var x = 0.5 + (int)r.Left;
                var y = 0.5 + (int)r.Top;
                var ri = 0.5 + (int)r.Right;
                var bo = 0.5 + (int)r.Bottom;
                return new Rectangle(x, y, ri - x, bo - y);
            }

            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }
    }
}