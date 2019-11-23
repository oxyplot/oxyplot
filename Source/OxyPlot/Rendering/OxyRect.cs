// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyRect.cs" company="OxyPlot">
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
    /// Describes the width, height, and point origin of a rectangle.
    /// </summary>
    public struct OxyRect : IFormattable, IEquatable<OxyRect>
    {
        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        private readonly double height;

        /// <summary>
        /// The x-coordinate location of the left side of the rectangle.
        /// </summary>
        private readonly double left;

        /// <summary>
        /// The y-coordinate location of the top side of the rectangle.
        /// </summary>
        private readonly double top;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        private readonly double width;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyRect" /> structure that has the specified x-coordinate, y-coordinate, width, and height.
        /// </summary>
        /// <param name="left">The x-coordinate location of the left side of the rectangle.</param>
        /// <param name="top">The y-coordinate location of the top side of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">width;The width should not be negative.
        /// or
        /// height;The height should not be negative.</exception>
        public OxyRect(double left, double top, double width, double height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", "The width should not be negative.");
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", "The height should not be negative.");
            }

            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyRect" /> struct that is exactly large enough to contain the two specified points.
        /// </summary>
        /// <param name="p0">The first point that the new rectangle must contain.</param>
        /// <param name="p1">The second point that the new rectangle must contain.</param>
        public OxyRect(ScreenPoint p0, ScreenPoint p1)
            : this(Math.Min(p0.X, p1.X), Math.Min(p0.Y, p1.Y), Math.Abs(p1.X - p0.X), Math.Abs(p1.Y - p0.Y))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyRect"/> struct by location and size.
        /// </summary>
        /// <param name="p0">The location.</param>
        /// <param name="size">The size.</param>
        public OxyRect(ScreenPoint p0, OxySize size)
            : this(p0.X, p0.Y, size.Width, size.Height)
        {
        }

        /// <summary>
        /// Gets the y-axis value of the bottom of the rectangle.
        /// </summary>
        /// <value>The bottom.</value>
        public double Bottom
        {
            get
            {
                return this.top + this.height;
            }
        }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        /// <value>The height.</value>
        public double Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Gets the x-axis value of the left side of the rectangle.
        /// </summary>
        /// <value>The left.</value>
        public double Left
        {
            get
            {
                return this.left;
            }
        }

        /// <summary>
        /// Gets the x-axis value of the right side of the rectangle.
        /// </summary>
        /// <value>The right.</value>
        public double Right
        {
            get
            {
                return this.left + this.width;
            }
        }

        /// <summary>
        /// Gets the y-axis position of the top of the rectangle.
        /// </summary>
        /// <value>The top.</value>
        public double Top
        {
            get
            {
                return this.top;
            }
        }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        /// <value>The width.</value>
        public double Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Gets the center point of the rectangle.
        /// </summary>
        /// <value>The center.</value>
        public ScreenPoint Center
        {
            get
            {
                return new ScreenPoint(this.left + (this.width * 0.5), this.top + (this.height * 0.5));
            }
        }

        /// <summary>
        /// Gets the top left corner of the rectangle.
        /// </summary>
        /// <value>The top left corner.</value>
        public ScreenPoint TopLeft => new ScreenPoint(this.Left, this.Top);

        /// <summary>
        /// Gets the top right corner of the rectangle.
        /// </summary>
        /// <value>The top right corner.</value>
        public ScreenPoint TopRight => new ScreenPoint(this.Right, this.Top);

        /// <summary>
        /// Gets the bottom left corner of the rectangle.
        /// </summary>
        /// <value>The bottom left corner.</value>
        public ScreenPoint BottomLeft => new ScreenPoint(this.Left, this.Bottom);

        /// <summary>
        /// Gets the bottom right corner of the rectangle.
        /// </summary>
        /// <value>The bottom right corner.</value>
        public ScreenPoint BottomRight => new ScreenPoint(this.Right, this.Bottom);

        /// <summary>
        /// Creates a rectangle from the specified corner coordinates.
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <returns>A rectangle.</returns>
        public static OxyRect Create(double x0, double y0, double x1, double y1)
        {
            return new OxyRect(Math.Min(x0, x1), Math.Min(y0, y1), Math.Abs(x1 - x0), Math.Abs(y1 - y0));
        }

        /// <summary>
        /// Determines whether the specified point is inside the rectangle.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(double x, double y)
        {
            return x >= this.Left && x <= this.Right && y >= this.Top && y <= this.Bottom;
        }

        /// <summary>
        /// Determines whether the specified point is inside the rectangle.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns><c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.</returns>
        public bool Contains(ScreenPoint p)
        {
            return this.Contains(p.x, p.y);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", this.left, this.top, this.width, this.height);
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
            builder.Append(this.Left.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.Top.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.Width.ToString(format, formatProvider));
            builder.Append(Separator);
            builder.Append(this.Height.ToString(format, formatProvider));
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:OxyRect" /> object have the same value.
        /// </summary>
        /// <param name="other">The rectangle to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OxyRect other)
        {
            return this.Left.Equals(other.Left) && this.Top.Equals(other.Top) && this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        /// <summary>
        /// Returns a rectangle that is expanded or shrunk by the specified width and height amounts, in all directions.
        /// </summary>
        /// <param name="dx">The amount by which to expand or shrink the left and right sides of the rectangle.</param>
        /// <param name="dy">The amount by which to expand or shrink the top and bottom sides of the rectangle.</param>
        /// <returns>The expanded/shrunk <see cref="OxyRect" />.</returns>
        public OxyRect Inflate(double dx, double dy)
        {
            return new OxyRect(this.left - dx, this.top - dy, this.width + (dx * 2), this.height + (dy * 2));
        }

        /// <summary>
        /// Returns a rectangle that is expanded by the specified thickness, in all directions.
        /// </summary>
        /// <param name="t">The thickness to apply to the rectangle.</param>
        /// <returns>The inflated <see cref="OxyRect" />.</returns>
        public OxyRect Inflate(OxyThickness t)
        {
            return new OxyRect(this.left - t.Left, this.top - t.Top, this.width + t.Left + t.Right, this.height + t.Top + t.Bottom);
        }

        /// <summary>
        /// Returns a rectangle that is shrunk by the specified thickness, in all directions.
        /// </summary>
        /// <param name="t">The thickness to apply to the rectangle.</param>
        /// <returns>The deflated <see cref="OxyRect" />.</returns>
        public OxyRect Deflate(OxyThickness t)
        {
            return new OxyRect(this.left + t.Left, this.top + t.Top, Math.Max(0, this.width - t.Left - t.Right), Math.Max(0, this.height - t.Top - t.Bottom));
        }

        /// <summary>
        /// Returns a rectangle that is moved by the specified horizontal and vertical amounts.
        /// </summary>
        /// <param name="offsetX">The amount to move the rectangle horizontally.</param>
        /// <param name="offsetY">The amount to move the rectangle vertically.</param>
        /// <returns>The moved <see cref="OxyRect" />.</returns>
        public OxyRect Offset(double offsetX, double offsetY)
        {
            return new OxyRect(this.left + offsetX, this.top + offsetY, this.width, this.height);
        }

        /// <summary>
        /// Returns a rectangle that is clipped to the outer bounds of the specified rectangle.
        /// </summary>
        /// <param name="clipRect">The rectangle that defines the outermost allowed coordinates for the clipped rectangle.</param>
        /// <returns>The clipped rectangle.</returns>
        public OxyRect Clip(OxyRect clipRect)
        {
            var clipRight = double.IsNegativeInfinity(clipRect.Left) && double.IsPositiveInfinity(clipRect.Width)
                            ? double.PositiveInfinity
                            : clipRect.Right;            
            
            var clipBottom = double.IsNegativeInfinity(clipRect.Top) && double.IsPositiveInfinity(clipRect.Height)
                            ? double.PositiveInfinity
                            : clipRect.Bottom;

            return Create(
                Math.Max(Math.Min(this.Left, clipRight), clipRect.Left),
                Math.Max(Math.Min(this.Top, clipBottom), clipRect.Top),
                Math.Max(Math.Min(this.Right, clipRight), clipRect.Left),
                Math.Max(Math.Min(this.Bottom, clipBottom), clipRect.Top));
        }
    }
}
