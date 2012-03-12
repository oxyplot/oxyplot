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
    /// The ColumnSeries is used to create clustered or stacked column charts.
    /// </summary>
    /// <remarks>
    /// A bar chart or bar graph is a chart with rectangular bars with lengths proportional to the values that they represent. 
    /// The bars can be plotted vertically or horizontally. 
    /// http://en.wikipedia.org/wiki/Bar_chart 
    /// The BarSeries requires a CategoryAxis. 
    /// The Values collection must contain the same number of elements as the number of categories in the CategoryAxis. 
    /// You can define a ItemsSource and a ValueField, or add the Values manually. 
    /// Use stacked bar charts with caution... http://lilt.ilstu.edu/gmklass/pos138/datadisplay/badchart.htm
    /// </remarks>
    public class ColumnSeries : BarSeriesBase
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

            OxyRect clippingRect = this.GetClippingRect();

            CategoryAxis categoryAxis = this.XAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category x-axis defined.");
            }

            var valueAxis = this.YAxis;
            if (valueAxis == null)
            {
                throw new InvalidOperationException("No value y-axis defined.");
            }

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

                ScreenPoint p0 = this.XAxis.Transform(i + dx, baseValue, this.YAxis);
                ScreenPoint p1 = this.XAxis.Transform(i + dx + this.BarWidth / numberOfSeries, topValue, this.YAxis);

                p0.X = (int)p0.X;
                p0.Y = (int)p0.Y;
                p1.X = (int)p1.X;
                p1.Y = (int)p1.Y;

                if ((v >= 0 && !double.IsNaN(categoryAxis.PositiveBaseValueScreen[i]))
                    || (v < 0 && !double.IsNaN(categoryAxis.NegativeBaseValueScreen[i])))
                {
                    if (this.IsStacked)
                    {
                        p0.Y = v < 0
                                   ? categoryAxis.NegativeBaseValueScreen[i]
                                   : categoryAxis.PositiveBaseValueScreen[i];
                    }
                    else
                    {
                        p0.X = categoryAxis.PositiveBaseValueScreen[i];
                    }
                }

                var rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);
                if (this.IsStacked)
                {
                    if (v < 0)
                    {
                        categoryAxis.NegativeBaseValue[i] = topValue;
                        categoryAxis.NegativeBaseValueScreen[i] = p1.Y;
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValue[i] = topValue;
                        categoryAxis.PositiveBaseValueScreen[i] = p1.Y;
                    }
                }
                else
                {
                    categoryAxis.PositiveBaseValueScreen[i] = p1.X;
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
                        this.LabelColor ?? model.TextColor,
                        model.ActualLegendFont,
                        model.LegendFontSize,
                        FontWeights.Normal,
                        0,
                        HorizontalTextAlign.Center,
                        va);
                }

                i++;
            }

            if (!this.IsStacked)
            {
                categoryAxis.BarOffset += this.BarWidth / categoryAxis.AttachedSeriesCount;
            }
        }
        #endregion

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
                throw new Exception("The x-axis should be a CategoryAxis.");
            }

            return this.XAxis as CategoryAxis;
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
    }
}