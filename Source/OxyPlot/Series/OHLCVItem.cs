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
    /// Represents an item in a <see cref="OHLCVSeries" />.
    /// </summary>
    public class OHLCVItem
    {
        /// <summary>
        /// The undefined.
        /// </summary>
		public static readonly OHLCVItem Undefined = new OHLCVItem(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        /// <summary>
		/// Initializes a new instance of the <see cref="OHLCVItem" /> class.
        /// </summary>
		public OHLCVItem()
        	{ }


        
		public OHLCVItem(
			double x, 
			double open, double high, double low, double close = double.NaN,
			double buyvolume = 0, double sellvolume = 0)
        {
			this.X = x;
			this.Open = open;
			this.High = high;
			this.Low = low;
			this.Close = close;
			this.BuyVolume = buyvolume;
			this.SellVolume = sellvolume;
        }


		// Properties

		/// <summary>
		/// Gets or sets the X value (time).
		/// </summary>
		/// <value>The X value.</value>
		public double X { get; set; }

		/// <summary>
		/// Gets or sets the open value.
		/// </summary>
		/// <value>The open value.</value>
		public double Open { get; set; }

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
        /// Gets or sets the close value.
        /// </summary>
        /// <value>The close value.</value>
        public double Close { get; set; }

		/// <summary>
		/// Gets or sets the buy volume.
		/// </summary>
		public double BuyVolume { get; set; }

		/// <summary>
		/// Gets or sets the sell volume.
		/// </summary>
		public double SellVolume { get; set; }


		// Functions

		/// <summary>
		/// Indicate whether is valid for rendering or not
		/// </summary>
		/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
		public bool IsValid ()
		{
			return !double.IsNaN (X) && !double.IsNaN (Open) && !double.IsNaN (High) && !double.IsNaN (Low) && !double.IsNaN (Close);
		}
    }
}