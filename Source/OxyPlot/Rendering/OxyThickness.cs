// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyThickness.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Describes the thickness of a frame around a rectangle. Four <see cref="System.Double" /> values describe the left, top, right, and bottom sides of the rectangle, respectively.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Describes the thickness of a frame around a rectangle. Four <see cref="System.Double" /> values describe the left, top, right, and bottom sides of the rectangle, respectively.
    /// </summary>
    public struct OxyThickness : ICodeGenerating
    {
        /// <summary>
        /// The bottom.
        /// </summary>
        private readonly double bottom;

        /// <summary>
        /// The left.
        /// </summary>
        private readonly double left;

        /// <summary>
        /// The right.
        /// </summary>
        private readonly double right;

        /// <summary>
        /// The top.
        /// </summary>
        private readonly double top;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyThickness" /> struct.
        /// </summary>
        /// <param name="thickness">The thickness.</param>
        public OxyThickness(double thickness)
            : this(thickness, thickness, thickness, thickness)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyThickness" /> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public OxyThickness(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Gets the bottom thickness.
        /// </summary>
        /// <value>The bottom thickness.</value>
        public double Bottom
        {
            get
            {
                return this.bottom;
            }
        }

        /// <summary>
        /// Gets the left thickness.
        /// </summary>
        /// <value>The left thickness.</value>
        public double Left
        {
            get
            {
                return this.left;
            }
        }

        /// <summary>
        /// Gets the right thickness.
        /// </summary>
        /// <value>The right thickness.</value>
        public double Right
        {
            get
            {
                return this.right;
            }
        }

        /// <summary>
        /// Gets the top thickness.
        /// </summary>
        /// <value>The top thickness.</value>
        public double Top
        {
            get
            {
                return this.top;
            }
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "new OxyThickness({0},{1},{2},{3})",
                this.Left,
                this.Top,
                this.Right,
                this.Bottom);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", this.left, this.top, this.right, this.bottom);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:OxyThickness" /> object have the same value.
        /// </summary>
        /// <param name="other">The thickness to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OxyThickness other)
        {
            return this.Left.Equals(other.Left) && this.Top.Equals(other.Top) && this.Right.Equals(other.Right) && this.Bottom.Equals(other.Bottom);
        }

        /// <summary>
        /// Creates a new <see cref="OxyThickness"/> with the maximum dimensions of this instance and the specified other instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>A new <see cref="OxyThickness"/>.</returns>
        public OxyThickness Include(OxyThickness other)
        {
            return new OxyThickness(Math.Max(other.Left, this.Left), Math.Max(other.Top, this.Top), Math.Max(other.Right, this.Right), Math.Max(other.Bottom, this.Bottom));
        }
    }
}
