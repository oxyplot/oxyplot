// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoxPlotSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for box plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for box plots.
    /// </summary>
    public class BoxPlotSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\nUpper Whisker: {3:N2}\nThird Quartil: {4:N2}\nMedian: {5:N2}\nFirst Quartil: {6:N2}\nLower Whisker: {7:N2}\nMean: {8:N2}";

        /// <summary>
        /// The items from the items source.
        /// </summary>
        private List<BoxPlotItem> itemsSourceItems;

        /// <summary>
        /// Specifies if the ownsItemsSourceItems list can be modified.
        /// </summary>
        private bool ownsItemsSourceItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxPlotSeries" /> class.
        /// </summary>
        public BoxPlotSeries()
        {
            this.Items = new List<BoxPlotItem>();
            this.TrackerFormatString = DefaultTrackerFormatString;
            this.OutlierTrackerFormatString = "{0}\n{1}: {2}\nY: {3:0.00}";
            this.Title = null;
            this.Fill = OxyColors.Automatic;
            this.Stroke = OxyColors.Black;
            this.BoxWidth = 0.3;
            this.StrokeThickness = 1;
            this.MedianThickness = 2;
            this.MeanThickness = 2;
            this.OutlierSize = 2;
            this.OutlierType = MarkerType.Circle;
            this.MedianPointSize = 2;
            this.MeanPointSize = 2;
            this.WhiskerWidth = 0.5;
            this.LineStyle = LineStyle.Solid;
            this.ShowMedianAsDot = false;
            this.ShowMeanAsDot = false;
            this.ShowBox = true;
        }

        /// <summary>
        /// Gets or sets the width of the boxes (specified in x-axis units).
        /// </summary>
        /// <value>The width of the boxes.</value>
        public double BoxWidth { get; set; }

        /// <summary>
        /// Gets or sets the fill color. If <c>null</c>, this color will be automatically set.
        /// </summary>
        /// <value>The fill color.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the box plot items.
        /// </summary>
        /// <value>The items.</value>
        public IList<BoxPlotItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the size of the median point.
        /// </summary>
        /// <remarks>This property is only used when ShowMedianAsDot = true.</remarks>
        public double MedianPointSize { get; set; }

        /// <summary>
        /// Gets or sets the median thickness, relative to the StrokeThickness.
        /// </summary>
        /// <value>The median thickness.</value>
        public double MedianThickness { get; set; }

        /// <summary>
        /// Gets or sets the size of the mean point.
        /// </summary>
        /// <remarks>This property is only used when ShowMeanAsDot = true.</remarks>
        public double MeanPointSize { get; set; }

        /// <summary>
        /// Gets or sets the mean thickness, relative to the StrokeThickness.
        /// </summary>
        /// <value>The mean thickness.</value>
        public double MeanThickness { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the outlier circles (specified in points).
        /// </summary>
        /// <value>The size of the outlier.</value>
        public double OutlierSize { get; set; }

        /// <summary>
        /// Gets or sets the tracker format string for the outliers.
        /// </summary>
        /// <value>The tracker format string for the outliers.</value>
        /// <remarks>Use {0} for series title, {1} for x- and {2} for y-value.</remarks>
        public string OutlierTrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the type of the outliers.
        /// </summary>
        /// <value>The type of the outliers.</value>
        /// <remarks>MarkerType.Custom is currently not supported.</remarks>
        public MarkerType OutlierType { get; set; }

        /// <summary>
        /// Gets or sets the a custom polygon outline for the outlier markers. Set <see cref="OutlierType" /> to <see cref="OxyPlot.MarkerType.Custom" /> to use this property.
        /// </summary>
        /// <value>A polyline. The default is <c>null</c>.</value>
        public ScreenPoint[] OutlierOutline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the boxes.
        /// </summary>
        public bool ShowBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the median as a dot.
        /// </summary>
        public bool ShowMedianAsDot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the mean as a dot.
        /// </summary>
        public bool ShowMeanAsDot { get; set; }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        /// <value>The stroke color.</value>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the width of the whiskers (relative to the BoxWidth).
        /// </summary>
        /// <value>The width of the whiskers.</value>
        public double WhiskerWidth { get; set; }

        /// <summary>
        /// Gets the list of items that should be rendered.
        /// </summary>
        protected IList<BoxPlotItem> ActualItems
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourceItems : this.Items;
            }
        }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (this.XAxis == null || this.YAxis == null)
            {
                return null;
            }

            double minimumDistance = double.MaxValue;
            TrackerHitResult result = null;
            foreach (var item in this.ActualItems)
            {
                foreach (var outlier in item.Outliers)
                {
                    var sp = this.Transform(item.X, outlier);
                    double d = (sp - point).LengthSquared;
                    if (d < minimumDistance)
                    {
                        result = new TrackerHitResult
                        {
                            Series = this,
                            DataPoint = new DataPoint(item.X, outlier),
                            Position = sp,
                            Item = item,
                            Text =
                                StringHelper.Format(
                                    this.ActualCulture,
                                    this.OutlierTrackerFormatString,
                                    item,
                                    this.Title,
                                    this.XAxis.Title ?? DefaultXAxisTitle,
                                    this.XAxis.GetValue(item.X),
                                    outlier)
                        };
                        minimumDistance = d;
                    }
                }

                var hitPoint = DataPoint.Undefined;

                // check if we are inside the box rectangle
                var rect = this.GetBoxRect(item);
                if (rect.Contains(point))
                {
                    var dp = this.InverseTransform(point);
                    hitPoint = new DataPoint(item.X, dp.Y);
                    minimumDistance = 0;
                }

                var topWhisker = this.Transform(item.X, item.UpperWhisker);
                var bottomWhisker = this.Transform(item.X, item.LowerWhisker);

                // check if we are near the line
                var p = ScreenPointHelper.FindPointOnLine(point, topWhisker, bottomWhisker);
                double d2 = (p - point).LengthSquared;
                if (d2 < minimumDistance)
                {
                    hitPoint = this.InverseTransform(p);
                    minimumDistance = d2;
                }

                if (hitPoint.IsDefined())
                {
                    result = new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = hitPoint,
                        Position = this.Transform(hitPoint),
                        Item = item,
                        Text =
                            StringHelper.Format(
                                this.ActualCulture,
                                this.TrackerFormatString,
                                item,
                                this.Title,
                                this.XAxis.Title ?? DefaultXAxisTitle,
                                this.XAxis.GetValue(item.X),
                                this.YAxis.GetValue(item.UpperWhisker),
                                this.YAxis.GetValue(item.BoxTop),
                                this.YAxis.GetValue(item.Median),
                                this.YAxis.GetValue(item.BoxBottom),
                                this.YAxis.GetValue(item.LowerWhisker),
                                this.YAxis.GetValue(item.Mean))
                    };
                }
            }

            if (minimumDistance < double.MaxValue)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified item contains a valid point.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="xaxis">The x axis.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns><c>true</c> if the point is valid; otherwise, <c>false</c> .</returns>
        public virtual bool IsValidPoint(BoxPlotItem item, Axis xaxis, Axis yaxis)
        {
            return !double.IsNaN(item.X) && !double.IsInfinity(item.X) && !item.Values.Any(double.IsNaN)
                   && !item.Values.Any(double.IsInfinity) && (xaxis != null && xaxis.IsValidValue(item.X))
                   && (yaxis != null && item.Values.All(yaxis.IsValidValue));
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.ActualItems.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            var outlierScreenPoints = new List<ScreenPoint>();
            var halfBoxWidth = this.BoxWidth * 0.5;
            var halfWhiskerWidth = halfBoxWidth * this.WhiskerWidth;
            var strokeColor = this.GetSelectableColor(this.Stroke);
            var fillColor = this.GetSelectableFillColor(this.Fill);

            var dashArray = this.LineStyle.GetDashArray();

            foreach (var item in this.ActualItems)
            {
                // Add the outlier points
                outlierScreenPoints.AddRange(item.Outliers.Select(outlier => this.Transform(item.X, outlier)));

                var topWhiskerTop = this.Transform(item.X, item.UpperWhisker);
                var topWhiskerBottom = this.Transform(item.X, item.BoxTop);
                var bottomWhiskerTop = this.Transform(item.X, item.BoxBottom);
                var bottomWhiskerBottom = this.Transform(item.X, item.LowerWhisker);

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { topWhiskerTop, topWhiskerBottom },
                        0,
                        strokeColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        dashArray,
                        LineJoin.Miter);
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { bottomWhiskerTop, bottomWhiskerBottom },
                        0,
                        strokeColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        dashArray,
                        LineJoin.Miter);
                }

                // Draw the whiskers
                if (this.WhiskerWidth > 0)
                {
                    var topWhiskerLine1 = this.Transform(item.X - halfWhiskerWidth, item.UpperWhisker);
                    var topWhiskerLine2 = this.Transform(item.X + halfWhiskerWidth, item.UpperWhisker);
                    var bottomWhiskerLine1 = this.Transform(item.X - halfWhiskerWidth, item.LowerWhisker);
                    var bottomWhiskerLine2 = this.Transform(item.X + halfWhiskerWidth, item.LowerWhisker);

                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { topWhiskerLine1, topWhiskerLine2 },
                        0,
                        strokeColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        null,
                        LineJoin.Miter);
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { bottomWhiskerLine1, bottomWhiskerLine2 },
                        0,
                        strokeColor,
                        this.StrokeThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        null,
                        LineJoin.Miter);
                }

                if (this.ShowBox)
                {
                    // Draw the box
                    var rect = this.GetBoxRect(item);
                    rc.DrawClippedRectangle(
                        clippingRect, 
                        rect, fillColor, 
                        strokeColor, 
                        this.StrokeThickness, 
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
                }

                if (!this.ShowMedianAsDot)
                {
                    // Draw the median line
                    var medianLeft = this.Transform(item.X - halfBoxWidth, item.Median);
                    var medianRight = this.Transform(item.X + halfBoxWidth, item.Median);
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { medianLeft, medianRight },
                        0,
                        strokeColor,
                        this.StrokeThickness * this.MedianThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        null,
                        LineJoin.Miter);
                }
                else
                {
                    var mc = this.Transform(item.X, item.Median);
                    if (clippingRect.Contains(mc))
                    {
                        var ellipseRect = new OxyRect(
                            mc.X - this.MedianPointSize,
                            mc.Y - this.MedianPointSize,
                            this.MedianPointSize * 2,
                            this.MedianPointSize * 2);
                        rc.DrawEllipse(ellipseRect, fillColor, OxyColors.Undefined, 0, this.EdgeRenderingMode);
                    }
                }

                if (!this.ShowMeanAsDot && !double.IsNaN(item.Mean))
                {
                    // Draw the median line
                    var meanLeft = this.Transform(item.X - halfBoxWidth, item.Mean);
                    var meanRight = this.Transform(item.X + halfBoxWidth, item.Mean);
                    rc.DrawClippedLine(
                        clippingRect,
                        new[] { meanLeft, meanRight },
                        0,
                        strokeColor,
                        this.StrokeThickness * this.MeanThickness,
                        this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                        LineStyle.Dash.GetDashArray(),
                        LineJoin.Miter);
                }
                else if (!double.IsNaN(item.Mean))
                {
                    var mc = this.Transform(item.X, item.Mean);
                    if (clippingRect.Contains(mc))
                    {
                        var ellipseRect = new OxyRect(
                            mc.X - this.MeanPointSize,
                            mc.Y - this.MeanPointSize,
                            this.MeanPointSize * 2,
                            this.MeanPointSize * 2);
                        rc.DrawEllipse(ellipseRect, fillColor, OxyColors.Undefined, 0, this.EdgeRenderingMode);
                    }
                }
            }

            if (this.OutlierType != MarkerType.None)
            {
                // Draw the outlier(s)
                var markerSizes = outlierScreenPoints.Select(o => this.OutlierSize).ToList();
                rc.DrawMarkers(
                    clippingRect,
                    outlierScreenPoints,
                    this.OutlierType,
                    this.OutlierOutline,
                    markerSizes,
                    fillColor,
                    strokeColor,
                    this.StrokeThickness,
                    this.EdgeRenderingMode);
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
            double ybottom = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.7);
            double ytop = legendBox.Top + ((legendBox.Bottom - legendBox.Top) * 0.3);
            double ymid = (ybottom + ytop) * 0.5;

            var halfBoxWidth = legendBox.Width * 0.24;
            var halfWhiskerWidth = halfBoxWidth * this.WhiskerWidth;
            const double LegendStrokeThickness = 1;
            var strokeColor = this.GetSelectableColor(this.Stroke);
            var fillColor = this.GetSelectableFillColor(this.Fill);

            // render the legend with EdgeRenderingMode.PreferGeometricAccuracy, because otherwise the fine geometry can look 'weird'
            rc.DrawLine(
                new[] { new ScreenPoint(xmid, legendBox.Top), new ScreenPoint(xmid, ytop) },
                strokeColor,
                LegendStrokeThickness,
                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy),
                LineStyle.Solid.GetDashArray(),
                LineJoin.Miter);

            rc.DrawLine(
                new[] { new ScreenPoint(xmid, ybottom), new ScreenPoint(xmid, legendBox.Bottom) },
                strokeColor,
                LegendStrokeThickness,
                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy),
                LineStyle.Solid.GetDashArray(),
                LineJoin.Miter);

            if (this.WhiskerWidth > 0)
            {
                // top whisker
                rc.DrawLine(
                    new[]
                        {
                            new ScreenPoint(xmid - halfWhiskerWidth, legendBox.Bottom),
                            new ScreenPoint(xmid + halfWhiskerWidth, legendBox.Bottom)
                        },
                    strokeColor,
                    LegendStrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy),
                    LineStyle.Solid.GetDashArray(),
                    LineJoin.Miter);

                // bottom whisker
                rc.DrawLine(
                    new[]
                        {
                            new ScreenPoint(xmid - halfWhiskerWidth, legendBox.Top),
                            new ScreenPoint(xmid + halfWhiskerWidth, legendBox.Top)
                        },
                    strokeColor,
                    LegendStrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy),
                    LineStyle.Solid.GetDashArray(),
                    LineJoin.Miter);
            }

            if (this.ShowBox)
            {
                // box
                rc.DrawRectangle(
                    new OxyRect(xmid - halfBoxWidth, ytop, 2 * halfBoxWidth, ybottom - ytop),
                    fillColor,
                    strokeColor,
                    LegendStrokeThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy));
            }

            // median
            if (!this.ShowMedianAsDot)
            {
                rc.DrawLine(
                    new[] { new ScreenPoint(xmid - halfBoxWidth, ymid), new ScreenPoint(xmid + halfBoxWidth, ymid) },
                    strokeColor,
                    LegendStrokeThickness * this.MedianThickness,
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy),
                    LineStyle.Solid.GetDashArray(),
                    LineJoin.Miter);
            }
            else
            {
                var ellipseRect = new OxyRect(
                    xmid - this.MedianPointSize,
                    ymid - this.MedianPointSize,
                    this.MedianPointSize * 2,
                    this.MedianPointSize * 2);
                rc.DrawEllipse(ellipseRect, fillColor, OxyColors.Undefined, 0, this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferGeometricAccuracy));
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

            var sourceAsListOfT = this.ItemsSource as IEnumerable<BoxPlotItem>;
            if (sourceAsListOfT != null)
            {
                this.itemsSourceItems = sourceAsListOfT.ToList();
                this.ownsItemsSourceItems = false;
                return;
            }

            this.ClearItemsSourceItems();

            this.itemsSourceItems.AddRange(this.ItemsSource.OfType<BoxPlotItem>());
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.ActualItems);
        }

        /// <summary>
        /// Updates the max and min of the series.
        /// </summary>
        /// <param name="items">The items.</param>
        protected void InternalUpdateMaxMin(IList<BoxPlotItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            double minx = this.MinX;
            double miny = this.MinY;
            double maxx = this.MaxX;
            double maxy = this.MaxY;

            foreach (var pt in items)
            {
                if (!this.IsValidPoint(pt, this.XAxis, this.YAxis))
                {
                    continue;
                }

                var x = pt.X;
                if (x < minx || double.IsNaN(minx))
                {
                    minx = x;
                }

                if (x > maxx || double.IsNaN(maxx))
                {
                    maxx = x;
                }

                foreach (var y in pt.Values)
                {
                    if (y < miny || double.IsNaN(miny))
                    {
                        miny = y;
                    }

                    if (y > maxy || double.IsNaN(maxy))
                    {
                        maxy = y;
                    }
                }
            }

            this.MinX = minx;
            this.MinY = miny;
            this.MaxX = maxx;
            this.MaxY = maxy;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            if (this.ItemsSource != null || this.ActualItems == null || this.ActualItems.Count == 0)
            {
                return base.GetItem(i);
            }

            return this.ActualItems[i];
        }

        /// <summary>
        /// Gets the screen rectangle for the box.
        /// </summary>
        /// <param name="item">The box item.</param>
        /// <returns>A rectangle.</returns>
        private OxyRect GetBoxRect(BoxPlotItem item)
        {
            var halfBoxWidth = this.BoxWidth * 0.5;

            var p1 = this.Transform(item.X - halfBoxWidth, item.BoxTop);
            var p2 = this.Transform(item.X + halfBoxWidth, item.BoxBottom);

            return new OxyRect(p1, p2);
        }

        /// <summary>
        /// Clears or creates the <see cref="itemsSourceItems"/> list.
        /// </summary>
        private void ClearItemsSourceItems()
        {
            if (!this.ownsItemsSourceItems || this.itemsSourceItems == null)
            {
                this.itemsSourceItems = new List<BoxPlotItem>();
            }
            else
            {
                this.itemsSourceItems.Clear();
            }

            this.ownsItemsSourceItems = true;
        }
    }
}
