// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSegmentSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Diagnostics;
    using System.Linq;
    using OxyPlot;

    /// <summary>
    /// Represents a line series where the points collection define line segments.
    /// </summary>
    public class LineSegmentSeries : LineSeries
    {
        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (Points.Count == 0)
            {
                return;
            }

            Debug.Assert(Points.Count % 2 == 0, "The number of points should be even.");
            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            var clippingRect = GetClippingRect();

            var screenPoints = Points.Select(this.Transform).ToList();

            for (int i = 0; i < screenPoints.Count; i += 2)
            {
                if (screenPoints[i].DistanceToSquared(screenPoints[i + 1]) < this.StrokeThickness)
                {
                    screenPoints[i] = new ScreenPoint(screenPoints[i].X - (this.StrokeThickness * 0.5), screenPoints[i].Y);
                    screenPoints[i + 1] = new ScreenPoint(screenPoints[i].X + (this.StrokeThickness * 0.5), screenPoints[i].Y);
                }
            }

            rc.DrawClippedLineSegments(screenPoints, clippingRect, this.ActualColor, this.StrokeThickness, this.LineStyle, this.LineJoin, false);

            rc.DrawMarkers(screenPoints, clippingRect, this.MarkerType, null, this.MarkerSize, this.MarkerFill, this.MarkerStroke, this.MarkerStrokeThickness);
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
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
                if (!this.IsValidPoint(p1, this.XAxis, this.YAxis) || !this.IsValidPoint(p2, this.XAxis, this.YAxis))
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
                return new TrackerHitResult(this, dpn, spn, this.GetItem((int)index)) { Index = index };
            }

            return null;
        }
    }
}
