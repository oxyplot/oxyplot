// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OldCandleStickSeries.cs" company="OxyPlot">
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
    /// Represents a series for candlestick charts.
    /// </summary>
    /// <remarks>See also <a href="http://en.wikipedia.org/wiki/Candlestick_chart">Wikipedia</a> and
    /// <a href="http://www.mathworks.com/help/toolbox/finance/candle.html">Matlab documentation</a>.</remarks>
    [Obsolete("use replacement CandleStickSeries instead")]
    public class OldCandleStickSeries : HighLowSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "OldCandleStickSeries" /> class.
        /// </summary>
        public OldCandleStickSeries()
        {
            this.CandleWidth = 10;
            this.IncreasingFill = OxyColors.Automatic;
            this.DecreasingFill = OxyColors.Undefined;
            this.ShadowEndColor = OxyColors.Undefined;
            this.ShadowEndLength = 1.0;
        }

        /// <summary>
        /// Gets or sets the width of the candle (in screen space units).
        /// </summary>
        public double CandleWidth { get; set; }

        /// <summary>
        /// Gets or sets the color used when the closing value is greater than opening value.
        /// </summary>
        public OxyColor IncreasingFill { get; set; }

        /// <summary>
        /// Gets or sets the fill color used when the closing value is less than opening value.
        /// </summary>
        public OxyColor DecreasingFill { get; set; }

        /// <summary>
        /// Gets or sets the end color of the shadow.
        /// </summary>
        /// <value>The end color of the shadow.</value>
        public OxyColor ShadowEndColor { get; set; }

        /// <summary>
        /// Gets or sets the lengths of the shadow ends.
        /// </summary>
        /// <value>The length relative to the width of the candle.</value>
        public double ShadowEndLength { get; set; }

        /// <summary>
        /// Gets the actual increasing fill color.
        /// </summary>
        public OxyColor ActualIncreasingFill
        {
            get
            {
                return this.IncreasingFill.GetActualColor(this.ActualColor);
            }
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.IsTransposed())
            {
                throw new Exception("OldCandleStickSeries does not support transposed mode. It can only be used with horizontal X axis and vertical Y axis.");
            }

            if (this.Items.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            var dashArray = this.LineStyle.GetDashArray();
            var actualColor = this.GetSelectableColor(this.ActualColor);
            var shadowEndColor = this.GetSelectableColor(this.ShadowEndColor);

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                foreach (var v in this.Items)
                {
                    if (!this.IsValidItem(v, this.XAxis, this.YAxis))
                    {
                        continue;
                    }

                    if (v.X <= this.XAxis.ActualMinimum || v.X >= this.XAxis.ActualMaximum)
                    {
                        continue;
                    }

                    var high = this.Transform(v.X, v.High);
                    var low = this.Transform(v.X, v.Low);

                    if (double.IsNaN(v.Open) || double.IsNaN(v.Close))
                    {
                        rc.DrawClippedLine(
                            clippingRect,
                            new[] { low, high },
                            0,
                            actualColor,
                            this.StrokeThickness,
                            this.EdgeRenderingMode,
                            dashArray,
                            this.LineJoin);
                    }
                    else
                    {
                        var open = this.Transform(v.X, v.Open);
                        var close = this.Transform(v.X, v.Close);
                        var max = new ScreenPoint(open.X, Math.Max(open.Y, close.Y));
                        var min = new ScreenPoint(open.X, Math.Min(open.Y, close.Y));

                        // Upper shadow
                        rc.DrawClippedLine(
                            clippingRect,
                            new[] { high, min },
                            0,
                            actualColor,
                            this.StrokeThickness,
                            this.EdgeRenderingMode,
                            dashArray,
                            this.LineJoin);

                        // Lower shadow
                        rc.DrawClippedLine(
                            clippingRect,
                            new[] { max, low },
                            0,
                            actualColor,
                            this.StrokeThickness,
                            this.EdgeRenderingMode,
                            dashArray,
                            this.LineJoin);

                        // Shadow ends
                        if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0)
                        {
                            var highLeft = new ScreenPoint(high.X - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, high.Y);
                            var highRight = new ScreenPoint(high.X + (this.CandleWidth * 0.5 * this.ShadowEndLength), high.Y);
                            rc.DrawClippedLine(
                                 clippingRect,
                                 new[] { highLeft, highRight },
                                 0,
                                 shadowEndColor,
                                 this.StrokeThickness,
                                 this.EdgeRenderingMode,
                                 dashArray,
                                 this.LineJoin);

                            var lowLeft = new ScreenPoint(low.X - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, low.Y);
                            var lowRight = new ScreenPoint(low.X + (this.CandleWidth * 0.5 * this.ShadowEndLength), low.Y);
                            rc.DrawClippedLine(
                                clippingRect,
                                new[] { lowLeft, lowRight },
                                0,
                                shadowEndColor,
                                this.StrokeThickness,
                                this.EdgeRenderingMode,
                                dashArray,
                                this.LineJoin);
                        }

                        // Body
                        var openLeft = open + new ScreenVector(-this.CandleWidth * 0.5, 0);
                        var rect = new OxyRect(openLeft.X, min.Y, this.CandleWidth, max.Y - min.Y);
                        var fillColor = v.Close > v.Open
                                            ? this.GetSelectableFillColor(this.ActualIncreasingFill)
                                            : this.GetSelectableFillColor(this.DecreasingFill);
                        rc.DrawClippedRectangle(clippingRect, rect, fillColor, actualColor, this.StrokeThickness, this.EdgeRenderingMode);
                    }
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
            double[] dashArray = this.LineStyle.GetDashArray();

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                    this.GetSelectableColor(this.ActualColor),
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);

                // Shadow ends
                if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0)
                {
                    var highLeft = new ScreenPoint(xmid - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, legendBox.Top);
                    var highRight = new ScreenPoint(xmid + (this.CandleWidth * 0.5 * this.ShadowEndLength), legendBox.Top);
                    rc.DrawLine(
                         new[] { highLeft, highRight },
                         this.GetSelectableColor(this.ShadowEndColor),
                         this.StrokeThickness,
                         this.EdgeRenderingMode,
                         dashArray,
                         this.LineJoin);

                    var lowLeft = new ScreenPoint(xmid - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, legendBox.Bottom);
                    var lowRight = new ScreenPoint(xmid + (this.CandleWidth * 0.5 * this.ShadowEndLength), legendBox.Bottom);
                    rc.DrawLine(
                        new[] { lowLeft, lowRight },
                        this.GetSelectableColor(this.ShadowEndColor),
                        this.StrokeThickness,
                        this.EdgeRenderingMode,
                        dashArray,
                        this.LineJoin);
                }
            }

            rc.DrawRectangle(
                new OxyRect(xmid - (this.CandleWidth * 0.5), yclose, this.CandleWidth, yopen - yclose),
                this.GetSelectableFillColor(this.ActualIncreasingFill),
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                this.EdgeRenderingMode);
        }
    }
}
