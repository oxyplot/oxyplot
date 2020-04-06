// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a base class for scatter series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    using OxyPlot.Axes;

    /// <summary>
    /// Provides a base class for scatter series.
    /// </summary>
    /// <typeparam name="T">The type of the data points.</typeparam>
    public abstract class ScatterSeries<T> : XYAxisSeries where T : ScatterPoint
    {
        /// <summary>
        /// The default color-axis title
        /// </summary>
        private const string DefaultColorAxisTitle = "Value";

        /// <summary>
        /// The list of data points.
        /// </summary>
        private readonly List<T> points = new List<T>();

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultMarkerFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries{T}"/> class.
        /// </summary>
        protected ScatterSeries()
        {
            this.MarkerFill = OxyColors.Automatic;
            this.MarkerSize = 5;
            this.MarkerType = MarkerType.Square;
            this.MarkerStroke = OxyColors.Automatic;
            this.MarkerStrokeThickness = 1;
            this.LabelMargin = 6;
        }

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>A list of <see cref="ScatterPoint" />.</value>
        /// <remarks>If the <see cref="ItemsSeries.ItemsSource" /> is specified, this list will not be used.</remarks>
        public List<T> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// Gets or sets the label format string. The default is <c>null</c> (no labels).
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label margins. The default is <c>6</c>.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets a function that maps from elements in the <see cref="ItemsSeries.ItemsSource" /> to <see cref="ScatterPoint" /> points to be rendered.
        /// </summary>
        /// <value>The mapping function. The default is <c>null</c>.</value>
        /// <remarks>Example: series1.Mapping = item => new DataPoint(((MyType)item).Time,((MyType)item).Value);
        /// </remarks>
        public Func<object, T> Mapping { get; set; }

        /// <summary>
        /// Gets or sets the size of the 'binning' feature. 
        /// If this number is greater than 1, bins of the specified is created for both x and y directions. Only one point will be drawn in each bin.
        /// </summary>
        /// <value>
        /// The size of the bins. The default is <c>0</c> - no binning.
        /// </value>
        public int BinSize { get; set; }

        /// <summary>
        /// Gets the actual color axis.
        /// </summary>
        /// <value>A <see cref="IColorAxis" />.</value>
        /// <remarks>This is used to map scatter point values to colors. Use the <see cref="ColorAxisKey" /> to specify a color axis. 
        /// If the <see cref="ColorAxisKey" /> is not specified, the first <see cref="IColorAxis" /> of the <see cref="PlotModel" /> will be used.</remarks>
        public IColorAxis ColorAxis { get; private set; }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key. The default is <c>null</c>.</value>
        /// <remarks>If set to <c>null</c>, the first <see cref="IColorAxis" /> of the <see cref="PlotModel" /> will be used. 
        /// Make sure that the points contains values.
        /// If your <see cref="PlotModel" /> contains a <see cref="IColorAxis" />, but you don't want to use a color axis, set the value to <c>string.Empty</c> or some other key that is not in use.</remarks>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that specifies X coordinates in the <see cref="ItemsSeries.ItemsSource" /> elements.
        /// </summary>
        /// <value>The name of the property. The default is <c>null</c>.</value>
        public string DataFieldX { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that specifies Y coordinates in the <see cref="ItemsSeries.ItemsSource" /> elements.
        /// </summary>
        /// <value>The name of the property. The default is <c>null</c>.</value>
        public string DataFieldY { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that specifies the size in the <see cref="ItemsSeries.ItemsSource" /> elements.
        /// </summary>
        /// <value>The name of the property. The default is <c>null</c>.</value>
        public string DataFieldSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that specifies the tag in the <see cref="ItemsSeries.ItemsSource" /> elements.
        /// </summary>
        /// <value>The name of the property. The default is <c>null</c>.</value>
        public string DataFieldTag { get; set; }

        /// <summary>
        /// Gets or sets the name of the property that specifies the color value in the <see cref="ItemsSeries.ItemsSource" /> elements.
        /// </summary>
        /// <value>The name of the property. The default is <c>null</c>.</value>
        public string DataFieldValue { get; set; }

        /// <summary>
        /// Gets or sets the marker fill color. If <c>null</c>, this color will be automatically set.
        /// </summary>
        /// <value>The fill color of the markers. The default is <see cref="OxyColors.Automatic" />.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualMarkerFillColor
        {
            get { return this.MarkerFill.GetActualColor(this.defaultMarkerFillColor); }
        }

        /// <summary>
        /// Gets or sets the custom marker outline polygon. Set <see cref="MarkerType" /> to <see cref="T:MarkerType.Custom" /> to use this.
        /// </summary>
        /// <value>A polyline. The default is <c>null</c>.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker (same size for all items).
        /// </summary>
        /// <value>The size of the markers. The default is <c>5</c>.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke. The default is <see cref="OxyColors.Automatic" />.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets thickness of the the marker strokes.
        /// </summary>
        /// <value>The thickness. The default is <c>1</c>.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker. The default is <see cref="T:MarkerType.Square" />.</value>
        /// <remarks>If <see cref="T:MarkerType.Custom" /> is used, the <see cref="MarkerOutline" /> property must be specified.</remarks>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets the maximum value of the points.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets the minimum value of the points.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the actual points.
        /// </summary>
        /// <value>
        /// A read-only list of points.
        /// </value>
        public System.Collections.ObjectModel.ReadOnlyCollection<T> ActualPoints
        {
            get
            {
                return this.ActualPointsList != null ? new System.Collections.ObjectModel.ReadOnlyCollection<T>(this.ActualPointsList) : null;
            }
        }

        /// <summary>
        /// Gets the list of points that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        protected List<T> ActualPointsList
        {
            get
            {
                return this.ItemsSource != null ? this.ItemsSourcePoints : this.points;
            }
        }

        /// <summary>
        /// Gets or sets the data points from the items source.
        /// </summary>
        protected List<T> ItemsSourcePoints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ItemsSourcePoints" /> list can be modified.
        /// </summary>
        protected bool OwnsItemsSourcePoints { get; set; }

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

            if (interpolate)
            {
                return null;
            }

            var actualPoints = this.ActualPointsList;
            if (actualPoints == null || actualPoints.Count == 0)
            {
                return null;
            }

            TrackerHitResult result = null;
            double minimumDistance = double.MaxValue;
            int i = 0;

            var xaxisTitle = this.XAxis.Title ?? DefaultXAxisTitle;
            var yaxisTitle = this.YAxis.Title ?? DefaultYAxisTitle;
            var colorAxisTitle = (this.ColorAxis != null ? ((Axis)this.ColorAxis).Title : null) ?? DefaultColorAxisTitle;

            foreach (var p in actualPoints)
            {
                if (p.X < this.XAxis.ActualMinimum || p.X > this.XAxis.ActualMaximum || p.Y < this.YAxis.ActualMinimum || p.Y > this.YAxis.ActualMaximum)
                {
                    i++;
                    continue;
                }

                var sp = this.Transform(p.X, p.Y);
                double dx = sp.x - point.x;
                double dy = sp.y - point.y;
                double d2 = (dx * dx) + (dy * dy);

                if (d2 < minimumDistance)
                {
                    var item = this.GetItem(i) ?? p;

                    object zvalue = null;

                    if (!double.IsNaN(p.Value) && !double.IsInfinity(p.Value))
                    {
                        zvalue = p.Value;
                    }

                    result = new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = new DataPoint(p.X, p.Y),
                        Position = sp,
                        Item = item,
                        Index = i,
                        Text =
                            StringHelper.Format(
                                this.ActualCulture,
                                this.TrackerFormatString,
                                item,
                                this.Title,
                                xaxisTitle,
                                this.XAxis.GetValue(p.X),
                                yaxisTitle,
                                this.YAxis.GetValue(p.Y),
                                colorAxisTitle,
                                zvalue),
                    };

                    minimumDistance = d2;
                }

                i++;
            }

            return result;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualPoints = this.ActualPointsList;

            if (actualPoints == null || actualPoints.Count == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            int n = actualPoints.Count;
            var allPoints = new List<ScreenPoint>(n);
            var allMarkerSizes = new List<double>(n);
            var selectedPoints = new List<ScreenPoint>();
            var selectedMarkerSizes = new List<double>(n);
            var groupPoints = new Dictionary<int, IList<ScreenPoint>>();
            var groupSizes = new Dictionary<int, IList<double>>();

            // check if any item of the series is selected
            bool isSelected = this.IsSelected();

            // Transform all points to screen coordinates
            for (int i = 0; i < n; i++)
            {
                var dp = new DataPoint(actualPoints[i].X, actualPoints[i].Y);

                // Skip invalid points
                if (!this.IsValidPoint(dp))
                {
                    continue;
                }

                double size = double.NaN;
                double value = double.NaN;

                var scatterPoint = actualPoints[i];
                if (scatterPoint != null)
                {
                    size = scatterPoint.Size;
                    value = scatterPoint.Value;
                }

                if (double.IsNaN(size))
                {
                    size = this.MarkerSize;
                }

                // Transform from data to screen coordinates
                var screenPoint = this.Transform(dp.X, dp.Y);

                if (isSelected && this.IsItemSelected(i))
                {
                    selectedPoints.Add(screenPoint);
                    selectedMarkerSizes.Add(size);
                    continue;
                }

                if (this.ColorAxis != null)
                {
                    if (double.IsNaN(value))
                    {
                        // The value is not defined, skip this point.
                        continue;
                    }

                    int group = this.ColorAxis.GetPaletteIndex(value);
                    if (!groupPoints.ContainsKey(group))
                    {
                        groupPoints.Add(group, new List<ScreenPoint>());
                        groupSizes.Add(group, new List<double>());
                    }

                    groupPoints[group].Add(screenPoint);
                    groupSizes[group].Add(size);
                }
                else
                {
                    allPoints.Add(screenPoint);
                    allMarkerSizes.Add(size);
                }
            }

            // Offset of the bins
            var binOffset = this.Transform(this.MinX, this.MaxY);

            rc.SetClip(clippingRect);

            if (this.ColorAxis != null)
            {
                // Draw the grouped (by color defined in ColorAxis) markers
                var markerIsStrokedOnly = this.MarkerType == MarkerType.Plus || this.MarkerType == MarkerType.Star || this.MarkerType == MarkerType.Cross;
                foreach (var group in groupPoints)
                {
                    var color = this.ColorAxis.GetColor(group.Key);
                    rc.DrawMarkers(
                        clippingRect,
                        group.Value,
                        this.MarkerType,
                        this.MarkerOutline,
                        groupSizes[group.Key],
                        this.MarkerFill.GetActualColor(color),
                        markerIsStrokedOnly ? color : this.MarkerStroke,
                        this.MarkerStrokeThickness,
                        this.EdgeRenderingMode,
                        this.BinSize,
                        binOffset);
                }
            }

            // Draw unselected markers
            rc.DrawMarkers(
                clippingRect,
                allPoints,
                this.MarkerType,
                this.MarkerOutline,
                allMarkerSizes,
                this.ActualMarkerFillColor,
                this.MarkerStroke,
                this.MarkerStrokeThickness,
                this.EdgeRenderingMode,
                this.BinSize,
                binOffset);

            // Draw the selected markers
            rc.DrawMarkers(
                clippingRect,
                selectedPoints,
                this.MarkerType,
                this.MarkerOutline,
                selectedMarkerSizes,
                this.PlotModel.SelectionColor,
                this.PlotModel.SelectionColor,
                this.MarkerStrokeThickness,
                this.EdgeRenderingMode,
                this.BinSize,
                binOffset);

            if (this.LabelFormatString != null)
            {
                // render point labels (not optimized for performance)
                this.RenderPointLabels(rc, clippingRect);
            }

            rc.ResetClip();
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;

            var midpt = new ScreenPoint(xmid, ymid);

            rc.DrawMarker(
                legendBox,
                midpt,
                this.MarkerType,
                this.MarkerOutline,
                this.MarkerSize,
                this.IsSelected() ? this.PlotModel.SelectionColor : this.ActualMarkerFillColor,
                this.IsSelected() ? this.PlotModel.SelectionColor : this.MarkerStroke,
                this.MarkerStrokeThickness,
                this.EdgeRenderingMode);
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            base.EnsureAxes();

            this.ColorAxis = this.ColorAxisKey != null ?
                             this.PlotModel.GetAxis(this.ColorAxisKey) as IColorAxis :
                             this.PlotModel.DefaultColorAxis as IColorAxis;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.MarkerFill.IsAutomatic())
            {
                this.defaultMarkerFillColor = this.PlotModel.GetDefaultColor();
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

            this.UpdateItemsSourcePoints();
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMinValue(this.ActualPointsList);
        }

        /// <summary>
        /// Renders the point labels.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        protected void RenderPointLabels(IRenderContext rc, OxyRect clippingRect)
        {
            var actualPoints = this.ActualPointsList;
            if (actualPoints == null || actualPoints.Count == 0)
            {
                return;
            }

            // TODO: share code with LineSeries
            int index = -1;
            foreach (var point in actualPoints)
            {
                index++;
                var dataPoint = new DataPoint(point.X, point.Y);
                if (!this.IsValidPoint(dataPoint))
                {
                    continue;
                }

                var pt = this.Transform(dataPoint) + new ScreenVector(0, -this.LabelMargin);

                if (!clippingRect.Contains(pt))
                {
                    continue;
                }

                var item = this.GetItem(index);
                var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, item, point.X, point.Y);

#if SUPPORTLABELPLACEMENT
                    switch (this.LabelPlacement)
                    {
                        case LabelPlacement.Inside:
                            pt = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Right;
                            break;
                        case LabelPlacement.Middle:
                            pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Center;
                            break;
                        case LabelPlacement.Base:
                            pt = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Left;
                            break;
                        default: // Outside
                            pt = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Left;
                            break;
                    }
#endif

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
                    VerticalAlignment.Bottom);
            }
        }

        /// <summary>
        /// Updates the Max/Min limits from the values in the specified point list.
        /// </summary>
        /// <param name="pts">The points.</param>
        protected void InternalUpdateMaxMinValue(List<T> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double minvalue = double.MaxValue;
            double maxx = double.MinValue;
            double maxy = double.MinValue;
            double maxvalue = double.MinValue;

            if (double.IsNaN(minx))
            {
                minx = double.MaxValue;
            }

            if (double.IsNaN(miny))
            {
                miny = double.MaxValue;
            }

            if (double.IsNaN(maxx))
            {
                maxx = double.MinValue;
            }

            if (double.IsNaN(maxy))
            {
                maxy = double.MinValue;
            }

            if (double.IsNaN(minvalue))
            {
                minvalue = double.MinValue;
            }

            if (double.IsNaN(maxvalue))
            {
                maxvalue = double.MinValue;
            }

            foreach (var pt in pts)
            {
                double x = pt.X;
                double y = pt.Y;

                // Check if the point is defined (the code below is faster than double.IsNaN)
#pragma warning disable 1718
                // ReSharper disable EqualExpressionComparison
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (x != x || y != y)
                // ReSharper restore CompareOfFloatsByEqualityOperator
                // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
                {
                    continue;
                }

                double value = pt.Value;

                if (x < minx)
                {
                    minx = x;
                }

                if (x > maxx)
                {
                    maxx = x;
                }

                if (y < miny)
                {
                    miny = y;
                }

                if (y > maxy)
                {
                    maxy = y;
                }

                if (value < minvalue)
                {
                    minvalue = value;
                }

                if (value > maxvalue)
                {
                    maxvalue = value;
                }
            }

            if (minx < double.MaxValue)
            {
                this.MinX = minx;
            }

            if (miny < double.MaxValue)
            {
                this.MinY = miny;
            }

            if (maxx > double.MinValue)
            {
                this.MaxX = maxx;
            }

            if (maxy > double.MinValue)
            {
                this.MaxY = maxy;
            }

            if (minvalue < double.MaxValue)
            {
                this.MinValue = minvalue;
            }

            if (maxvalue > double.MinValue)
            {
                this.MaxValue = maxvalue;
            }

            var colorAxis = this.ColorAxis as Axis;
            if (colorAxis != null)
            {
                colorAxis.Include(this.MinValue);
                colorAxis.Include(this.MaxValue);
            }
        }

        /// <summary>
        /// Updates the Max/Min limits from the values in the specified point list.
        /// </summary>
        /// <param name="pts">The points.</param>
        protected void InternalUpdateMaxMinValue(IList<ScatterPoint> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minvalue = double.NaN;
            double maxvalue = double.NaN;

            foreach (var pt in pts)
            {
                double value = pt.Value;

                if (value < minvalue || double.IsNaN(minvalue))
                {
                    minvalue = value;
                }

                if (value > maxvalue || double.IsNaN(maxvalue))
                {
                    maxvalue = value;
                }
            }

            this.MinValue = minvalue;
            this.MaxValue = maxvalue;

            var colorAxis = this.ColorAxis as Axis;
            if (colorAxis != null)
            {
                colorAxis.Include(this.MinValue);
                colorAxis.Include(this.MaxValue);
            }
        }

        /// <summary>
        /// Clears or creates the <see cref="ItemsSourcePoints"/> list.
        /// </summary>
        protected void ClearItemsSourcePoints()
        {
            if (!this.OwnsItemsSourcePoints || this.ItemsSourcePoints == null)
            {
                this.ItemsSourcePoints = new List<T>();
            }
            else
            {
                this.ItemsSourcePoints.Clear();
            }

            this.OwnsItemsSourcePoints = true;
        }

        /// <summary>
        /// Updates the <see cref="F:ItemsSourcePoints" /> from the <see cref="P:ItemsSource" /> and data fields.
        /// </summary>
        protected abstract void UpdateFromDataFields();

        /// <summary>
        /// Updates the points from the <see cref="ItemsSeries.ItemsSource" />.
        /// </summary>
        private void UpdateItemsSourcePoints()
        {
            // Use the Mapping property to generate the points
            if (this.Mapping != null)
            {
                this.ClearItemsSourcePoints();
                foreach (var item in this.ItemsSource)
                {
                    this.ItemsSourcePoints.Add(this.Mapping(item));
                }

                return;
            }

            var sourceAsListOfScatterPoints = this.ItemsSource as List<T>;
            if (sourceAsListOfScatterPoints != null)
            {
                this.ItemsSourcePoints = sourceAsListOfScatterPoints;
                this.OwnsItemsSourcePoints = false;
                return;
            }

            this.ClearItemsSourcePoints();

            var sourceAsEnumerableScatterPoints = this.ItemsSource as IEnumerable<T>;
            if (sourceAsEnumerableScatterPoints != null)
            {
                this.ItemsSourcePoints.AddRange(sourceAsEnumerableScatterPoints);
                return;
            }

            // If DataFieldX or DataFieldY is not set, try to get the points from the ItemsSource
            if (this.DataFieldX == null || this.DataFieldY == null)
            {
                foreach (var item in this.ItemsSource)
                {
                    if (item is T)
                    {
                        this.ItemsSourcePoints.Add((T)item);
                        continue;
                    }

                    var idpp = item as IScatterPointProvider;
                    if (idpp != null)
                    {
                        this.ItemsSourcePoints.Add((T)idpp.GetScatterPoint());
                    }
                }

                return;
            }

            // Use reflection to add scatter points
            this.UpdateFromDataFields();
        }
    }
}
