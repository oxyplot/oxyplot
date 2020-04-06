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
    using System.Diagnostics;

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
            this.VolumeStyle = VolumeStyle.Combined;

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
        /// Gets or sets the style of volume rendering (defaults to Combined)
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
        /// Gets or sets the minimum volume seen in the data series.
        /// </summary>
        public double MinimumVolume { get; protected set; }

        /// <summary>
        /// Gets or sets the maximum volume seen in the data series.
        /// </summary>
        public double MaximumVolume { get; protected set; }

        /// <summary>
        /// Gets or sets the average volume seen in the data series.
        /// </summary>
        public double AverageVolume { get; protected set; }

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
        public override void Render(IRenderContext rc)
        {
            if (this.data == null || this.data.Count == 0)
            {
                return;
            }

            var items = this.data;
            var nitems = this.data.Count;

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();

            var datacandlewidth = (this.BarWidth > 0) ? this.BarWidth : this.minDx * 0.80;
            var halfDataCandleWidth = datacandlewidth * 0.5;

            // colors
            var fillUp = this.GetSelectableFillColor(this.PositiveColor);
            var fillDown = this.GetSelectableFillColor(this.NegativeColor);

            var barfillUp = this.PositiveHollow ? OxyColors.Transparent : fillUp;
            var barfillDown = this.NegativeHollow ? OxyColors.Transparent : fillDown;

            var lineUp = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));
            var lineDown = this.GetSelectableColor(this.NegativeColor.ChangeIntensity(this.StrokeIntensity));

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

                var p1 = this.Transform(bar.X - halfDataCandleWidth, 0);

                switch (this.VolumeStyle)
                {
                    case VolumeStyle.Combined:
                        {
                            var p2 = this.Transform(bar.X + halfDataCandleWidth, Math.Abs(bar.BuyVolume - bar.SellVolume));
                            var fillcolor = (bar.BuyVolume > bar.SellVolume) ? barfillUp : barfillDown;
                            var linecolor = (bar.BuyVolume > bar.SellVolume) ? lineUp : lineDown;
                            var rect1 = new OxyRect(p1, p2);
                            rc.DrawClippedRectangle(clippingRect, rect1, fillcolor, linecolor, this.StrokeThickness, this.EdgeRenderingMode);
                        }

                        break;

                    case VolumeStyle.PositiveNegative:
                        {
                            var p2Buy = this.Transform(bar.X + halfDataCandleWidth, bar.BuyVolume);
                            var p2Bell = this.Transform(bar.X + halfDataCandleWidth, -bar.SellVolume);

                            var rectBuy = new OxyRect(p1, p2Buy);
                            var rectSell = new OxyRect(p1, p2Bell);

                            rc.DrawClippedRectangle(clippingRect, rectBuy, fillUp, lineUp, this.StrokeThickness, this.EdgeRenderingMode);
                            rc.DrawClippedRectangle(clippingRect, rectSell, fillDown, lineDown, this.StrokeThickness, this.EdgeRenderingMode);
                        }

                        break;

                    case VolumeStyle.Stacked:
                        {
                            var p2Buy = this.Transform(bar.X + halfDataCandleWidth, bar.BuyVolume);
                            var p2Sell = this.Transform(bar.X + halfDataCandleWidth, bar.SellVolume);
                            var pBoth = this.Transform(bar.X - halfDataCandleWidth, bar.BuyVolume + bar.SellVolume);

                            OxyRect rectBuy;
                            OxyRect rectSell;

                            if (bar.BuyVolume > bar.SellVolume)
                            {
                                rectSell = new OxyRect(p1, p2Sell);
                                rectBuy = new OxyRect(p2Sell, pBoth);
                            }
                            else
                            {
                                rectBuy = new OxyRect(p1, p2Buy);
                                rectSell = new OxyRect(p2Buy, pBoth);
                            }

                            rc.DrawClippedRectangle(clippingRect, rectBuy, fillUp, lineUp, this.StrokeThickness, this.EdgeRenderingMode);
                            rc.DrawClippedRectangle(clippingRect, rectSell, fillDown, lineDown, this.StrokeThickness, this.EdgeRenderingMode);

                            break;
                        }
                    case VolumeStyle.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (this.InterceptStrokeThickness > 0 && this.InterceptLineStyle != LineStyle.None)
            {
                // draw volume y=0 line
                var p1 = this.InverseTransform(clippingRect.BottomLeft);
                var p2 = this.InverseTransform(clippingRect.TopRight);
                var lineA = this.Transform(p1.X, 0);
                var lineB = this.Transform(p2.X, 0);

                rc.DrawClippedLine(
                    clippingRect,
                    new[] { lineA, lineB },
                    0,
                    this.InterceptColor,
                    this.InterceptStrokeThickness,
                    this.EdgeRenderingMode,
                    this.InterceptLineStyle.GetDashArray(),
                    LineJoin.Miter);
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

            var fillUp = this.GetSelectableFillColor(this.PositiveColor);
            var lineUp = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(this.StrokeIntensity));

            var candlewidth = legendBox.Width * 0.75;

            if (this.StrokeThickness > 0)
            {
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                    lineUp,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);

                rc.DrawRectangle(
                    new OxyRect(xmid - (candlewidth * 0.5), yclose, candlewidth, yopen - yclose),
                    fillUp,
                    lineUp,
                    this.StrokeThickness,
                    this.EdgeRenderingMode);
            }
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null || interpolate || this.data == null || this.data.Count == 0)
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

            Func<OhlcvItem, double> distance = bar =>
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

            if (this.data == null || this.data.Count == 0)
            {
                return;
            }

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
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);

            var ymin = this.MinimumVolume;
            var ymax = this.MaximumVolume;
            var yavg = this.AverageVolume;

            var yquartile = (ymax - ymin) / 4.0;

            switch (this.VolumeStyle)
            {
                case VolumeStyle.PositiveNegative:
                    ymin = -(yavg + (yquartile / 2.0));
                    ymax = +(yavg + (yquartile / 2.0));
                    break;
                case VolumeStyle.Stacked:
                    ymax = yavg + yquartile;
                    ymin = 0;
                    break;
                default:
                    ymax = yavg + (yquartile / 2.0);
                    ymin = 0;
                    break;
            }

            ymin = Math.Max(this.YAxis.FilterMinValue, ymin);
            ymax = Math.Min(this.YAxis.FilterMaxValue, ymax);
            this.YAxis.Include(ymin);
            this.YAxis.Include(ymax);
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = 0.0;
            var ymax = double.MinValue;

            var nvol = 0.0;
            var cumvol = 0.0;

            foreach (var bar in this.Items)
            {
                if (!bar.IsValid())
                {
                    continue;
                }

                if (bar.SellVolume > 0)
                {
                    nvol++;
                }

                if (bar.BuyVolume > 0)
                {
                    nvol++;
                }

                cumvol += bar.BuyVolume;
                cumvol += bar.SellVolume;

                xmin = Math.Min(xmin, bar.X);
                xmax = Math.Max(xmax, bar.X);
                ymin = Math.Min(ymin, -bar.SellVolume);
                ymax = Math.Max(ymax, +bar.BuyVolume);
            }

            this.MinX = Math.Max(this.XAxis.FilterMinValue, xmin);
            this.MaxX = Math.Min(this.XAxis.FilterMaxValue, xmax);

            this.MinimumVolume = ymin;
            this.MaximumVolume = ymax;
            this.AverageVolume = cumvol / nvol;
        }
    }
}
