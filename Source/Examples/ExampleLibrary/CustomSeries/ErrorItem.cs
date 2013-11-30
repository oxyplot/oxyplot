// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorItem.cs" company="OxyPlot">
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
//   Represents an error item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExampleLibrary
{
    using OxyPlot;

    /// <summary>
    /// Represents an error item.
    /// </summary>
    public class ErrorItem : IDataPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem"/> class.
        /// </summary>
        public ErrorItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="xerror">The xerror.</param>
        /// <param name="yerror">The yerror.</param>
        public ErrorItem(double x, double y, double xerror, double yerror)
        {
            this.X = x;
            this.Y = y;
            this.XError = xerror;
            this.YError = yerror;
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the X error.
        /// </summary>
        public double XError { get; set; }

        /// <summary>
        /// Gets or sets the Y error.
        /// </summary>
        public double YError { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>
        /// C# code.
        /// </returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", this.X, this.Y, this.XError, this.YError);
        }
    }
}