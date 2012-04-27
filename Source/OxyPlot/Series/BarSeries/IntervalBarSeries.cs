// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The IntervalBarSeries is used to create bars that has to/from values.
    /// </summary>
    public class IntervalBarSeries : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="IntervalBarSeries" /> class.
        /// </summary>
        public IntervalBarSeries()
        {
            this.Items = new List<IntervalBarItem>();

            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 1;
            this.BarWidth = 0.5;

            this.TrackerFormatString = "{0}";
            this.LabelMargin = 4;

            this.LabelFormatString = "{2}"; // title
            // this.LabelFormatString = "{0}-{1}"; // Minimum-Maximum
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the width of the bars (as a fraction of the available width). The default value is 0.5 (50%)
        /// </summary>
        /// <value> The width of the bars. </value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets the range bar items.
        /// </summary>
        public IList<IntervalBarItem> Items { get; private set; }

        /// <summary>
        ///   Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        ///   Gets or sets the label field.
        /// </summary>
        public string LabelField { get; set; }

        /// <summary>
        ///   Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        ///   Gets or sets the maximum value field.
        /// </summary>
        public string MaximumField { get; set; }

        /// <summary>
        ///   Gets or sets the default color of the interior of the Maximum bars.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        ///   Gets or sets the format string for the maximum labels.
        /// </summary>
        public string LabelFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the minimum value field.
        /// </summary>
        public string MinimumField { get; set; }

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

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the actual rectangles for the maximum bars.
        /// </summary>
        internal IList<OxyRect> ActualBarRectangles { get; set; }

        /// <summary>
        /// Gets or sets the valid items
        /// </summary>
        internal IList<IntervalBarItem> ValidItems { get; set; }

        /// <summary>
        /// Gets or sets the dictionary which stores the index-inversion for the valid items
        /// </summary>
        internal Dictionary<int, int> ValidItemsIndexInversion { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <param name="interpolate">
        /// The interpolate. 
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit. 
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            for (int i = 0; i < this.ActualBarRectangles.Count; i++)
            {
                var r = this.ActualBarRectangles[i];
                if (r.Contains(point))
                {
                    var labelId = this.GetCategoryAxis().Labels.IndexOf(this.ValidItems[i].Label);
                    var item = this.GetItem(this.ValidItemsIndexInversion[i]);
                    double value = (this.ValidItems[i].Start + this.ValidItems[i].End) / 2;
                    ScreenPoint sp = point;
                    var dp = new DataPoint(labelId, value);
                    var text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, this.Items[i].Start, this.Items[i].End, this.Items[i].Title);
                    return new TrackerHitResult(this, dp, sp, item, i, text);
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the specified value is valid.
        /// </summary>
        /// <param name="v">
        /// The value. 
        /// </param>
        /// <param name="yaxis">
        /// The y axis. 
        /// </param>
        /// <returns>
        /// True if the value is valid. 
        /// </returns>
        public virtual bool IsValidPoint(double v, Axis yaxis)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }

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
            if (this.ValidItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            var categoryAxis = this.YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            var valueAxis = this.XAxis;

            this.ActualBarRectangles = new List<OxyRect>();

            var actualBarWidth = this.BarWidth / categoryAxis.MaxWidth * categoryAxis.CategoryWidth;

            for (var i = 0; i < this.ValidItems.Count; i++)
            {
                var item = this.ValidItems[i];
                var label = item.Label;
                if (!categoryAxis.Labels.Contains(label))
                {
                    continue;
                }

                var labelId = categoryAxis.Labels.IndexOf(label);

                var p0 = this.Transform(item.Start, labelId - 0.5 + categoryAxis.BarOffset[labelId]);
                var p1 = this.Transform(item.End, labelId - 0.5 + categoryAxis.BarOffset[labelId] + actualBarWidth);

                var rectangle = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);

                this.ActualBarRectangles.Add(rectangle);

                rc.DrawClippedRectangleAsPolygon(
                    rectangle,
                    clippingRect,
                    this.GetSelectableFillColor(item.Color ?? this.FillColor),
                    this.StrokeColor,
                    this.StrokeThickness);

                if (this.LabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, this.GetItem(i), item.Start, item.End, item.Title);

                    var pt = new ScreenPoint((rectangle.Left + rectangle.Right) / 2, (rectangle.Top + rectangle.Bottom) / 2);

                    rc.DrawClippedText(
                        clippingRect,
                        pt,
                        s,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        HorizontalTextAlign.Center,
                        VerticalTextAlign.Middle);
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context. 
        /// </param>
        /// <param name="legendBox">
        /// The legend rectangle. 
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            double height = (legendBox.Bottom - legendBox.Top) * 0.8;
            double width = height;
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableFillColor(this.FillColor),
                this.StrokeColor,
                this.StrokeThickness);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">
        /// An axis which should be checked if used
        /// </param>
        /// <returns>
        /// True if the axis is in use. 
        /// </returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        /// The set default values.
        /// </summary>
        /// <param name="model">
        /// The model. 
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.FillColor == null)
            {
                this.FillColor = model.GetDefaultColor();
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

                var filler = new ListFiller<IntervalBarItem>();
                filler.Add(this.LabelField, (item, value) => item.Label = Convert.ToString(value));
                filler.Add(this.MinimumField, (item, value) => item.Start = Convert.ToDouble(value));
                filler.Add(this.MaximumField, (item, value) => item.End = Convert.ToDouble(value));
                filler.FillT(this.Items, this.ItemsSource);
            }
        }

        /// <summary>
        /// Updates the valid items
        /// </summary>
        protected internal override void UpdateValidData()
        {
            this.ValidItems = new List<IntervalBarItem>();
            this.ValidItemsIndexInversion = new Dictionary<int, int>();
            var labels = this.GetCategoryAxis().Labels;
            var valueAxis = this.GetValueAxis();

            for (var i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                if (labels.Contains(item.Label) && valueAxis.IsValidValue(item.Start) && valueAxis.IsValidValue(item.End))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                    this.ValidItems.Add(item);
                }
            }
        }

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

            double minValue = double.MaxValue;
            double maxValue = double.MinValue;

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

        /// <summary>
        ///   Gets the category axis.
        /// </summary>
        /// <returns> The category axis. </returns>
        private CategoryAxis GetCategoryAxis()
        {
            var categoryAxis = this.YAxis as CategoryAxis;
            if (categoryAxis == null)
            {
                throw new InvalidOperationException("No category axis defined.");
            }

            return categoryAxis;
        }

        /// <summary>
        ///   Gets the value axis.
        /// </summary>
        /// <returns> The value axis. </returns>
        private Axis GetValueAxis()
        {
            return this.XAxis;
        }

        #endregion
    }
}