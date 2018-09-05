// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for clustered or stacked bar charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for clustered or stacked bar charts.
    /// </summary>
    public class BarSeries : BarSeriesBase<BarItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeries" /> class.
        /// </summary>
        public BarSeries()
        {
            this.BarWidth = 1;
        }

        /// <summary>
        /// Gets or sets the width (height) of the bars.
        /// </summary>
        /// <value>The width of the bars.</value>
        public double BarWidth { get; set; }

        /// <summary>
        /// Gets or sets the width of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <value>The width of the bars.</value>
        /// <returns>The fractional width.</returns>
        /// <remarks>The available space will be determined by the GapWidth of the CategoryAxis used by this series.</remarks>
        internal override double GetBarWidth()
        {
            return this.BarWidth;
        }

        /// <summary>
        /// Gets the actual width/height of the items of this series.
        /// </summary>
        /// <returns>The width or height.</returns>
        /// <remarks>The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.</remarks>
        protected override double GetActualBarWidth()
        {
            var categoryAxis = this.GetCategoryAxis();
            return this.BarWidth / (1 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>The category axis.</returns>
        protected override CategoryAxis GetCategoryAxis()
        {
            if (!(this.YAxis is CategoryAxis))
            {
                throw new Exception(
                    "A BarSeries requires a CategoryAxis on the y-axis. Use a ColumnSeries if you want vertical bars.");
            }

            return this.YAxis as CategoryAxis;
        }

        /// <summary>
        /// Gets the rectangle for the specified values.
        /// </summary>
        /// <param name="baseValue">The base value of the bar</param>
        /// <param name="topValue">The top value of the bar</param>
        /// <param name="beginValue">The begin value of the bar</param>
        /// <param name="endValue">The end value of the bar</param>
        /// <returns>The rectangle.</returns>
        protected override OxyRect GetRectangle(double baseValue, double topValue, double beginValue, double endValue)
        {
            return new OxyRect(this.Transform(baseValue, beginValue), this.Transform(topValue, endValue));
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>The value axis.</returns>
        protected override Axis GetValueAxis()
        {
            return this.XAxis;
        }

        /// <summary>
        /// Renders the item label.
        /// </summary>
        /// <param name="rc">The render context</param>
        /// <param name="clippingRect">The clipping rectangle</param>
        /// <param name="rect">The rectangle of the item.</param>
        /// <param name="value">The value of the label.</param>
        /// <param name="index">The index of the bar item.</param>
        protected override void RenderLabel(IRenderContext rc, OxyRect clippingRect, OxyRect rect, double value, int index)
        {
            var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[index]), value);
            HorizontalAlignment ha;
            ScreenPoint pt;
            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    pt = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalAlignment.Right;
                    break;
                case LabelPlacement.Middle:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalAlignment.Center;
                    break;
                case LabelPlacement.Base:
                    pt = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalAlignment.Left;
                    break;
                default: // Outside
                    // Puts label left for negative series, right for positive
                    if (value < 0)
                    {
                        pt = new ScreenPoint(rect.Left - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                        ha = HorizontalAlignment.Right;
                    }
                    else
                    {
                        pt = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                        ha = HorizontalAlignment.Left;
                    }
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
                ha,
                VerticalAlignment.Middle);
        }

        /// <summary>
        /// Updates the <see cref="F:itemsSourceItems" /> from the <see cref="P:ItemsSource" /> and data fields.
        /// </summary>
        protected override void UpdateFromDataFields()
        {
            // Using reflection to add items by value and color (optional)
            var filler = new ListBuilder<BarItem>();
            filler.Add(this.ValueField, double.NaN);
            filler.Add(this.ColorField, OxyColors.Automatic);
            filler.Fill(this.ItemsSourceItems, this.ItemsSource, args => new BarItem(Convert.ToDouble(args[0])) { Color = (OxyColor)args[1] });
        }
    }
}
