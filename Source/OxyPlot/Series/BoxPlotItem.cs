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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using static OxyPlot.EnumerableExtensions;

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
        [Obsolete("Will be removed in a future version. Please use the other constructor in combination with an Object Initzializer.")]
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
            this.Mean = double.NaN;
            this.Outliers = outliers ?? new List<double>();
            this.Tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotItem" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="lowerWhisker">The lower whisker.</param>
        /// <param name="boxBottom">The box bottom.</param>
        /// <param name="median">The median.</param>
        /// <param name="boxTop">The box top.</param>
        /// <param name="upperWhisker">The upper whisker.</param>
        public BoxPlotItem(
            double x,
            double lowerWhisker,
            double boxBottom,
            double median,
            double boxTop,
            double upperWhisker)
            : this()
        {
            this.X = x;
            this.LowerWhisker = lowerWhisker;
            this.BoxBottom = boxBottom;
            this.Median = median;
            this.BoxTop = boxTop;
            this.UpperWhisker = upperWhisker;
            this.Mean = double.NaN;
            this.Outliers = new List<double>();
            this.Tag = null;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotItem" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="dataPoints">Data series with the original values</param>
        /// <param name="whiskerType">Type of whisker values.</param>
        public BoxPlotItem(double x,
                           IList<double> dataPoints,
                            OxyPlot.Series.BoxPlotWhiskerType whiskerType = OxyPlot.Series.BoxPlotWhiskerType.IQR)
        {
            double[] dataPointsArr = dataPoints.ToArray();
            Array.Sort(dataPointsArr);
            this.Datapoints = dataPointsArr;
            double mean = this.Datapoints.Mean();
            double median = this.Datapoints.Median();
            double UpperWhisker = median;
            double LowerWhisker = median;
            double Q1 = this.Datapoints.FirstQuartile();
            double Q3 = this.Datapoints.ThirdQuartile();

            this.Outliers = new List<double>();
            this.Tag = null;

            switch (whiskerType)
            {
                case OxyPlot.Series.BoxPlotWhiskerType.IQR:
                    double IQR = Q3 - Q1;
                    UpperWhisker = (Q3 + 1.5 * IQR);
                    LowerWhisker = (Q1 - 1.5 * IQR);
                    break;
                case OxyPlot.Series.BoxPlotWhiskerType.MinMax:
                    double min = (from bpi in this.Datapoints
                                  select bpi).Min();
                    double max = (from bpi in this.Datapoints
                                  select bpi).Max();
                    UpperWhisker = max;
                    LowerWhisker = min;
                    break;
                case OxyPlot.Series.BoxPlotWhiskerType.NinthPercentile:
                    double NinthPercentile = this.Datapoints.PercentilePos(9);
                    double NinetyFirstPercentile = this.Datapoints.PercentilePos(91);
                    UpperWhisker = NinetyFirstPercentile;
                    LowerWhisker = NinthPercentile;
                    break;
                case OxyPlot.Series.BoxPlotWhiskerType.SecondPercentile:
                    double SecondPercentile = this.Datapoints.PercentilePos(2);
                    double NinetyEighthPercentile = this.Datapoints.PercentilePos(98);
                    UpperWhisker = NinetyEighthPercentile;
                    LowerWhisker = SecondPercentile;
                    break;
                case OxyPlot.Series.BoxPlotWhiskerType.StDev:
                    double StDev = this.Datapoints.StandardDeviation();
                    UpperWhisker = mean + StDev;
                    LowerWhisker = mean - StDev;
                    break;
            }

            foreach (double d in this.Datapoints)
            {
                if (d < LowerWhisker || d > UpperWhisker)
                {
                    this.Outliers.Add(d);
                }
            }
            this.X = x;
            this.LowerWhisker = LowerWhisker;
            this.BoxBottom = Q1;
            this.Median = median;
            this.BoxTop = Q3;
            this.UpperWhisker = UpperWhisker;
            this.Mean = double.NaN;
        }

        /// <summary>
        /// Gets or sets the box bottom value (usually the 25th percentile, Q1).
        /// </summary>
        /// <value>The lower quartile value.</value>
        public double BoxBottom
        { get; set; }

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
        /// Gets or sets the mean.
        /// </summary>
        /// <value>The mean.</value>
        public double Mean { get; set; }

        /// <summary>
        /// Gets or sets the outliers.
        /// </summary>
        /// <value>The outliers.</value>
        public IList<double> Outliers { get; set; }

        /// <summary>
        /// Gets or sets the datapoints.
        /// </summary>
        /// <value>The datapoints.</value>
        public IList<double> Datapoints { get; set; }


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

                // As mean is an optional value and should not be checked for validation if not set don't add it if NaN
                if (!double.IsNaN(this.Mean))
                {
                    values.Add(this.Mean);
                }

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
                "{0} {1} {2} {3} {4} {5} {6} ",
                this.X,
                this.LowerWhisker,
                this.BoxBottom,
                this.Median,
                this.Mean,
                this.BoxTop,
                this.UpperWhisker);
        }
    }
}