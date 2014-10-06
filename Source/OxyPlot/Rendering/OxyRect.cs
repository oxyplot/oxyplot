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
    public struct OxyRect : IFormattable
    {
        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        private double height;

        /// <summary>
        /// The x-coordinate location of the left side of the rectangle.
        /// </summary>
        private double left;

        /// <summary>
        /// The y-coordinate location of the top side of the rectangle.
        /// </summary>
        private double top;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        private double width;

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
        /// Gets or sets the y-axis value of the bottom of the rectangle.
        /// </summary>
        /// <value>The bottom.</value>
        public double Bottom
        {
            get
            {
                return this.top + this.height;
            }

            set
            {
                this.height = value - this.top;
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        /// <value>The height.</value>
        public double Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-axis value of the left side of the rectangle.
        /// </summary>
        /// <value>The left.</value>
        public double Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-axis value of the right side of the rectangle.
        /// </summary>
        /// <value>The right.</value>
        public double Right
        {
            get
            {
                return this.left + this.width;
            }

            set
            {
                this.width = value - this.left;
            }
        }

        /// <summary>
        /// Gets or sets the y-axis position of the top of the rectangle.
        /// </summary>
        /// <value>The top.</value>
        public double Top
        {
            get
            {
                return this.top;
            }

            set
            {
                this.top = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        /// <value>The width.</value>
        public double Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
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
    }
}