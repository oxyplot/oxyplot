// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyCircle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Describes the width, height, and point origin of a rectangle.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Describes the center, the inner and the outer radius of a hypoid.
    /// </summary>
    public struct OxyAnnulus : IShape, IEquatable<OxyAnnulus>, IFormattable
    {

        /// <summary>
        /// Gets the center point of the hypoid.
        /// </summary>
        /// <value>The center.</value>
        public ScreenPoint Center { get; set; }

        /// <summary>
        /// Gets the inner radius of the hypoid
        /// </summary>
        public double InnerRadius { get; set; }

        /// <summary>
        /// Gets the outer radius of the hypoid
        /// </summary>
        public double OuterRadius { get; set; }

        /// <summary>
        /// Gets the inner diameter of the hypoid.
        /// </summary>
        /// <value>The inner diameter.</value>
        public double InnerDiameter
        {
            get
            {
                return this.InnerRadius * 2;
            }
        }

        /// <summary>
        /// Gets the outer diameter of the hypoid.
        /// </summary>
        /// <value>The outer diameter.</value>
        public double OuterDiameter
        {
            get
            {
                return this.OuterRadius * 2;
            }
        }

        /// <summary>
        /// Gets the inner top of the hypoid.
        /// </summary>
        /// <value>The top inner point.</value>
        public ScreenPoint InnerTop => new ScreenPoint(this.Center.X, this.Center.Y + this.InnerRadius);

        /// <summary>
        /// Gets the inner bottom of the hypoid.
        /// </summary>
        /// <value>The bottom inner point.</value>
        public ScreenPoint InnerBottom => new ScreenPoint(this.Center.X, this.Center.Y - this.InnerRadius);

        /// <summary>
        /// Gets the inner left of the hypoid.
        /// </summary>
        /// <value>The most left inner point.</value>
        public ScreenPoint InnerLeft => new ScreenPoint(this.Center.X - this.InnerRadius, this.Center.Y);

        /// <summary>
        /// Gets the inner right of the hypoid.
        /// </summary>
        /// <value>The most right inner point.</value>
        public ScreenPoint InnerRight => new ScreenPoint(this.Center.X + this.InnerRadius, this.Center.Y);

        /// <summary>
        /// Gets the outer top of the hypoid.
        /// </summary>
        /// <value>The top outer point.</value>
        public ScreenPoint OuterTop => new ScreenPoint(this.Center.X, this.Center.Y + this.OuterRadius);

        /// <summary>
        /// Gets the outer bottom of the hypoid.
        /// </summary>
        /// <value>The bottom outer point.</value>
        public ScreenPoint OuterBottom => new ScreenPoint(this.Center.X, this.Center.Y - this.OuterRadius);

        /// <summary>
        /// Gets the outer left of the hypoid.
        /// </summary>
        /// <value>The most left outer point.</value>
        public ScreenPoint OuterLeft => new ScreenPoint(this.Center.X - this.OuterRadius, this.Center.Y);

        /// <summary>
        /// Gets the outer right of the hypoid.
        /// </summary>
        /// <value>The most right outer point.</value>
        public ScreenPoint OuterRight => new ScreenPoint(this.Center.X + this.OuterRadius, this.Center.Y);

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyAnnulus"/> structure that has a specified center a inner and outer radius
        /// </summary>
        /// <param name="center">The center location of the hypoid</param>
        /// <param name="r1">The inner radius of the hypoid</param>
        /// <param name="r2">The outer radius of the hypoid</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r1;The inner radius should not be negative.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">r2;The outer radius should not be equal or smaller as the inner radius.</exception>
        public OxyAnnulus(ScreenPoint center, double r1, double r2)
        {
            if (r1 < 0)
                throw new ArgumentOutOfRangeException("r1", "The inner radius should not be negative.");
            if (r2 <= r1)
                throw new ArgumentOutOfRangeException("r2", "The outer radius should not be equal or smaller as the inner radius.");

            this.Center = new ScreenPoint(center.X, center.Y);
            this.InnerRadius = r1;
            this.OuterRadius = r2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyAnnulus"/> structure that has a specified center a inner and outer radius
        /// </summary>
        /// <param name="x">The x-coordinate location of the center of the circle</param>
        /// <param name="y">The x-coordinate location of the center of the circle</param>
        /// <param name="r1">The inner radius of the hypoid</param>
        /// <param name="r2">The outer radius of the hypoid</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r1;The inner radius should not be negative.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">r2;The outer radius should not be equal or smaller as the inner radius.</exception>
        public OxyAnnulus(double x, double y, double r1, double r2) : this(new ScreenPoint(x, y), r1, r2)
        {
        }

        /// <summary>
        /// Determines whether the specified point is inside the hypoid.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the hypoid contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(double x, double y)
        {
            return this.Center.DistanceTo(new ScreenPoint(x, y)) < this.OuterRadius && this.Center.DistanceTo(new ScreenPoint(x, y)) > this.InnerRadius;
        }

        /// <summary>
        /// Determines whether the specified point is inside the hypoid.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns><c>true</c> if the hypoid contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(ScreenPoint p)
        {
            return this.Contains(p.x, p.y);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:OxyHypoid" /> object have the same value.
        /// </summary>
        /// <param name="other">The circle to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OxyAnnulus other)
        {
            return this.Center.Equals(other.Center) && this.InnerRadius.Equals(other.InnerRadius) && this.InnerRadius.Equals(other.OuterRadius);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", this.Center.X, this.Center.Y, this.InnerRadius, this.OuterRadius);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            const string Separator = ", ";
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(this.Center.X.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.Center.Y.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.InnerRadius.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.OuterRadius.ToString(format, formatProvider));
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Returns a hypoid that is expanded or shrunk by the specified radius amounts.
        /// </summary>
        /// <param name="dr1">The amount by which to expand or shrink the inner radius of the hypoid.</param>
        /// /// <param name="dr2">The amount by which to expand or shrink the outerradius of the hypoid.</param>
        /// <returns>The expanded/shrunk <see cref="OxyAnnulus" />.</returns>
        public OxyAnnulus Inflate(double dr1, double dr2)
        {
            return new OxyAnnulus(this.Center, this.InnerRadius + dr1, this.OuterRadius + dr2);
        }

        /// <summary>
        /// Returns a hypoid that is moved by the specified horizontal and vertical amounts.
        /// </summary>
        /// <param name="offsetX">The amount to move the hypoid horizontally.</param>
        /// <param name="offsetY">The amount to move the hypoid vertically.</param>
        /// <returns>The moved <see cref="OxyAnnulus" />.</returns>
        public OxyAnnulus Offset(double offsetX, double offsetY)
        {
            return new OxyAnnulus(this.Center.X + offsetX, this.Center.Y + offsetY, this.InnerRadius, this.OuterRadius);
        }
    }
}
