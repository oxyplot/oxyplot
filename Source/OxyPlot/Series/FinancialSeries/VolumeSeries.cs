// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a volume view on OHLCV bars
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a dual view (candlestick + volume) series for OHLCV bars
    /// </summary>
    /// <remarks>See <a href="http://www.mathworks.com/help/toolbox/finance/highlowfts.html">link</a></remarks>
    public class VolumeSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = 
            "Time: {0}\nBuy Volume: {1}\nSell Volume: {2}";

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
        /// Initializes a new instance of the <see cref = "VolumeSeries" /> class.
        /// </summary>
        public VolumeSeries()
        {
            this.PositiveColor = OxyColors.DarkGreen;
            this.NegativeColor = OxyColors.Red;
            this.BarWidth = 0;
            this.StrokeThickness = 1;
            this.NegativeHollow = false;
            this.PositiveHollow = true;
            this.StrokeIntensity = 0.80;

            this.InterceptColor = OxyColors.Gray;
            this.InterceptLineStyle = LineStyle.Dash;
            this.InterceptStrokeThickness = 1;

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
        /// Gets or sets the stroke color of the Y=0 intercept
        /// </summary>
        public OxyColor InterceptColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the Y=0 intercept
        /// </summary>
        public double InterceptStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the line style of the Y=0 intercept
        /// </summary>
        public LineStyle InterceptLineStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether positive bars are shown as filled (false) or hollow (true) candlesticks
        /// </summary>
        public bool PositiveHollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether negative bars are shown as filled (false) or hollow (true) candlesticks
        /// </summary>
        public bool NegativeHollow { get; set; }

        /// <summary>
        /// Gets or sets the bar width in data units (for example if the X axis is date/time based, then should
        /// use the difference of DateTimeAxis.ToDouble(date) to indicate the width).  By default candlestick
        /// series will use 0.80 x the minimum difference in data points.
        /// </summary>
        public double BarWidth { get; set; }

        /// <summary>
        /// Append a bar to the series (must be in X order)
        /// </summary>
        /// <param name="bar">The Bar.</param>
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

            var clipping = this.GetClippingRect();

            var datacandlewidth = (this.BarWidth > 0) ? this.BarWidth : this.minDx * 0.80;
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

                var leftX = this.XAxis.Transform(bar.X) - (this.BarWidth / 2.0);
                var y0 = this.YAxis.Transform(0); 

                switch (VolumeStyle)
                {
                    case VolumeStyle.Combined:
                        {
                            var adj = this.YAxis.Transform(Math.Abs(bar.BuyVolume - bar.SellVolume));
                            var fillcolor = (bar.BuyVolume > bar.SellVolume) ? barfill_up : barfill_down;
                            var linecolor = (bar.BuyVolume > bar.SellVolume) ? line_up : line_down;
                            var rect1 = new OxyRect(leftX, adj, candlewidth, Math.Abs(adj - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect1, fillcolor, linecolor, this.StrokeThickness);
                        }

                        break;

                    case VolumeStyle.PositiveNegative:
                        {
                            var buyY = this.YAxis.Transform(bar.BuyVolume);
                            var sellY = this.YAxis.Transform(-bar.SellVolume);
                            var rect1 = new OxyRect(leftX, buyY, candlewidth, Math.Abs(buyY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect1, fill_up, line_up, this.StrokeThickness);
                            var rect2 = new OxyRect(leftX, y0, candlewidth, Math.Abs(sellY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect2, fill_down, line_down, this.StrokeThickness);
                        }

                        break;

                    case VolumeStyle.Stacked:
                        if (bar.BuyVolume > bar.SellVolume)
                        {
                            var buyY = this.YAxis.Transform(bar.BuyVolume);
                            var sellY = this.YAxis.Transform(bar.SellVolume);
                            var dyoffset = sellY - y0;
                            var rect2 = new OxyRect(leftX, sellY, candlewidth, Math.Abs(sellY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect2, fill_down, line_down, this.StrokeThickness);
                            var rect1 = new OxyRect(leftX, buyY + dyoffset, candlewidth, Math.Abs(buyY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect1, fill_up, line_up, this.StrokeThickness);
                        }
                        else
                        {
                            var buyY = this.YAxis.Transform(bar.BuyVolume);
                            var sellY = this.YAxis.Transform(bar.SellVolume);
                            var dyoffset = buyY - y0;
                            var rect1 = new OxyRect(leftX, buyY, candlewidth, Math.Abs(buyY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect1, fill_up, line_up, this.StrokeThickness);
                            var rect2 = new OxyRect(leftX, sellY + dyoffset, candlewidth, Math.Abs(sellY - y0));
                            rc.DrawClippedRectangleAsPolygon(clipping, rect2, fill_down, line_down, this.StrokeThickness);
                        }

                        break;
                }
            }

            // draw volume y=0 line
            var intercept = this.YAxis.Transform(0); 
            rc.DrawClippedLine(
                clipping,
                new[] { new ScreenPoint(clipping.Left, intercept), new ScreenPoint(clipping.Right, intercept) },
                0,
                this.InterceptColor,
                this.InterceptStrokeThickness,
                this.InterceptLineStyle.GetDashArray(),
                LineJoin.Miter,
                true);
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

            var datacandlewidth = (this.BarWidth > 0) ? this.BarWidth : this.minDx * 0.80;

            var fill_up = this.GetSelectableFillColor(this.PositiveColor);
            var line_up = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));

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
            var xy = this.InverseTransform(point);
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
                Position = this.Transform(hit),
                Item = mbar,
                Index = midx,
                Text = StringHelper.Format(
                    this.ActualCulture,
                    this.TrackerFormatString,
                    mbar,
                    this.XAxis.GetValue(mbar.X),
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
    }
}