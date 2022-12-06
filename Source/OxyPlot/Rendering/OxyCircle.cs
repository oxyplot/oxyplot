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
    public struct OxyCircle : IShape, IEquatable<OxyCircle>, IFormattable
    {

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public ScreenPoint Center { get; set; }

        /// <summary>
        /// Gets the radius of the circle
        /// </summary>
        public double Radius { get; set; }

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
        /// <param name="r">The radius of the circle</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r;The radius should not be negative.</exception>
        public OxyCircle(ScreenPoint center, double r)
        {
            if (r < 0)
                throw new ArgumentOutOfRangeException("r", "The radius should not be negative.");

            this.Center = new ScreenPoint(center.X, center.Y);
            this.Radius = r;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyCircle"/> structure that has a specified center and a radius
        /// </summary>
        /// <param name="x">The x-coordinate location of the center of the circle</param>
        /// <param name="y">The x-coordinate location of the center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <exception cref="System.ArgumentOutOfRangeException">r;The radius should not be negative.</exception>
        public OxyCircle(double x, double y, double r) : this(new ScreenPoint(x, y), r)
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

        /// <summary>
        /// Returns a circle that is expanded or shrunk by the specified radius amounts.
        /// </summary>
        /// <param name="dr">The amount by which to expand or shrink the radius of the circle.</param>
        /// <returns>The expanded/shrunk <see cref="OxyCircle" />.</returns>
        public OxyCircle Inflate(double dr)
        {
            return new OxyCircle(this.Center, this.Radius + dr);
        }

        /// <summary>
        /// Returns a circle that is moved by the specified horizontal and vertical amounts.
        /// </summary>
        /// <param name="offsetX">The amount to move the circle horizontally.</param>
        /// <param name="offsetY">The amount to move the circle vertically.</param>
        /// <returns>The moved <see cref="OxyCircle" />.</returns>
        public OxyCircle Offset(double offsetX, double offsetY)
        {
            return new OxyCircle(this.Center.X + offsetX, this.Center.Y + offsetY, this.Radius);
        }

        /// <summary>
        /// Intersects this <see cref="OxyCircle"/> with another <see cref="OxyCircle"/> with an equal center point.
        /// </summary>
        /// <param name="circle">The other <see cref="OxyCircle"/>.</param>
        /// <returns>The intersection between this <see cref="OxyCircle"/> and the other <see cref="OxyCircle"/>.</returns>
        /// <exception cref="System.ArgumentException">circle;The center should be equal to this center.</exception>
        public OxyAnnulus Intersect(OxyCircle circle)
        {
            if (this.Center.DistanceTo(circle.Center) > 0)
                throw new ArgumentOutOfRangeException("circle", "The center should be equal to this center.");

            var innerRadius = Math.Min(this.Radius, circle.Radius);
            var outerRadius = Math.Max(this.Radius, circle.Radius);

            return new OxyAnnulus(this.Center, innerRadius, outerRadius);
        }

    }
}
