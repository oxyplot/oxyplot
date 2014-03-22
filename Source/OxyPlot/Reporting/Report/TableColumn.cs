// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableColumn.cs" company="OxyPlot">
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
//   Represents a table column definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a table column definition.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TableColumn" /> class.
        /// </summary>
        public TableColumn()
        {
            this.Width = double.NaN;
            this.Alignment = Alignment.Center;
        }

        /// <summary>
        /// Gets the actual width (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; internal set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the column.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is a header.
        /// </summary>
        public bool IsHeader { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        /// <remarks>
        /// NaN: auto width.
        /// Negative numbers: weights
        /// </remarks>
        public double Width { get; set; }
    }
}