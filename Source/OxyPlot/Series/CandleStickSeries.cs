// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    /// <remarks>
    /// See also <a href="http://en.wikipedia.org/wiki/Candlestick_chart">Wikipedia</a> and 
    /// <a href="http://www.mathworks.com/help/toolbox/finance/candle.html">Matlab documentation</a>.
    /// </remarks>
    public class CandleStickSeries : HighLowSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "CandleStickSeries" /> class.
        /// </summary>
        public CandleStickSeries()
        {
            this.CandleWidth = 10;
            this.IncreasingFill = OxyColors.Automatic;
            this.DecreasingFill = OxyColors.Undefined;
            this.ShadowEndColor = OxyColors.Undefined;
            this.ShadowEndLength = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleStickSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public CandleStickSeries(string title)
            : this()
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleStickSeries"/> class.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public CandleStickSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this(title)
        {
            this.Color = color;
            this.StrokeThickness = strokeThickness;
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
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The owner plot model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Items.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();

            foreach (var v in this.Items)
            {
                if (!this.IsValidItem(v, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    var high = this.Transform(v.X, v.High);
                    var low = this.Transform(v.X, v.Low);

                    if (double.IsNaN(v.Open) || double.IsNaN(v.Close))
                    {
                        rc.DrawClippedLine(
                            new[] { low, high },
                            clippingRect,
                            0,
                            this.GetSelectableColor(this.ActualColor),
                            this.StrokeThickness,
                            this.LineStyle,
                            this.LineJoin,
                            false);
                    }
                    else
                    {
                        var open = this.Transform(v.X, v.Open);
                        var close = this.Transform(v.X, v.Close);
                        var max = new ScreenPoint(open.X, Math.Max(open.Y, close.Y));
                        var min = new ScreenPoint(open.X, Math.Min(open.Y, close.Y));

                        // Upper shadow
                        rc.DrawClippedLine(
                            new[] { high, min },
                            clippingRect,
                            0,
                            this.GetSelectableColor(this.ActualColor),
                            this.StrokeThickness,
                            this.LineStyle,
                            this.LineJoin,
                            true);

                        // Lower shadow
                        rc.DrawClippedLine(
                            new[] { max, low },
                            clippingRect,
                            0,
                            this.GetSelectableColor(this.ActualColor),
                            this.StrokeThickness,
                            this.LineStyle,
                            this.LineJoin,
                            true);

                        // Shadow ends
                        if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0)
                        {
                            var highLeft = new ScreenPoint(high.X - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, high.Y);
                            var highRight = new ScreenPoint(high.X + (this.CandleWidth * 0.5 * this.ShadowEndLength), high.Y);
                            rc.DrawClippedLine(
                                 new[] { highLeft, highRight },
                                 clippingRect,
                                 0,
                                 this.GetSelectableColor(this.ShadowEndColor),
                                 this.StrokeThickness,
                                 this.LineStyle,
                                 this.LineJoin,
                                 true);

                            var lowLeft = new ScreenPoint(low.X - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, low.Y);
                            var lowRight = new ScreenPoint(low.X + (this.CandleWidth * 0.5 * this.ShadowEndLength), low.Y);
                            rc.DrawClippedLine(
                                new[] { lowLeft, lowRight },
                                clippingRect,
                                0,
                                this.GetSelectableColor(this.ShadowEndColor),
                                this.StrokeThickness,
                                this.LineStyle,
                                this.LineJoin,
                                true);
                        }

                        // Body
                        var openLeft = open + new ScreenVector(-this.CandleWidth * 0.5, 0);
                        var rect = new OxyRect(openLeft.X, min.Y, this.CandleWidth, max.Y - min.Y);
                        var fillColor = v.Close > v.Open
                                            ? this.GetSelectableFillColor(this.ActualIncreasingFill)
                                            : this.GetSelectableFillColor(this.DecreasingFill);
                        var strokeColor = this.GetSelectableColor(this.ActualColor);
                        rc.DrawClippedRectangleAsPolygon(rect, clippingRect, fillColor, strokeColor, this.StrokeThickness);
                    }
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol for the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The bounding rectangle of the legend box.
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double yopen = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.7);
            double yclose = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.3);
            double[] dashArray = this.LineStyle.GetDashArray();
            rc.DrawLine(
                new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) },
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                dashArray,
                OxyPenLineJoin.Miter,
                true);

            // Shadow ends
            if (this.ShadowEndColor.IsVisible() && this.ShadowEndLength > 0)
            {
                var highLeft = new ScreenPoint(xmid - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, legendBox.Top);
                var highRight = new ScreenPoint(xmid + (this.CandleWidth * 0.5 * this.ShadowEndLength), legendBox.Top);
                rc.DrawLine(
                     new[] { highLeft, highRight },
                     this.GetSelectableColor(this.ShadowEndColor),
                     this.StrokeThickness,
                     dashArray,
                     this.LineJoin,
                     true);

                var lowLeft = new ScreenPoint(xmid - (this.CandleWidth * 0.5 * this.ShadowEndLength) - 1, legendBox.Bottom);
                var lowRight = new ScreenPoint(xmid + (this.CandleWidth * 0.5 * this.ShadowEndLength), legendBox.Bottom);
                rc.DrawLine(
                    new[] { lowLeft, lowRight },
                    this.GetSelectableColor(this.ShadowEndColor),
                    this.StrokeThickness,
                    dashArray,
                    this.LineJoin,
                    true);
            }

            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (this.CandleWidth * 0.5), yclose, this.CandleWidth, yopen - yclose),
                this.GetSelectableFillColor(this.ActualIncreasingFill),
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness);
        }
    }
}