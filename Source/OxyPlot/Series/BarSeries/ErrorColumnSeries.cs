// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorColumnSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for clustered or stacked column charts with an error value.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a series for clustered or stacked column charts with an error value.
    /// </summary>
    public class ErrorColumnSeries : ColumnSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}, Error: {Error:0.###}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorColumnSeries" /> class.
        /// </summary>
        public ErrorColumnSeries()
        {
            this.ErrorWidth = 0.4;
            this.ErrorStrokeThickness = 1;
            this.TrackerFormatString = DefaultTrackerFormatString;
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the error line.
        /// </summary>
        /// <value>The stroke thickness of the error line.</value>
        public double ErrorStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the width of the error end lines.
        /// </summary>
        /// <value>The width of the error end lines.</value>
        public double ErrorWidth { get; set; }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            //// Todo: refactor (lots of duplicate code here)
            if (this.ValidItems == null || this.ValidItems.Count == 0)
            {
                return;
            }

            var categoryAxis = this.GetCategoryAxis();

            double minValue = double.MaxValue, maxValue = double.MinValue;
            if (this.IsStacked)
            {
                var labels = this.GetCategoryAxis().ActualLabels;
                for (var i = 0; i < labels.Count; i++)
                {
                    int j = 0;
                    var items = this.ValidItems.Where(item => item.GetCategoryIndex(j++) == i).ToList();
                    var values = items.Select(item => item.Value).Concat(new[] { 0d }).ToList();
                    var minTemp = values.Where(v => v <= 0).Sum();
                    var maxTemp = values.Where(v => v >= 0).Sum() + ((ErrorColumnItem)items.Last()).Error;

                    int stackIndex = categoryAxis.GetStackIndex(this.StackGroup);
                    var stackedMinValue = categoryAxis.GetCurrentMinValue(stackIndex, i);
                    if (!double.IsNaN(stackedMinValue))
                    {
                        minTemp += stackedMinValue;
                    }

                    categoryAxis.SetCurrentMinValue(stackIndex, i, minTemp);

                    var stackedMaxValue = categoryAxis.GetCurrentMaxValue(stackIndex, i);
                    if (!double.IsNaN(stackedMaxValue))
                    {
                        maxTemp += stackedMaxValue;
                    }

                    categoryAxis.SetCurrentMaxValue(stackIndex, i, maxTemp);

                    minValue = Math.Min(minValue, minTemp + this.BaseValue);
                    maxValue = Math.Max(maxValue, maxTemp + this.BaseValue);
                }
            }
            else
            {
                var valuesMin = this.ValidItems.Select(item => item.Value - ((ErrorColumnItem)item).Error).Concat(new[] { 0d }).ToList();
                var valuesMax = this.ValidItems.Select(item => item.Value + ((ErrorColumnItem)item).Error).Concat(new[] { 0d }).ToList();
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

        /// <summary>
        /// Renders the bar/column item.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="topValue">The end value of the bar.</param>
        /// <param name="categoryValue">The category value.</param>
        /// <param name="actualBarWidth">The actual width of the bar.</param>
        /// <param name="item">The item.</param>
        /// <param name="rect">The rectangle of the bar.</param>
        protected override void RenderItem(
            IRenderContext rc,
            OxyRect clippingRect,
            double topValue,
            double categoryValue,
            double actualBarWidth,
            BarItemBase item,
            OxyRect rect)
        {
            base.RenderItem(rc, clippingRect, topValue, categoryValue, actualBarWidth, item, rect);

            var errorItem = item as ErrorColumnItem;
            if (errorItem == null)
            {
                return;
            }

            // Render the error
            var lowerValue = topValue - errorItem.Error;
            var upperValue = topValue + errorItem.Error;
            var left = 0.5 - (this.ErrorWidth / 2);
            var right = 0.5 + (this.ErrorWidth / 2);
            var leftValue = categoryValue + (left * actualBarWidth);
            var middleValue = categoryValue + (0.5 * actualBarWidth);
            var rightValue = categoryValue + (right * actualBarWidth);

            var lowerErrorPoint = this.Transform(middleValue, lowerValue);
            var upperErrorPoint = this.Transform(middleValue, upperValue);

            rc.DrawClippedLine(
                clippingRect,
                new List<ScreenPoint> { lowerErrorPoint, upperErrorPoint },
                0,
                this.StrokeColor,
                this.ErrorStrokeThickness,
                null,
                LineJoin.Miter,
                true);

            if (this.ErrorWidth > 0)
            {
                var lowerLeftErrorPoint = this.Transform(leftValue, lowerValue);
                var lowerRightErrorPoint = this.Transform(rightValue, lowerValue);
                rc.DrawClippedLine(
                    clippingRect,
                    new List<ScreenPoint> { lowerLeftErrorPoint, lowerRightErrorPoint },
                    0,
                    this.StrokeColor,
                    this.ErrorStrokeThickness,
                    null,
                    LineJoin.Miter,
                    true);

                var upperLeftErrorPoint = this.Transform(leftValue, upperValue);
                var upperRightErrorPoint = this.Transform(rightValue, upperValue);
                rc.DrawClippedLine(
                    clippingRect,
                    new List<ScreenPoint> { upperLeftErrorPoint, upperRightErrorPoint },
                    0,
                    this.StrokeColor,
                    this.ErrorStrokeThickness,
                    null,
                    LineJoin.Miter,
                    true);
            }
        }
    }
}