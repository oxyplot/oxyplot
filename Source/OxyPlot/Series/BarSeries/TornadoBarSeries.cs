// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series that can be used to create tornado plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series that can be used to create tornado plots.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Tornado_diagram.</remarks>
    public class TornadoBarSeries : CategorizedSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}";

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultMaximumFillColor;

        /// <summary>
        /// The default minimum fill color.
        /// </summary>
        private OxyColor defaultMinimumFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TornadoBarSeries" /> class.
        /// </summary>
        public TornadoBarSeries()
        {
            this.Items = new List<TornadoBarItem>();

            this.MaximumFillColor = OxyColor.FromRgb(216, 82, 85);
            this.MinimumFillColor = OxyColor.FromRgb(84, 138, 209);

            this.LabelColor = OxyColors.Automatic;
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 1;
            this.BarWidth = 1;

            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelMargin = 4;

            this.MinimumLabelFormatString = "{0}";
            this.MaximumLabelFormatString = "{0}";
        }

        /// <summary>
        /// Gets or sets the width of the bars (as a fraction of the available width). The default value is 0.5 (50%)
        /// </summary>
        /// <value>The width of the bars.</value>
        public double BarWidth { get; set; }

        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        /// <value>The base value.</value>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets the tornado bar items.
        /// </summary>
        /// <value>The items.</value>
        public IList<TornadoBarItem> Items { get; private set; }

        /// <summary>
        /// Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the label field.
        /// </summary>
        public string LabelField { get; set; }

        /// <summary>
        /// Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value field.
        /// </summary>
        public string MaximumField { get; set; }

        /// <summary>
        /// Gets or sets the color of the interior of the Maximum bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor MaximumFillColor { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualMaximumFillColor
        {
            get { return this.MaximumFillColor.GetActualColor(this.defaultMaximumFillColor); }
        }

        /// <summary>
        /// Gets or sets the format string for the maximum labels.
        /// </summary>
        public string MaximumLabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the minimum value field.
        /// </summary>
        public string MinimumField { get; set; }

        /// <summary>
        /// Gets or sets the default color of the interior of the Minimum bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor MinimumFillColor { get; set; }

        /// <summary>
        /// Gets the actual minimum fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualMinimumFillColor
        {
            get { return this.MinimumFillColor.GetActualColor(this.defaultMinimumFillColor); }
        }

        /// <summary>
        /// Gets or sets the format string for the minimum labels.
        /// </summary>
        public string MinimumLabelFormatString { get; set; }

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
        /// Gets or sets the actual rectangles for the maximum bars.
        /// </summary>
        protected internal IList<OxyRect> ActualMaximumBarRectangles { get; set; }

        /// <summary>
        /// Gets or sets the actual rectangles for the minimum bars.
        /// </summary>
        protected internal IList<OxyRect> ActualMinimumBarRectangles { get; set; }

        /// <summary>
        /// Gets or sets the valid items
        /// </summary>
        protected internal IList<TornadoBarItem> ValidItems { get; set; }

        /// <summary>
        /// Gets or sets the dictionary which stores the index-inversion for the valid items
        /// </summary>
        protected internal Dictionary<int, int> ValidItemsIndexInversion { get; set; }

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">The interpolate.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            for (int i = 0; i < this.ActualMinimumBarRectangles.Count; i++)
            {
                var insideMinimumRectangle = this.ActualMinimumBarRectangles[i].Contains(point);
                var insideMaximumRectangle = this.ActualMaximumBarRectangles[i].Contains(point);
                if (insideMinimumRectangle || insideMaximumRectangle)
                {
                    var item = (TornadoBarItem)this.GetItem(this.ValidItemsIndexInversion[i]);
                    var categoryIndex = item.GetCategoryIndex(i);
                    var value = insideMaximumRectangle ? this.ValidItems[i].Maximum : this.ValidItems[i].Minimum;
                    var dp = new DataPoint(categoryIndex, value);
                    var categoryAxis = this.GetCategoryAxis();
                    var valueAxis = this.GetValueAxis();
                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = dp,
                        Position = point,
                        Item = item,
                        Index = i,
                        Text =
                            StringHelper.Format(
                                this.ActualCulture, 
                                this.TrackerFormatString,
                                item,
                                this.Title,
                                categoryAxis.Title ?? DefaultCategoryAxisTitle,
                                categoryAxis.FormatValue(categoryIndex),
                                valueAxis.Title ?? DefaultValueAxisTitle,
                                valueAxis.GetValue(value))
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the specified value is valid.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns>True if the value is valid.</returns>
        public virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

        /// <summary>
        /// Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            this.ActualMinimumBarRectangles = new List<OxyRect>();
            this.ActualMaximumBarRectangles = new List<OxyRect>();

            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();
            var categoryAxis = this.GetCategoryAxis();
            var actualBarWidth = this.GetActualBarWidth();

            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];

                var categoryIndex = item.GetCategoryIndex(i);

                var baseValue = double.IsNaN(item.BaseValue) ? this.BaseValue : item.BaseValue;
                var barOffset = categoryAxis.GetCurrentBarOffset(categoryIndex);
                var p0 = this.Transform(item.Minimum, categoryIndex - 0.5 + barOffset);
                var p1 = this.Transform(item.Maximum, categoryIndex - 0.5 + barOffset + actualBarWidth);
                var p2 = this.Transform(baseValue, categoryIndex - 0.5 + barOffset);
                p2 = new ScreenPoint((int)p2.X, p2.Y);

                var minimumRectangle = OxyRect.Create(p0.X, p0.Y, p2.X, p1.Y);
                var maximumRectangle = OxyRect.Create(p2.X, p0.Y, p1.X, p1.Y);

                this.ActualMinimumBarRectangles.Add(minimumRectangle);
                this.ActualMaximumBarRectangles.Add(maximumRectangle);

                rc.DrawClippedRectangleAsPolygon(
                    clippingRect,
                    minimumRectangle,
                    item.MinimumColor.GetActualColor(this.ActualMinimumFillColor),
                    this.StrokeColor,
                    this.StrokeThickness);
                rc.DrawClippedRectangleAsPolygon(
                    clippingRect,
                    maximumRectangle,
                    item.MaximumColor.GetActualColor(this.ActualMaximumFillColor),
                    this.StrokeColor,
                    this.StrokeThickness);

                if (this.MinimumLabelFormatString != null)
                {
                    var s = StringHelper.Format(
                        this.ActualCulture, 
                        this.MinimumLabelFormatString,
                        this.GetItem(this.ValidItemsIndexInversion[i]),
                        item.Minimum);
                    var pt = new ScreenPoint(
                        minimumRectangle.Left - this.LabelMargin, (minimumRectangle.Top + minimumRectangle.Bottom) / 2);

                    rc.DrawClippedText(
                        clippingRect,
                        pt,
                        s,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        HorizontalAlignment.Right,
                        VerticalAlignment.Middle);
                }

                if (this.MaximumLabelFormatString != null)
                {
                    var s = StringHelper.Format(
                        this.ActualCulture, 
                        this.MaximumLabelFormatString,
                        this.GetItem(this.ValidItemsIndexInversion[i]),
                        item.Maximum);
                    var pt = new ScreenPoint(
                        maximumRectangle.Right + this.LabelMargin, (maximumRectangle.Top + maximumRectangle.Bottom) / 2);

                    rc.DrawClippedText(
                        clippingRect,
                        pt,
                        s,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        HorizontalAlignment.Left,
                        VerticalAlignment.Middle);
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
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            double height = (legendBox.Bottom - legendBox.Top) * 0.8;
            double width = height;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), 0.5 * width, height),
                this.ActualMinimumFillColor,
                this.StrokeColor,
                this.StrokeThickness);
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid, ymid - (0.5 * height), 0.5 * width, height),
                this.ActualMaximumFillColor,
                this.StrokeColor,
                this.StrokeThickness);
        }

        /// <summary>
        /// Gets or sets the width/height of the columns/bars (as a fraction of the available space).
        /// </summary>
        /// <value>The width of the bars.</value>
        /// <returns>The fractional width.</returns>
        /// <remarks>The available space will be determined by the GapWidth of the CategoryAxis used by this series.</remarks>
        internal override double GetBarWidth()
        {
            return this.BarWidth;
        }

        /// <summary>
        /// Gets the items of this series.
        /// </summary>
        /// <returns>The items.</returns>
        protected internal override IList<CategorizedItem> GetItems()
        {
            return this.Items.Cast<CategorizedItem>().ToList();
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
            if (this.MaximumFillColor.IsAutomatic())
            {
                this.defaultMaximumFillColor = this.PlotModel.GetDefaultColor();
            }

            if (this.MinimumFillColor.IsAutomatic())
            {
                this.defaultMinimumFillColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the axis maximum and minimum values.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource != null)
            {
                this.Items.Clear();

                var filler = new ListBuilder<TornadoBarItem>();
                filler.Add(this.MinimumField, double.NaN);
                filler.Add(this.MaximumField, double.NaN);
                filler.FillT(this.Items, this.ItemsSource, args => new TornadoBarItem() { Minimum = Convert.ToDouble(args[0]), Maximum = Convert.ToDouble(args[1]) });
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

            double minValue = double.MaxValue;
            double maxValue = double.MinValue;

            foreach (var item in this.ValidItems)
            {
                minValue = Math.Min(minValue, item.Minimum);
                maxValue = Math.Max(maxValue, item.Maximum);
            }

            this.MinX = minValue;
            this.MaxX = maxValue;
        }

        /// <summary>
        /// Updates the valid items
        /// </summary>
        protected internal override void UpdateValidData()
        {
            this.ValidItems = new List<TornadoBarItem>();
            this.ValidItemsIndexInversion = new Dictionary<int, int>();
            var valueAxis = this.GetValueAxis();

            for (var i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                if (valueAxis.IsValidValue(item.Minimum) && valueAxis.IsValidValue(item.Maximum))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                    this.ValidItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the actual width/height of the items of this series.
        /// </summary>
        /// <returns>The width or height.</returns>
        /// <remarks>The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.</remarks>
        protected override double GetActualBarWidth()
        {
            var categoryAxis = this.GetCategoryAxis();
            return this.BarWidth / (1 + categoryAxis.GapWidth) / categoryAxis.GetMaxWidth();
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>The category axis.</returns>
        protected override CategoryAxis GetCategoryAxis()
        {
            var categoryAxis = this.YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            return categoryAxis;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            if (this.ItemsSource != null || this.Items == null || this.Items.Count == 0)
            {
                return base.GetItem(i);
            }

            return this.Items[i];
        }

        /// <summary>
        /// Gets the value axis.
        /// </summary>
        /// <returns>The value axis.</returns>
        private Axis GetValueAxis()
        {
            return this.XAxis;
        }
    }
}