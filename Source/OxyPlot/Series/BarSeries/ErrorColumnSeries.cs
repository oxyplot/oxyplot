// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorColumnSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The ErrorColumnSeries is used to create clustered or stacked column charts with an error value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The ErrorColumnSeries is used to create clustered or stacked column charts with an error value.
    /// </summary>
    public class ErrorColumnSeries : ColumnSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorColumnSeries"/> class.
        /// </summary>
        public ErrorColumnSeries()
        {
            this.TrackerFormatString = "{0}, {1}: {2}, Error: {3}";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="model">
        /// The model. 
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            this.ActualBarRectangles = new List<OxyRect>();

            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();

            var actualBarWidth = this.GetActualBarWidth();
            var stackIndex = this.IsStacked ? categoryAxis.StackIndexMapping[this.StackGroup] : 0;
            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = (ErrorColumnItem) this.ValidItems[i];
                var categoryIndex = this.ValidItems[i].GetCategoryIndex(i);

                var value = item.Value;

                // Get base- and topValue
                var baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = value < 0
                                    ? categoryAxis.NegativeBaseValues[stackIndex, categoryIndex]
                                    : categoryAxis.PositiveBaseValues[stackIndex, categoryIndex];
                }

                if (double.IsNaN(baseValue))
                {
                    baseValue = this.BaseValue;
                }

                var topValue = this.IsStacked ? baseValue + value : value;

                // Calculate offset
                double categoryValue;
                if (this.IsStacked)
                {
                    categoryValue = categoryAxis.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);
                }
                else
                {
                    categoryValue = categoryIndex - 0.5 + categoryAxis.BarOffset[categoryIndex];
                }

                var rect = this.GetRectangle(baseValue, topValue, categoryValue, categoryValue + actualBarWidth);

                if (this.IsStacked)
                {
                    if (value < 0)
                    {
                        categoryAxis.NegativeBaseValues[stackIndex, categoryIndex] = topValue;
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValues[stackIndex, categoryIndex] = topValue;
                    }
                }

                this.ActualBarRectangles.Add(rect);

                // Get Color
                var actualFillColor = item.Color;
                if (actualFillColor == null)
                {
                    actualFillColor = this.FillColor;
                    if (value < 0 && this.NegativeFillColor != null)
                    {
                        actualFillColor = this.NegativeFillColor;
                    }
                }

                rc.DrawClippedRectangleAsPolygon(
                    rect,
                    clippingRect,
                    this.GetSelectableFillColor(actualFillColor),
                    this.StrokeColor,
                    this.StrokeThickness);

                // Draw Error
                var lowerValue = topValue - item.ErrorValue;
                var upperValue = topValue + item.ErrorValue;
                var leftValue = categoryValue + (0.3 * actualBarWidth);
                var middleValue = categoryValue + (0.5 * actualBarWidth);
                var rightValue = categoryValue + (0.7 * actualBarWidth);

                var lowerErrorPoint = this.Transform(middleValue, lowerValue);
                var upperErrorPoint = this.Transform(middleValue, upperValue);
                rc.DrawClippedLine(
                    new List<ScreenPoint> { lowerErrorPoint, upperErrorPoint },
                    clippingRect,
                    0,
                    StrokeColor,
                    this.StrokeThickness,
                    LineStyle.Solid,
                    OxyPenLineJoin.Miter,
                    true);

                var lowerLeftErrorPoint = this.Transform(leftValue, lowerValue);
                var lowerRightErrorPoint = this.Transform(rightValue, lowerValue);
                rc.DrawClippedLine(
                   new List<ScreenPoint> { lowerLeftErrorPoint, lowerRightErrorPoint },
                   clippingRect,
                   0,
                   StrokeColor,
                   this.StrokeThickness,
                   LineStyle.Solid,
                   OxyPenLineJoin.Miter,
                   true);

                var upperLeftErrorPoint = this.Transform(leftValue, upperValue);
                var upperRightErrorPoint = this.Transform(rightValue, upperValue);
                rc.DrawClippedLine(
                   new List<ScreenPoint> { upperLeftErrorPoint, upperRightErrorPoint },
                   clippingRect,
                   0,
                   StrokeColor,
                   this.StrokeThickness,
                   LineStyle.Solid,
                   OxyPenLineJoin.Miter,
                   true);

                if (this.LabelFormatString != null)
                {
                    this.DrawLabel(rc, clippingRect, rect, value, i);
                }

                if (!this.IsStacked)
                {
                    categoryAxis.BarOffset[categoryIndex] += actualBarWidth;
                }
            }
        }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="interpolate">
        /// interpolate if set to <c>true</c> . 
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit. 
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.ActualBarRectangles == null)
            {
                return null;
            }

            var categoryAxis = this.GetCategoryAxis();

            var i = 0;
            foreach (var rectangle in this.ActualBarRectangles)
            {
                if (rectangle.Contains(point))
                {
                    var categoryIndex = this.ValidItems[i].GetCategoryIndex(i);

                    var dp = new DataPoint(categoryIndex, this.ValidItems[i].Value);
                    var item = this.GetItem(this.ValidItemsIndexInversion[i]);

                    var text = StringHelper.Format(
                        this.ActualCulture,
                        this.TrackerFormatString,
                        item,
                        this.Title,
                        categoryAxis.FormatValueForTracker(categoryIndex),
                        this.ValidItems[i].Value,
                        ((ErrorColumnItem)this.ValidItems[i]).ErrorValue);
                    return new TrackerHitResult(this, dp, point, item, i, text);
                }

                i++;
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the maximum/minimum value on the value axis from the bar values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (this.ValidItems == null || this.ValidItems.Count == 0)
            {
                return;
            }

            var categoryAxis = this.GetCategoryAxis();

            double minValue = double.MaxValue, maxValue = double.MinValue;
            if (this.IsStacked)
            {
                var labels = this.GetCategoryAxis().Labels;
                for (var i = 0; i < labels.Count; i++)
                {
                    // var label = labels[i];
                    int j = 0;
                    var items = this.ValidItems.Where(item => item.GetCategoryIndex(j++) == i).ToList();
                    var values = items.Select(item => item.Value).Concat(new[] { 0d }).ToList();
                    var minTemp = values.Where(v => v <= 0).Sum();
                    var maxTemp = values.Where(v => v >= 0).Sum() + ((ErrorColumnItem)items.Last()).ErrorValue;

                    int stackIndex = categoryAxis.StackIndexMapping[this.StackGroup];
                    var stackedMinValue = categoryAxis.MinValue[stackIndex, i];
                    if (!double.IsNaN(stackedMinValue))
                    {
                        minTemp += stackedMinValue;
                    }

                    categoryAxis.MinValue[stackIndex, i] = minTemp;

                    var stackedMaxValue = categoryAxis.MaxValue[stackIndex, i];
                    if (!double.IsNaN(stackedMaxValue))
                    {
                        maxTemp += stackedMaxValue;
                    }

                    categoryAxis.MaxValue[stackIndex, i] = maxTemp;

                    minValue = Math.Min(minValue, minTemp + this.BaseValue);
                    maxValue = Math.Max(maxValue, maxTemp + this.BaseValue);
                }
            }
            else
            {
                var valuesMin = this.ValidItems.
                    Select(item => item.Value - ((ErrorColumnItem)item).ErrorValue).
                    Concat(new[] { 0d }).ToList();
                var valuesMax = this.ValidItems.
                    Select(item => item.Value + ((ErrorColumnItem)item).ErrorValue).
                    Concat(new[] { 0d }).ToList();
                minValue = valuesMin.Min();
                maxValue = valuesMax.Max();
                if (this.BaseValue < minValue)
                {
                    minValue = this.BaseValue;
                }

                if (this.BaseValue > maxValue)
                {
                    maxValue = this.BaseValue;
                }
            }

            var valueAxis = this.GetValueAxis();
            if (valueAxis.IsVertical())
            {
                this.MinY = minValue;
                this.MaxY = maxValue;
            }
            else
            {
                this.MinX = minValue;
                this.MaxX = maxValue;
            }
        }

        #endregion
    }
}
