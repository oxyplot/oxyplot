// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a <see cref="HighLowSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item in a <see cref="HighLowSeries" />.
    /// </summary>
    public class HighLowItem
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly HighLowItem Undefined = new HighLowItem(double.NaN, double.NaN, double.NaN);

        /// <summary>
        /// The close.
        /// </summary>
        private double close;

        /// <summary>
        /// The high.
        /// </summary>
        private double high;

        /// <summary>
        /// The low.
        /// </summary>
        private double low;

        /// <summary>
        /// The open.
        /// </summary>
        private double open;

        /// <summary>
        /// The x.
        /// </summary>
        private double x;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowItem" /> class.
        /// </summary>
        public HighLowItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowItem" /> class.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="high">The high value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="open">The open value.</param>
        /// <param name="close">The close value.</param>
        public HighLowItem(double x, double high, double low, double open = double.NaN, double close = double.NaN)
        {
            this.x = x;
            this.high = high;
            this.low = low;
            this.open = open;
            this.close = close;
        }

        /// <summary>
        /// Gets or sets the close value.
        /// </summary>
        /// <value>The close value.</value>
        public double Close
        {
            get
            {
                return this.close;
            }

            set
            {
                this.close = value;
            }
        }

        /// <summary>
        /// Gets or sets the high value.
        /// </summary>
        /// <value>The high value.</value>
        public double High
        {
            get
            {
                return this.high;
            }

            set
            {
                this.high = value;
            }
        }

        /// <summary>
        /// Gets or sets the low value.
        /// </summary>
        /// <value>The low value.</value>
        public double Low
        {
            get
            {
                return this.low;
            }

            set
            {
                this.low = value;
            }
        }

        /// <summary>
        /// Gets or sets the open value.
        /// </summary>
        /// <value>The open value.</value>
        public double Open
        {
            get
            {
                return this.open;
            }

            set
            {
                this.open = value;
            }
        }

        /// <summary>
        /// Gets or sets the X value (time).
        /// </summary>
        /// <value>The X value.</value>
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }
    }
}