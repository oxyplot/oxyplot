// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// The AreaSeries class fills the polygon defined by two sets of points.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        #region Constants and Fields

        /// <summary>
        ///   The second list of points.
        /// </summary>
        protected List<IDataPoint> points2 = new List<IDataPoint>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            this.Reverse2 = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a constant value for the area definition.
        ///   This is used if DataFieldBase and BaselineValues are null.
        /// </summary>
        /// <value>The baseline.</value>
        public double ConstantY2 { get; set; }

        /// <summary>
        ///   Gets or sets the second X data field.
        /// </summary>
        public string DataFieldX2 { get; set; }

        /// <summary>
        ///   Gets or sets the second Y data field.
        /// </summary>
        public string DataFieldY2 { get; set; }

        /// <summary>
        ///   Gets or sets the area fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        ///   Gets the second list of points.
        /// </summary>
        /// <value>The second list of points.</value>
        public List<IDataPoint> Points2
        {
            get
            {
                return this.points2;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the second 
        ///   data collection should be reversed.
        ///   The first dataset is not reversed, and normally
        ///   the second dataset should be reversed to get a 
        ///   closed polygon.
        /// </summary>
        public bool Reverse2 { get; set; }

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
            int index;
            TrackerHitResult result1 = null;
            TrackerHitResult result2 = null;
            IDataPoint dpn;
            ScreenPoint spn1;
            ScreenPoint spn2;

            if (interpolate)
            {
                if (this.GetNearestInterpolatedPointInternal(this.points, point, out dpn, out spn1, out index))
                {
                    object item = this.GetItem(index);
                    result1 = new TrackerHitResult(this, dpn, spn1, item);
                }

                if (this.GetNearestInterpolatedPointInternal(this.points2, point, out dpn, out spn2, out index))
                {
                    object item = this.GetItem(index);
                    result2 = new TrackerHitResult(this, dpn, spn2, item);
                }
            }
            else
            {
                if (this.GetNearestPointInternal(this.points, point, out dpn, out spn1, out index))
                {
                    object item = this.GetItem(index);
                    result1 = new TrackerHitResult(this, dpn, spn1, item);
                }

                if (this.GetNearestPointInternal(this.points2, point, out dpn, out spn2, out index))
                {
                    object item = this.GetItem(index);
                    result2 = new TrackerHitResult(this, dpn, spn2, item);
                }
            }

            if (result1 != null && result2 != null)
            {
                double dist1 = spn1.DistanceTo(point);
                double dist2 = spn2.DistanceTo(point);
                return dist1 < dist2 ? result1 : result2;
            }

            if (result1 != null)
            {
                return result1;
            }

            if (result2 != null)
            {
                return result2;
            }

            return null;
        }

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="rc">
        /// The rc.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (this.points.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis is not defined.");

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            OxyRect clippingRect = this.GetClippingRect();

            // Transform all points to screen coordinates
            int n0 = this.points.Count;
            IList<ScreenPoint> pts0 = new ScreenPoint[n0];
            for (int i = 0; i < n0; i++)
            {
                pts0[i] = this.XAxis.Transform(this.points[i].X, this.points[i].Y, this.YAxis);
            }

            int n1 = this.points2.Count;
            IList<ScreenPoint> pts1 = new ScreenPoint[n1];
            for (int i = 0; i < n1; i++)
            {
                int j = this.Reverse2 ? n1 - 1 - i : i;
                pts1[j] = this.XAxis.Transform(this.points2[i].X, this.points2[i].Y, this.YAxis);
            }

            if (this.Smooth)
            {
                IList<ScreenPoint> rpts0 = ScreenPointHelper.ResamplePoints(pts0, this.MinimumSegmentLength);
                IList<ScreenPoint> rpts1 = ScreenPointHelper.ResamplePoints(pts1, this.MinimumSegmentLength);

                pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25);
                pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25);
            }

            // draw the clipped lines
            rc.DrawClippedLine(
                pts0, 
                clippingRect, 
                minDistSquared, 
                this.Color, 
                this.StrokeThickness, 
                this.LineStyle, 
                this.LineJoin, 
                false);
            rc.DrawClippedLine(
                pts1, 
                clippingRect, 
                minDistSquared, 
                this.Color, 
                this.StrokeThickness, 
                this.LineStyle, 
                this.LineJoin, 
                false);

            // combine the two lines and draw the clipped area
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts1);
            pts.AddRange(pts0);

            // pts = SutherlandHodgmanClipping.ClipPolygon(clippingRect, pts);
            rc.DrawClippedPolygon(pts, clippingRect, minDistSquared, this.Fill, null);

            // draw the markers on top
            rc.DrawMarkers(
                pts0, 
                clippingRect, 
                this.MarkerType, 
                null, 
                new[] { this.MarkerSize }, 
                this.MarkerFill, 
                this.MarkerStroke, 
                this.MarkerStrokeThickness, 
                1);
            rc.DrawMarkers(
                pts1, 
                clippingRect, 
                this.MarkerType, 
                null, 
                new[] { this.MarkerSize }, 
                this.MarkerFill, 
                this.MarkerStroke, 
                this.MarkerStrokeThickness, 
                1);
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the 
        ///   specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The bounding rectangle of the legend box.
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double y0 = legendBox.Top * 0.2 + legendBox.Bottom * 0.8;
            double y1 = legendBox.Top * 0.4 + legendBox.Bottom * 0.6;
            double y2 = legendBox.Top * 0.8 + legendBox.Bottom * 0.2;

            var pts0 = new[] { new ScreenPoint(legendBox.Left, y0), new ScreenPoint(legendBox.Right, y0) };
            var pts1 = new[] { new ScreenPoint(legendBox.Right, y2), new ScreenPoint(legendBox.Left, y1) };
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts0);
            pts.AddRange(pts1);
            rc.DrawLine(pts0, this.Color, this.StrokeThickness, LineStyleHelper.GetDashArray(this.LineStyle));
            rc.DrawLine(pts1, this.Color, this.StrokeThickness, LineStyleHelper.GetDashArray(this.LineStyle));
            rc.DrawPolygon(pts, this.Fill, null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();

            if (this.ItemsSource == null)
            {
                return;
            }

            this.points2.Clear();

            // Using reflection on DataFieldX2 and DataFieldY2
            this.AddDataPoints(this.points2, this.ItemsSource, this.DataFieldX2, this.DataFieldY2);
        }

        /// <summary>
        /// The update max min.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.points2);
        }

        #endregion
    }
}