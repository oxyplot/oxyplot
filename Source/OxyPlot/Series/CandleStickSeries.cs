//-----------------------------------------------------------------------
// <copyright file="CandleStickSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Use CandleStickSeries to create a candlestick chart
    /// http://en.wikipedia.org/wiki/Candlestick_chart
    /// http://www.mathworks.com/help/toolbox/finance/candle.html
    /// </summary>
    public class CandleStickSeries : HighLowSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleStickSeries"/> class.
        /// </summary>
        public CandleStickSeries()
        {
            this.CandleWidth = 10;
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
            : this()
        {
            this.Color = color;
            this.StrokeThickness = strokeThickness;
            this.Title = title;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the width of the candle.
        /// </summary>
        /// <value>The width of the candle.</value>
        public double CandleWidth { get; set; }

        #endregion

        #region Public Methods

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
            if (this.items.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            OxyRect clippingRect = this.GetClippingRect();

            foreach (HighLowItem v in this.items)
            {
                if (!this.IsValidPoint(v, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    ScreenPoint high = this.XAxis.Transform(v.X, v.High, this.YAxis);
                    ScreenPoint low = this.XAxis.Transform(v.X, v.Low, this.YAxis);

                    if (double.IsNaN(v.Open) || double.IsNaN(v.Close))
                    {
                        rc.DrawClippedLine(
                            new[] { low, high }, 
                            clippingRect, 
                            0, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            false);
                    }
                    else
                    {
                        ScreenPoint open = this.XAxis.Transform(v.X, v.Open, this.YAxis);
                        ScreenPoint close = this.XAxis.Transform(v.X, v.Close, this.YAxis);
                        var max = new ScreenPoint(open.X, Math.Max(open.Y, close.Y));
                        var min = new ScreenPoint(open.X, Math.Min(open.Y, close.Y));

                        rc.DrawClippedLine(
                            new[] { high, min }, 
                            clippingRect, 
                            0, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            true);

                        rc.DrawClippedLine(
                            new[] { max, low }, 
                            clippingRect, 
                            0, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            true);
                        ScreenPoint openLeft = open;
                        openLeft.X -= this.CandleWidth * 0.5;
                        ScreenPoint closeRight = close;
                        closeRight.X += this.CandleWidth * 0.5;
                        var rect = new OxyRect(openLeft.X, min.Y, this.CandleWidth, max.Y - min.Y);
                        rc.DrawClippedRectangleAsPolygon(
                            rect, clippingRect, v.Open > v.Close ? this.Color : null, this.Color, this.StrokeThickness);
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
            double yOpen = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.7;
            double yClose = legendBox.Top + (legendBox.Bottom - legendBox.Top) * 0.3;
            double[] dashArray = LineStyleHelper.GetDashArray(this.LineStyle);
            rc.DrawLine(
                new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, legendBox.Bottom) }, 
                this.Color, 
                this.StrokeThickness, 
                dashArray, 
                OxyPenLineJoin.Miter, 
                true);
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - this.CandleWidth * 0.5, yClose, this.CandleWidth, yOpen - yClose), 
                this.Color, 
                this.Color, 
                this.StrokeThickness);
        }

        #endregion
    }
}
