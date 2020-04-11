// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSegmentSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a line series where the points collection define line segments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Represents a line series where the points collection define line segments.
    /// </summary>
    public class LineSegmentSeries : LineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegmentSeries" /> class.
        /// </summary>
        public LineSegmentSeries()
        {
            this.ShowVerticals = true;
            this.Epsilon = 1e-8;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show vertical lines where there is no gap in x-coordinate.
        /// </summary>
        /// <value><c>true</c> if verticals should be shown; otherwise, <c>false</c>.</value>
        public bool ShowVerticals { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate gap tolerance.
        /// </summary>
        /// <value>The epsilon value.</value>
        public double Epsilon { get; set; }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (Points.Count == 0)
            {
                return;
            }

            if (Points.Count % 2 != 0)
            {
                throw new InvalidOperationException("The number of points should be even.");
            }

            if (this.XAxis == null || this.YAxis == null)
            {
                throw new InvalidOperationException("Axis has not been defined.");
            }

            var clippingRect = this.GetClippingRect();

            var screenPoints = Points.Select(this.Transform).ToList();
            var verticalLines = new List<ScreenPoint>();

            for (int i = 0; i < screenPoints.Count; i += 2)
            {
                if (screenPoints[i].DistanceToSquared(screenPoints[i + 1]) < this.StrokeThickness)
                {
                    screenPoints[i] = new ScreenPoint(screenPoints[i].X - (this.StrokeThickness * 0.5), screenPoints[i].Y);
                    screenPoints[i + 1] = new ScreenPoint(screenPoints[i].X + (this.StrokeThickness * 0.5), screenPoints[i].Y);
                }

                if (this.ShowVerticals && i > 0 && Math.Abs(screenPoints[i - 1].X - screenPoints[i].X) < this.Epsilon)
                {
                    verticalLines.Add(screenPoints[i - 1]);
                    verticalLines.Add(screenPoints[i]);
                }
            }

            if (this.StrokeThickness > 0)
            {
                if (this.LineStyle != LineStyle.None)
                {
                    rc.DrawClippedLineSegments(clippingRect, screenPoints, this.ActualColor, this.StrokeThickness, this.EdgeRenderingMode, this.LineStyle.GetDashArray(), this.LineJoin);
                }

                rc.DrawClippedLineSegments(clippingRect, verticalLines, this.ActualColor, this.StrokeThickness / 3, this.EdgeRenderingMode, LineStyle.Dash.GetDashArray(), this.LineJoin);
            }

            rc.DrawMarkers(screenPoints, clippingRect, this.MarkerType, null, this.MarkerSize, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness, this.EdgeRenderingMode);
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var points = this.Points;

            if (points == null)
            {
                return null;
            }

            var spn = default(ScreenPoint);
            var dpn = default(DataPoint);
            double index = -1;

            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                var p1 = points[i];
                var p2 = points[i + 1];
                if (!this.IsValidPoint(p1) || !this.IsValidPoint(p2))
                {
                    continue;
                }

                var sp1 = this.Transform(p1);
                var sp2 = this.Transform(p2);

                // Find the nearest point on the line segment.
                var spl = ScreenPointHelper.FindPointOnLine(point, sp1, sp2);

                if (ScreenPoint.IsUndefined(spl))
                {
                    // P1 && P2 coincident
                    continue;
                }

                double l2 = (point - spl).LengthSquared;

                if (l2 < minimumDistance)
                {
                    double u = (spl - sp1).Length / (sp2 - sp1).Length;
                    dpn = new DataPoint(p1.X + (u * (p2.X - p1.X)), p1.Y + (u * (p2.Y - p1.Y)));
                    spn = spl;
                    minimumDistance = l2;
                    index = i + u;
                }
            }

            if (minimumDistance < double.MaxValue)
            {
                return new TrackerHitResult
                {
                    Series = this,
                    DataPoint = dpn,
                    Position = spn,
                    Item = this.GetItem((int)index),
                    Index = index
                };
            }

            return null;
        }
    }
}
