// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorPoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a point in a <see cref="ScatterErrorSeries" />.
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
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="errorX">The X error.</param>
        /// <param name="errorY">The Y error.</param>
        /// <param name="size">The size.</param>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag.</param>
        public ScatterErrorPoint(double x, double y, double errorX, double errorY, double size = double.NaN, double value = double.NaN, object tag = null)
            : base(x, y, size, value, tag)
        {
            this.ErrorX = errorX;
            this.ErrorY = errorY;
        }

        /// <summary>
        /// Gets the error in X.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public double ErrorX { get; private set; }

        /// <summary>
        /// Gets the error in Y.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public double ErrorY { get; private set; }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public override string ToCode()
        {
            if (double.IsNaN(this.Size) && double.IsNaN(this.Value))
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}", this.X, this.Y, this.ErrorX, this.ErrorY);
            }

            if (double.IsNaN(this.Value))
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}, {4}", this.X, this.Y, this.ErrorX, this.ErrorY, this.Size);
            }

            return CodeGenerator.FormatConstructor(
                this.GetType(), "{0}, {1}, {2}, {3}, {3}, {4}, {5}", this.X, this.Y, this.ErrorX, this.ErrorY, this.Size, this.Value);
        }
    }
}