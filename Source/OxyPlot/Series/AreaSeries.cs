// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an area series that fills the polygon defined by two sets of points or one set of points and a constant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an area series that fills the polygon defined by two sets of points or one set of points and a constant.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// The second list of points.
        /// </summary>
        private readonly List<DataPoint> points2 = new List<DataPoint>();

        /// <summary>
        /// The secondary data points from the items source.
        /// </summary>
        private readonly List<DataPoint> itemsSourcePoints2 = new List<DataPoint>();

        /// <summary>
        /// Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            this.Reverse2 = true;
            this.Color2 = OxyColors.Automatic;
            this.Fill = OxyColors.Automatic;
        }

        /// <summary>
        /// Gets or sets a constant value for the area definition.
        /// This is used if DataFieldBase and BaselineValues are <c>null</c>.
        /// </summary>
        /// <value>The baseline.</value>
        public double ConstantY2 { get; set; }

        /// <summary>
        /// Gets or sets the second X data field.
        /// </summary>
        public string DataFieldX2 { get; set; }

        /// <summary>
        /// Gets or sets the second Y data field.
        /// </summary>
        public string DataFieldY2 { get; set; }

        /// <summary>
        /// Gets or sets the color of the second line.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color2 { get; set; }

        /// <summary>
        /// Gets the actual color of the second line.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor2
        {
            get
            {
                return this.Color2.GetActualColor(this.ActualColor);
            }
        }

        /// <summary>
        /// Gets or sets the area fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets the actual fill color.
        /// </summary>
        /// <value>The actual fill.</value>
        public OxyColor ActualFill
        {
            get
            {
                return this.Fill.GetActualColor(OxyColor.FromAColor(100, this.ActualColor));
            }
        }

        /// <summary>
        /// Gets the second list of points.
        /// </summary>
        /// <value>The second list of points.</value>
        public List<DataPoint> Points2
        {
            get
            {
                return this.points2;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the second
        /// data collection should be reversed.
        /// The first dataset is not reversed, and normally
        /// the second dataset should be reversed to get a
        /// closed polygon.
        /// </summary>
        public bool Reverse2 { get; set; }

        /// <summary>
        /// Gets the second list of points.
        /// </summary>
        /// <value>The second list of points.</value>
        protected List<DataPoint> ActualPoints2
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourcePoints2 : this.points2;
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
            TrackerHitResult result1, result2;
            if (interpolate && this.CanTrackerInterpolatePoints)
            {
                result1 = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
                result2 = this.GetNearestInterpolatedPointInternal(this.ActualPoints2, point);
            }
            else
            {
                result1 = this.GetNearestPointInternal(this.ActualPoints, point);
                result2 = this.GetNearestPointInternal(this.ActualPoints2, point);
            }

            TrackerHitResult result;
            if (result1 != null && result2 != null)
            {
                double dist1 = result1.Position.DistanceTo(point);
                double dist2 = result2.Position.DistanceTo(point);
                result = dist1 < dist2 ? result1 : result2;
            }
            else
            {
                result = result1 ?? result2;
            }

            if (result != null)
            {
                result.Text = this.Format(
                    this.TrackerFormatString,
                    result.Item,
                    this.Title,
                    this.XAxis.Title ?? XYAxisSeries.DefaultXAxisTitle,
                    this.XAxis.GetValue(result.DataPoint.X),
                    this.YAxis.Title ?? XYAxisSeries.DefaultYAxisTitle,
                    this.YAxis.GetValue(result.DataPoint.Y));
            }

            return result;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            var actualPoints = this.ActualPoints;
            var actualPoints2 = this.ActualPoints2;
            int n0 = actualPoints.Count;
            if (n0 == 0)
            {
                return;
            }

            this.VerifyAxes();

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            // Transform all points to screen coordinates
            IList<ScreenPoint> pts0 = new ScreenPoint[n0];
            for (int i = 0; i < n0; i++)
            {
                pts0[i] = this.XAxis.Transform(actualPoints[i].X, actualPoints[i].Y, this.YAxis);
            }

            int n1 = actualPoints2.Count;
            IList<ScreenPoint> pts1 = new ScreenPoint[n1];
            for (int i = 0; i < n1; i++)
            {
                int j = this.Reverse2 ? n1 - 1 - i : i;
                pts1[j] = this.XAxis.Transform(actualPoints2[i].X, actualPoints2[i].Y, this.YAxis);
            }

            if (this.Smooth)
            {
                var rpts0 = ScreenPointHelper.ResamplePoints(pts0, this.MinimumSegmentLength);
                var rpts1 = ScreenPointHelper.ResamplePoints(pts1, this.MinimumSegmentLength);

                pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25);
                pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25);
            }

            var dashArray = this.ActualDashArray;

            // draw the clipped lines
            rc.DrawClippedLine(
                clippingRect,
                pts0,
                minDistSquared,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                dashArray,
                this.LineJoin,
                false);
            rc.DrawClippedLine(
                clippingRect,
                pts1,
                minDistSquared,
                this.GetSelectableColor(this.ActualColor2),
                this.StrokeThickness,
                dashArray,
                this.LineJoin,
                false);

            // combine the two lines and draw the clipped area
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts1);
            pts.AddRange(pts0);

            // pts = SutherlandHodgmanClipping.ClipPolygon(clippingRect, pts);
            rc.DrawClippedPolygon(clippingRect, pts, minDistSquared, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);

            var markerSizes = new[] { this.MarkerSize };

            // draw the markers on top
            rc.DrawMarkers(
                clippingRect,
                pts0,
                this.MarkerType,
                null,
                markerSizes,
                this.MarkerFill,
                this.MarkerStroke,
                this.MarkerStrokeThickness,
                1);
            rc.DrawMarkers(
                clippingRect,
                pts1,
                this.MarkerType,
                null,
                markerSizes,
                this.MarkerFill,
                this.MarkerStroke,
                this.MarkerStrokeThickness,
                1);

            rc.ResetClip();
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the
        /// specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double y0 = (legendBox.Top * 0.2) + (legendBox.Bottom * 0.8);
            double y1 = (legendBox.Top * 0.4) + (legendBox.Bottom * 0.6);
            double y2 = (legendBox.Top * 0.8) + (legendBox.Bottom * 0.2);

            var pts0 = new[] { new ScreenPoint(legendBox.Left, y0), new ScreenPoint(legendBox.Right, y0) };
            var pts1 = new[] { new ScreenPoint(legendBox.Right, y2), new ScreenPoint(legendBox.Left, y1) };
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts0);
            pts.AddRange(pts1);
            rc.DrawLine(pts0, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawLine(pts1, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawPolygon(pts, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
        }

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

            this.itemsSourcePoints2.Clear();

            // Using reflection on DataFieldX2 and DataFieldY2
            ReflectionExtensions.AddRange(this.itemsSourcePoints2, this.ItemsSource, this.DataFieldX2, this.DataFieldY2);
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.ActualPoints2);
        }
    }
}