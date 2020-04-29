// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a RectangleSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents an item in a <see cref="RectangleSeries" />.
    /// </summary>
    /// <remarks><see cref="RectangleItem" />s are transformed to <see cref="OxyRect" />s.</remarks>
    public class RectangleItem : ICodeGenerating, IEquatable<RectangleItem>
    {
        /// <summary>
        /// The undefined rectangle item.
        /// </summary>
        public static readonly RectangleItem Undefined = new RectangleItem(DataPoint.Undefined, DataPoint.Undefined, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleItem" /> struct.
        /// </summary>
        /// <param name="x1">The x coordinate of the first corner.</param>
        /// <param name="x2">The x coordinate of the diagonally-opposite corner.</param>
        /// <param name="y1">The y coordinate of the first corner.</param>
        /// <param name="y2">The y coordinate of the diagonally-opposite corner.</param>
        /// <param name="value">The value of the data rect.</param>
        public RectangleItem(double x1, double x2, double y1, double y2, double value)
        {
            this.A = new DataPoint(x1, y1);
            this.B = new DataPoint(x2, y2);
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleItem" /> struct.
        /// </summary>
        /// <param name="a">The first corner.</param>
        /// <param name="b">The diagonally-opposite corner.</param>
        /// <param name="value">The value of the data rect.</param>
        public RectangleItem(DataPoint a, DataPoint b, double value)
        {
            this.A = a;
            this.B = b;
            this.Value = value;
        }

        /// <summary>
        /// Gets the first data point.
        /// </summary>
        /// <value>The first data point.</value>
        public DataPoint A { get; }

        /// <summary>
        /// Gets the diagonally-opposite data point.
        /// </summary>
        /// <value>The diagonally-opposite data point.</value>
        public DataPoint B { get; }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        /// <value>The value can be used to color-code the rectangle.</value>
        public double Value { get; }

        /// <summary>
        /// Determines whether the specified point lies within the boundary of the rectangle.
        /// </summary>
        /// <returns><c>true</c> if the value of the <param name="p"/> parameter is inside the bounds of this instance.</returns>
        public bool Contains(DataPoint p)
        {
            return (p.X <= this.B.X && p.X >= this.A.X && p.Y <= this.B.Y && p.Y >= this.A.Y) ||
                   (p.X <= this.A.X && p.X >= this.B.X && p.Y <= this.A.Y && p.Y >= this.B.Y);
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", this.A, this.B, this.Value);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:RectangleItem" /> object have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(RectangleItem other)
        {
            return this.A.Equals(other.A) && this.B.Equals(other.B);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{this.A} {this.B} {this.Value}";
        }

        /// <summary>
        /// Determines whether this rectangle item is defined.
        /// </summary>
        /// <returns><c>true</c> if this point is defined; otherwise, <c>false</c>.</returns>
        public bool IsDefined()
        {
            // check that x and y is not NaN (the code below is faster than double.IsNaN)
#pragma warning disable 1718
            // ReSharper disable EqualExpressionComparison
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.A.IsDefined() && this.B.IsDefined() && !double.IsNaN(this.Value);
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }
    }
}