// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for candlestick charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents a "higher performance" ordered OHLC series for candlestick charts
    /// <para>
    /// Does the following:
    /// - automatically calculates the appropriate bar width based on available screen + # of bars
    /// - can render and pan within millions of bars, using a fast approach to indexing in series
    /// - convenience methods
    /// </para>
    /// This implementation is associated with <a href="https://github.com/oxyplot/oxyplot/issues/369">issue 369</a>.
    /// </summary>
    /// <remarks>See also <a href="http://en.wikipedia.org/wiki/Candlestick_chart">Wikipedia</a> and
    /// <a href="http://www.mathworks.com/help/toolbox/finance/candle.html">Matlab documentation</a>.</remarks>
    public class CandleStickSeries : HighLowSeries
    {
        /// <summary>
        /// The minimum X gap between successive data items
        /// </summary>
        private double minDx;

        /// <summary>
        /// Initializes a new instance of the <see cref = "CandleStickSeries" /> class.
        /// </summary>
        public CandleStickSeries()
        {
            this.IncreasingColor = OxyColors.DarkGreen;
            this.DecreasingColor = OxyColors.Red;
            this.CandleWidth = 0;
        }

        /// <summary>
        /// Gets or sets the color used when the closing value is greater than opening value.
        /// </summary>
        public OxyColor IncreasingColor { get; set; }

        /// <summary>
        /// Gets or sets the fill color used when the closing value is less than opening value.
        /// </summary>
        public OxyColor DecreasingColor { get; set; }

        /// <summary>
        /// Gets or sets the bar width in data units (for example if the X axis is date/time based, then should
        /// use the difference of DateTimeAxis.ToDouble(date) to indicate the width).  By default candlestick
        /// series will use 0.80 x the minimum difference in data points.
        /// </summary>
        public double CandleWidth { get; set; }

        /// <summary>
        /// Fast index of bar where max(bar[i].X) &lt;= x 
        /// </summary>
        /// <returns>The index of the bar closest to X, where max(bar[i].X) &lt;= x.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="startIndex">starting index</param> 
        public int FindByX(double x, int startIndex = -1)
        {
            if (startIndex < 0)
            {
                startIndex = this.WindowStartIndex;
            }

            return this.FindWindowStartIndex(this.Items, item => item.X, x, startIndex);
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var nitems = this.Items.Count;
            var items = this.Items;

            if (nitems == 0 || this.StrokeThickness <= 0 || this.LineStyle == LineStyle.None)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            var dashArray = this.LineStyle.GetDashArray();

            var dataCandlewidth = (this.CandleWidth > 0) ? this.CandleWidth : this.minDx * 0.80;
            var halfDataCandlewidth = .5 * dataCandlewidth;
            
            // colors
            var fillUp = this.GetSelectableFillColor(this.IncreasingColor);
            var fillDown = this.GetSelectableFillColor(this.DecreasingColor);
            var lineUp = this.GetSelectableColor(this.IncreasingColor.ChangeIntensity(0.70));
            var lineDown = this.GetSelectableColor(this.DecreasingColor.ChangeIntensity(0.70));

            // determine render range
            var xmin = this.XAxis.ActualMinimum;
            var xmax = this.XAxis.ActualMaximum;
            this.WindowStartIndex = this.UpdateWindowStartIndex(items, item => item.X, xmin, this.WindowStartIndex);

            for (int i = this.WindowStartIndex; i < nitems; i++)
            {
                var bar = items[i];

                // if item beyond visible range, done
                if (bar.X > xmax)
                {
                    return;
                }

                // check to see whether is valid
                if (!this.IsValidItem(bar, this.XAxis, this.YAxis))
                {
                    continue;
                }

                var fillColor = bar.Close > bar.Open ? fillUp : fillDown;
                var lineColor = bar.Close > bar.Open ? lineUp : lineDown;

                var high = this.Transform(bar.X, bar.High);
                var low = this.Transform(bar.X, bar.Low);
                var max = this.Transform(bar.X, Math.Max(bar.Open, bar.Close));
                var min = this.Transform(bar.X, Math.Min(bar.Open, bar.Close));

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    // Upper extent
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { high, max },
                        0,
                        lineColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode,
                        dashArray,
                        this.LineJoin);

                    // Lower extent
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { min, low },
                        0,
                        lineColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode,
                        dashArray,
                        this.LineJoin);
                }

                var p1 = this.Transform(bar.X - halfDataCandlewidth, bar.Open);
                var p2 = this.Transform(bar.X + halfDataCandlewidth, bar.Close);
                var rect = new OxyRect(p1, p2);
                rc.DrawClippedRectangle(clippingRect, rect, fillColor, lineColor, this.StrokeThickness, this.EdgeRenderingMode);
            }
        }

        /// <summary>
        /// Renders the legend symbol for the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double yopen = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.7);
            double yclose = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.3);
            double[] dashArray = this.LineStyle.GetDashArray();

            var candlewidth = legendBox.Width * 0.75;

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                    this.GetSelectableColor(this.ActualColor),
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);
            }

            rc.DrawRectangle(
                new OxyRect(xmid - (candlewidth * 0.5), yclose, candlewidth, yopen - yclose),
                this.GetSelectableFillColor(this.IncreasingColor),
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                this.EdgeRenderingMode);
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null || interpolate || this.Items.Count == 0)
            {
                return null;
            }

            var nbars = this.Items.Count;
            var xy = this.InverseTransform(point);
            var targetX = xy.X;

            // punt if beyond start & end of series
            if (targetX > (this.Items[nbars - 1].X + this.minDx) || targetX < (this.Items[0].X - this.minDx))
            {
                return null;
            }

            var pidx = this.FindWindowStartIndex(this.Items, item => item.X, targetX, this.WindowStartIndex);
            var nidx = ((pidx + 1) < this.Items.Count) ? pidx + 1 : pidx;

            Func<HighLowItem, double> distance = bar =>
            {
                var dx = bar.X - xy.X;
                var dyo = bar.Open - xy.Y;
                var dyh = bar.High - xy.Y;
                var dyl = bar.Low - xy.Y;
                var dyc = bar.Close - xy.Y;

                var d2O = (dx * dx) + (dyo * dyo);
                var d2H = (dx * dx) + (dyh * dyh);
                var d2L = (dx * dx) + (dyl * dyl);
                var d2C = (dx * dx) + (dyc * dyc);

                return Math.Min(d2O, Math.Min(d2H, Math.Min(d2L, d2C)));
            };

            // determine closest point
            var midx = distance(this.Items[pidx]) <= distance(this.Items[nidx]) ? pidx : nidx; 
            var mbar = this.Items[midx];

            var hit = new DataPoint(mbar.X, mbar.Close);
            return new TrackerHitResult 
            {
                Series = this,
                DataPoint = hit,
                Position = this.Transform(hit),
                Item = mbar,
                Index = midx,
                Text = StringHelper.Format(
                    this.ActualCulture,
                    this.TrackerFormatString,
                    mbar,
                    this.Title,
                    this.XAxis.Title ?? DefaultXAxisTitle,
                    this.XAxis.GetValue(mbar.X),
                    this.YAxis.GetValue(mbar.High),
                    this.YAxis.GetValue(mbar.Low),
                    this.YAxis.GetValue(mbar.Open),
                    this.YAxis.GetValue(mbar.Close))
            };
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();
            
            // determine minimum X gap between successive points
            var items = this.Items;
            var nitems = items.Count;
            this.minDx = double.MaxValue;

            for (int i = 1; i < nitems; i++)
            {
                this.minDx = Math.Min(this.minDx, items[i].X - items[i - 1].X);
                if (this.minDx < 0)
                {
                    throw new ArgumentException("bars are out of order, must be sequential in x");
                }
            }

            if (nitems <= 1)
            {
                this.minDx = 1;
            }
        }
    }
}
