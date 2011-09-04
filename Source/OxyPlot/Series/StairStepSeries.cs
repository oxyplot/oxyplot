// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   StairStepSeries is used to create stairstep graphs.
//   http://www.mathworks.com/help/techdoc/ref/stairs.html
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// StairStepSeries is used to create stairstep graphs.
    /// http://www.mathworks.com/help/techdoc/ref/stairs.html
    /// </summary>
    public class StairStepSeries : LineSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries"/> class.
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
        /// Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// The interpolate.
        /// </param>
        /// <returns>
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result = null;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < this.Points.Count; i++)
            {
                IDataPoint p1 = this.Points[i];
                IDataPoint p2 = this.Points[i + 1];
                ScreenPoint sp1 = this.XAxis.Transform(p1.X, p1.Y, this.YAxis);
                ScreenPoint sp2 = this.XAxis.Transform(p2.X, p1.Y, this.YAxis);

                double sp21X = sp2.x - sp1.x;
                double sp21Y = sp2.y - sp1.y;
                double u1 = (point.x - sp1.x) * sp21X + (point.y - sp1.y) * sp21Y;
                double u2 = sp21X * sp21X + sp21Y * sp21Y;
                double ds = sp21X * sp21X + sp21Y * sp21Y;

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0;
                    u2 = 1;
                }

                if (u2 == 0)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0 || u > 1)
                {
                    continue; // outside line
                }

                double sx = sp1.x + u * sp21X;
                double sy = sp1.y + u * sp21Y;

                double dx = point.x - sx;
                double dy = point.y - sy;
                double distance = dx * dx + dy * dy;

                if (distance < minimumDistance)
                {
                    double px = p1.X + u * (p2.X - p1.X);
                    double py = p1.Y;
                    result = new TrackerHitResult(
                        this, new DataPoint(px, py), new ScreenPoint(sx, sy), this.GetItem(this.ItemsSource, i), null);
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

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            OxyRect clippingRect = this.GetClippingRect();

            Action<IList<ScreenPoint>, IList<ScreenPoint>> renderPoints = (lpts, mpts) =>
                {
                    // clip the line segments with the clipping rectangle
                    if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                    {
                        rc.DrawClippedLine(
                            lpts,
                            clippingRect,
                            minDistSquared,
                            this.Color,
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
            foreach (IDataPoint point in this.Points)
            {
                if (!this.IsValidPoint(point, this.XAxis, this.YAxis))
                {
                    renderPoints(linePoints, markerPoints);
                    linePoints.Clear();
                    markerPoints.Clear();
                    previousY = double.NaN;
                    continue;
                }

                ScreenPoint transformedPoint = AxisBase.Transform(point, this.XAxis, this.YAxis);
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