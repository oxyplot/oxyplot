// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickAndVolumeSeries.cs" company="OxyPlot">
//   Copyright (c) 2015 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for candlestick charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using OxyPlot.Axes;
using System.Collections.Generic;


namespace OxyPlot.Series
{
    using System;


    /// <summary>
    /// Represents a "higher performance" ordered OHLC series for candlestick charts
    /// 
    /// Does the following:
    /// - automatically calculates the appropriate bar width based on available screen + # of bars
    /// - can render and pan within millions of bars, using a fast approach to indexing in series
    /// - convenience methods
    /// 
    /// This implementation is associated with <a href="https://github.com/oxyplot/oxyplot/issues/369">issue 369</a>.
    /// </summary>
    /// <remarks>See also <a href="http://en.wikipedia.org/wiki/Candlestick_chart">Wikipedia</a> and
    /// <a href="http://www.mathworks.com/help/toolbox/finance/candle.html">Matlab documentation</a>.</remarks>
    public class FastCandleStickSeries : HighLowSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "CandleStickSeries" /> class.
        /// </summary>
        public FastCandleStickSeries ()
        {
            this.IncreasingColor = OxyColors.DarkGreen;
            this.DecreasingColor = OxyColors.Red;
            this.CandleWidthInDataUnits = 0;
        }


        // Properties

        /// <summary>
        /// Gets or sets the color used when the closing value is greater than opening value.
        /// </summary>
        public OxyColor IncreasingColor { get; set; }

        /// <summary>
        /// Gets or sets the fill color used when the closing value is less than opening value.
        /// </summary>
        public OxyColor DecreasingColor { get; set; }

        /// <summary>
        /// Gets or sets the bar width in data units (for example if the X axis is datetime based, then should
        /// use the difference of DateTimeAxis.ToDouble(date) to indicate the width).  By default candlestick
        /// series will use 0.80 x the minimum difference in data points.
        /// </summary>
        public double CandleWidthInDataUnits { get; set; }


        // Functions


        /// <summary>
        /// Append a bar to the series (must be in X order)
        /// </summary>
        /// <param name="bar">Bar.</param>
        public void Append (object bar)
        {
            var nbar = ToNativeBar (bar);
            if (Items.Count > 0 && Items [Items.Count - 1].X > nbar.X)
                throw new ArgumentException ("cannot append bar out of order, must be sequential in X");

            Items.Add (nbar);
        }


        /// <summary>
        /// Fast index of bar where max(bar[i].X) &lt;= x 
        /// </summary>
        /// <returns>The index of the bar closest to X, where max(bar[i].X) &lt;= x.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="Istarting">starting index</param> 
        public int FindByX (double x, int Istarting = -1)
        {
            if (Istarting < 0)
                Istarting = _pindex;

            return FindIndex (Items, x, Istarting);
        }


        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render (IRenderContext rc, PlotModel model)
        {
            var nitems = this.Items.Count;
            var items = this.Items;

            if (nitems == 0 || this.StrokeThickness <= 0 || this.LineStyle == LineStyle.None)
            {
                return;
            }

            this.VerifyAxes ();

            var clippingRect = this.GetClippingRect ();
            var dashArray = this.LineStyle.GetDashArray ();

            var datacandlewidth = (CandleWidthInDataUnits > 0) ? CandleWidthInDataUnits : _dx * 0.80;
            var candlewidth = 
                this.XAxis.Transform (items [0].X + datacandlewidth) -
                this.XAxis.Transform (items [0].X); 

            // colors
            var fill_up = this.GetSelectableFillColor (this.IncreasingColor);
            var fill_down = this.GetSelectableFillColor (this.DecreasingColor);
            var line_up = this.GetSelectableColor (this.IncreasingColor.ChangeIntensity (0.70));
            var line_down = this.GetSelectableColor (this.DecreasingColor.ChangeIntensity (0.70));


            // determine render range
            var xmin = this.XAxis.ActualMinimum;
            var xmax = this.XAxis.ActualMaximum;
            _pindex = FindIndex (items, xmin, _pindex);

            for (int i = _pindex; i < nitems; i++)
            {
                var bar = items [i];

                // if item beyond visible range, done
                if (bar.X > xmax)
                    return;
                if (bar.X < xmin)
                    continue;

                // check to see whether is valid
                if (!this.IsValidItem (bar, this.XAxis, this.YAxis))
                    continue;

                var fillColor = bar.Close > bar.Open ? fill_up : fill_down;
                var lineColor = bar.Close > bar.Open ? line_up : line_down;

                var high = this.Transform (bar.X, bar.High);
                var low = this.Transform (bar.X, bar.Low);

                var open = this.Transform (bar.X, bar.Open);
                var close = this.Transform (bar.X, bar.Close);
                var max = new ScreenPoint (open.X, Math.Max (open.Y, close.Y));
                var min = new ScreenPoint (open.X, Math.Min (open.Y, close.Y));

                // Upper extent
                rc.DrawClippedLine (
                    clippingRect,
                    new[] { high, min },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    dashArray,
                    this.LineJoin,
                    true);

                // Lower extent
                rc.DrawClippedLine (
                    clippingRect,
                    new[] { max, low },
                    0,
                    lineColor,
                    this.StrokeThickness,
                    dashArray,
                    this.LineJoin,
                    true);

                // Body
                var openLeft = open + new ScreenVector (-candlewidth * 0.5, 0);
                var rect = new OxyRect (openLeft.X, min.Y, candlewidth, max.Y - min.Y);
                rc.DrawClippedRectangleAsPolygon (clippingRect, rect, fillColor, lineColor, this.StrokeThickness);
            }
        }


        /// <summary>
        /// Renders the legend symbol for the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend (IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double yopen = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.7);
            double yclose = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.3);
            double[] dashArray = this.LineStyle.GetDashArray ();

            var datacandlewidth = (CandleWidthInDataUnits > 0) ? CandleWidthInDataUnits : _dx * 0.80;

            var candlewidth = 
                this.XAxis.Transform (this.Items [0].X + datacandlewidth) -
                this.XAxis.Transform (this.Items [0].X); 

            rc.DrawLine (
                new[] { new ScreenPoint (xmid, legendBox.Top), new ScreenPoint (xmid, legendBox.Bottom) },
                this.GetSelectableColor (this.ActualColor),
                this.StrokeThickness,
                dashArray,
                LineJoin.Miter,
                true);

            rc.DrawRectangleAsPolygon (
                new OxyRect (xmid - (candlewidth * 0.5), yclose, candlewidth, yopen - yclose),
                this.GetSelectableFillColor (this.IncreasingColor),
                this.GetSelectableColor (this.ActualColor),
                this.StrokeThickness);
        }


        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData ()
        {
            base.UpdateData ();
            _pindex = 0;

            // determine minimum X gap between successive points
            var items = Items;
            var nitems = items.Count;
            _dx = double.MaxValue;

            for (int i = 1; i < nitems; i++)
            {
                _dx = Math.Min (_dx, items [i].X - items [i - 1].X);
                if (_dx < 0)
                    throw new ArgumentException ("bars are out of order, must be sequential in x");
            }

            if (nitems <= 1)
                _dx = 1;
        }


        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint (ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null || interpolate || Items.Count == 0)
                return null;

            var nbars = Items.Count;
            var xy = InverseTransform (point);
            var targetX = xy.X;

            // punt if beyond start & end of series
            if (targetX > (Items [nbars - 1].X + _dx))
                return null;
            if (targetX < (Items [0].X - _dx))
                return null;

            var pidx = FindIndex (Items, targetX, _pindex);
            var nidx = ((pidx + 1) < Items.Count) ? pidx + 1 : pidx;

            Func<HighLowItem,double> distance = (bar) =>
            {
                var dx = bar.X - xy.X;
                var dyo = bar.Open - xy.Y;
                var dyh = bar.High - xy.Y;
                var dyl = bar.Low - xy.Y;
                var dyc = bar.Close - xy.Y;

                var d2o = (dx * dx) + (dyo * dyo);
                var d2h = (dx * dx) + (dyh * dyh);
                var d2l = (dx * dx) + (dyl * dyl);
                var d2c = (dx * dx) + (dyc * dyc);

                return Math.Min (d2o, Math.Min (d2h, Math.Min (d2l, d2c)));
            };

            // determine closest point
            var midx = distance (Items [pidx]) <= distance (Items [nidx]) ? pidx : nidx; 
            var mbar = Items [midx];

            var hit = new DataPoint (mbar.X, mbar.Close);
            return new TrackerHitResult {
                Series = this,
                DataPoint = hit,
                Position = Transform (hit),
                Item = mbar,
                Index = midx,
                Text = StringHelper.Format (
                    this.ActualCulture,
                    this.TrackerFormatString,
                    mbar,
                    this.Title,
                    this.XAxis.Title ?? DefaultXAxisTitle,
                    this.XAxis.GetValue (mbar.X),
                    this.YAxis.GetValue (mbar.High),
                    this.YAxis.GetValue (mbar.Low),
                    this.YAxis.GetValue (mbar.Open),
                    this.YAxis.GetValue (mbar.Close))
            };
        }


        /// <summary>
        /// Comvert incoming bar to native bar
        /// </summary>
        /// <returns>The native bar.</returns>
        /// <param name="bar">Bar.</param>
        private HighLowItem ToNativeBar (object bar)
        {
            var nativebar = bar as HighLowItem;

            // if native bar can add direcly
            if (nativebar != null)
                return nativebar;

            // otherwise must translate to native bar
            var x = FieldValueOf (bar, DataFieldX);
            var open = FieldValueOf (bar, DataFieldOpen);
            var high = FieldValueOf (bar, DataFieldHigh);
            var low = FieldValueOf (bar, DataFieldLow);
            var close = FieldValueOf (bar, DataFieldClose);
            return new HighLowItem (x, high, low, open, close);
        }


        /// <summary>
        /// Get named field on 
        /// </summary>
        /// <returns>The value of.</returns>
        /// <param name="bar">Bar.</param>
        /// <param name="field">Field.</param>
        private double FieldValueOf (object bar, string field)
        {
            if (field != null)
            {
                var type = bar.GetType ();
                var prop = type.GetProperty (field);
                return Axis.ToDouble (prop.GetValue (bar, null));
            } else
                return double.NaN;
        }


        /// <summary>
        /// Find index of max(x) &lt;= target x
        /// </summary>
        /// <param name='items'>
        /// vector of bars
        /// </param>
        /// <param name='targetX'>
        /// target x.
        /// </param>
        /// <param name='Iguess'>
        /// initial guess.
        /// </param>
        /// <returns>
        /// index of x with max(x) &lt;= target x or -1 if cannot find
        /// </returns>
        private static int FindIndex (List<HighLowItem> items, double targetX, int Iguess)
        {
            int Ilastguess = 0;
            int Istart = 0;
            int Iend = items.Count - 1;

            while (Istart <= Iend)
            {
                if (Iguess < Istart)
                    return Ilastguess;
                if (Iguess > Iend)
                    return Iend;

                var Xguess = items [Iguess].X;
                if (Xguess == targetX)
                    return Iguess;

                if (Xguess > targetX)
                {
                    Iend = Iguess - 1;
                    if (Iend < Istart)
                        return Ilastguess;
                    if (Iend == Istart)
                        return Iend;
                }
                else
                { 
                    Istart = Iguess + 1; 
                    Ilastguess = Iguess; 
                }

                if (Istart >= Iend)
                    return Ilastguess;

                var Xend = items [Iend].X;
                var Xstart = items [Istart].X;

                var m = (double)(Iend - Istart + 1) / (Xend - Xstart);
                Iguess = Istart + (int)((targetX - Xstart) * m);
            }

            return Ilastguess;
        }


        // Variables

        private double _dx;
        private int _pindex;
    }
}