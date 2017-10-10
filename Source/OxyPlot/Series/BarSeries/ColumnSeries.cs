// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for clustered or stacked column charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for clustered or stacked column charts.
    /// </summary>
    public class ColumnSeries : BarSeriesBase<ColumnItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSeries" /> class.
        /// </summary>
        public ColumnSeries()
        {
            this.ColumnWidth = 1;
        }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        public double ColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets the width/height of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <value>The width of the bars.</value>
        /// <returns>The fractional width.</returns>
        /// <remarks>The available space will be determined by the GapWidth of the CategoryAxis used by this series.</remarks>
        internal override double GetBarWidth()
        {
            return this.ColumnWidth;
        }

        /// <summary>
        /// Gets the actual width/height of the items of this series.
        /// </summary>
        /// <returns>The width or height.</returns>
        /// <remarks>The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.</remarks>
        protected override double GetActualBarWidth()
        {
            var categoryAxis = this.GetCategoryAxis();
            return this.ColumnWidth / (1 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>The category axis.</returns>
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
        /// <param name="baseValue">The base value of the bar</param>
        /// <param name="topValue">The top value of the bar</param>
        /// <param name="beginValue">The begin value of the bar</param>
        /// <param name="endValue">The end value of the bar</param>
        /// <returns>The rectangle.</returns>
        protected override OxyRect GetRectangle(double baseValue, double topValue, double beginValue, double endValue)
        {
            return new OxyRect(this.Transform(beginValue, baseValue), this.Transform(endValue, topValue));
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>The value axis.</returns>
        protected override Axis GetValueAxis()
        {
            return this.YAxis;
        }

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="rect">The column rectangle.</param>
        /// <param name="value">The value.</param>
        /// <param name="i">The index.</param>
        protected override void RenderLabel(IRenderContext rc, OxyRect clippingRect, OxyRect rect, double value, int i)
        {
            var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(this.ValidItemsIndexInversion[i]), value);
            ScreenPoint pt;
            VerticalAlignment va;
            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top + this.LabelMargin);
                    va = VerticalAlignment.Top;
                    break;
                case LabelPlacement.Middle:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Bottom + rect.Top) / 2);
                    va = VerticalAlignment.Middle;
                    break;
                case LabelPlacement.Base:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Bottom - this.LabelMargin);
                    va = VerticalAlignment.Bottom;
                    break;
                default: // outside
                    // Puts label below for negative series, above for positive
                    if (value < 0)
                    {
                        pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Bottom + this.LabelMargin);
                        va = VerticalAlignment.Top;
                    }
                    else
                    {
                        pt = new ScreenPoint((rect.Left + rect.Right) / 2, rect.Top - this.LabelMargin);
                        va = VerticalAlignment.Bottom;
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
                HorizontalAlignment.Center,
                va);
        }

        /// <summary>
        /// Updates the <see cref="F:itemsSourceItems" /> from the <see cref="P:ItemsSource" /> and data fields.
        /// </summary>
        protected override void UpdateFromDataFields()
        {
            // Using reflection to add items by value and color (optional)
            var filler = new ListBuilder<ColumnItem>();
            filler.Add(this.ValueField, double.NaN);
            filler.Add(this.ColorField, OxyColors.Automatic);
            filler.Fill(this.ItemsSourceItems, this.ItemsSource, args => new ColumnItem(Convert.ToDouble(args[0])) { Color = (OxyColor)args[1] });
        }
    }
}