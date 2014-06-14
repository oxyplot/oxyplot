// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorPoint.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Represents a point in a ScatterErrorSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents a point in a <see cref="ScatterErrorSeries" />.
    /// </summary>
    public class ScatterErrorPoint : ScatterPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterErrorPoint"/> class.
        /// </summary>
        public ScatterErrorPoint()
        {
            this.Error = double.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterErrorPoint"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="error">The error.</param>
        /// <param name="size">The size.</param>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag.</param>
        public ScatterErrorPoint(double x, double y, double error, double size = double.NaN, double value = double.NaN, object tag = null)
            : base(x, y, size, value, tag)
        {
            this.Error = error;
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public double Error { get; set; }
    }
}
