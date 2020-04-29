// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OHLCVItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item in a <see cref="CandleStickAndVolumeSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an item in a <see cref="CandleStickAndVolumeSeries" />.
    /// </summary>
    public class OhlcvItem
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly OhlcvItem Undefined = new OhlcvItem(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="OhlcvItem" /> class.
        /// </summary>
        public OhlcvItem()
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.Series.OhlcvItem"/> class.
        /// </summary>
        /// <param name="x">The x coordinate / time.</param>
        /// <param name="open">Open value.</param>
        /// <param name="high">High value.</param>
        /// <param name="low">Low value.</param>
        /// <param name="close">Close value.</param>
        /// <param name="buyvolume">Buy volume.</param>
        /// <param name="sellvolume">Sell volume.</param>
        public OhlcvItem(
            double x, 
            double open, 
            double high, 
            double low, 
            double close,
            double buyvolume = 0, 
            double sellvolume = 0)
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
        /// Find index of max(x) &lt;= target x in a list of OHLCV items
        /// </summary>
        /// <param name='items'>
        /// vector of bars
        /// </param>
        /// <param name='targetX'>
        /// target x.
        /// </param>
        /// <param name='guessIdx'>
        /// initial guess.
        /// </param>
        /// <returns>
        /// index of x with max(x) &lt;= target x or -1 if cannot find
        /// </returns>
        public static int FindIndex(List<OhlcvItem> items, double targetX, int guessIdx)
        {
            int lastguess = 0;
            int start = 0;
            int end = items.Count - 1;

            while (start <= end)
            {
                if (guessIdx < start)
                {
                    return lastguess;
                }
                else if (guessIdx > end)
                {
                    return end;
                }

                var guessX = items[guessIdx].X;
                if (guessX.Equals(targetX))
                {
                    return guessIdx;
                }
                else if (guessX > targetX)
                {
                    end = guessIdx - 1;
                    if (end < start)
                    {
                        return lastguess;
                    }
                    else if (end == start)
                    {
                        return end;
                    }
                }
                else
                { 
                    start = guessIdx + 1; 
                    lastguess = guessIdx; 
                }

                if (start >= end)
                {
                    return lastguess;
                }

                var endX = items[end].X;
                var startX = items[start].X;

                var m = (end - start + 1) / (endX - startX);
                guessIdx = start + (int)((targetX - startX) * m);
            }

            return lastguess;
        }

        /// <summary>
        /// Indicate whether is valid for rendering or not
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            return !double.IsNaN(this.X) && !double.IsNaN(this.Open) && !double.IsNaN(this.High) && !double.IsNaN(this.Low) && !double.IsNaN(this.Close);
        }
    }
}