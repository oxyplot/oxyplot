// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The ColumnSeries is used to create clustered or stacked column charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The ColumnSeries is used to create clustered or stacked column charts.
    /// </summary>
    public class ColumnSeries : BarSeriesBase<ColumnItem>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries"/> class.
        /// </summary>
        public ColumnSeries()
        {
            this.ColumnWidth = 1;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>
        /// The width of the column. 
        /// </value>
        public double ColumnWidth { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the width/height of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <returns>
        /// The fractional width. 
        /// </returns>
        /// <value>
        /// The width of the bars. 
        /// </value>
        /// <remarks>
        /// The available space will be determined by the GapWidth of the CategoryAxis used by this series.
        /// </remarks>
        internal override double GetBarWidth()
        {
            return this.ColumnWidth;
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="rc">
        /// The rc. 
        /// </param>
        /// <param name="clippingRect">
        /// The clipping rect. 
        /// </param>
        /// <param name="rect">
        /// The rect. 
        /// </param>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="i">
        /// The i. 
        /// </param>
        protected override void DrawLabel(IRenderContext rc, OxyRect clippingRect, OxyRect rect, double value, int i)
        {
            var s = StringHelper.Format(
                this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[i]), value);
            ScreenPoint pt;
            VerticalTextAlign va;
            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top + this.LabelMargin);
                    va = VerticalTextAlign.Top;
                    break;
                case LabelPlacement.Middle:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Bottom + rect.Top) / 2);
                    va = VerticalTextAlign.Middle;
                    break;
                case LabelPlacement.Base:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Bottom - this.LabelMargin);
                    va = VerticalTextAlign.Bottom;
                    break;
                default: // outside
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top - this.LabelMargin);
                    va = VerticalTextAlign.Bottom;
                    break;
            }

            rc.DrawClippedText(
                clippingRect, 
                pt, 
                s, 
                this.ActualTextColor, 
                this.ActualFont, 
                this.ActualFontSize, 
                this.ActualFontWeight, 
                0, 
                HorizontalTextAlign.Center, 
                va);
        }

        /// <summary>
        /// Gets the actual width/height of the items of this series.
        /// </summary>
        /// <returns>
        /// The width or height. 
        /// </returns>
        /// <remarks>
        /// The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.
        /// </remarks>
        protected override double GetActualBarWidth()
        {
            var categoryAxis = this.GetCategoryAxis();
            return this.ColumnWidth / (1 + categoryAxis.GapWidth) / categoryAxis.MaxWidth;
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>
        /// The category axis. 
        /// </returns>
        protected override CategoryAxis GetCategoryAxis()
        {
            if (!(this.XAxis is CategoryAxis))
            {
                throw new Exception(
                    "A ColumnSeries requires a CategoryAxis on the x-axis. Use a BarSeries if you want horizontal bars.");
            }

            return this.XAxis as CategoryAxis;
        }

        /// <summary>
        /// Gets the rectangle for the specified values.
        /// </summary>
        /// <param name="baseValue">
        /// The base value of the bar 
        /// </param>
        /// <param name="topValue">
        /// The top value of the bar 
        /// </param>
        /// <param name="beginValue">
        /// The begin value of the bar 
        /// </param>
        /// <param name="endValue">
        /// The end value of the bar 
        /// </param>
        /// <returns>
        /// The rectangle. 
        /// </returns>
        protected override OxyRect GetRectangle(double baseValue, double topValue, double beginValue, double endValue)
        {
            return OxyRect.Create(this.Transform(beginValue, baseValue), this.Transform(endValue, topValue));
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>
        /// The value axis. 
        /// </returns>
        protected override Axis GetValueAxis()
        {
            return this.YAxis;
        }

        #endregion
    }
}