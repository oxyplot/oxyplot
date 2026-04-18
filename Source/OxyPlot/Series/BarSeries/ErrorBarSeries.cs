// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBarSeries.cs" company="OxyPlot">
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
    public class ErrorBarSeries : BarSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}, Error: {Error:0.###}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSeries" /> class.
        /// </summary>
        public ErrorBarSeries()
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
            if (this.ValidItems.Count == 0)
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
                    var maxTemp = values.Where(v => v >= 0).Sum() + ((ErrorBarItem)items.Last()).Error;

                    int stackIndex = this.Manager.GetStackIndex(this.StackGroup);
                    var stackedMinValue = this.Manager.GetCurrentMinValue(stackIndex, i);
                    if (!double.IsNaN(stackedMinValue))
                    {
                        minTemp += stackedMinValue;
                    }

                    this.Manager.SetCurrentMinValue(stackIndex, i, minTemp);

                    var stackedMaxValue = this.Manager.GetCurrentMaxValue(stackIndex, i);
                    if (!this.OverlapsStack && !double.IsNaN(stackedMaxValue))
                    {
                        maxTemp += stackedMaxValue;
                    }

                    this.Manager.SetCurrentMaxValue(stackIndex, i, maxTemp);

                    minValue = Math.Min(minValue, minTemp + this.BaseValue);
                    maxValue = Math.Max(maxValue, maxTemp + this.BaseValue);
                }
            }
            else
            {
                var valuesMin = this.ValidItems.Select(item => item.Value - ((ErrorBarItem)item).Error).Concat(new[] { 0d }).ToList();
                var valuesMax = this.ValidItems.Select(item => item.Value + ((ErrorBarItem)item).Error).Concat(new[] { 0d }).ToList();
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

            this.MinX = minValue;
            this.MaxX = maxValue;
        }
        private ScreenPoint GetScreenPoint(ScreenPoint screenPoint, double spacing)
        {

            if (!this.IsTransposed())
            {
                return new ScreenPoint(screenPoint.X, screenPoint.Y + spacing);
            }
            else
            {
                return new ScreenPoint(screenPoint.X - spacing, screenPoint.Y);
            }
        }
        /// <inheritdoc/>
        protected override void RenderItem(
            IRenderContext rc,
            double barValue,
            double categoryValue,
            double actualBarWidth,
            BarItem item,
            OxyRect rect)
        {

            ErrorBarItem errorItem = item as ErrorBarItem;
            if (errorItem == null)
            {
                base.RenderItem(rc, barValue, categoryValue, actualBarWidth, item, rect);
                return;
            }
            else
            {
                OxyRect errRect = new OxyRect(rect.Left, rect.Top + errorItem.Spacing, rect.Width, rect.Height);
                if (this.IsTransposed())
                {

                    errRect = new OxyRect(rect.Left - errorItem.Spacing, rect.Top, rect.Width, rect.Height);
                }
                base.RenderItem(rc, barValue, categoryValue, actualBarWidth, item, errRect);
            }

            // Render the error
            var errorStart = barValue - errorItem.Error;
            var errorEnd = barValue + errorItem.Error;
            var start = 0.5 - (this.ErrorWidth / 2);
            var end = 0.5 + (this.ErrorWidth / 2);
            var categoryStart = categoryValue + (start * actualBarWidth);
            var categoryMiddle = categoryValue + (0.5 * actualBarWidth);
            var categoryEnd = categoryValue + (end * actualBarWidth);

            var lowerErrorPoint = this.GetScreenPoint(this.Transform(errorStart, categoryMiddle), errorItem.Spacing);
            var upperErrorPoint = this.GetScreenPoint(this.Transform(errorEnd, categoryMiddle), errorItem.Spacing);

            rc.DrawLine(
                new List<ScreenPoint> { lowerErrorPoint, upperErrorPoint },
                this.StrokeColor,
                this.ErrorStrokeThickness,
                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                null,
                LineJoin.Miter);

            if (this.ErrorWidth > 0)
            {
                var lowerLeftErrorPoint = this.GetScreenPoint(this.Transform(errorStart, categoryStart), errorItem.Spacing);
                var lowerRightErrorPoint = this.GetScreenPoint(this.Transform(errorStart, categoryEnd), errorItem.Spacing);
                rc.DrawLine(
                    new List<ScreenPoint> { lowerLeftErrorPoint, lowerRightErrorPoint },
                    this.StrokeColor,
                    this.ErrorStrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                    null,
                    LineJoin.Miter);

                var upperLeftErrorPoint = this.GetScreenPoint(this.Transform(errorEnd, categoryStart), errorItem.Spacing);
                var upperRightErrorPoint = this.GetScreenPoint(this.Transform(errorEnd, categoryEnd), errorItem.Spacing);
                rc.DrawLine(
                    new List<ScreenPoint> { upperLeftErrorPoint, upperRightErrorPoint },
                    this.StrokeColor,
                    this.ErrorStrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                    null,
                    LineJoin.Miter);
            }
            ErrorBarItem errorBarItem = item as ErrorBarItem;
            if (errorBarItem != null)
            {
                if (errorBarItem.IsMarkerVisible)
                {
                    double x = upperErrorPoint.X + errorBarItem.MarkerOffset.X;
                    double y = upperErrorPoint.Y + errorBarItem.MarkerOffset.Y;
                    if (this.IsTransposed())
                    {
                        x = upperErrorPoint.X + errorBarItem.MarkerOffset.Y;
                        y = upperErrorPoint.Y - errorBarItem.MarkerOffset.X;
                    }
                    ScreenPoint screenPoint = new ScreenPoint(x, y);
                    rc.DrawMarker(screenPoint,
                        errorBarItem.MarkerType,
                        errorBarItem.CustomOutline,
                        errorBarItem.MarkerSize,
                        errorBarItem.MarkerColor,
                        errorBarItem.MarkerStrokeColor,
                        errorBarItem.MarkerStrokeThickness,
                        this.EdgeRenderingMode);
                }
            }
        }
    }
}
