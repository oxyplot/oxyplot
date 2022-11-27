﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorSeries.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series that can be bound to a collection of VectorItems representing a vector field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axes;

    /// <summary>
    /// Represents a series that can be bound to a collection of <see cref="VectorItem"/>.
    /// </summary>
    public class VectorSeries : XYAxisSeries
    {
        /// <summary>
        /// The items originating from the items source.
        /// </summary>
        private List<VectorItem> actualItems;

        /// <summary>
        /// Specifies if the <see cref="actualItems" /> list can be modified.\
        /// </summary>
        private bool ownsActualItems;

        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}\nΔ{1}: {7}\nΔ{3}: {8}";

        /// <summary>
        /// The default color-axis title
        /// </summary>
        private const string DefaultColorAxisTitle = "Value";

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMapSeries" /> class.
        /// </summary>
        public VectorSeries()
        {
            this.HeadLength = 3;
            this.HeadWidth = 2;
            this.Color = OxyColors.Black;
            this.MinimumSegmentLength = 2;
            this.StrokeThickness = 2;
            this.LineStyle = LineStyle.Solid;
            this.LineJoin = LineJoin.Miter;

            this.TrackerFormatString = DefaultTrackerFormatString;
            this.LabelFormatString = "0.00";
            this.LabelFontSize = 0;
        }

        /// <summary>
        /// Gets or sets the color of the arrow.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the minimum value of the dataset.
        /// </summary>
        public double MinValue { get; private set; }

        /// <summary>
        /// Gets the maximum value of the dataset.
        /// </summary>
        public double MaxValue { get; private set; }

        /// <summary>
        /// Gets or sets the length of the head (relative to the stroke thickness) (the default value is 10).
        /// </summary>
        /// <value>The length of the head.</value>
        public double HeadLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the head (relative to the stroke thickness) (the default value is 3).
        /// </summary>
        /// <value>The width of the head.</value>
        public double HeadWidth { get; set; }

        /// <summary>
        /// Gets or sets the line join type.
        /// </summary>
        /// <value>The line join type.</value>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the start point of the arrow.
        /// </summary>
        /// <remarks>This property is overridden by the ArrowDirection property, if set.</remarks>
        public DataPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness (the default value is 2).
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of an interpolated line segment.
        /// Increasing this number will increase performance,
        /// but make the curve less accurate. The default is <c>2</c>.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        /// Gets or sets the 'veeness' of the arrow head (relative to thickness) (the default value is 0).
        /// </summary>
        /// <value>The 'veeness'.</value>
        public double Veeness { get; set; }

        /// <summary>
        /// Gets or sets the color axis.
        /// </summary>
        /// <value>The color axis.</value>
        public IColorAxis ColorAxis { get; protected set; }

        /// <summary>
        /// Gets or sets the color axis key.
        /// </summary>
        /// <value>The color axis key.</value>
        public string ColorAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the format string for the cell labels. The default value is <c>0.00</c>.
        /// </summary>
        /// <value>The format string.</value>
        /// <remarks>The label format string is only used when <see cref="LabelFontSize" /> is greater than 0.</remarks>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the font size of the labels. The default value is <c>0</c> (labels not visible).
        /// </summary>
        /// <value>The font size relative to the cell height.</value>
        public double LabelFontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to map from <see cref="ItemsSeries.ItemsSource" /> to <see cref="VectorItem" />. The default is <c>null</c>.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new VectorItem(new DataPoint((MyType)item).Time1, ((MyType)item).Value1), new DataPoint((MyType)item).Time2, ((MyType)item).Value2));</remarks>
        public Func<object, VectorItem> Mapping { get; set; }

        /// <summary>
        /// Gets the list of Vectors.
        /// </summary>
        /// <value>A list of <see cref="VectorItem" />. This list is used if <see cref="ItemsSeries.ItemsSource" /> is not set.</value>
        public List<VectorItem> Items { get; } = new List<VectorItem>();

        /// <summary>
        /// Gets the list of Vectors that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="VectorItem" />.</value>
        protected List<VectorItem> ActualItems => this.ItemsSource != null ? this.actualItems : this.Items;

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualRects = this.ActualItems;

            this.VerifyAxes();

            this.RenderVectors(rc, actualRects);
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

            this.UpdateActualItems();
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            var items = this.ActualItems;
            if (this.ItemsSource == null && items != null && i < items.Count)
            {
                return items[i];
            }

            return base.GetItem(i);
        }

        /// <summary>
        /// Clears or creates the <see cref="actualItems"/> list.
        /// </summary>
        private void ClearActualItems()
        {
            if (!this.ownsActualItems || this.actualItems == null)
            {
                this.actualItems = new List<VectorItem>();
            }
            else
            {
                this.actualItems.Clear();
            }

            this.ownsActualItems = true;
        }

        /// <summary>
        /// Updates the points from the <see cref="ItemsSeries.ItemsSource" />.
        /// </summary>
        private void UpdateActualItems()
        {
            // Use the Mapping property to generate the points
            if (this.Mapping != null)
            {
                this.ClearActualItems();
                foreach (var item in this.ItemsSource)
                {
                    this.actualItems.Add(this.Mapping(item));
                }

                return;
            }

            var sourceAsListOfDataRects = this.ItemsSource as List<VectorItem>;
            if (sourceAsListOfDataRects != null)
            {
                this.actualItems = sourceAsListOfDataRects;
                this.ownsActualItems = false;
                return;
            }

            this.ClearActualItems();

            var sourceAsEnumerableDataRects = this.ItemsSource as IEnumerable<VectorItem>;
            if (sourceAsEnumerableDataRects != null)
            {
                this.actualItems.AddRange(sourceAsEnumerableDataRects);
            }
        }

        /// <summary>
        /// Renders the points as line, broken line and markers.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="items">The Items to render.</param>
        protected void RenderVectors(IRenderContext rc, ICollection<VectorItem> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                OxyColor vectorColor;
                if (this.ColorAxis != null)
                {
                    vectorColor = this.ColorAxis.GetColor(item.Value);
                }
                else
                {
                    vectorColor = this.Color;
                }

                vectorColor = this.GetSelectableColor(vectorColor, i);

                this.DrawClippedVector(
                    rc,
                    item.Origin,
                    item.Direction,
                    vectorColor
                    );

                if (this.LabelFontSize > 0)
                {
                    rc.DrawText(
                        this.Transform(item.Origin), 
                        item.Value.ToString(this.LabelFormatString), 
                        this.ActualTextColor, 
                        this.ActualFont, 
                        this.LabelFontSize, 
                        this.ActualFontWeight, 
                        0, 
                        HorizontalAlignment.Center, 
                        VerticalAlignment.Middle);
                }

                i++;
            }
        }

        private void DrawClippedVector(IRenderContext rc, DataPoint point, DataVector vector, OxyColor color)
        {
            var points = new List<DataPoint>() { point, point + vector };
            var screenPoints = new List<ScreenPoint>();
            RenderingExtensions.TransformAndInterpolateLines(this, points, screenPoints, this.MinimumSegmentLength);

            if (screenPoints.Count >= 2)
            {
                this.DrawArrow(rc, screenPoints, screenPoints[screenPoints.Count - 1] - screenPoints[screenPoints.Count - 2], color);
            }
        }

        private static List<ScreenPoint> InterpolatePoints(Func<double, ScreenPoint> source, double minSegmentLength)
        {
            var minLengthSquared = minSegmentLength * minSegmentLength;

            var candidates = new Stack<double>();
            var candidatePoints = new Stack<ScreenPoint>();
            candidates.Push(1.0);
            candidatePoints.Push(source(1.0));

            var points = new List<ScreenPoint>();

            var last = 0.0;
            var lastPoint = source(0.0);
            points.Add(lastPoint);

            while (candidates.Count > 0)
            {
                var next = candidates.Peek();
                var nextPoint = candidatePoints.Peek();

                if (nextPoint.DistanceToSquared(lastPoint) < minLengthSquared)
                {
                    last = next;
                    lastPoint = nextPoint;
                    points.Add(nextPoint);

                    candidates.Pop();
                    candidatePoints.Pop();
                }
                else
                {
                    next = last + (next - last) / 2.0;
                    nextPoint = source(next);

                    candidates.Push(next);
                    candidatePoints.Push(nextPoint);
                }
            }

            return points;
        }

        /// <summary>
        /// Draws an arrow.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points along the arrow to draw, which will be modified.</param>
        /// <param name="direction">The direction in which the arrow head should face</param>
        /// <param name="color">The color of the arrow.</param>
        private void DrawArrow(IRenderContext rc, IList<ScreenPoint> points, ScreenVector direction, OxyColor color)
        {
            // draws a line with an arrowhead glued on the tip (the arrowhead does not point to the end point)

            var d = direction;
            d.Normalize();
            var n = new ScreenVector(d.Y, -d.X);

            var endPoint = points.Last();

            var veeness = d * this.Veeness * this.StrokeThickness;
            var p1 = endPoint + (d * this.HeadLength * this.StrokeThickness);
            var p2 = endPoint + (n * this.HeadWidth * this.StrokeThickness) - veeness;
            var p3 = endPoint - (n * this.HeadWidth * this.StrokeThickness) - veeness;

            var dashArray = this.LineStyle.GetDashArray();

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                rc.DrawLine(
                    points,
                    color,
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    this.LineJoin);

                rc.DrawPolygon(
                    new[] { p3, p1, p2, endPoint },
                    color,
                    OxyColors.Undefined,
                    0,
                    this.EdgeRenderingMode);
            }
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var p = this.InverseTransform(point);

            var colorAxis = this.ColorAxis as Axis;
            var colorAxisTitle = colorAxis?.Title ?? DefaultColorAxisTitle;

            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                var item = Utilities.Helpers.ArgMin(this.ActualItems, i => this.Transform(i.Origin).DistanceToSquared(point));
                p = item.Origin;

                return new TrackerHitResult
                {
                    Series = this,
                    DataPoint = p,
                    Position = this.Transform(p),
                    Item = null,
                    Index = -1,
                    Text = StringHelper.Format(
                    this.ActualCulture,
                    this.TrackerFormatString,
                    item,
                    this.Title,
                    this.XAxis.Title ?? DefaultXAxisTitle,
                    this.XAxis.GetValue(p.X),
                    this.YAxis.Title ?? DefaultYAxisTitle,
                    this.YAxis.GetValue(p.Y),
                    colorAxisTitle,
                    item.Value,
                    item.Direction.X,
                    item.Direction.Y)
                };
            }

            // if no vectors, return null
            return null;
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
        /// Updates the maximum and minimum values of the series for the x and y dimensions only.
        /// </summary>
        protected internal void UpdateMaxMinXY()
        {
            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinX = Math.Min(this.ActualItems.Min(r => r.Origin.X), this.ActualItems.Min(r => r.Direction.X));
                this.MaxX = Math.Max(this.ActualItems.Max(r => r.Origin.X), this.ActualItems.Max(r => r.Direction.X));
                this.MinY = Math.Min(this.ActualItems.Min(r => r.Origin.Y), this.ActualItems.Min(r => r.Direction.Y));
                this.MaxY = Math.Max(this.ActualItems.Max(r => r.Origin.Y), this.ActualItems.Max(r => r.Direction.Y));
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            var allDataPoints = new List<DataPoint>();
            allDataPoints.AddRange(this.ActualItems.Select(item => item.Origin));
            allDataPoints.AddRange(this.ActualItems.Select(item => item.Origin + item.Direction));
            this.InternalUpdateMaxMin(allDataPoints);

            this.UpdateMaxMinXY();

            if (this.ActualItems != null && this.ActualItems.Count > 0)
            {
                this.MinValue = this.ActualItems.Min(r => r.Value);
                this.MaxValue = this.ActualItems.Max(r => r.Value);
            }
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            base.UpdateAxisMaxMin();
            var colorAxis = this.ColorAxis as Axis;
            if (colorAxis != null)
            {
                colorAxis.Include(this.MinValue);
                colorAxis.Include(this.MaxValue);
            }
        }
    }
}
