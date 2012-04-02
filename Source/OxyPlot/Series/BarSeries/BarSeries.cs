// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The BarSeries is used to create clustered or stacked bar charts.
    /// </summary>
    /// <remarks>
    /// A bar chart or bar graph is a chart with rectangular horizontal bars with lengths proportional to the values that they represent. 
    /// http://en.wikipedia.org/wiki/Bar_chart The BarSeries requires a CategoryAxis. 
    /// The Values collection must contain the same number of elements as the number of categories in the CategoryAxis. 
    /// You can define a ItemsSource and a ValueField, or add the Values manually. 
    /// Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </remarks>
    public class BarSeries : BarSeriesBase
    {
        #region Public Methods

        /// <summary>
        /// Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="model">
        /// The model. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Values.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();
            var valueAxis = this.GetValueAxis();

            double dx = categoryAxis.BarOffset - this.BarWidth * 0.5;

            int i = 0;
            this.ActualBarRectangles = new List<OxyRect>();

            foreach (double v in this.Values)
            {
                if (!this.IsValidPoint(v, valueAxis))
                {
                    continue;
                }

                double baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = v < 0 ? categoryAxis.NegativeBaseValue[i] : categoryAxis.PositiveBaseValue[i];
                }

                if (double.IsNaN(baseValue))
                {
                    baseValue = this.BaseValue;
                }

                double topValue = this.IsStacked ? baseValue + v : v;
                int numberOfSeries = this.IsStacked ? 1 : categoryAxis.AttachedSeriesCount;

                var p0 = this.XAxis.Transform(baseValue, i + dx, this.YAxis);
                var p1 = this.XAxis.Transform(topValue, i + dx + this.BarWidth / numberOfSeries, this.YAxis);

                p0.X = (int)p0.X;
                p0.Y = (int)p0.Y;
                p1.X = (int)p1.X;
                p1.Y = (int)p1.Y;

                if ((v >= 0 && !double.IsNaN(categoryAxis.PositiveBaseValueScreen[i]))
                    || (v < 0 && !double.IsNaN(categoryAxis.NegativeBaseValueScreen[i])))
                {
                    if (this.IsStacked)
                    {
                        p0.X = v < 0 ? categoryAxis.NegativeBaseValueScreen[i] : categoryAxis.PositiveBaseValueScreen[i];
                    }
                    else
                    {
                        p0.Y = categoryAxis.PositiveBaseValueScreen[i];
                    }
                }

                var rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                if (this.IsStacked)
                {
                    if (v < 0)
                    {
                        categoryAxis.NegativeBaseValue[i] = topValue;
                        categoryAxis.NegativeBaseValueScreen[i] = p1.X;
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValue[i] = topValue;
                        categoryAxis.PositiveBaseValueScreen[i] = p1.X;
                    }
                }
                else
                {
                    categoryAxis.PositiveBaseValueScreen[i] = p1.Y;
                }

                this.ActualBarRectangles.Add(rect);

                var actualFillColor = this.FillColor;
                if (v < 0 && this.NegativeFillColor != null)
                {
                    actualFillColor = this.NegativeFillColor;
                }

                rc.DrawClippedRectangleAsPolygon(rect, clippingRect, actualFillColor, this.StrokeColor, this.StrokeThickness);

                if (this.LabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(i), v);
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

                i++;
            }

            if (!this.IsStacked)
            {
                categoryAxis.BarOffset += this.BarWidth / categoryAxis.AttachedSeriesCount;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>
        /// The category axis. 
        /// </returns>
        protected override CategoryAxis GetCategoryAxis()
        {
            if (!(this.YAxis is CategoryAxis))
            {
                throw new Exception("The y-axis should be a CategoryAxis.");
            }

            return this.YAxis as CategoryAxis;
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>
        /// The value axis. 
        /// </returns>
        protected override Axis GetValueAxis()
        {
            return this.XAxis;
        }

        #endregion
    }
}