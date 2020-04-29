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
    public class HighLowItem : ICodeGenerating
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly HighLowItem Undefined = new HighLowItem(double.NaN, double.NaN, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowItem" /> class.
        /// </summary>
        public HighLowItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighLowItem"/> class.
        /// </summary>
        /// <param name="x">
        /// The x value.
        /// </param>
        /// <param name="high">
        /// The high value.
        /// </param>
        /// <param name="low">
        /// The low value.
        /// </param>
        /// <param name="open">
        /// The open value.
        /// </param>
        /// <param name="close">
        /// The close value.
        /// </param>
        public HighLowItem(double x, double high, double low, double open = double.NaN, double close = double.NaN)
        {
            this.X = x;
            this.High = high;
            this.Low = low;
            this.Open = open;
            this.Close = close;
        }

        /// <summary>
        /// Gets or sets the close value.
        /// </summary>
        /// <value>The close value.</value>
        public double Close { get; set; }

        /// <summary>
        /// Gets or sets the high value.
        /// </summary>
        /// <value>The high value.</value>
        public double High { get; set; }

        /// <summary>
        /// Gets or sets the low value.
        /// </summary>
        /// <value>The low value.</value>
        public double Low { get; set; }

        /// <summary>
        /// Gets or sets the open value.
        /// </summary>
        /// <value>The open value.</value>
        public double Open { get; set; }

        /// <summary>
        /// Gets or sets the X value (time).
        /// </summary>
        /// <value>The X value.</value>
        public double X { get; set; }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The C# code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(
                this.GetType(), "{0},{1},{2},{3},{4}", this.X, this.High, this.Low, this.Open, this.Close);
        }
    }
}