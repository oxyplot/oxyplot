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
            this.VolumeStyle = VolumeStyle.Combined;
            this.VolumeAxisKey = "Volume";
            this.BarAxisKey = null;

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
                return this.data ?? (this.data = new List<OhlcvItem>());
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
                return (LinearAxis)this.YAxis;
            }
        }

        /// <summary>
        /// Gets the portion of the Y axis associated with volume
        /// </summary>
        public LinearAxis VolumeAxis { get; private set; }

        /// <summary>
        /// Gets or sets the volume axis key (defaults to "Volume")
        /// </summary>
        public string VolumeAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the bar axis key (defaults to null, as is the primary axis).
        /// </summary>
        public string BarAxisKey { get; set; }

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
        // ReSharper disable once FunctionComplexityOverflow
        public override void Render(IRenderContext rc)
        {
            if (this.IsTransposed())
            {
                throw new Exception("CandleStickAndVolumeSeries does not support transposed mode. It can only be used with horizontal X axis and vertical Y axis.");
            }

            if (this.data == null || this.data.Count == 0)
            {
                return;
            }

            var items = this.data;
            var nitems = this.data.Count;

            this.VerifyAxes();

            var clippingBar = this.GetClippingRect(this.BarAxis);
            var clippingSep = this.GetSeparationClippingRect();
            var clippingVol = this.GetClippingRect(this.VolumeAxis);

            var datacandlewidth = (this.CandleWidth > 0) ? this.CandleWidth : this.minDx * 0.80;
            var candlewidth =
                this.XAxis.Transform(items[0].X + datacandlewidth) -
                this.XAxis.Transform(items[0].X) - this.StrokeThickness;

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

                var fillColor = bar.Close > bar.Open ? barfillUp : barfillDown;
                var lineColor = bar.Close > bar.Open ? lineUp : lineDown;

                var high = this.Transform(bar.X, bar.High);
                var low = this.Transform(bar.X, bar.Low);

                var open = this.Transform(bar.X, bar.Open);
                var close = this.Transform(bar.X, bar.Close);

                var max = new ScreenPoint(open.X, Math.Max(open.Y, close.Y));
                var min = new ScreenPoint(open.X, Math.Min(open.Y, close.Y));

                // Bar part
                rc.DrawClippedLine(
                    clippingBar,
                    new[] { high, min },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    null,
                    LineJoin.Miter);

                // Lower extent
                rc.DrawClippedLine(
                    clippingBar,
                    new[] { max, low },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    null,
                    LineJoin.Miter);

                // Body
                var openLeft = open + new ScreenVector(-candlewidth * 0.5, 0);

                if (max.Y - min.Y < 1.0)
                {
                    var leftPoint = new ScreenPoint(openLeft.X - this.StrokeThickness, min.Y);
                    var rightPoint = new ScreenPoint(openLeft.X + this.StrokeThickness + candlewidth, min.Y);
                    rc.DrawClippedLine(
                        clippingBar, 
                        new[] { leftPoint, rightPoint }, 
                        leftPoint.DistanceToSquared(rightPoint), 
                        lineColor, 
                        this.StrokeThickness, 
                        this.EdgeRenderingMode,
                        null, LineJoin.Miter);

                    leftPoint = new ScreenPoint(openLeft.X - this.StrokeThickness, max.Y);
                    rightPoint = new ScreenPoint(openLeft.X + this.StrokeThickness + candlewidth, max.Y);
                    rc.DrawClippedLine(
                        clippingBar, 
                        new[] { leftPoint, rightPoint }, 
                        leftPoint.DistanceToSquared(rightPoint), 
                        lineColor, 
                        this.StrokeThickness, 
                        this.EdgeRenderingMode,
                        null, 
                        LineJoin.Miter);
                }
                else
                {
                    var rect = new OxyRect(openLeft.X, min.Y, candlewidth, max.Y - min.Y);
                    rc.DrawClippedRectangle(clippingBar, rect, fillColor, lineColor, this.StrokeThickness, this.EdgeRenderingMode);
                }

                // Volume Part
                if (this.VolumeAxis == null || this.VolumeStyle == VolumeStyle.None)
                {
                    continue;
                }

                var iY0 = this.VolumeAxis.Transform(0);
                switch (this.VolumeStyle)
                {
                    case VolumeStyle.Combined:
                        {
                            var adj = this.VolumeAxis.Transform(Math.Abs(bar.BuyVolume - bar.SellVolume));
                            var fillcolor = (bar.BuyVolume > bar.SellVolume) ? barfillUp : barfillDown;
                            var linecolor = (bar.BuyVolume > bar.SellVolume) ? lineUp : lineDown;
                            var rect1 = new OxyRect(openLeft.X, adj, candlewidth, Math.Abs(adj - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect1, fillcolor, linecolor, this.StrokeThickness, this.EdgeRenderingMode);
                        }

                        break;

                    case VolumeStyle.PositiveNegative:
                        {
                            var buyY = this.VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = this.VolumeAxis.Transform(-bar.SellVolume);
                            var rect1 = new OxyRect(openLeft.X, buyY, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect1, fillUp, lineUp, this.StrokeThickness, this.EdgeRenderingMode);
                            var rect2 = new OxyRect(openLeft.X, iY0, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect2, fillDown, lineDown, this.StrokeThickness, this.EdgeRenderingMode);
                        }

                        break;

                    case VolumeStyle.Stacked:
                        if (bar.BuyVolume > bar.SellVolume)
                        {
                            var buyY = this.VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = this.VolumeAxis.Transform(bar.SellVolume);
                            var dyoffset = sellY - iY0;
                            var rect2 = new OxyRect(openLeft.X, sellY, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect2, fillDown, lineDown, this.StrokeThickness, this.EdgeRenderingMode);
                            var rect1 = new OxyRect(openLeft.X, buyY + dyoffset, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect1, fillUp, lineUp, this.StrokeThickness, this.EdgeRenderingMode);
                        }
                        else
                        {
                            var buyY = this.VolumeAxis.Transform(bar.BuyVolume);
                            var sellY = this.VolumeAxis.Transform(bar.SellVolume);
                            var dyoffset = buyY - iY0;
                            var rect1 = new OxyRect(openLeft.X, buyY, candlewidth, Math.Abs(buyY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect1, fillUp, lineUp, this.StrokeThickness, this.EdgeRenderingMode);
                            var rect2 = new OxyRect(openLeft.X, sellY + dyoffset, candlewidth, Math.Abs(sellY - iY0));
                            rc.DrawClippedRectangle(clippingVol, rect2, fillDown, lineDown, this.StrokeThickness, this.EdgeRenderingMode);
                        }

                        break;
                }
            }

            if (this.SeparatorStrokeThickness > 0 && this.SeparatorLineStyle != LineStyle.None)
            {
                // draw volume & bar separation line
                if (this.VolumeStyle != VolumeStyle.None)
                {
                    var ysep = (clippingSep.Bottom + clippingSep.Top) / 2.0;
                    rc.DrawClippedLine(
                        clippingSep,
                        new[] { new ScreenPoint(clippingSep.Left, ysep), new ScreenPoint(clippingSep.Right, ysep) },
                        0,
                        this.SeparatorColor,
                        this.SeparatorStrokeThickness,
                        this.EdgeRenderingMode,
                        this.SeparatorLineStyle.GetDashArray(),
                        LineJoin.Miter);
                }

                // draw volume y=0 line
                if (this.VolumeAxis != null && this.VolumeStyle == VolumeStyle.PositiveNegative)
                {
                    var y0 = this.VolumeAxis.Transform(0);
                    rc.DrawClippedLine(
                        clippingVol,
                        new[] { new ScreenPoint(clippingVol.Left, y0), new ScreenPoint(clippingVol.Right, y0) },
                        0,
                        OxyColors.Goldenrod,
                        this.SeparatorStrokeThickness,
                        this.EdgeRenderingMode,
                        this.SeparatorLineStyle.GetDashArray(),
                        LineJoin.Miter);
                }
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

            var fillUp = this.GetSelectableFillColor(this.PositiveColor);
            var lineUp = this.GetSelectableColor(this.PositiveColor.ChangeIntensity(0.70));

            var candlewidth = Math.Min(
                legendBox.Width,
                this.XAxis.Transform(this.data[0].X + datacandlewidth) - this.XAxis.Transform(this.data[0].X));

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
            // find volume axis
            this.VolumeAxis = (LinearAxis)this.PlotModel.Axes.FirstOrDefault(a => a.Key == this.VolumeAxisKey);

            // now setup XYSeries axes, where BarAxisKey indicates the primary Y axis
            this.YAxisKey = this.BarAxisKey;
            base.EnsureAxes();
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
            this.YAxis.Include(this.MinY);
            this.YAxis.Include(this.MaxY);

            // we may not have a volume axis, if so, skip adjustments
            if (this.VolumeAxis == null)
            {
                return;
            }

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

            ymin = Math.Max(this.VolumeAxis.FilterMinValue, ymin);
            ymax = Math.Min(this.VolumeAxis.FilterMaxValue, ymax);
            this.VolumeAxis.Include(ymin);
            this.VolumeAxis.Include(ymax);
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            double xmin = double.MaxValue;
            double xmax = double.MinValue;
            double yminBar = double.MaxValue;
            double ymaxBar = double.MinValue;
            double yminVol = double.MaxValue;
            double ymaxVol = double.MinValue;

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
                yminBar = Math.Min(yminBar, bar.Low);
                ymaxBar = Math.Max(ymaxBar, bar.High);
                yminVol = Math.Min(yminVol, -bar.SellVolume);
                ymaxVol = Math.Max(ymaxVol, +bar.BuyVolume);
            }

            this.MinX = Math.Max(this.XAxis.FilterMinValue, xmin);
            this.MaxX = Math.Min(this.XAxis.FilterMaxValue, xmax);
            this.MinY = Math.Max(this.YAxis.FilterMinValue, yminBar);
            this.MaxY = Math.Min(this.YAxis.FilterMaxValue, ymaxBar);

            this.MinimumVolume = yminVol;
            this.MaximumVolume = ymaxVol;
            this.AverageVolume = cumvol / nvol;
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

            double minY;
            double maxY;
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
