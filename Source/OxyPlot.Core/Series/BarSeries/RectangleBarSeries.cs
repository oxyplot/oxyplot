// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for bar charts where the bars are defined by rectangles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series for bar charts where the bars are defined by rectangles.
    /// </summary>
    public class RectangleBarSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2} {3}\n{4}: {5} {6}";

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleBarSeries" /> class.
        /// </summary>
        public RectangleBarSeries()
        {
            this.Items = new List<RectangleBarItem>();

            this.FillColor = OxyColors.Automatic;
            this.LabelColor = OxyColors.Automatic;
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 1;

            this.TrackerFormatString = DefaultTrackerFormatString;

            this.LabelFormatString = "{4}"; // title

            // this.LabelFormatString = "{0}-{1},{2}-{3}"; // X0-X1,Y0-Y1
        }


        /// <summary>
        /// Gets or sets the default color of the interior of the rectangles.
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
        /// Gets or sets the color of the border around the rectangles.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border around the rectangles.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the actual rectangles for the rectangles.
        /// </summary>
        internal IList<OxyRect> ActualBarRectangles { get; set; }

        /// <summary>
        /// Gets or sets the actual rectangle bar items.
        /// </summary>
        internal IList<RectangleBarItem> ActualItems { get; set; }
        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Specifies whether to interpolate or not.</param>
        /// <returns>A <see cref="TrackerHitResult" /> for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.ActualBarRectangles == null)
            {
                return null;
            }

            for (int i = 0; i < this.ActualBarRectangles.Count; i++)
            {
                var r = this.ActualBarRectangles[i];
                if (r.Contains(point))
                {
                    double value = (this.ActualItems[i].Y0 + this.ActualItems[i].Y1) / 2;
                    var sp = point;
                    var dp = new DataPoint(i, value);
                    var item = this.ActualItems[i];
                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = dp,
                        Position = sp,
                        Item = item,
                        Index = i,
                        Text = StringHelper.Format(
                        this.ActualCulture,
                        this.TrackerFormatString,
                        item,
                        this.Title,
                        this.XAxis.Title ?? DefaultXAxisTitle,
                        this.XAxis.GetValue(this.ActualItems[i].X0),
                        this.XAxis.GetValue(this.ActualItems[i].X1),
                        this.YAxis.Title ?? DefaultYAxisTitle,
                        this.YAxis.GetValue(this.ActualItems[i].Y0),
                        this.YAxis.GetValue(this.ActualItems[i].Y1),
                        this.ActualItems[i].Title)
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.Items.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            int startIdx = 0;
            double xmax = double.MaxValue;

            this.ActualBarRectangles = new List<OxyRect>();
            this.ActualItems = new List<RectangleBarItem>();

            if (this.IsXMonotonic)
            {
                var xmin = this.XAxis.ActualMinimum;
                xmax = this.XAxis.ActualMaximum;
                this.WindowStartIndex = this.UpdateWindowStartIndex(this.Items, rect => rect.X0, xmin, this.WindowStartIndex);

                startIdx = this.WindowStartIndex;
            }

            int clipCount = 0;
            for (int i = startIdx; i < this.Items.Count; i++){
                var item = this.Items[i];
                if (!this.IsValid(item.X0) || !this.IsValid(item.X1)
                    || !this.IsValid(item.Y0) || !this.IsValid(item.Y1))
                {
                    continue;
                }

                var p0 = this.Transform(item.X0, item.Y0);
                var p1 = this.Transform(item.X1, item.Y1);

                var rectangle = OxyRect.Create(p0.X, p0.Y, p1.X, p1.Y);

                this.ActualBarRectangles.Add(rectangle);
                this.ActualItems.Add(item);

                rc.DrawClippedRectangle(
                    clippingRect,
                    rectangle,
                    this.GetSelectableFillColor(item.Color.GetActualColor(this.ActualFillColor)),
                    this.StrokeColor,
                    this.StrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));

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
                        HorizontalAlignment.Center,
                        VerticalAlignment.Middle);
                }

                clipCount += item.X0 > xmax ? 1 : 0;
                if (clipCount > 1)
                {
                    break;
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
            rc.DrawRectangle(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableFillColor(this.ActualFillColor),
                this.StrokeColor,
                this.StrokeThickness,
                this.EdgeRenderingMode);
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
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Items.Clear();

            // ReflectionExtensions.AddRange(
            // this.ItemsSource,
            // this.Items,
            // new[] { this.MinimumField, this.MaximumField },
            // (item, value) => item.Minimum = Convert.ToDouble(value),
            // (item, value) => item.Maximum = Convert.ToDouble(value));
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            this.IsXMonotonic = true;

            if (this.Items == null || this.Items.Count == 0)
            {
                return;
            }

            double minValueX = double.MaxValue;
            double maxValueX = double.MinValue;
            double minValueY = double.MaxValue;
            double maxValueY = double.MinValue;

            double lastX0 = double.MinValue;
            double lastX1 = double.MinValue;
            foreach (var item in this.Items)
            {
                if (item.X0 < lastX0 || item.X1 < lastX1)
                {
                    this.IsXMonotonic = false;
                }

                minValueX = Math.Min(minValueX, Math.Min(item.X0, item.X1));
                maxValueX = Math.Max(maxValueX, Math.Max(item.X1, item.X0));
                minValueY = Math.Min(minValueY, Math.Min(item.Y0, item.Y1));
                maxValueY = Math.Max(maxValueY, Math.Max(item.Y0, item.Y1));

                lastX0 = item.X0;
                lastX1 = item.X1;
            }

            this.MinX = minValueX;
            this.MaxX = maxValueX;
            this.MinY = minValueY;
            this.MaxY = maxValueY;
        }

        /// <summary>
        /// Checks if the specified value is valid.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>True if the value is valid.</returns>
        protected virtual bool IsValid(double v)
        {
            return !double.IsNaN(v) && !double.IsInfinity(v);
        }
    }
}
