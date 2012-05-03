// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    #region

    using System;

    #endregion

    /// <summary>
    ///   The BarSeries is used to create clustered or stacked bar charts.
    /// </summary>
    /// <remarks>
    ///   A bar chart or bar graph is a chart with rectangular horizontal bars with lengths proportional to the values that they represent. http://en.wikipedia.org/wiki/Bar_chart The BarSeries requires a CategoryAxis. The Values collection must contain the same number of elements as the number of categories in the CategoryAxis. You can define a ItemsSource and a ValueField, or add the Values manually. Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </remarks>
    public class BarSeries : BarSeriesBase
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
            HorizontalTextAlign ha;
            ScreenPoint pt;
            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    pt = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalTextAlign.Right;
                    break;
                case LabelPlacement.Middle:
                    pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalTextAlign.Center;
                    break;
                case LabelPlacement.Base:
                    pt = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalTextAlign.Left;
                    break;
                default: // Outside
                    pt = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                    ha = HorizontalTextAlign.Left;
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
                VerticalTextAlign.Middle);
        }

        /// <summary>
        ///   Gets the category axis.
        /// </summary>
        /// <returns> The category axis. </returns>
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
            p0 = this.Transform(baseValue, beginValue);
            p1 = this.Transform(topValue, endValue);
        }

        /// <summary>
        ///   Gets the value axis.
        /// </summary>
        /// <returns> The value axis. </returns>
        protected override Axis GetValueAxis()
        {
            return this.XAxis;
        }

        #endregion
    }
}