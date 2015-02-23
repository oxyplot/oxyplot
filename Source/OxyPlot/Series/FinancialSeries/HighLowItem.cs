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
    using System.Collections.Generic;

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
        /// Find index of max(x) &lt;= target x in a list of OHLC items
        /// </summary>
        /// <param name='items'>
        /// vector of bars
        /// </param>
        /// <param name='targetX'>
        /// target x.
        /// </param>
        /// <param name='guess'>
        /// initial guess index.
        /// </param>
        /// <returns>
        /// index of x with max(x) &lt;= target x or -1 if cannot find
        /// </returns>
        public static int FindIndex(List<HighLowItem> items, double targetX, int guess)
        {
            int lastguess = 0;
            int start = 0;
            int end = items.Count - 1;

            while (start <= end)
            {
                if (guess < start)
                {
                    return lastguess;
                }
                else if (guess > end)
                {
                    return end;
                }

                var guessX = items[guess].X;
                if (guessX.Equals(targetX))
                {
                    return guess;
                }
                else if (guessX > targetX)
                {
                    end = guess - 1;
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
                    start = guess + 1; 
                    lastguess = guess; 
                }

                if (start >= end)
                {
                    return lastguess;
                }

                var endX = items[end].X;
                var startX = items[start].X;

                var m = (end - start + 1) / (endX - startX);
                guess = start + (int)((targetX - startX) * m);
            }

            return lastguess;
        }
    }
}