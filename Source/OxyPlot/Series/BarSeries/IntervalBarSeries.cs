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
                    double value = (this.Items[i].Start + this.Items[i].End) / 2;
                    ScreenPoint sp = point;
                    var dp = new DataPoint(i, value);
                    var item = this.GetItem(i);
                    var text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, this.Items[i].Start, this.Items[i].End, this.Items[i].Title);
                    return new TrackerHitResult(this, dp, sp, item, text);
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
            if (this.Items.Count == 0)
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

            double dx = categoryAxis.BarOffset - this.BarWidth * 0.5;

            int i = 0;

            this.ActualBarRectangles = new List<OxyRect>();

            foreach (var item in this.Items)
            {
                if (!this.IsValidPoint(item.Start, valueAxis) || !this.IsValidPoint(item.End, valueAxis))
                {
                    continue;
                }

                var p0 = this.XAxis.Transform(item.Start, i + dx, this.YAxis);
                var p1 = this.XAxis.Transform(item.End, i + dx + this.BarWidth, this.YAxis);

                var rectangle = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);

                this.ActualBarRectangles.Add(rectangle);

                rc.DrawClippedRectangleAsPolygon(
                    rectangle,
                    clippingRect,
                    item.Color ?? this.FillColor,
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

                i++;
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
                new OxyRect(xmid - 0.5 * width, ymid - 0.5 * height, width, height),
                this.FillColor,
                this.StrokeColor,
                this.StrokeThickness);
        }

        #endregion

        #region Methods

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
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Items.Clear();

            ReflectionHelper.FillManyValues(
                this.ItemsSource,
                this.Items,
                new[] { this.MinimumField, this.MaximumField },
                (item, value) => item.Start = Convert.ToDouble(value),
                (item, value) => item.End = Convert.ToDouble(value));
        }

        /// <summary>
        /// Updates the maximum/minimum value on the value axis from the bar values.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (this.Items == null || this.Items.Count == 0)
            {
                return;
            }

            double minValue = double.MaxValue;
            double maxValue = double.MinValue;

            foreach (var item in this.Items)
            {
                minValue = Math.Min(minValue, item.Start);
                minValue = Math.Min(minValue, item.End);
                maxValue = Math.Max(maxValue, item.Start);
                maxValue = Math.Max(maxValue, item.End);
            }

            this.MinX = minValue;
            this.MaxX = maxValue;
        }
        #endregion
    }
}