// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a HistogramSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item in a <see cref="HistogramSeries" />, a bin (range) and its area.
    /// </summary>
    public class HistogramItem : ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramItem" /> class.
        /// </summary>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <param name="area">The area.</param>
        /// <param name="count">The count.</param>
        public HistogramItem(double rangeStart, double rangeEnd, double area, int count)
            : this(rangeStart, rangeEnd, area, count, OxyColors.Automatic)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramItem" /> class.
        /// </summary>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <param name="area">The area.</param>
        /// <param name="count">The count.</param>
        /// <param name="color">The color.</param>
        public HistogramItem(double rangeStart, double rangeEnd, double area, int count, OxyColor color)
        {
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.Area = area;
            this.Count = count;
            this.Color = color;
        }

        /// <summary>
        /// Gets or sets the range start.
        /// </summary>
        /// <value>The range start.</value>
        public double RangeStart { get; set; }

        /// <summary>
        /// Gets or sets the range end.
        /// </summary>
        /// <value>The range end.</value>
        public double RangeEnd { get; set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public double Area { get; set; }

        /// <summary>
        /// Gets the center of the item.
        /// </summary>
        public double RangeCenter => this.RangeStart + ((this.RangeEnd - this.RangeStart) / 2);

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        /// <remarks>If set to Automatic, the FillColor of the RectangleBarSeries will be used.</remarks>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the computed width of the item.
        /// </summary>
        public double Width => this.RangeEnd - this.RangeStart;

        /// <summary>
        /// Gets the computed height of the item.
        /// </summary>
        /// <value>The computed height of the item.</value>
        public double Height => this.Area / this.Width;

        /// <summary>
        /// Gets the value of the item. Equivalent to the Height.
        /// </summary>
        /// <value>The value of the item.</value>
        public double Value => this.Height;

        /// <summary>
        /// Determines whether the specified point lies within the boundary of the <see cref="HistogramItem" />.
        /// </summary>
        /// <param name="p">The DataPoint to determine whether or not lies within the boundary of the <see cref="HistogramItem" />.</param>
        /// <returns><c>true</c> if the value of the p parameter is inside the bounds of this instance.</returns>
        public bool Contains(DataPoint p)
        {
            // height is taken as one Y value, the other is 0
            if (this.Height < 0)
            {
                return (p.X <= this.RangeEnd && p.X >= this.RangeStart && p.Y >= this.Height && p.Y <= 0) ||
                       (p.X <= this.RangeStart && p.X >= this.RangeEnd && p.Y >= this.Height && p.Y <= 0);
            }
            else
            {
                return (p.X <= this.RangeEnd && p.X >= this.RangeStart && p.Y <= this.Height && p.Y >= 0) ||
                       (p.X <= this.RangeStart && p.X >= this.RangeEnd && p.Y <= this.Height && p.Y >= 0);
            }
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            if (!this.Color.IsAutomatic())
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3},{4}", this.RangeStart, this.RangeEnd, this.Area, this.Count, this.Color);
            }
            else
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2},{3}", this.RangeStart, this.RangeEnd, this.Area, this.Count);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2} {3}",
                this.RangeStart,
                this.RangeEnd,
                this.Area,
                this.Count);
        }
    }
}
