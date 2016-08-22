// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenVector.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a vector defined in screen space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a vector defined in screen space.
    /// </summary>
    public struct ScreenVector : IEquatable<ScreenVector>
    {
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
        /// Initializes a new instance of the <see cref="ScreenVector" /> structure.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public ScreenVector(double x, double y)
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
        public static ScreenVector operator *(ScreenVector v, double d)
        {
            return new ScreenVector(v.x * d, v.y * d);
        }

        /// <summary>
        /// Adds a vector to another vector.
        /// </summary>
        /// <param name="v">The vector to add to.</param>
        /// <param name="d">The vector to be added.</param>
        /// <returns>The result of the operation.</returns>
        public static ScreenVector operator +(ScreenVector v, ScreenVector d)
        {
            return new ScreenVector(v.x + d.x, v.y + d.y);
        }

        /// <summary>
        /// Subtracts one specified vector from another.
        /// </summary>
        /// <param name="v">The vector to subtract from.</param>
        /// <param name="d">The vector to be subtracted.</param>
        /// <returns>The result of operation.</returns>
        public static ScreenVector operator -(ScreenVector v, ScreenVector d)
        {
            return new ScreenVector(v.x - d.x, v.y - d.y);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="v">The vector to negate.</param>
        /// <returns>The result of operation.</returns>
        public static ScreenVector operator -(ScreenVector v)
        {
            return new ScreenVector(-v.x, -v.y);
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:ScreenVector" /> object have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ScreenVector other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }
    }
}