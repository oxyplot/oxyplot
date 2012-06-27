// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series for stairstep graphs.
    /// </summary>
    /// <remarks>
    /// See http://www.mathworks.com/help/techdoc/ref/stairs.html
    /// </remarks>
    public class StairStepSeries : LineSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StairStepSeries" /> class.
        /// </summary>
        public StairStepSeries()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public StairStepSeries(string title)
            : base(title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries"/> class.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public StairStepSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : base(color, strokeThickness, title)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result = null;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < this.Points.Count; i++)
            {
                var p1 = this.Points[i];
                var p2 = this.Points[i + 1];
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
                double distance = (dx * dx) + (dy * dy);

                if (distance < minimumDistance)
                {
                    double px = p1.X + (u * (p2.X - p1.X));
                    double py = p1.Y;
                    result = new TrackerHitResult(this, new DataPoint(px, py), new ScreenPoint(sx, sy), this.GetItem(i), i);
                    minimumDistance = distance;
                }
            }

            return result;
        }

        /// <summary>
        /// Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The owner plot model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.Points.Count == 0)
            {
                return;
            }

            if (this.XAxis == null || this.YAxis == null)
            {
                Trace("Axis not defined.");
                return;
            }

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            var clippingRect = this.GetClippingRect();

            Action<IList<ScreenPoint>, IList<ScreenPoint>> renderPoints = (lpts, mpts) =>
                {
                    // clip the line segments with the clipping rectangle
                    if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                    {
                        rc.DrawClippedLine(
                            lpts,
                            clippingRect,
                            minDistSquared,
                            this.GetSelectableColor(this.ActualColor),
                            this.StrokeThickness,
                            this.LineStyle,
                            this.LineJoin,
                            false);
                    }

                    if (this.MarkerType != MarkerType.None)
                    {
                        rc.DrawMarkers(
                            mpts,
                            clippingRect,
                            this.MarkerType,
                            this.MarkerOutline,
                            new[] { this.MarkerSize },
                            this.MarkerFill,
                            this.MarkerStroke,
                            this.MarkerStrokeThickness);
                    }
                };

            // Transform all points to screen coordinates
            // Render the line when invalid points occur
            var linePoints = new List<ScreenPoint>();
            var markerPoints = new List<ScreenPoint>();
            double previousY = double.NaN;
            foreach (var point in this.Points)
            {
                if (!this.IsValidPoint(point, this.XAxis, this.YAxis))
                {
                    renderPoints(linePoints, markerPoints);
                    linePoints.Clear();
                    markerPoints.Clear();
                    previousY = double.NaN;
                    continue;
                }

                ScreenPoint transformedPoint = this.Transform(point);
                if (!double.IsNaN(previousY))
                {
                    linePoints.Add(new ScreenPoint(transformedPoint.X, previousY));
                }

                linePoints.Add(transformedPoint);
                markerPoints.Add(transformedPoint);
                previousY = transformedPoint.Y;
            }

            renderPoints(linePoints, markerPoints);
        }

        #endregion
    }
}