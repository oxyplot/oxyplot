// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
    public class ErrorItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem" /> class.
        /// </summary>
        public ErrorItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorItem" /> class.
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
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", this.X, this.Y, this.XError, this.YError);
        }
    }
}