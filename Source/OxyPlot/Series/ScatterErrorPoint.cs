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
        /// <param name="lowerErrorX">The lower X error.</param>
        /// <param name="upperErrorX">The upper X error.</param>
        /// <param name="lowerErrorY">The lower Y error.</param>
        /// <param name="upperErrorY">The upper Y error.</param>
        public ScatterErrorPoint(double x, double y, double errorX = double.NaN, double errorY = double.NaN, double size = double.NaN, double value = double.NaN, object tag = null, double lowerErrorX = double.NaN, double upperErrorX = double.NaN, double lowerErrorY = double.NaN, double upperErrorY = double.NaN)
            : base(x, y, size, value, tag)
        {
            this.ErrorX = errorX;
            this.ErrorY = errorY;
            this.LowerErrorX = lowerErrorX;
            this.UpperErrorX = upperErrorX;
            this.LowerErrorY = lowerErrorY;
            this.UpperErrorY = upperErrorY;
            
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
        /// Gets the lower X value.
        /// </summary>
        public double LowerErrorX { get; private set; } = double.NaN;

        /// <summary>
        /// Gets the upper X value.
        /// </summary>
        public double UpperErrorX { get; private set; } = double.NaN;

        /// <summary>
        /// Gets the lower Y value.
        /// </summary>
        public double LowerErrorY { get; private set; } = double.NaN;

        /// <summary>
        /// Gets the upper Y value.
        /// </summary>
        public double UpperErrorY { get; private set; } = double.NaN;

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public override string ToCode()
        {
            if (double.IsNaN(this.LowerErrorX) && double.IsNaN(this.UpperErrorX) && double.IsNaN(this.LowerErrorY) && double.IsNaN(this.UpperErrorY))
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
            else
            {
                if (double.IsNaN(this.Size) && double.IsNaN(this.Value))
                {
                    return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}, {4}", this.X, this.Y, this.LowerErrorX, this.UpperErrorX, this.LowerErrorY, this.UpperErrorY);
                }

                if (double.IsNaN(this.Value))
                {
                    return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}, {3}, {4}, {5}", this.X, this.Y, this.LowerErrorX, this.UpperErrorX, this.LowerErrorY, this.UpperErrorY, this.Size);
                }


                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0}, {1}, {2}, {3}, {3}, {4}, {5}, {6}", this.X, this.Y, this.LowerErrorX, this.UpperErrorX, this.LowerErrorY, this.UpperErrorY, this.Size, this.Value);
            }

        }
    }
}
