//-----------------------------------------------------------------------
// <copyright file="StemSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// StemSeries is used to plot discrete data in a stemplot.
    /// </summary>
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Stemplot
    ///   http://www.mathworks.com/help/techdoc/ref/stem.html
    /// </remarks>
    public class StemSeries : LineSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StemSeries" /> class.
        /// </summary>
        public StemSeries()
        {
            this.Base = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StemSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public StemSeries(string title)
            : base(title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StemSeries"/> class.
        /// </summary>
        /// <param name="color">
        /// The color of the line stroke.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness (optional).
        /// </param>
        /// <param name="title">
        /// The title (optional).
        /// </param>
        public StemSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : base(color, strokeThickness, title)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Base.
        /// </summary>
        public double Base { get; set; }

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
            if (interpolate)
            {
                return null;
            }

            TrackerHitResult result = null;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i < this.points.Count; i++)
            {
                IDataPoint p1 = this.points[i];
                var basePoint = new DataPoint(p1.X, this.Base);
                ScreenPoint sp1 = AxisBase.Transform(p1, this.XAxis, this.YAxis);
                ScreenPoint sp2 = AxisBase.Transform(basePoint, this.XAxis, this.YAxis);

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
                    result = new TrackerHitResult(
                        this, 
                        new DataPoint(p1.X, p1.Y), 
                        new ScreenPoint(sp1.x, sp1.y), 
                        this.GetItem(this.ItemsSource, i), 
                        null);
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
            if (this.points.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            OxyRect clippingRect = this.GetClippingRect();

            // Transform all points to screen coordinates
            // Render the line when invalid points occur
            var markerPoints = new List<ScreenPoint>();
            foreach (DataPoint point in this.points)
            {
                if (!this.IsValidPoint(point, this.XAxis, this.YAxis))
                {
                    continue;
                }

                ScreenPoint p0 = this.XAxis.Transform(point.X, this.Base, this.YAxis);
                ScreenPoint p1 = this.XAxis.Transform(point.X, point.Y, this.YAxis);

                if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                {
                    rc.DrawClippedLine(
                        new[] { p0, p1 }, 
                        clippingRect, 
                        minDistSquared, 
                        this.Color, 
                        this.StrokeThickness, 
                        this.LineStyle, 
                        this.LineJoin, 
                        false);
                }

                markerPoints.Add(p1);
            }

            if (this.MarkerType != MarkerType.None)
            {
                rc.DrawMarkers(
                    markerPoints, 
                    clippingRect, 
                    this.MarkerType, 
                    this.MarkerOutline, 
                    new[] { this.MarkerSize }, 
                    this.MarkerFill, 
                    this.MarkerStroke, 
                    this.MarkerStrokeThickness);
            }
        }

        #endregion
    }
}
