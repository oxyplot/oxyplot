// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a <see cref="BoxPlotSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an item in a <see cref="BoxPlotSeries" />.
    /// </summary>
    public struct BoxPlotItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotItem" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="lowerWhisker">The lower whisker.</param>
        /// <param name="boxBottom">The box bottom.</param>
        /// <param name="median">The median.</param>
        /// <param name="boxTop">The box top.</param>
        /// <param name="upperWhisker">The upper whisker.</param>
        /// <param name="outliers">The outliers.</param>
        /// <param name="tag">The tag.</param>
        public BoxPlotItem(
            double x,
            double lowerWhisker,
            double boxBottom,
            double median,
            double boxTop,
            double upperWhisker,
            IList<double> outliers,
            object tag = null)
            : this()
        {
            this.X = x;
            this.LowerWhisker = lowerWhisker;
            this.BoxBottom = boxBottom;
            this.Median = median;
            this.BoxTop = boxTop;
            this.UpperWhisker = upperWhisker;
            this.Outliers = outliers;
            this.Tag = tag;
        }

        /// <summary>
        /// Gets or sets the box bottom value (usually the 25th percentile, Q1).
        /// </summary>
        /// <value>The lower quartile value.</value>
        public double BoxBottom { get; set; }

        /// <summary>
        /// Gets or sets the box top value (usually the 75th percentile, Q3)).
        /// </summary>
        /// <value>The box top value.</value>
        public double BoxTop { get; set; }

        /// <summary>
        /// Gets or sets the lower whisker value.
        /// </summary>
        /// <value>The lower whisker value.</value>
        public double LowerWhisker { get; set; }

        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>The median.</value>
        public double Median { get; set; }

        /// <summary>
        /// Gets or sets the outliers.
        /// </summary>
        /// <value>The outliers.</value>
        public IList<double> Outliers { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the upper whisker value.
        /// </summary>
        /// <value>The upper whisker value.</value>
        public double UpperWhisker { get; set; }

        /// <summary>
        /// Gets a list of all the values in the item.
        /// </summary>
        public IList<double> Values
        {
            get
            {
                var values = new List<double> { this.LowerWhisker, this.BoxBottom, this.Median, this.BoxTop, this.UpperWhisker };
                values.AddRange(this.Outliers);
                return values;
            }
        }

        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        /// <value>The X value.</value>
        public double X { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2} {3} {4} {5} ",
                this.X,
                this.LowerWhisker,
                this.BoxBottom,
                this.Median,
                this.BoxTop,
                this.UpperWhisker);
        }
    }
}