// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorItem.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a VectorSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents an item in a <see cref="VectorSeries" />.
    /// </summary>
    public class VectorItem : ICodeGenerating, IEquatable<VectorItem>
    {
        /// <summary>
        /// The undefined Vector item.
        /// </summary>
        public static readonly VectorItem Undefined = new VectorItem(DataPoint.Undefined, DataVector.Undefined, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorItem" /> struct.
        /// </summary>
        /// <param name="origin">The origin whence the vector originates.</param>
        /// <param name="direction">The direction of the vector.</param>
        /// <param name="value">The value of the vector.</param>
        public VectorItem(DataPoint origin, DataVector direction, double value)
        {
            this.Origin = origin;
            this.Direction = direction;
            this.Value = value;
        }

        /// <summary>
        /// Gets the origin of the vector.
        /// </summary>
        /// <value>The origin of the vector.</value>
        public DataPoint Origin { get; }

        /// <summary>
        /// Gets the direction of the vector.
        /// </summary>
        /// <value>The direction of the vector.</value>
        public DataVector Direction { get; }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        /// <value>The value can be used to color-code the Vector.</value>
        public double Value { get; }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", this.Origin, this.Direction, this.Value);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:VectorItem" /> object have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(VectorItem other)
        {
            // TODO: this is copied from RectangleItem and looks wrong
            return this.Origin.Equals(other.Origin) && this.Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{this.Origin} {this.Direction} {this.Value}";
        }

        /// <summary>
        /// Determines whether this Vector item is defined.
        /// </summary>
        /// <returns><c>true</c> if this point is defined; otherwise, <c>false</c>.</returns>
        public bool IsDefined()
        {
            // check that x and y is not NaN (the code below is faster than double.IsNaN)
#pragma warning disable 1718
            // ReSharper disable EqualExpressionComparison
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.Origin.IsDefined() && this.Direction.IsDefined() && !double.IsNaN(this.Value);
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }
    }
}
