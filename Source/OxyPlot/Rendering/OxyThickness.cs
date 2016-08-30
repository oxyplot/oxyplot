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
        /// Gets the height.
        /// </summary>
        public double Height
        {
            get
            {
                return this.Bottom - this.Top;
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
        /// Gets the width.
        /// </summary>
        public double Width
        {
            get
            {
                return this.Right - this.Left;
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
            return this.Left.Equals(other.Left) && this.Top.Equals(other.Top) && this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }
    }
}