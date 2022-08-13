// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for stair step graphs.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series for stair step graphs.
    /// </summary>
    public class StairStepSeries : LineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "StairStepSeries" /> class.
        /// </summary>
        public StairStepSeries()
        {
            this.VerticalStrokeThickness = double.NaN;
            this.VerticalLineStyle = this.LineStyle;
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the vertical line segments.
        /// </summary>
        /// <value>The vertical stroke thickness.</value>
        /// <remarks>Set the value to NaN to use the StrokeThickness property for both horizontal and vertical segments.
        /// Using the VerticalStrokeThickness property will have a small performance hit.</remarks>
        public double VerticalStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the line style of the vertical line segments.
        /// </summary>
        /// <value>The vertical line style.</value>
        public LineStyle VerticalLineStyle { get; set; }

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

            // http://paulbourke.net/geometry/pointlineplane/
            double minimumDistanceSquared = 16 * 16;

            // snap to the nearest point
            var result = this.GetNearestPointInternal(this.ActualPoints, point);
            if (!interpolate && result != null && result.Position.DistanceToSquared(point) < minimumDistanceSquared)
            {
                result.Text = StringHelper.Format(
                    this.ActualCulture, 
                    this.TrackerFormatString,
                    result.Item,
                    this.Title,
                    this.XAxis.Title ?? XYAxisSeries.DefaultXAxisTitle,
                    this.XAxis.GetValue(result.DataPoint.X),
                    this.YAxis.Title ?? XYAxisSeries.DefaultYAxisTitle,
                    this.YAxis.GetValue(result.DataPoint.Y));
                return result;
            }

            result = null;

            // find the nearest point on the horizontal line segments
            int n = this.ActualPoints.Count;
            for (int i = 0; i < n; i++)
            {
                var p1 = this.ActualPoints[i];
                var p2 = this.ActualPoints[i + 1 < n ? i + 1 : i];
                var sp1 = this.Transform(p1.X, p1.Y);
                var sp2 = this.Transform(p2.X, p1.Y);

                double spdx = sp2.x - sp1.x;
                double spdy = sp2.y - sp1.y;
                double u1 = ((point.x - sp1.x) * spdx) + ((point.y - sp1.y) * spdy);
                double u2 = (spdx * spdx) + (spdy * spdy);
                double ds = (spdx * spdx) + (spdy * spdy);

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0;
                    u2 = 1;
                }

                if (Math.Abs(u2) < double.Epsilon)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0 || u > 1)
                {
                    continue; // outside line
                }

                double sx = sp1.x + (u * spdx);
                double sy = sp1.y + (u * spdy);

                double dx = point.x - sx;
                double dy = point.y - sy;
                double distanceSquared = (dx * dx) + (dy * dy);

                if (distanceSquared < minimumDistanceSquared)
                {
                    double px = p1.X + (u * (p2.X - p1.X));
                    double py = p1.Y;
                    var item = this.GetItem(i);
                    result = new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = new DataPoint(px, py),
                        Position = new ScreenPoint(sx, sy),
                        Item = item,
                        Index = i,
                        Text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, this.Title, this.XAxis.Title ?? DefaultXAxisTitle, this.XAxis.GetValue(px), this.YAxis.Title ?? DefaultYAxisTitle, this.YAxis.GetValue(py))
                    };
                    minimumDistanceSquared = distanceSquared;
                }
            }

            return result;
        }

        /// <summary>
        /// Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.ActualPoints == null || this.ActualPoints.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var dashArray = this.ActualDashArray;
            var verticalLineDashArray = this.VerticalLineStyle.GetDashArray();
            var lineStyle = this.ActualLineStyle;
            var verticalStrokeThickness = double.IsNaN(this.VerticalStrokeThickness)
                                              ? this.StrokeThickness
                                              : this.VerticalStrokeThickness;

            var actualColor = this.GetSelectableColor(this.ActualColor);
            var edgeRenderingMode = this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness);

            Action<IList<ScreenPoint>, IList<ScreenPoint>> renderPoints = (linePoints, markerPoints) =>
                {
                    if (this.StrokeThickness > 0 && lineStyle != LineStyle.None)
                    {
                        // TODO: Need ActualVerticalLineStyle; without it, when VerticalLineStyle is
                        // Automatic, this code will not take the fast path even when it could.
                        if (!verticalStrokeThickness.Equals(this.StrokeThickness) || this.VerticalLineStyle != lineStyle)
                        {
                            var hLinePoints = new List<ScreenPoint>(linePoints.Count);
                            var vLinePoints = new List<ScreenPoint>(linePoints.Count);
                            if (linePoints.Count >= 2)
                            {
                                hLinePoints.Add(linePoints[0]);
                                hLinePoints.Add(linePoints[1]);
                                for (int i = 1; i + 2 < linePoints.Count; i += 2)
                                {
                                    vLinePoints.Add(linePoints[i]);
                                    vLinePoints.Add(linePoints[i + 1]);
                                    hLinePoints.Add(linePoints[i + 1]);
                                    hLinePoints.Add(linePoints[i + 2]);
                                }
                            }

                            rc.DrawLineSegments(
                                hLinePoints,
                                actualColor,
                                this.StrokeThickness,
                                edgeRenderingMode,
                                dashArray,
                                this.LineJoin);
                            rc.DrawLineSegments(
                                vLinePoints,
                                actualColor,
                                verticalStrokeThickness,
                                edgeRenderingMode,
                                verticalLineDashArray,
                                this.LineJoin);
                        }
                        else
                        {
                            rc.DrawLine(
                                linePoints,
                                actualColor,
                                this.StrokeThickness,
                                edgeRenderingMode,
                                dashArray,
                                this.LineJoin);
                        }
                    }

                    if (this.MarkerType != MarkerType.None)
                    {
                        rc.DrawMarkers(
                            markerPoints,
                            this.MarkerType,
                            this.MarkerOutline,
                            new[] { this.MarkerSize },
                            this.ActualMarkerFill,
                            this.MarkerStroke,
                            this.MarkerStrokeThickness,
                            this.EdgeRenderingMode);
                    }
                };

            var points = this.ActualPoints;

            int offset = 0;
            double xClipMax = double.MaxValue;

            if (this.IsXMonotonic)
            {
                double xClipMin = this.XAxis.ClipMinimum;
                xClipMax = this.XAxis.ClipMaximum;

                this.WindowStartIndex = this.UpdateWindowStartIndex(points, point => point.X, xClipMin, this.WindowStartIndex);
                offset = this.WindowStartIndex;
            }

            var linePoints = new List<ScreenPoint>();
            var markerPoints = new List<ScreenPoint>();

            for (int i = offset; i < points.Count;)
            {
                bool hasValid = this.FindNextValidSegment(points, i, xClipMax, out int validOffset, out int endOffset);
                if (!hasValid)
                    break;

                ScreenPoint transformedPoint = default;
                DataPoint previousPoint = DataPoint.Undefined;
                bool xIncreased = false;
                bool xDecreased = false;

                for (i = validOffset; i < endOffset; ++i)
                {
                    var point = points[i];

                    // For performance we want to draw as few lines as possible.  Assuming sane
                    // (orthogonal and monotonic) axis transformations, as long as point Xs are all
                    // either increasing or decreasing and point Ys are equal, we can coalesce the
                    // horizontal lines and omit the vertical lines.

                    xIncreased |= point.X > previousPoint.X;
                    xDecreased |= point.X < previousPoint.X;

                    if (xIncreased && xDecreased)
                    {
                        // Vertical line end point/horizontal line start point.
                        linePoints.Add(this.Transform(previousPoint));

                        // Horizontal line end point/vertical line start point.
                        linePoints.Add(this.Transform(previousPoint));

                        xIncreased = point.X > previousPoint.X;
                        xDecreased = point.X < previousPoint.X;
                    }

                    transformedPoint = this.Transform(point);

                    if (point.Y != previousPoint.Y)
                    {
                        // Vertical line start point.
                        if (!double.IsNaN(previousPoint.Y))
                            linePoints.Add(this.Transform(new DataPoint(point.X, previousPoint.Y)));

                        // Vertical line end point/horizontal line start point.
                        linePoints.Add(transformedPoint);

                        xIncreased = false;
                        xDecreased = false;
                    }

                    previousPoint = point;

                    markerPoints.Add(transformedPoint);
                }

                if (i < points.Count &&
                    this.XAxis.IsValidValue(points[i].X))
                {
                    // Horizontal line continues until next point (either invalid Y or clipped X).
                    linePoints.Add(this.Transform(new DataPoint(points[i].X, previousPoint.Y)));
                }
                else
                {
                    // Horizontal line ends at last point.
                    linePoints.Add(transformedPoint);
                }

                renderPoints(linePoints, markerPoints);

                linePoints.Clear();
                markerPoints.Clear();
            }

            if (this.LabelFormatString != null)
            {
                // render point labels (not optimized for performance)
                this.RenderPointLabels(rc);
            }
        }

        private bool FindNextValidSegment(List<DataPoint> points, int offset, double xClipMax, out int validOffset, out int endOffset)
        {
            // Skip invalid points.
            for (; ; ++offset)
            {
                if (offset >= points.Count)
                {
                    validOffset = default;
                    endOffset = default;
                    return false;
                }

                var point = points[offset];
                if (point.X > xClipMax)
                {
                    validOffset = default;
                    endOffset = default;
                    return false;
                }

                if (this.IsValidPoint(point))
                    break;
            }

            validOffset = offset;

            // Skip valid points.
            for (; ; ++offset)
            {
                if (offset >= points.Count)
                    break;

                var point = points[offset];

                if (!this.IsValidPoint(point))
                    break;

                if (point.X > xClipMax)
                    break;
            }

            endOffset = offset;
            return true;
        }
    }
}
