// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickAndVolumeSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a dual view (candlestick + volume) series for OHLCV bars
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot.Axes;

    /// <summary>
    /// Represents a dual view (candlestick + volume) series for OHLCV bars
    /// <para/>
    /// Note that to use this series, one *must* define two y-axes, one named "Bars" and the other named
    /// "Volume".  Typically would set up the volume on StartPosition =0, EndPosition = fraction and for
    /// the bar axis StartPosition = fraction + delta, EndPosition = 1.0.
    /// </summary>
    /// <remarks>See <a href="http://www.mathworks.com/help/toolbox/finance/highlowfts.html">link</a></remarks>
    public class CandleStickAndVolumeSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = 
            "Time: {0}\nHigh: {1}\nLow: {2}\nOpen: {3}\nClose: {4}\nBuy Volume: {5}\nSell Volume: {6}";

        /// <summary>
        /// The data series
        /// </summary>
        private List<OhlcvItem> data;

        /// <summary>
        /// The minimum X gap between successive data items
        /// </summary>
        private double minDx;

        /// <summary>
        /// The index of the data item at the start of visible window
        /// </summary>
        private int winIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref = "CandleStickAndVolumeSeries" /> class.
        /// </summary>
        public CandleStickAndVolumeSeries()
        {
            this.YAxisKey = "Bars";
            this.PositiveColor = OxyColors.DarkGreen;
            this.NegativeColor = OxyColors.Red;
            this.SeparatorColor = OxyColors.Black;
            this.CandleWidth = 0;
            this.SeparatorStrokeThickness = 1;
            this.SeparatorLineStyle = LineStyle.Dash;
            this.StrokeThickness = 1;
            this.NegativeHollow = false;
            this.PositiveHollow = true;
            this.StrokeIntensity = 0.80;

            this.TrackerFormatString = DefaultTrackerFormatString;
        }

        /// <summary>
        /// Gets or sets the items of the series.
        /// </summary>
        /// <value>The items.</value>
        public List<OhlcvItem> Items
        { 
            get 
            { 
                return (this.data != null) ? this.data : (this.data = new List<OhlcvItem>()); 
            }

            set
            { 
                this.data = value; 
            }
        }

        /// <summary>
        /// Gets the portion of the Y axis associated with bars
        /// </summary>
        public LinearAxis BarAxis  
        { 
            get 
            { 
                return (LinearAxis)YAxis; 
            } 
        }

        /// <summary>
        /// Gets the portion of the Y axis associated with volume
        /// </summary>
        public LinearAxis VolumeAxis { get; private set; }

        /// <summary>
        /// Gets or sets the style of volume rendering
        /// </summary>
        public VolumeStyle VolumeStyle { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the bar lines
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke intensity scale (used to generate stroke color from positive or negative color).
        /// For example, 1.0 = same color and 0.5 is 1/2 of the intensity of the source fill color.
        /// </summary>
        public double StrokeIntensity { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the volume / bar separator
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double SeparatorStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the line style for the volume / bar separator
        /// </summary>
        public LineStyle SeparatorLineStyle { get; set; }

        /// <summary>
        /// Gets or sets the color used when the closing value is greater than opening value or
        /// for buying volume.
        /// </summary>
        public OxyColor PositiveColor { get; set; }

        /// <summary>
        /// Gets or sets the fill color used when the closing value is less than opening value or
        /// for selling volume
        /// </summary>
        public OxyColor NegativeColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the separator line
        /// </summary>
        public OxyColor SeparatorColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether positive bars are shown as filled (false) or hollow (true) candlesticks
        /// </summary>
        public bool PositiveHollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether negative bars are shown as filled (false) or hollow (true) candlesticks
        /// </summary>
        public bool NegativeHollow { get; set; }

        /// <summary>
        /// Gets or sets the bar width in data units (for example if the X axis is date-time based, then should
        /// use the difference of DateTimeAxis.ToDouble(date) to indicate the width).  By default candlestick
        /// series will use 0.80 x the minimum difference in data points.
        /// </summary>
        public double CandleWidth { get; set; }

        /// <summary>
        /// Append a bar to the series (must be in X order)
        /// </summary>
        /// <param name="bar">Bar object.</param>
        public void Append(OhlcvItem bar)
        {
            if (this.data == null)
            {
                this.data = new List<OhlcvItem>();
            }

            if (this.data.Count > 0 && this.data[this.data.Count - 1].X > bar.X)
            {
                throw new ArgumentException("cannot append bar out of order, must be sequential in X");
            }

            this.data.Add(bar);
        }

        /// <summary>
        /// Fast index of bar where max(bar[i].X) &lt;= x 
        /// </summary>
        /// <returns>The index of the bar closest to X, where max(bar[i].X) &lt;= x.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="startingIndex">starting index</param> 
        public int FindByX(double x, int startingIndex = -1)
        {
            if (startingIndex < 0)
            {
                startingIndex = this.winIndex;
            }

            return OhlcvItem.FindIndex(this.data, x, startingIndex);
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.data == null || this.data.Count == 0)
            {
                return;
            }

            var items = this.data;
            var nitems = this.data.Count;

            this.VerifyAxes();

            var clipping_bar = this.GetClippingRect(this.BarAxis);
            var clipping_sep = this.GetSeparationClippingRect();
            var clipping_vol = this.GetClippingRect(this.VolumeAxis);

            var datacandlewidth = (this.CandleWidth > 0) ? this.CandleWidth : this.minDx * 0.80;
            var candlewidth = 
                this.XAxis.Transform(items[0].X + datacandlewidth) -
                this.XAxis.Transform(items[0].X) - this.StrokeThickness; 

            // colors
            var fill_up = this.GetSelectableFillColor(this.PositiveColor);
            var fill_down = this.GetSelectableFillColor(this.NegativeColor);

            var barfill_up = this.PositiveHollow ? 
                OxyColors.Transparent : fill_up;
            var barfill_down = this.NegativeHollow ? 
                OxyColors.Transparent : fill_down;

            var line_up = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));
            var line_down = this.GetSelectableColor(this.NegativeColor.ChangeIntensity(this.StrokeIntensity));

            // determine render range
            var xmin = this.XAxis.ActualMinimum;
            var xmax = this.XAxis.ActualMaximum;
            this.winIndex = OhlcvItem.FindIndex(items, xmin, this.winIndex);

            for (int i = this.winIndex; i < nitems; i++)
            {
                var bar = items[i];

                // if item beyond visible range, done
                if (bar.X > xmax)
                {
                    break;
                }

                // check to see whether is valid
                if (!bar.IsValid())
                {
                    continue;
                }

                var fillColor = bar.Close > bar.Open ? barfill_up : barfill_down;
                var lineColor = bar.Close > bar.Open ? line_up : line_down;

                var high = this.Transform(bar.X, bar.High);
                var low = this.Transform(bar.X, bar.Low);

                var open = this.Transform(bar.X, bar.Open);
                var close = this.Transform(bar.X, bar.Close);

                var max = new ScreenPoint(open.X, Math.Max(open.Y, close.Y));
                var min = new ScreenPoint(open.X, Math.Min(open.Y, close.Y));

                // Bar part
                rc.DrawClippedLine(
                    clipping_bar,
                    new[] { high, min },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    null,
                    LineJoin.Miter,
                    true);

                // Lower extent
                rc.DrawClippedLine(
                    clipping_bar,
                    new[] { max, low },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    null,
                    LineJoin.Miter,
                    true);

                // Body
                var openLeft = open + new ScreenVector(-candlewidth * 0.5, 0);
                var rect = new OxyRect(openLeft.X, min.Y, candlewidth, max.Y - min.Y);
                rc.DrawClippedRectangleAsPolygon(
                    clipping_bar, 
                    rect, 
                    fillColor, 
                    lineColor, 
                    this.StrokeThickness);

                // Volume Part
                if (this.VolumeAxis == null || this.VolumeStyle == VolumeStyle.None)
                {
                    continue;
                }

                var iY0 = this.VolumeAxis.Transform(0); 
                switch (VolumeStyle)
                {
                    case VolumeStyle.Combined:
                        {
                            var adj = this.VolumeAxis.Transform(Math.Abs(bar.BuyVolume - bar.SellVolume));
                            var fillcolor = (bar.BuyVolume > bar.SellVolume) ? barfill_up : barfill_down;
                            var linecolor = (bar.BuyVolume > bar.SellVolume) ? line_up : line_down;
                            var rect1 = new OxyRect(openLeft.X, adj, candlewidth, Math.Abs(adj - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect1, fillcolor, linecolor, this.StrokeThickness);
                        }

                        break;

                    case VolumeStyle.PositiveNegative:
                        {
                            var buyY = this.VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = this.VolumeAxis.Transform(-bar.SellVolume);
                            var rect1 = new OxyRect(openLeft.X, buyY, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect1, fill_up, line_up, this.StrokeThickness);
                            var rect2 = new OxyRect(openLeft.X, iY0, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect2, fill_down, line_down, this.StrokeThickness);
                        }

                        break;

                    case VolumeStyle.Stacked:
                        if (bar.BuyVolume > bar.SellVolume)
                        {
                            var buyY = this.VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = this.VolumeAxis.Transform(bar.SellVolume);
                            var dyoffset = sellY - iY0;
                            var rect2 = new OxyRect(openLeft.X, sellY, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect2, fill_down, line_down, this.StrokeThickness);
                            var rect1 = new OxyRect(openLeft.X, buyY + dyoffset, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect1, fill_up, line_up, this.StrokeThickness);
                        }
                        else
                        {
                            var buyY = VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = VolumeAxis.Transform(bar.SellVolume);
                            var dyoffset = buyY - iY0;
                            var rect1 = new OxyRect(openLeft.X, buyY, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect1, fill_up, line_up, this.StrokeThickness);
                            var rect2 = new OxyRect(openLeft.X, sellY + dyoffset, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangleAsPolygon(clipping_vol, rect2, fill_down, line_down, this.StrokeThickness);
                        }

                        break;
                }
            }

            // draw volume & bar separation line
            if (VolumeStyle != VolumeStyle.None)
            {
                var ysep = (clipping_sep.Bottom + clipping_sep.Top) / 2.0;
                rc.DrawClippedLine(
                    clipping_sep,
                    new[] { new ScreenPoint(clipping_sep.Left, ysep), new ScreenPoint(clipping_sep.Right, ysep) },
                    0,
                    this.SeparatorColor,
                    this.SeparatorStrokeThickness,
                    this.SeparatorLineStyle.GetDashArray(),
                    LineJoin.Miter,
                    true);
            }

            // draw volume y=0 line
            if (VolumeStyle == VolumeStyle.PositiveNegative)
            {
                var y0 = this.VolumeAxis.Transform(0); 
                rc.DrawClippedLine(
                    clipping_vol,
                    new[] { new ScreenPoint(clipping_vol.Left, y0), new ScreenPoint(clipping_vol.Right, y0) },
                    0,
                    OxyColors.Goldenrod,
                    this.SeparatorStrokeThickness,
                    this.SeparatorLineStyle.GetDashArray(),
                    LineJoin.Miter,
                    true);
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
            double[] dashArray = LineStyle.Solid.GetDashArray();

            var datacandlewidth = (this.CandleWidth > 0) ? this.CandleWidth : this.minDx * 0.80;

            var fill_up = this.GetSelectableFillColor(this.PositiveColor);
            var line_up = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(0.70));

            var candlewidth = 
                this.XAxis.Transform(this.data[0].X + datacandlewidth) -
                this.XAxis.Transform(this.data[0].X); 

            rc.DrawLine(
                new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                line_up,
                this.StrokeThickness,
                dashArray,
                LineJoin.Miter,
                true);

            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (candlewidth * 0.5), yclose, candlewidth, yopen - yclose),
                fill_up,
                line_up,
                this.StrokeThickness);
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null || interpolate || this.data.Count == 0)
            {
                return null;
            }

            var nbars = this.data.Count;
            var xy = InverseTransform(point);
            var targetX = xy.X;

            // punt if beyond start & end of series
            if (targetX > (this.data[nbars - 1].X + this.minDx))
            {
                return null;
            }
            else if (targetX < (this.data[0].X - this.minDx))
            {
                return null;
            }

            var pidx = OhlcvItem.FindIndex(this.data, targetX, this.winIndex);
            var nidx = ((pidx + 1) < this.data.Count) ? pidx + 1 : pidx;

            Func<OhlcvItem, double> distance = (bar) =>
            {
                var dx = bar.X - xy.X;
                return dx * dx;
            };

            // determine closest point
            var midx = distance(this.data[pidx]) <= distance(this.data[nidx]) ? pidx : nidx; 
            var mbar = this.data[midx];

            var hit = new DataPoint(mbar.X, mbar.Close);
            return new TrackerHitResult 
            {
                Series = this,
                DataPoint = hit,
                Position = Transform(hit),
                Item = mbar,
                Index = midx,
                Text = StringHelper.Format(
                    this.ActualCulture,
                    this.TrackerFormatString,
                    mbar,
                    this.XAxis.GetValue(mbar.X),
                    this.YAxis.GetValue(mbar.High),
                    this.YAxis.GetValue(mbar.Low),
                    this.YAxis.GetValue(mbar.Open),
                    this.YAxis.GetValue(mbar.Close),
                    this.YAxis.GetValue(mbar.BuyVolume),
                    this.YAxis.GetValue(mbar.SellVolume))
            };
        }
 
        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();
            this.winIndex = 0;

            // determine minimum X gap between successive points
            var items = this.data;
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

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            base.EnsureAxes();
            this.VolumeAxis = (LinearAxis)this.PlotModel.Axes.FirstOrDefault(a => a.Key == "Volume");
        }

        /// <summary>
        /// Gets the clipping rectangle for the given combination of existing X-Axis and specific Y-Axis
        /// </summary>
        /// <returns>The clipping rectangle.</returns>
        /// <param name="yaxis">Y axis.</param>
        protected OxyRect GetClippingRect(Axis yaxis)
        {
            if (yaxis == null)
            {
                return default(OxyRect);
            }

            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double minY = Math.Min(yaxis.ScreenMin.Y, yaxis.ScreenMax.Y);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxY = Math.Max(yaxis.ScreenMin.Y, yaxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the clipping rectangle between plots
        /// </summary>
        /// <returns>The clipping rectangle.</returns>
        protected OxyRect GetSeparationClippingRect()
        {
            if (this.VolumeAxis == null)
            {
                return default(OxyRect);
            }

            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);

            double minY = 0.0;
            double maxY = 0.0;
            if (this.VolumeAxis.ScreenMax.Y < this.BarAxis.ScreenMin.Y)
            {
                maxY = this.BarAxis.ScreenMin.Y;
                minY = this.VolumeAxis.ScreenMax.Y;
            }
            else
            {
                maxY = this.VolumeAxis.ScreenMin.Y;
                minY = this.BarAxis.ScreenMax.Y;
            }

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }
   }
}