// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVector.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a point in the data space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a vector in the data space.
    /// </summary>
    public struct DataVector : ICodeGenerating, IEquatable<DataVector>
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly DataVector Undefined = new DataVector(double.NaN, double.NaN);

        /// <summary>
        /// The x-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        internal double x;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        internal double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataVector" /> structure.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public DataVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt((this.x * this.x) + (this.y * this.y));
            }
        }

        /// <summary>
        /// Gets the length squared.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return (this.x * this.x) + (this.y * this.y);
            }
        }

        /// <summary>
        /// Gets the x-coordinate.
        /// </summary>
        /// <value>The x-coordinate.</value>
        public double X
        {
            get
            {
                return this.x;
            }
        }

        /// <summary>
        /// Gets the y-coordinate.
        /// </summary>
        /// <value>The y-coordinate.</value>
        public double Y
        {
            get
            {
                return this.y;
            }
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="d">The multiplication factor.</param>
        /// <returns>The result of the operator.</returns>
        public static DataVector operator *(DataVector v, double d)
        {
            return new DataVector(v.x * d, v.y * d);
        }

        /// <summary>
        /// Adds a vector to another vector.
        /// </summary>
        /// <param name="v">The vector to add to.</param>
        /// <param name="d">The vector to be added.</param>
        /// <returns>The result of the operation.</returns>
        public static DataVector operator +(DataVector v, DataVector d)
        {
            return new DataVector(v.x + d.x, v.y + d.y);
        }

        /// <summary>
        /// Subtracts one specified vector from another.
        /// </summary>
        /// <param name="v">The vector to subtract from.</param>
        /// <param name="d">The vector to be subtracted.</param>
        /// <returns>The result of operation.</returns>
        public static DataVector operator -(DataVector v, DataVector d)
        {
            return new DataVector(v.x - d.x, v.y - d.y);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="v">The vector to negate.</param>
        /// <returns>The result of operation.</returns>
        public static DataVector operator -(DataVector v)
        {
            return new DataVector(-v.x, -v.y);
        }

        /// <summary>
        /// Adds a vector to a <see cref="DataPoint"/>.
        /// </summary>
        /// <param name="p">The point to add to.</param>
        /// <param name="v">The vector to be added.</param>
        /// <returns>The <see cref="DataPoint"/> result of the operation.</returns>
        public static DataPoint operator +(DataPoint p, DataVector v)
        {
            return new DataPoint(p.x + v.x, p.y + v.y);
        }

        /// <summary>
        /// Adds a vector to a <see cref="DataPoint"/>.
        /// </summary>
        /// <param name="v">The vector to be added.</param>
        /// <param name="p">The point to add to.</param>
        /// <returns>The <see cref="DataPoint"/> result of the operation.</returns>
        public static DataPoint operator +(DataVector v, DataPoint p)
        {
            return new DataPoint(v.x + p.x, v.y + p.y);
        }

        /// <summary>
        /// Subtracts one specified vector from a <see cref="DataPoint"/>.
        /// </summary>
        /// <param name="p">The point to subtract from.</param>
        /// <param name="v">The vector to be subtracted.</param>
        /// <returns>The <see cref="DataPoint"/> result of the operation.</returns>
        public static DataPoint operator -(DataPoint p, DataVector v)
        {
            return new DataPoint(p.x - v.x, p.y - v.y);
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            double l = Math.Sqrt((this.x * this.x) + (this.y * this.y));
            if (l > 0)
            {
                this.x /= l;
                this.y /= l;
            }
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.x, this.y);
        }

        /// <summary>
        /// Determines whether this and another specified <see cref="T:DataVector" /> have the same value.
        /// </summary>
        /// <param name="other">The vector to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DataVector other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
        }

        /// <summary>
        /// Determines whether this point is defined.
        /// </summary>
        /// <returns><c>true</c> if this point is defined; otherwise, <c>false</c>.</returns>
        public bool IsDefined()
        {
            // check that x and y is not NaN (the code below is faster than double.IsNaN)
#pragma warning disable 1718
            // ReSharper disable EqualExpressionComparison
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.x == this.x && this.y == this.y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }
    }
}
