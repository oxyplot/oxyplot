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
    /// Describes the center and the radius of a circle.
    /// </summary>
    public struct OxyCircle : IOxyRegion, IEquatable<OxyCircle>, IFormattable
    {

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public ScreenPoint Center { get; private set; }

        /// <summary>
        /// Gets the radius of the circle
        /// </summary>
        public double Radius { get; private set; }

        /// <summary>
        /// Gets the diameter of the circle.
        /// </summary>
        /// <value>The width.</value>
        public double Diameter
        {
            get
            {
                return this.Radius * 2;
            }
        }

        /// <summary>
        /// Gets the top of the circle.
        /// </summary>
        /// <value>The top point.</value>
        public ScreenPoint Top => new ScreenPoint(this.Center.X, this.Center.Y + this.Radius);

        /// <summary>
        /// Gets the bottom of the circle.
        /// </summary>
        /// <value>The bottom point.</value>
        public ScreenPoint Bottom => new ScreenPoint(this.Center.X, this.Center.Y - this.Radius);

        /// <summary>
        /// Gets the left of the circle.
        /// </summary>
        /// <value>The most left point.</value>
        public ScreenPoint Left => new ScreenPoint(this.Center.X - this.Radius, this.Center.Y);

        /// <summary>
        /// Gets the right of the circle.
        /// </summary>
        /// <value>The most right point.</value>
        public ScreenPoint Right => new ScreenPoint(this.Center.X + this.Radius, this.Center.Y);

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyCircle"/> structure that has a specified center and a radius
        /// </summary>
        /// <param name="center">The center location of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r;The radius should not be negative.</exception>
        public OxyCircle(ScreenPoint center, double radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "The radius should not be negative.");
            this.Center = new ScreenPoint(center.X, center.Y);
            this.Radius = radius;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyCircle"/> structure that has a specified center and a radius
        /// </summary>
        /// <param name="x">The x-coordinate location of the center of the circle</param>
        /// <param name="y">The x-coordinate location of the center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r;The radius should not be negative.</exception>
        public OxyCircle(double x, double y, double radius) : this(new ScreenPoint(x, y), radius)
        {
        }

        /// <summary>
        /// Determines whether the specified point is inside the circle.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the circle contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(double x, double y)
        {
            return this.Center.DistanceTo(new ScreenPoint(x, y)) < this.Radius;
        }

        /// <summary>
        /// Determines whether the specified point is inside the circle.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns><c>true</c> if the circle contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(ScreenPoint p)
        {
            return this.Contains(p.x, p.y);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:OxyCircle" /> object have the same value.
        /// </summary>
        /// <param name="other">The circle to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OxyCircle other)
        {
            return this.Center.Equals(other.Center) && this.Radius.Equals(other.Radius);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", this.Center.X, this.Center.Y, this.Radius);
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
            builder.Append(this.Radius.ToString(format, formatProvider));
            builder.Append(")");
            return builder.ToString();
        }
    }
}
