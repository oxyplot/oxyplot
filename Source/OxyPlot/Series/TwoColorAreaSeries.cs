// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorAreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a two-color area series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a two-color area series.
    /// </summary>
    public class TwoColorAreaSeries : TwoColorLineSeries
    {
        /// <summary>
        /// The second list of points representing limit line.
        /// </summary>
        private readonly List<DataPoint> points2 = new List<DataPoint>();

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorAreaSeries" /> class.
        /// </summary>
        public TwoColorAreaSeries()
        {
            this.Fill = OxyColors.Automatic;
            this.Fill2 = OxyColors.Automatic;

            this.MarkerFill2 = OxyColors.Automatic;
            this.MarkerStroke2 = OxyColors.Automatic;
        }

        /// <summary>
        /// Gets or sets the area fill color above the limit line.
        /// </summary>
        /// <value>The fill above the limit line.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the area fill color below the limit line.
        /// </summary>
        /// <value>The fill below the limit line.</value>
        public OxyColor Fill2 { get; set; }

        /// <summary>
        /// Gets the actual fill color above the limit line.
        /// </summary>
        /// <value>The actual fill above the limit line.</value>
        public OxyColor ActualFill
        {
            get
            {
                return this.Fill.GetActualColor(OxyColor.FromAColor(100, this.ActualColor));
            }
        }

        /// <summary>
        /// Gets the actual fill color below the limit line.
        /// </summary>
        /// <value>The actual fill below the limit line.</value>
        public OxyColor ActuallFill2
        {
            get
            {
                return this.Fill2.GetActualColor(OxyColor.FromAColor(100, this.ActualColor2));
            }
        }

        /// <summary>
        /// Gets or sets the marker fill color which is below the limit line. The default is <see cref="OxyColors.Automatic" />.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill2 { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke which is below the limit line. The default is <c>OxyColors.Automatic</c>.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke2 { get; set; }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result;

            if (interpolate && this.CanTrackerInterpolatePoints)
            {
                result = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
            }
            else
            {
                result = this.GetNearestPointInternal(this.ActualPoints, point);
            }

            if (result != null)
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
            }

            return result;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualPoints = this.ActualPoints;
            var actualPoints2 = this.points2;

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
            IList<ScreenPoint> pts0 = new List<ScreenPoint>();

            for (int i = 0; i < n0; i++)
            {
                pts0.Add(this.XAxis.Transform(actualPoints[i].X, actualPoints[i].Y, this.YAxis));
            }

            int n1 = actualPoints2.Count;
            IList<ScreenPoint> pts1 = new ScreenPoint[n1];

            for (int i = 0; i < n1; i++)
            {
                int j = n1 - 1 - i;
                pts1[j] = this.XAxis.Transform(actualPoints2[i].X, actualPoints2[i].Y, this.YAxis);
            }

            if (this.Smooth)
            {
                var rpts0 = ScreenPointHelper.ResamplePoints(pts0, this.MinimumSegmentLength);

                // var rpts1 = ScreenPointHelper.ResamplePoints(pts1, this.MinimumSegmentLength);
                pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25);

                // pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25);                
            }

            var dashArray = this.ActualDashArray;
            var dashArray2 = this.ActualDashArray2;

            var limit = this.YAxis.Transform(this.Limit);

            if (limit < clippingRect.Top)
            {
                limit = clippingRect.Top;
            }

            if (limit > clippingRect.Bottom)
            {
                limit = clippingRect.Bottom;
            }

            var markerSizes = new[] { this.MarkerSize };

            var bottom = clippingRect.Bottom;
            var top = clippingRect.Top;

            clippingRect = new OxyRect(clippingRect.Left, limit, clippingRect.Width, bottom - limit);

            // draw the clipped lines belove the limit line 
            rc.DrawClippedLine(
                clippingRect,
                pts0,
                minDistSquared,
                this.GetSelectableColor(this.ActualColor2),
                this.StrokeThickness,
                dashArray2,
                this.LineJoin,
                false);

            // combine the two lines and draw the clipped area
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts1);
            pts.AddRange(pts0);

            // fill the area belove the limit line
            rc.DrawClippedPolygon(clippingRect, pts, minDistSquared, this.GetSelectableFillColor(this.ActuallFill2), OxyColors.Undefined);

            // draw the markers on line belove the limit line
            rc.DrawMarkers(
                clippingRect,
                pts0,
                this.MarkerType,
                null,
                markerSizes,
                this.MarkerFill2,
                this.MarkerStroke2,
                this.MarkerStrokeThickness,
                1);

            clippingRect = new OxyRect(clippingRect.Left, top, clippingRect.Width, limit - top);

            // draw the clipped lines above the limit line
            rc.DrawClippedLine(
                clippingRect,
                pts0,
                minDistSquared,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                dashArray,
                this.LineJoin,
                false);

            // fill the area above the limit line
            rc.DrawClippedPolygon(clippingRect, pts, minDistSquared, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);

            // draw the markers on line above the limit line
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
            rc.DrawLine(pts0, this.GetSelectableColor(this.ActualColor2), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawLine(pts1, this.GetSelectableColor(this.ActualColor), this.StrokeThickness, this.ActualLineStyle.GetDashArray());
            rc.DrawPolygon(pts, this.GetSelectableFillColor(this.ActualFill), OxyColors.Undefined);
        }

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();

            // update points from the limit line
            if (this.ActualPoints != null)
            {
                this.points2.Clear();

                if (this.ActualPoints.Count > 1)
                {
                    this.points2.Add(new DataPoint(this.ActualPoints.Min(el => el.X), this.Limit));
                    this.points2.Add(new DataPoint(this.ActualPoints.Max(el => el.X), this.Limit));
                }
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.points2);
        }
    }
}