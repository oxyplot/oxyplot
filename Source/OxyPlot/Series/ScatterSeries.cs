// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a series for scatter plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for scatter plots.
    /// </summary>
    /// <remarks>See http://en.wikipedia.org/wiki/Scatter_plot</remarks>
    public class ScatterSeries : XYAxisSeries
    {
        /// <summary>
        /// The list of data points.
        /// </summary>
        private readonly List<ScatterPoint> points = new List<ScatterPoint>();

        /// <summary>
        /// The data points from the items source.
        /// </summary>
        private List<ScatterPoint> itemsSourcePoints;

        /// <summary>
        /// Specifies if the itemsSourcePoints list can be modified.
        /// </summary>
        private bool ownsItemsSourcePoints;

        /// <summary>
        /// The default fill color.
        /// </summary>
        private OxyColor defaultMarkerFillColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries" /> class.
        /// </summary>
        public ScatterSeries()
        {
            this.DataFieldSize = null;
            this.DataFieldValue = null;

            this.MarkerFill = OxyColors.Automatic;
            this.MarkerSize = 5;
            this.MarkerType = MarkerType.Square;
            this.MarkerStroke = OxyColors.Automatic;
            this.MarkerStrokeThickness = 1.0;
        }

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>A list of <see cref="ScatterPoint" />.</value>
        public List<ScatterPoint> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// Gets or sets the mapping delegate.
        /// Example: series1.Mapping = item => new DataPoint(((MyType)item).Time,((MyType)item).Value);
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, ScatterPoint> Mapping { get; set; }

        /// <summary>
        /// Gets or sets the screen resolution. If this number is greater than 1, bins of that size is created for both x and y directions. Only one point will be drawn in each bin.
        /// </summary>
        public int BinSize { get; set; }

        /// <summary>
        /// Gets the color axis.
        /// </summary>
        /// <value>The color axis.</value>
        /// <remarks>This is used to map scatter point values to colors.</remarks>
        public IColorAxis ColorAxis { get; private set; }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key.</value>
        /// <remarks>If set to <c>null</c>, the default color axis of the <see cref="PlotModel" /> will be used. Make sure that the points contains values.
        /// If your <see cref="PlotModel" /> contains a <see cref="IColorAxis" />, but you don't want to use a color axis, set the value to <c>string.Empty</c> or some other key that is not in use.</remarks>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the data field X.
        /// </summary>
        /// <value>The data field X.</value>
        public string DataFieldX { get; set; }

        /// <summary>
        /// Gets or sets the data field Y.
        /// </summary>
        /// <value>The data field Y.</value>
        public string DataFieldY { get; set; }

        /// <summary>
        /// Gets or sets the data field for the size.
        /// </summary>
        /// <value>The size data field.</value>
        public string DataFieldSize { get; set; }

        /// <summary>
        /// Gets or sets the tag data field.
        /// </summary>
        /// <value>The tag data field.</value>
        public string DataFieldTag { get; set; }

        /// <summary>
        /// Gets or sets the value data field.
        /// </summary>
        /// <value>The value data field.</value>
        public string DataFieldValue { get; set; }

        /// <summary>
        /// Gets or sets the marker fill color. If <c>null</c>, this color will be automatically set.
        /// </summary>
        /// <value>The marker fill color.</value>
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
        /// Gets or sets the marker outline polygon. Set MarkerType to Custom to use this.
        /// </summary>
        /// <value>The marker outline.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker (same size for all items).
        /// </summary>
        /// <value>The size of the markers.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke thickness.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        /// <remarks>If MarkerType.Custom is used, the MarkerOutline property must be specified.</remarks>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets the max value of the points.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets the min value of the points.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the list of points that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        protected List<ScatterPoint> ActualPoints
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourcePoints : this.points;
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

            if (interpolate)
            {
                return null;
            }

            TrackerHitResult result = null;
            double minimumDistance = double.MaxValue;
            int i = 0;

            var xaxisTitle = this.XAxis.Title ?? "X";
            var yaxisTitle = this.YAxis.Title ?? "Y";
            var colorAxisTitle = (this.ColorAxis != null ? ((Axis)this.ColorAxis).Title : null) ?? "Z";

            var formatString = TrackerFormatString;
            if (string.IsNullOrEmpty(this.TrackerFormatString))
            {
                // Create a default format string
                formatString = "{1}: {2}\n{3}: {4}";
                if (this.ColorAxis != null)
                {
                    formatString += "\n{5}: {6}";
                }
            }

            foreach (var p in this.ActualPoints)
            {
                if (p.X < this.XAxis.ActualMinimum || p.X > this.XAxis.ActualMaximum || p.Y < this.YAxis.ActualMinimum || p.Y > this.YAxis.ActualMaximum)
                {
                    i++;
                    continue;
                }

                var sp = this.XAxis.Transform(p.X, p.Y, this.YAxis);
                double dx = sp.x - point.x;
                double dy = sp.y - point.y;
                double d2 = (dx * dx) + (dy * dy);

                if (d2 < minimumDistance)
                {
                    var item = this.GetItem(i);

                    object xvalue = this.XAxis.GetValue(p.X);
                    object yvalue = this.YAxis.GetValue(p.Y);
                    object zvalue = null;

                    if (!double.IsNaN(p.Value) && !double.IsInfinity(p.Value))
                    {
                        zvalue = p.Value;
                    }

                    var text = this.Format(
                        formatString,
                        item,
                        this.Title,
                        xaxisTitle,
                        xvalue,
                        yaxisTitle,
                        yvalue,
                        colorAxisTitle,
                        zvalue);

                    result = new TrackerHitResult(this, new DataPoint(p.X, p.Y), sp, item, i, text);

                    minimumDistance = d2;
                }

                i++;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified point is valid.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="xaxis">The x axis.</param>
        /// <param name="yaxis">The y axis.</param>
        /// <returns><c>true</c> if the point is valid; otherwise, <c>false</c> .</returns>
        public virtual bool IsValidPoint(ScatterPoint pt, Axis xaxis, Axis yaxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.Y) && !double.IsInfinity(pt.Y)
                   && (xaxis != null && xaxis.IsValidValue(pt.X)) && (yaxis != null && yaxis.IsValidValue(pt.Y));
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            var actualPoints = this.ActualPoints;
            int n = actualPoints.Count;
            if (n == 0)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

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
                var screenPoint = this.XAxis.Transform(dp.X, dp.Y, this.YAxis);

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
            var binOffset = this.XAxis.Transform(this.MinX, this.MaxY, this.YAxis);

            rc.SetClip(clippingRect);

            if (this.ColorAxis != null)
            {
                // Draw the grouped (by color defined in ColorAxis) markers
                var markerIsStrokedOnly = this.MarkerType == MarkerType.Plus || this.MarkerType == MarkerType.Star || this.MarkerType == MarkerType.Cross;
                foreach (var group in groupPoints)
                {
                    var color = this.ColorAxis.GetColor(group.Key);
                    rc.DrawMarkers(
                        group.Value,
                        clippingRect,
                        this.MarkerType,
                        this.MarkerOutline,
                        groupSizes[group.Key],
                        color,
                        markerIsStrokedOnly ? color : this.MarkerStroke,
                        this.MarkerStrokeThickness,
                        this.BinSize,
                        binOffset);
                }
            }

            // Draw unselected markers
            rc.DrawMarkers(
                    allPoints,
                    clippingRect,
                    this.MarkerType,
                    this.MarkerOutline,
                    allMarkerSizes,
                    this.ActualMarkerFillColor,
                    this.MarkerStroke,
                    this.MarkerStrokeThickness,
                    this.BinSize,
                    binOffset);

            // Draw the selected markers
            rc.DrawMarkers(
                selectedPoints,
                clippingRect,
                this.MarkerType,
                this.MarkerOutline,
                selectedMarkerSizes,
                this.PlotModel.SelectionColor,
                this.PlotModel.SelectionColor,
                this.MarkerStrokeThickness,
                this.BinSize,
                binOffset);

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
                midpt,
                legendBox,
                this.MarkerType,
                this.MarkerOutline,
                this.MarkerSize,
                this.IsSelected() ? PlotModel.SelectionColor : this.ActualMarkerFillColor,
                this.IsSelected() ? PlotModel.SelectionColor : this.MarkerStroke,
                this.MarkerStrokeThickness);
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            base.EnsureAxes();

            this.ColorAxis = PlotModel.GetAxisOrDefault(this.ColorAxisKey, (Axis)PlotModel.DefaultColorAxis) as IColorAxis;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="model">The model.</param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.MarkerFill.IsAutomatic())
            {
                this.defaultMarkerFillColor = model.GetDefaultColor();
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
            this.InternalUpdateMaxMinValue(this.ActualPoints);
        }

        /// <summary>
        /// Updates the Max/Min limits from the values in the specified point list.
        /// </summary>
        /// <param name="pts">The points.</param>
        protected void InternalUpdateMaxMinValue(List<ScatterPoint> pts)
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
                // ReSharper disable EqualExpressionComparison
                // ReSharper disable CompareOfFloatsByEqualityOperator
#pragma warning disable 1718
                if (x != x || y != y)
                // ReSharper restore CompareOfFloatsByEqualityOperator
                // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
                {
                    continue;
                }

                double value = pt.value;

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
        /// Adds scatter points specified by a items source and data fields.
        /// </summary>
        /// <param name="target">The destination collection.</param>
        /// <param name="itemsSource">The items source.</param>
        /// <param name="dataFieldX">The data field x.</param>
        /// <param name="dataFieldY">The data field y.</param>
        /// <param name="dataFieldSize">The data field size.</param>
        /// <param name="dataFieldValue">The data field value.</param>
        /// <param name="dataFieldTag">The data field tag.</param>
        protected void AddScatterPoints(
            IList<ScatterPoint> target,
            IEnumerable itemsSource,
            string dataFieldX,
            string dataFieldY,
            string dataFieldSize,
            string dataFieldValue,
            string dataFieldTag)
        {
            var filler = new ListFiller<ScatterPoint>();
            filler.Add(dataFieldX, (item, value) => item.X = Convert.ToDouble(value));
            filler.Add(dataFieldY, (item, value) => item.Y = Convert.ToDouble(value));
            filler.Add(dataFieldSize, (item, value) => item.Size = Convert.ToDouble(value));
            filler.Add(dataFieldValue, (item, value) => item.Value = Convert.ToDouble(value));
            filler.Add(dataFieldTag, (item, value) => item.Tag = value);
            filler.FillT(target, itemsSource);
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
                double value = pt.value;

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
        /// Clears or creates the <see cref="itemsSourcePoints"/> list.
        /// </summary>
        private void ClearItemsSourcePoints()
        {
            if (!this.ownsItemsSourcePoints || this.itemsSourcePoints == null)
            {
                this.itemsSourcePoints = new List<ScatterPoint>();
            }
            else
            {
                this.itemsSourcePoints.Clear();
            }

            this.ownsItemsSourcePoints = true;
        }

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
                    this.itemsSourcePoints.Add(this.Mapping(item));
                }

                return;
            }

            var sourceAsListOfScatterPoints = this.ItemsSource as List<ScatterPoint>;
            if (sourceAsListOfScatterPoints != null)
            {
                this.itemsSourcePoints = sourceAsListOfScatterPoints;
                this.ownsItemsSourcePoints = false;
                return;
            }

            this.ClearItemsSourcePoints();

            var sourceAsEnumerableScatterPoints = this.ItemsSource as IEnumerable<ScatterPoint>;
            if (sourceAsEnumerableScatterPoints != null)
            {
                this.itemsSourcePoints.AddRange(sourceAsEnumerableScatterPoints);
                return;
            }

            // If DataFieldX or DataFieldY is not set, try to get the points from the ItemsSource
            if (this.DataFieldX == null || this.DataFieldY == null)
            {
                foreach (var item in this.ItemsSource)
                {
                    var point = item as ScatterPoint;
                    if (point != null)
                    {
                        this.itemsSourcePoints.Add(point);
                        continue;
                    }

                    var idpp = item as IScatterPointProvider;
                    if (idpp != null)
                    {
                        this.itemsSourcePoints.Add(idpp.GetScatterPoint());
                    }
                }

                return;
            }

            // Use reflection to add scatter points
            var filler = new ListFiller<ScatterPoint>();
            filler.Add(this.DataFieldX, (item, value) => item.X = Convert.ToDouble(value));
            filler.Add(this.DataFieldY, (item, value) => item.Y = Convert.ToDouble(value));
            filler.Add(this.DataFieldSize, (item, value) => item.Size = Convert.ToDouble(value));
            filler.Add(this.DataFieldValue, (item, value) => item.Value = Convert.ToDouble(value));
            filler.Add(this.DataFieldTag, (item, value) => item.Tag = value);
            filler.Fill(this.itemsSourcePoints, this.ItemsSource);
        }
    }
}