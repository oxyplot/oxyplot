// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    ///   Base class that provides common properties and methods for the BarSeries and ColumnSeries.
    /// </summary>
    public abstract class BarSeriesBase : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="BarSeriesBase" /> class. Initializes a new instance of the <see
        ///    cref="BarSeriesBase" /> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.Items = new List<BarItem>();
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.BarWidth = 1;
            this.TrackerFormatString = "{0}, {1}: {2}";
            this.LabelMargin = 2;
            this.StackIndex = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the width of the bars (as a fraction of the available width).
        /// </summary>
        /// <value> The width of the bars. </value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets or sets the base value.
        /// </summary>
        /// <value> The base value. </value>
        public double BaseValue { get; set; }

        /// <summary>
        ///   Gets or sets the color field.
        /// </summary>
        public string ColorField { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this bar series is stacked.
        /// </summary>
        public bool IsStacked { get; set; }

        /// <summary>
        ///   Gets or sets the items.
        /// </summary>
        /// <value> The values. </value>
        public IList<BarItem> Items { get; set; }

        /// <summary>
        ///   Gets or sets the label field.
        /// </summary>
        public string LabelField { get; set; }

        /// <summary>
        ///   Gets or sets the label format string.
        /// </summary>
        /// <value> The label format string. </value>
        public string LabelFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        ///   Gets or sets label placements.
        /// </summary>
        public LabelPlacement LabelPlacement { get; set; }

        /// <summary>
        ///   Gets or sets the color of the interior of the bars when the value is negative.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor NegativeFillColor { get; set; }

        /// <summary>
        ///   Gets or sets the stack index indication to which stack the series belongs. Default is 0. Hence, all stacked series belong to the same stack.
        /// </summary>
        public int StackIndex { get; set; }

        /// <summary>
        ///   Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value> The color of the stroke. </value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value> The stroke thickness. </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the value field.
        /// </summary>
        public string ValueField { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the valid items
        /// </summary>
        internal IList<BarItem> ValidItems { get; set; }

        /// <summary>
        ///   Gets or sets the dictionary which stores the index-inversion for the valid items
        /// </summary>
        internal Dictionary<int, int> ValidItemsIndexInversion { get; set; }

        /// <summary>
        ///   Gets or sets the actual rectangles for the bars.
        /// </summary>
        protected IList<OxyRect> ActualBarRectangles { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Gets the nearest point.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="interpolate"> interpolate if set to <c>true</c> . </param>
        /// <returns> A TrackerHitResult for the current hit. </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var categoryAxis = this.GetCategoryAxis();

            var i = 0;
            foreach (var r in this.ActualBarRectangles)
            {
                if (point.X >= r.Left && point.X <= r.Right && point.Y >= r.Top && point.Y <= r.Bottom)
                {
                    var labelId = categoryAxis.Labels.IndexOf(this.ValidItems[i].Label);

                    var sp = point;
                    var dp = new DataPoint(labelId, this.ValidItems[i].Value);
                    var item = this.GetItem(this.ValidItemsIndexInversion[i]);

                    var text = StringHelper.Format(
                        this.ActualCulture,
                        this.TrackerFormatString,
                        item,
                        this.Title,
                        categoryAxis.FormatValueForTracker(labelId),
                        this.ValidItems[i].Value);
                    return new TrackerHitResult(this, dp, sp, item, i, text);
                }

                i++;
            }

            return null;
        }

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name="rc"> The rendering context. </param>
        /// <param name="model"> The model. </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();

            this.ActualBarRectangles = new List<OxyRect>();

            var actualBarWidth = this.BarWidth * categoryAxis.CategoryWidth / categoryAxis.MaxWidth;
            var stackRank = this.IsStacked ? categoryAxis.StackIndexMapping[this.StackIndex] : 0;
            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];
                var label = item.Label;
                var labelId = categoryAxis.Labels.IndexOf(label);
                var value = item.Value;

                // Get base- and topValue
                var baseValue = double.NaN;
                if (this.IsStacked)
                {
                    baseValue = value < 0
                                    ? categoryAxis.NegativeBaseValues[stackRank, labelId]
                                    : categoryAxis.PositiveBaseValues[stackRank, labelId];
                }

                if (double.IsNaN(baseValue))
                {
                    baseValue = this.BaseValue;
                }

                var topValue = this.IsStacked ? baseValue + value : value;

                // Calculate offset
                double offset;
                if (this.IsStacked)
                {
                    var offsetBegin = categoryAxis.StackedBarOffset[stackRank, labelId];
                    var offsetEnd = categoryAxis.StackedBarOffset[stackRank + 1, labelId];
                    offset = (offsetEnd + offsetBegin - actualBarWidth) * 0.5;
                }
                else
                {
                    offset = categoryAxis.BarOffset[labelId];
                }

                ScreenPoint p0, p1;
                this.GetPoints(
                    baseValue, topValue, labelId - 0.5 + offset, labelId - 0.5 + offset + actualBarWidth, out p0, out p1);

                p0.X = (int)p0.X;
                p0.Y = (int)p0.Y;
                p1.X = (int)p1.X;
                p1.Y = (int)p1.Y;

                var rect = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);

                if (this.IsStacked)
                {
                    if (value < 0)
                    {
                        categoryAxis.NegativeBaseValues[stackRank, labelId] = topValue;
                    }
                    else
                    {
                        categoryAxis.PositiveBaseValues[stackRank, labelId] = topValue;
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

                if (this.LabelFormatString != null)
                {
                    this.DrawLabel(rc, clippingRect, rect, value, i);
                }

                if (!this.IsStacked)
                {
                    categoryAxis.BarOffset[labelId] += actualBarWidth;
                }
            }
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc"> The rendering context. </param>
        /// <param name="legendBox"> The legend rectangle. </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableColor(this.FillColor),
                this.StrokeColor,
                this.StrokeThickness);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis"> An axis which should be checked if used </param>
        /// <returns> True if the axis is in use. </returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        ///   The set default values.
        /// </summary>
        /// <param name="model"> The model. </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.FillColor == null)
            {
                this.FillColor = model.GetDefaultColor();
            }
        }

        /// <summary>
        ///   The update axis max min.
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
        ///   The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource != null)
            {
                var dest = new List<BarItem>();

                // Using reflection to add points
                var filler = new ListFiller<BarItem>();
                filler.Add(this.LabelField, (item, value) => item.Label = Convert.ToString(value));
                filler.Add(this.ValueField, (item, value) => item.Value = Convert.ToDouble(value));
                filler.Add(this.ColorField, (item, value) => item.Color = (OxyColor)value);
                filler.Fill(dest, this.ItemsSource);
                this.Items = dest;
            }
        }

        /// <summary>
        ///   Updates the maximum/minimum value on the value axis from the bar values.
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
                    var label = labels[i];
                    var values =
                        this.ValidItems.Where(item => item.Label == label).Select(item => item.Value).Concat(
                            new[] { 0d }).ToList();
                    var minTemp = values.Where(v => v <= 0).Sum();
                    var maxTemp = values.Where(v => v >= 0).Sum();

                    var stackedMinValue = categoryAxis.MinValue[categoryAxis.StackIndexMapping[this.StackIndex], i];
                    if (!double.IsNaN(stackedMinValue))
                    {
                        minTemp += stackedMinValue;
                    }

                    categoryAxis.MinValue[categoryAxis.StackIndexMapping[this.StackIndex], i] = minTemp;

                    var stackedMaxValue = categoryAxis.MaxValue[categoryAxis.StackIndexMapping[this.StackIndex], i];
                    if (!double.IsNaN(stackedMaxValue))
                    {
                        maxTemp += stackedMaxValue;
                    }

                    categoryAxis.MaxValue[categoryAxis.StackIndexMapping[this.StackIndex], i] = maxTemp;

                    minValue = Math.Min(minValue, minTemp + this.BaseValue);
                    maxValue = Math.Max(maxValue, maxTemp + this.BaseValue);
                }
            }
            else
            {
                var values = this.ValidItems.Select(item => item.Value).Concat(new[] { 0d }).ToList();
                minValue = values.Min() + this.BaseValue;
                maxValue = values.Max() + this.BaseValue;
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
        ///   Updates the valid items
        /// </summary>
        protected internal override void UpdateValidData()
        {
            this.ValidItems = new List<BarItem>();
            this.ValidItemsIndexInversion = new Dictionary<int, int>();
            var labels = this.GetCategoryAxis().Labels;
            var valueAxis = this.GetValueAxis();

            for (var i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                if (labels.Contains(item.Label) && valueAxis.IsValidValue(item.Value))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                    this.ValidItems.Add(item);
                }
            }
        }

        /// <summary>
        ///   Draw the Bar label
        /// </summary>
        /// <param name="rc"> The render context </param>
        /// <param name="clippingRect"> The clipping rectangle </param>
        /// <param name="rect"> The OxyRectangle </param>
        /// <param name="value"> The value of the label </param>
        /// <param name="i"> The index of the bar item </param>
        protected abstract void DrawLabel(IRenderContext rc, OxyRect clippingRect, OxyRect rect, double value, int i);

        /// <summary>
        ///   Gets the category axis.
        /// </summary>
        /// <returns> The category axis. </returns>
        protected abstract CategoryAxis GetCategoryAxis();

        /// <summary>
        ///   Get the left-base and right-top point
        /// </summary>
        /// <param name="baseValue"> The base value of the bar </param>
        /// <param name="topValue"> The top value of the bar </param>
        /// <param name="beginValue"> The begin value of the bar </param>
        /// <param name="endValue"> The end value of the bar </param>
        /// <param name="p0"> The left-base point of the bar </param>
        /// <param name="p1"> The right-top point of the bar </param>
        protected abstract void GetPoints(
            double baseValue,
            double topValue,
            double beginValue,
            double endValue,
            out ScreenPoint p0,
            out ScreenPoint p1);

        /// <summary>
        ///   Gets the value axis.
        /// </summary>
        /// <returns> The value axis. </returns>
        protected abstract Axis GetValueAxis();

        /// <summary>
        ///   Checks if the specified value is valid.
        /// </summary>
        /// <param name="v"> The value. </param>
        /// <param name="yaxis"> The y axis. </param>
        /// <returns> True if the value is valid. </returns>
        protected virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        #endregion
    }
}