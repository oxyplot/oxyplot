// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Base class for BarSeries and ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Axes;

    /// <summary>
    /// Base class for BarSeries and ColumnSeries.
    /// </summary>
    public abstract class BarSeriesBase : CategorizedSeries, IStackableSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}";

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesBase" /> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.FillColor = OxyColors.Automatic;
            this.NegativeFillColor = OxyColors.Undefined;
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelMargin = 2;
            this.StackGroup = string.Empty;
        }

        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        /// <value>The base value.</value>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        public string ColorField { get; set; }

        /// <summary>
        /// Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualFillColor
        {
            get { return this.FillColor.GetActualColor(this.defaultFillColor); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        /// <summary>
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets label placements.
        /// </summary>
        public LabelPlacement LabelPlacement { get; set; }

        /// <summary>
        /// Gets or sets the color of the interior of the bars when the value is negative.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor NegativeFillColor { get; set; }

        /// <summary>
        /// Gets or sets the stack index indication to which stack the series belongs. Default is 0. Hence, all stacked series belong to the same stack.
        /// </summary>
        public string StackGroup { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        /// <summary>
        /// Gets or sets the valid items
        /// </summary>
        protected IList<BarItemBase> ValidItems { get; set; }

        /// <summary>
        /// Gets or sets the dictionary which stores the index-inversion for the valid items
        /// </summary>
        protected Dictionary<int, int> ValidItemsIndexInversion { get; set; }

        /// <summary>
        /// Gets or sets the actual rectangles for the bars.
        /// </summary>
        protected IList<OxyRect> ActualBarRectangles { get; set; }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.ActualBarRectangles == null || this.ValidItems == null)
            {
                return null;
            }

            var i = 0;
            foreach (var rectangle in this.ActualBarRectangles)
            {
                if (rectangle.Contains(point))
                {
                    // get the item corresponding to this bar/column rectangle
                    var item = this.ValidItems[i];
                    var categoryIndex = item.GetCategoryIndex(i);
                    var dp = new DataPoint(categoryIndex, this.ValidItems[i].Value);

                    // get the item that the bar/column is bound to, or the item from the Items collection
                    var boundItem = this.GetItem(this.ValidItemsIndexInversion[i]);

                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = dp,
                        Position = point,
                        Item = boundItem,
                        Index = i,
                        Text = this.GetTrackerText(item, boundItem, categoryIndex)
                    };
                }

                i++;
            }

            return null;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            this.ActualBarRectangles = new List<OxyRect>();

            if (this.ValidItems == null || this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();

            var actualBarWidth = this.GetActualBarWidth();
            var stackIndex = this.IsStacked ? categoryAxis.GetStackIndex(this.StackGroup) : 0;
            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];
                var categoryIndex = this.ValidItems[i].GetCategoryIndex(i);

                var value = item.Value;

                // Get base- and topValue
                var baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = categoryAxis.GetCurrentBaseValue(stackIndex, categoryIndex, value < 0);
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
                    categoryValue = categoryIndex - 0.5 + categoryAxis.GetCurrentBarOffset(categoryIndex);
                }

                if (this.IsStacked)
                {
                    categoryAxis.SetCurrentBaseValue(stackIndex, categoryIndex, value < 0, topValue);
                }

                var rect = this.GetRectangle(baseValue, topValue, categoryValue, categoryValue + actualBarWidth);
                this.ActualBarRectangles.Add(rect);

                this.RenderItem(rc, clippingRect, topValue, categoryValue, actualBarWidth, item, rect);

                if (this.LabelFormatString != null)
                {
                    this.RenderLabel(rc, clippingRect, rect, value, i);
                }

                if (!this.IsStacked)
                {
                    categoryAxis.IncreaseCurrentBarOffset(categoryIndex, actualBarWidth);
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableColor(this.ActualFillColor),
                this.StrokeColor,
                this.StrokeThickness);
        }

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">An axis which should be checked if used</param>
        /// <returns>True if the axis is in use.</returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.FillColor.IsAutomatic())
            {
                this.defaultFillColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            var valueAxis = this.GetValueAxis();
            if (valueAxis.IsVertical())
            {
                valueAxis.Include(this.MinY);
                valueAxis.Include(this.MaxY);
            }
            else
            {
                valueAxis.Include(this.MinX);
                valueAxis.Include(this.MaxX);
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
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
                var labels = this.GetCategoryAxis().ActualLabels;
                for (var i = 0; i < labels.Count; i++)
                {
                    int j = 0;
                    var values =
                        this.ValidItems.Where(item => item.GetCategoryIndex(j++) == i).Select(item => item.Value).Concat(new[] { 0d }).ToList();
                    var minTemp = values.Where(v => v <= 0).Sum();
                    var maxTemp = values.Where(v => v >= 0).Sum();

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
                var values = this.ValidItems.Select(item => item.Value).Concat(new[] { 0d }).ToList();
                minValue = values.Min();
                maxValue = values.Max();
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
        /// Updates the valid items
        /// </summary>
        protected internal override void UpdateValidData()
        {
            this.ValidItems = new List<BarItemBase>();
            this.ValidItemsIndexInversion = new Dictionary<int, int>();
            var categories = this.GetCategoryAxis().ActualLabels.Count;
            var valueAxis = this.GetValueAxis();

            int i = 0;
            foreach (var item in this.GetItems())
            {
                var barSeriesItem = item as BarItemBase;

                if (barSeriesItem != null && item.GetCategoryIndex(i) < categories
                    && valueAxis.IsValidValue(barSeriesItem.Value))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                    this.ValidItems.Add(barSeriesItem);
                }

                i++;
            }
        }

        /// <summary>
        /// Gets the rectangle for the specified values.
        /// </summary>
        /// <param name="baseValue">The base value of the bar</param>
        /// <param name="topValue">The top value of the bar</param>
        /// <param name="beginValue">The begin value of the bar</param>
        /// <param name="endValue">The end value of the bar</param>
        /// <returns>The rectangle.</returns>
        protected abstract OxyRect GetRectangle(double baseValue, double topValue, double beginValue, double endValue);

        /// <summary>
        /// Gets the tracker text for the specified item.
        /// </summary>
        /// <param name="barItem">The bar/column item.</param>
        /// <param name="item">The bound item.</param>
        /// <param name="categoryIndex">Category index of the item.</param>
        /// <returns>
        /// The tracker text.
        /// </returns>
        protected virtual string GetTrackerText(BarItemBase barItem, object item, int categoryIndex)
        {
            var categoryAxis = this.GetCategoryAxis();
            var valueAxis = this.GetValueAxis();

            var text = StringHelper.Format(
                this.ActualCulture, 
                this.TrackerFormatString,
                item,
                this.Title,
                categoryAxis.FormatValue(categoryIndex),
                valueAxis.GetValue(barItem.Value));
            return text;
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>The value axis.</returns>
        protected abstract Axis GetValueAxis();

        /// <summary>
        /// Checks if the specified value is valid.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns>True if the value is valid.</returns>
        protected virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
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
        protected virtual void RenderItem(
            IRenderContext rc,
            OxyRect clippingRect,
            double topValue,
            double categoryValue,
            double actualBarWidth,
            BarItemBase item,
            OxyRect rect)
        {
            // Get the color of the item
            var actualFillColor = item.Color;
            if (actualFillColor.IsAutomatic())
            {
                actualFillColor = this.ActualFillColor;
                if (item.Value < 0 && !this.NegativeFillColor.IsUndefined())
                {
                    actualFillColor = this.NegativeFillColor;
                }
            }

            rc.DrawClippedRectangleAsPolygon(clippingRect, rect, this.GetSelectableFillColor(actualFillColor), this.StrokeColor, this.StrokeThickness);
        }

        /// <summary>
        /// Renders the item label.
        /// </summary>
        /// <param name="rc">The render context</param>
        /// <param name="clippingRect">The clipping rectangle</param>
        /// <param name="rect">The rectangle of the item.</param>
        /// <param name="value">The value of the label.</param>
        /// <param name="index">The index of the bar item.</param>
        protected abstract void RenderLabel(
            IRenderContext rc, OxyRect clippingRect, OxyRect rect, double value, int index);
    }
}