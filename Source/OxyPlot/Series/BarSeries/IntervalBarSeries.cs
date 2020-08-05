// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for bar charts defined by to/from values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series for bar charts defined by to/from values.
    /// </summary>
    public class IntervalBarSeries : BarSeriesBase<IntervalBarItem>, IStackableSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalBarSeries" /> class.
        /// </summary>
        public IntervalBarSeries()
        {
            this.LabelColor = OxyColors.Automatic;
            this.FillColor = OxyColors.Automatic;
            this.StrokeThickness = 1;

            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelMargin = 4;

            this.LabelFormatString = "{2}"; // title
        }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualFillColor => this.FillColor.GetActualColor(this.defaultFillColor);

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        public string ColorField { get; set; }

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        public string EndField { get; set; }

        /// <summary>
        /// Gets or sets the default color of the interior of the Maximum bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <inheritdoc/>
        public bool IsStacked => true;

        /// <inheritdoc/>
        public bool OverlapsStack => true;

        /// <summary>
        /// Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the format string for the maximum labels.
        /// </summary>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <inheritdoc/>
        public string StackGroup => string.Empty;

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        public string StartField { get; set; }

        /// <summary>
        /// Gets or sets the actual rectangles for the maximum bars.
        /// </summary>
        protected internal IList<OxyRect> ActualBarRectangles { get; set; }

        /// <inheritdoc/>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            for (var i = 0; i < this.ActualBarRectangles.Count; i++)
            {
                var r = this.ActualBarRectangles[i];
                if (r.Contains(point))
                {
                    var item = (IntervalBarItem)this.GetItem(this.ValidItemsIndexInversion[i]);
                    var categoryIndex = item.GetCategoryIndex(i);
                    var value = (this.ValidItems[i].Start + this.ValidItems[i].End) / 2;
                    var dp = new DataPoint(categoryIndex, value);
                    var categoryAxis = this.GetCategoryAxis();
                    var valueAxis = this.XAxis;
                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = dp,
                        Position = point,
                        Item = item,
                        Index = i,
                        Text = StringHelper.Format(
                        this.ActualCulture,
                        this.TrackerFormatString,
                        item,
                        this.Title,
                        categoryAxis.Title ?? DefaultCategoryAxisTitle,
                        categoryAxis.FormatValue(categoryIndex),
                        valueAxis.Title ?? DefaultValueAxisTitle,
                        valueAxis.GetValue(this.Items[i].Start),
                        valueAxis.GetValue(this.Items[i].End),
                        this.Items[i].Title)
                    };
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangle(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableFillColor(this.ActualFillColor),
                this.StrokeColor,
                this.StrokeThickness,
                this.EdgeRenderingMode);
        }

        /// <inheritdoc/>
        protected internal override void SetDefaultValues()
        {
            if (this.FillColor.IsAutomatic())
            {
                this.defaultFillColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <inheritdoc/>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var minValue = double.MaxValue;
            var maxValue = double.MinValue;

            foreach (var item in this.ValidItems)
            {
                minValue = Math.Min(minValue, item.Start);
                minValue = Math.Min(minValue, item.End);
                maxValue = Math.Max(maxValue, item.Start);
                maxValue = Math.Max(maxValue, item.End);
            }

            this.MinX = minValue;
            this.MaxX = maxValue;
        }

        /// <inheritdoc/>
        protected override bool IsValid(IntervalBarItem item)
        {
            return this.XAxis.IsValidValue(item.Start) && this.XAxis.IsValidValue(item.End);
        }

        /// <inheritdoc/>
        public override void Render(IRenderContext rc)
        {
            this.ActualBarRectangles = new List<OxyRect>();

            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            var actualBarWidth = this.GetActualBarWidth();
            var stackIndex = this.Manager.GetStackIndex(this.StackGroup);

            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];

                var categoryIndex = item.GetCategoryIndex(i);
                var categoryValue = this.Manager.GetCategoryValue(categoryIndex, stackIndex, actualBarWidth);

                var p0 = this.Transform(item.Start, categoryValue);
                var p1 = this.Transform(item.End, categoryValue + actualBarWidth);

                var rectangle = new OxyRect(p0, p1);

                this.ActualBarRectangles.Add(rectangle);

                rc.DrawClippedRectangle(
                    clippingRect,
                    rectangle,
                    this.GetSelectableFillColor(item.Color.GetActualColor(this.ActualFillColor)),
                    this.StrokeColor,
                    this.StrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));

                if (this.LabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(i), item.Start, item.End, item.Title);

                    var pt = new ScreenPoint(
                        (rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);

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
                        VerticalAlignment.Middle);
                }
            }
        }

        /// <inheritdoc/>
        protected override bool UpdateFromDataFields()
        {
            if (this.StartField == null || this.EndField == null)
            {
                return false;
            }

            var filler = new ListBuilder<BarItem>();
            filler.Add(this.StartField, double.NaN);
            filler.Add(this.EndField, double.NaN);
            filler.Add(this.ColorField, OxyColors.Automatic);
            filler.Fill(
                this.ItemsSourceItems,
                this.ItemsSource,
                args => new IntervalBarItem(Convert.ToDouble(args[0]), Convert.ToDouble(args[1])) { Color = (OxyColor)args[2] });

            return true;
        }
    }
}
