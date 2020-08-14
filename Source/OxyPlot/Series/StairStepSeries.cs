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

            var clippingRect = this.GetClippingRect();
            var dashArray = this.ActualDashArray;
            var verticalLineDashArray = this.VerticalLineStyle.GetDashArray();
            var lineStyle = this.ActualLineStyle;
            var verticalStrokeThickness = double.IsNaN(this.VerticalStrokeThickness)
                                              ? this.StrokeThickness
                                              : this.VerticalStrokeThickness;

            var actualColor = this.GetSelectableColor(this.ActualColor);

            Action<IList<ScreenPoint>, IList<ScreenPoint>> renderPoints = (lpts, mpts) =>
                {
                    // clip the line segments with the clipping rectangle
                    if (this.StrokeThickness > 0 && lineStyle != LineStyle.None)
                    {
                        if (!verticalStrokeThickness.Equals(this.StrokeThickness) || this.VerticalLineStyle != lineStyle)
                        {
                            // TODO: change to array
                            var hlpts = new List<ScreenPoint>();
                            var vlpts = new List<ScreenPoint>();
                            for (int i = 0; i + 2 < lpts.Count; i += 2)
                            {
                                hlpts.Add(lpts[i]);
                                hlpts.Add(lpts[i + 1]);
                                vlpts.Add(lpts[i + 1]);
                                vlpts.Add(lpts[i + 2]);
                            }

                            rc.DrawClippedLineSegments(
                                clippingRect,
                                hlpts,
                                actualColor,
                                this.StrokeThickness,
                                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                                dashArray,
                                this.LineJoin);
                            rc.DrawClippedLineSegments(
                                clippingRect,
                                vlpts,
                                actualColor,
                                verticalStrokeThickness,
                                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                                verticalLineDashArray,
                                this.LineJoin);
                        }
                        else
                        {
                            rc.DrawClippedLine(
                                clippingRect,
                                lpts,
                                0,
                                actualColor,
                                this.StrokeThickness,
                                this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness),
                                dashArray,
                                this.LineJoin);
                        }
                    }

                    if (this.MarkerType != MarkerType.None)
                    {
                        rc.DrawMarkers(
                            clippingRect,
                            mpts,
                            this.MarkerType,
                            this.MarkerOutline,
                            new[] { this.MarkerSize },
                            this.ActualMarkerFill,
                            this.MarkerStroke,
                            this.MarkerStrokeThickness,
                            this.EdgeRenderingMode);
                    }
                };

            // Transform all points to screen coordinates
            // Render the line when invalid points occur
            var linePoints = new List<ScreenPoint>();
            var markerPoints = new List<ScreenPoint>();
            double previousY = double.NaN;
            foreach (var point in this.ActualPoints)
            {
                if (!this.IsValidPoint(point))
                {
                    renderPoints(linePoints, markerPoints);
                    linePoints.Clear();
                    markerPoints.Clear();
                    previousY = double.NaN;
                    continue;
                }

                var transformedPoint = this.Transform(point);
                if (!double.IsNaN(previousY))
                {
                    // Horizontal line from the previous point to the current x-coordinate
                    linePoints.Add(this.Transform(new DataPoint(point.X, previousY)));
                }

                linePoints.Add(transformedPoint);
                markerPoints.Add(transformedPoint);
                previousY = point.Y;
            }

            renderPoints(linePoints, markerPoints);

            if (this.LabelFormatString != null)
            {
                // render point labels (not optimized for performance)
                this.RenderPointLabels(rc, clippingRect);
            }
        }
    }
}
