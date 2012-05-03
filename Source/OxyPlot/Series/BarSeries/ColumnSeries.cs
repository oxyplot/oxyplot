// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    #region

    using System;

    #endregion

    /// <summary>
    ///   The ColumnSeries is used to create clustered or stacked column charts.
    /// </summary>
    /// <remarks>
    ///   A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. The bars can be plotted vertically or horizontally. http://en.wikipedia.org/wiki/Bar_chart The BarSeries requires a CategoryAxis. The Values collection must contain the same number of elements as the number of categories in the CategoryAxis. You can define a ItemsSource and a ValueField, or add the Values manually. Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </remarks>
    public class ColumnSeries : BarSeriesBase
    {
        #region Methods

        /// <summary>
        ///   Draw the Bar label
        /// </summary>
        /// <param name="rc"> The render context </param>
        /// <param name="clippingRect"> The clipping rectangle </param>
        /// <param name="rect"> The OxyRectangle </param>
        /// <param name="value"> The value of the label </param>
        /// <param name="i"> The index of the bar item </param>
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
        ///   Gets the category axis.
        /// </summary>
        /// <returns> The category axis. </returns>
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
        ///   Get the left-base and right-top point
        /// </summary>
        /// <param name="baseValue"> The base value of the bar </param>
        /// <param name="topValue"> The top value of the bar </param>
        /// <param name="beginValue"> The begin value of the bar </param>
        /// <param name="endValue"> The end value of the bar </param>
        /// <param name="p0"> The left-base point of the bar </param>
        /// <param name="p1"> The right-top point of the bar </param>
        protected override void GetPoints(
            double baseValue,
            double topValue,
            double beginValue,
            double endValue,
            out ScreenPoint p0,
            out ScreenPoint p1)
        {
            p0 = this.Transform(beginValue, baseValue);
            p1 = this.Transform(endValue, topValue);
        }

        /// <summary>
        ///   Gets the value axis.
        /// </summary>
        /// <returns> The value axis. </returns>
        protected override Axis GetValueAxis()
        {
            return this.YAxis;
        }

        #endregion
    }
}