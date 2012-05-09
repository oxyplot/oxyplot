// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleBarSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The RectangleBarSeries is used to create bars that has to/from values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The RectangleBarSeries is used to create bars that has to/from values.
    /// </summary>
    public class RectangleBarSeries : XYAxisSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleBarSeries"/> class.
        /// </summary>
        public RectangleBarSeries()
        {
            this.Items = new List<RectangleBarItem>();

            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 1;

            this.TrackerFormatString = "{0}";

            this.LabelFormatString = "{4}"; // title

            // this.LabelFormatString = "{0}-{1},{2}-{3}"; // X0-X1,Y0-Y1
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the default color of the interior of the Maximum bars.
        /// </summary>
        /// <value>
        /// The color. 
        /// </value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        /// Gets the rectangle bar items.
        /// </summary>
        public IList<RectangleBarItem> Items { get; private set; }

        /// <summary>
        /// Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the format string for the labels.
        /// </summary>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>
        /// The color of the stroke. 
        /// </value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>
        /// The stroke thickness. 
        /// </value>
        public double StrokeThickness { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the actual rectangles for the maximum bars.
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
                    double value = (this.Items[i].Y0 + this.Items[i].Y1) / 2;
                    var sp = point;
                    var dp = new DataPoint(i, value);
                    var item = this.GetItem(i);
                    var text = StringHelper.Format(
                        this.ActualCulture, 
                        this.TrackerFormatString, 
                        item, 
                        this.Items[i].X0, 
                        this.Items[i].X1, 
                        this.Items[i].Y0, 
                        this.Items[i].Y1, 
                        this.Items[i].Title);
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
            if (this.Items.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            int i = 0;

            this.ActualBarRectangles = new List<OxyRect>();

            foreach (var item in this.Items)
            {
                if (!this.IsValidPoint(item.X0, this.XAxis) || !this.IsValidPoint(item.X1, this.XAxis)
                    || !this.IsValidPoint(item.Y0, this.YAxis) || !this.IsValidPoint(item.Y1, this.YAxis))
                {
                    continue;
                }

                var p0 = this.Transform(item.X0, item.Y0);
                var p1 = this.Transform(item.X1, item.Y1);

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
                    var s = StringHelper.Format(
                        this.ActualCulture, 
                        this.LabelFormatString, 
                        this.GetItem(i), 
                        item.X0, 
                        item.X1, 
                        item.Y0, 
                        item.Y1, 
                        item.Title);

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
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height), 
                this.GetSelectableFillColor(this.FillColor), 
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
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Items.Clear();

            // ReflectionHelper.FillList(
            // this.ItemsSource,
            // this.Items,
            // new[] { this.MinimumField, this.MaximumField },
            // (item, value) => item.Minimum = Convert.ToDouble(value),
            // (item, value) => item.Maximum = Convert.ToDouble(value));
            throw new NotImplementedException();
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

            double minValueX = double.MaxValue;
            double maxValueX = double.MinValue;
            double minValueY = double.MaxValue;
            double maxValueY = double.MinValue;

            foreach (var item in this.Items)
            {
                minValueX = Math.Min(minValueX, Math.Min(item.X0, item.X1));
                maxValueX = Math.Max(maxValueX, Math.Max(item.X1, item.X0));
                minValueY = Math.Min(minValueY, Math.Min(item.Y0, item.Y1));
                maxValueY = Math.Max(maxValueY, Math.Max(item.Y0, item.Y1));
            }

            this.MinX = minValueX;
            this.MaxX = maxValueX;
            this.MinY = minValueY;
            this.MaxY = maxValueY;
        }

        #endregion
    }
}