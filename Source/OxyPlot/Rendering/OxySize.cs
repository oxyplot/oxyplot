// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySize.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Describes the size of an object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Describes the size of an object.
    /// </summary>
    public struct OxySize : IFormattable, IEquatable<OxySize>
    {
        /// <summary>
        /// Empty Size.
        /// </summary>
        public static readonly OxySize Empty = new OxySize(0, 0);

        /// <summary>
        /// The height
        /// </summary>
        private readonly double height;

        /// <summary>
        /// The width
        /// </summary>
        private readonly double width;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxySize" /> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public OxySize(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the height.
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
        /// Gets the width.
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", this.Width, this.Height);
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
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(this.Width.ToString(format, formatProvider));
            builder.Append(","); // or get from culture?
            builder.Append(" ");
            builder.Append(this.Height.ToString(format, formatProvider));
            builder.Append(")");
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:OxySize" /> object have the same value.
        /// </summary>
        /// <param name="other">The size to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(OxySize other)
        {
            return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        /// <summary>
        /// Creates a new <see cref="OxySize"/> with the maximum dimensions of this instance and the specified other instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>A new <see cref="OxySize"/>.</returns>
        public OxySize Include(OxySize other)
        {
            return new OxySize(Math.Max(this.Width, other.Width), Math.Max(this.Height, other.Height));
        }
    }
}
