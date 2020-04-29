// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterPoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a point in a <see cref="ScatterSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents a point in a <see cref="ScatterSeries" />.
    /// </summary>
    public class ScatterPoint : ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterPoint" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="size">The size.</param>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag.</param>
        public ScatterPoint(double x, double y, double size = double.NaN, double value = double.NaN, object tag = null)
        {
            this.X = x;
            this.Y = y;
            this.Size = size;
            this.Value = value;
            this.Tag = tag;
        }

        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <value>The X.</value>
        public double X { get; private set; }

        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        public double Y { get; private set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public double Size { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public virtual string ToCode()
        {
            if (double.IsNaN(this.Size) && double.IsNaN(this.Value))
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}", this.X, this.Y);
            }

            if (double.IsNaN(this.Value))
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0}, {1}, {2}", this.X, this.Y, this.Size);
            }

            return CodeGenerator.FormatConstructor(
                this.GetType(), "{0}, {1}, {2}, {3}", this.X, this.Y, this.Size, this.Value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.X + " " + this.Y;
        }
    }
}