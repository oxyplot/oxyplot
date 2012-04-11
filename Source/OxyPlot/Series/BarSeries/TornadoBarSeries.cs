// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The TornadoBarSeries is used to create tornado plots.
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Tornado_diagram.
    /// </remarks>
    public class TornadoBarSeries : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TornadoBarSeries" /> class.
        /// </summary>
        public TornadoBarSeries()
        {
            this.Items = new List<TornadoBarItem>();

            this.MaximumFillColor = OxyColor.FromRGB(216, 82, 85);
            this.MinimumFillColor = OxyColor.FromRGB(84, 138, 209);

            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 1;
            this.BarWidth = 0.5;

            this.TrackerFormatString = "{0}";
            this.LabelMargin = 4;

            this.MinimumLabelFormatString = "{0}";
            this.MaximumLabelFormatString = "{0}";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the width of the bars (as a fraction of the available width). The default value is 0.5 (50%)
        /// </summary>
        /// <value> The width of the bars. </value>
        public double BarWidth { get; set; }

        /// <summary>
        ///   Gets or sets the base value.
        /// </summary>
        /// <value> The base value. </value>
        public double BaseValue { get; set; }

        /// <summary>
        ///   Gets the tornado bar items.
        /// </summary>
        /// <value> The items. </value>
        public IList<TornadoBarItem> Items { get; private set; }

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
        public OxyColor MaximumFillColor { get; set; }

        /// <summary>
        ///   Gets or sets the format string for the maximum labels.
        /// </summary>
        public string MaximumLabelFormatString { get; set; }

        /// <summary>
        ///   Gets or sets the minimum value field.
        /// </summary>
        public string MinimumField { get; set; }

        /// <summary>
        ///   Gets or sets the default color of the interior of the Minimum bars.
        /// </summary>
        /// <value> The color. </value>
        public OxyColor MinimumFillColor { get; set; }

        /// <summary>
        ///   Gets or sets the format string for the minimum labels.
        /// </summary>
        public string MinimumLabelFormatString { get; set; }

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
        internal IList<OxyRect> ActualMaximumBarRectangles { get; set; }

        /// <summary>
        ///   Gets or sets the actual rectangles for the minimum bars.
        /// </summary>
        internal IList<OxyRect> ActualMinimumBarRectangles { get; set; }

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
            for (int i = 0; i < this.ActualMinimumBarRectangles.Count; i++)
            {
                var r = this.ActualMinimumBarRectangles[i];
                if (r.Contains(point))
                {
                    double value = this.Items[i].Minimum;
                    var dp = new DataPoint(i, value);
                    var item = this.GetItem(i);
                    var text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, value);
                    return new TrackerHitResult(this, dp, point, item, text);
                }

                r = this.ActualMaximumBarRectangles[i];
                if (r.Contains(point))
                {
                    double value = this.Items[i].Maximum;
                    var dp = new DataPoint(i, value);
                    var item = this.GetItem(i);
                    var text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, value);
                    return new TrackerHitResult(this, dp, point, item, text);
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

            double dx = categoryAxis.BarOffset - (this.BarWidth * 0.5);

            int i = 0;

            this.ActualMinimumBarRectangles = new List<OxyRect>();
            this.ActualMaximumBarRectangles = new List<OxyRect>();

            foreach (var item in this.Items)
            {
                if (!this.IsValidPoint(item.Minimum, valueAxis) || !this.IsValidPoint(item.Maximum, valueAxis))
                {
                    continue;
                }

                double baseValue = double.IsNaN(item.BaseValue) ? this.BaseValue : item.BaseValue;

                var p0 = this.Transform(item.Minimum, i + dx);
                var p1 = this.Transform(item.Maximum, i + dx + this.BarWidth);
                var p2 = this.Transform(baseValue, i + dx);
                p2.X = (int)p2.X;

                var minimumRectangle = OxyRect.Create(p0.X, p0.Y, p2.X, p1.Y);
                var maximumRectangle = OxyRect.Create(p2.X, p0.Y, p1.X, p1.Y);

                this.ActualMinimumBarRectangles.Add(minimumRectangle);
                this.ActualMaximumBarRectangles.Add(maximumRectangle);

                rc.DrawClippedRectangleAsPolygon(
                    minimumRectangle,
                    clippingRect,
                    item.MinimumColor ?? this.MinimumFillColor,
                    this.StrokeColor,
                    this.StrokeThickness);
                rc.DrawClippedRectangleAsPolygon(
                    maximumRectangle,
                    clippingRect,
                    item.MaximumColor ?? this.MaximumFillColor,
                    this.StrokeColor,
                    this.StrokeThickness);

                if (this.MinimumLabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.MinimumLabelFormatString, this.GetItem(i), item.Minimum);
                    var pt = new ScreenPoint(minimumRectangle.Left - this.LabelMargin, (minimumRectangle.Top + minimumRectangle.Bottom) / 2);
                    var alignment = HorizontalTextAlign.Right;

                    rc.DrawClippedText(
                        clippingRect,
                        pt,
                        s,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        alignment,
                        VerticalTextAlign.Middle);
                }

                if (this.MaximumLabelFormatString != null)
                {
                    var s = StringHelper.Format(this.ActualCulture, this.MaximumLabelFormatString, this.GetItem(i), item.Maximum);
                    var pt = new ScreenPoint(maximumRectangle.Right + this.LabelMargin, (maximumRectangle.Top + maximumRectangle.Bottom) / 2);
                    var alignment = HorizontalTextAlign.Left;

                    rc.DrawClippedText(
                        clippingRect,
                        pt,
                        s,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        alignment,
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
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), 0.5 * width, height),
                this.MinimumFillColor,
                this.StrokeColor,
                this.StrokeThickness);
            rc.DrawRectangleAsPolygon(
                new OxyRect(xmid, ymid - (0.5 * height), 0.5 * width, height),
                this.MaximumFillColor,
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
            if (this.MaximumFillColor == null)
            {
                this.MaximumFillColor = model.GetDefaultColor();
            }

            if (this.MinimumFillColor == null)
            {
                this.MinimumFillColor = model.GetDefaultColor();
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

            ReflectionHelper.FillList(
                this.ItemsSource,
                this.Items,
                new[] { this.MinimumField, this.MaximumField },
                (item, value) => item.Minimum = Convert.ToDouble(value),
                (item, value) => item.Maximum = Convert.ToDouble(value));
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
                minValue = Math.Min(minValue, item.Minimum);
                maxValue = Math.Max(maxValue, item.Maximum);
            }

            this.MinX = minValue;
            this.MaxX = maxValue;
        }

        #endregion
    }
}