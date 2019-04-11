﻿// --------------------------------------------------------------------------------------------------------------------
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
        public HistogramItem(double rangeStart, double rangeEnd, double area)
        {
            this.RangeStart = rangeStart;
            this.RangeEnd = rangeEnd;
            this.Area = area;
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
        /// Gets the computed width of the item.
        /// </summary>
        public double Width => this.RangeEnd - this.RangeStart;
        
        /// <summary>
        /// Gets the computed height of the item.
        /// </summary>
        /// <value>The computed height of the item</value>
        public double Height => this.Area / this.Width;
        
        /// <summary>
        /// Gets the value of the item. Equivalent to the Height;
        /// </summary>
        /// <value>The value of the item.</value>
        public double Value => this.Height;

        /// <summary>
        /// Determines whether the specified point lies within the boundary of the <see cref="HistogramItem" />.
        /// </summary>
        /// <param name="p">The DataPoint to determine whether or not lies within the boundary of the <see cref="HistogramItem" />.</param>
        /// <returns><c>true</c> if the value of the <param name="p"/> parameter is inside the bounds of this instance.</returns>
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
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", this.RangeStart, this.RangeEnd, this.Area);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2}",
                this.RangeStart,
                this.RangeEnd,
                this.Area);
        }
    }
}
