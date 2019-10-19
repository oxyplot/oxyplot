// --------------------------------------------------------------------------------------------------------------------
// <copyright company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Core.Drawing
{
    using System.Drawing;

    /// <summary>
    /// Implements extension methods coverting to/from OxyPlot data structures.
    /// </summary>
    public static class OxyPlotExtensions
    {
        /// <summary>
        /// Converts an <see cref="OxyRect"/> to a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        /// <returns>
        /// The converted rectangle.
        /// </returns>
        public static Rectangle ToRect(this OxyRect r)
        {
            return new Rectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height);
        }

        /// <summary>
        /// Converts a color to a Brush.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// A SolidColorBrush.
        /// </returns>
        public static Brush ToBrush(this OxyColor c)
        {
            return new SolidBrush(c.ToColor());
        }

        /// <summary>
        /// Converts an OxyColor to a Color.
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
    }
}
