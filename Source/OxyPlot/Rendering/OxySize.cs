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
    public struct OxySize : IFormattable
    {
        /// <summary>
        /// Empty Size.
        /// </summary>
        public static OxySize Empty = new OxySize(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="OxySize" /> struct.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public OxySize(double width, double height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; set; }

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
    }
}