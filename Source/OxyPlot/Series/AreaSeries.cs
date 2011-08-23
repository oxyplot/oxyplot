using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OxyPlot
{
    /// <summary>
    ///   The AreaSeries class fills the polygon defined by two sets of points.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        protected List<IDataPoint> points2 = new List<IDataPoint>();

        public AreaSeries()
        {
            Reverse2 = true;
        }

        /// <summary>
        ///   Gets or sets the area fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        ///   Gets or sets the second X data field.
        /// </summary>
        public string DataFieldX2 { get; set; }

        /// <summary>
        ///   Gets or sets the second Y data field.
        /// </summary>
        public string DataFieldY2 { get; set; }

        /// <summary>
        ///   Gets or sets a constant value for the area definition.
        ///   This is used if DataFieldBase and BaselineValues are null.
        /// </summary>
        /// <value>The baseline.</value>
        public double ConstantY2 { get; set; }

        /// <summary>
        ///   Gets or sets the second set of points.
        /// </summary>
        /// <value>The second set of points.</value>
        public List<IDataPoint> Points2
        {
            get { return points2; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the second 
        ///   data collection should be reversed.
        ///   The first dataset is not reversed, and normally
        ///   the second dataset must be reversed to get a 
        ///   closed polygon.
        /// </summary>
        public bool Reverse2 { get; set; }

        protected internal override void UpdateData()
        {
            base.UpdateData();

            if (ItemsSource == null)
            {
                return;
            }

            points2.Clear();

            // Using reflection on DataFieldX2 and DataFieldY2
            AddDataPoints(points2, ItemsSource, DataFieldX2, DataFieldY2);
        }

        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            InternalUpdateMaxMin(points2);
        }

        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (points.Count == 0)
            {
                return;
            }

            Debug.Assert(XAxis != null && YAxis != null, "Axis is not defined.");

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            var clippingRect = GetClippingRect();

            // Transform all points to screen coordinates
            int n0 = points.Count;
            var pts0 = new ScreenPoint[n0];
            for (int i = 0; i < n0; i++)
            {
                pts0[i] = XAxis.Transform(points[i].X,points[i].Y, YAxis);
            }

            int n1 = points2.Count;
            var pts1 = new ScreenPoint[n1];
            for (int i = 0; i < n1; i++)
            {
                int j = Reverse2 ? n1 - 1 - i : i;
                pts1[j] = XAxis.Transform(points2[i].X,points2[i].Y, YAxis);
            }

            if (Smooth)
            {
                var rpts0 = ScreenPointHelper.ResamplePoints(pts0, MinimumSegmentLength);
                var rpts1 = ScreenPointHelper.ResamplePoints(pts1, MinimumSegmentLength);

                pts0 = CanonicalSplineHelper.CreateSpline(rpts0, 0.5, null, false, 0.25).ToArray();
                pts1 = CanonicalSplineHelper.CreateSpline(rpts1, 0.5, null, false, 0.25).ToArray();
            }

            // draw the clipped lines
            rc.DrawClippedLine(pts0, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin,false);
            rc.DrawClippedLine(pts1, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin,false);

            // combine the two lines and draw the clipped area
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts1);
            pts.AddRange(pts0);
            pts = SutherlandHodgmanClipping.ClipPolygon(clippingRect, pts);
            rc.DrawPolygon(pts, Fill, null);

            // draw the markers on top
            rc.DrawMarkers(pts0, clippingRect, MarkerType, null, new [] {MarkerSize}, MarkerFill, MarkerStroke, MarkerStrokeThickness, 1);
            rc.DrawMarkers(pts1, clippingRect, MarkerType, null, new [] {MarkerSize}, MarkerFill, MarkerStroke, MarkerStrokeThickness, 1);
        }

        /// <summary>
        ///   Renders the legend symbol for the line series on the 
        ///   specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double y0= legendBox.Top*0.2 + legendBox.Bottom*0.8;
            double y1 = legendBox.Top * 0.4 + legendBox.Bottom * 0.6;
            double y2 = legendBox.Top * 0.8 + legendBox.Bottom * 0.2;
            
            var pts0 = new[]
                          {
                              new ScreenPoint(legendBox.Left, y0), 
                              new ScreenPoint(legendBox.Right, y0)
                          };
            var pts1 = new[]
                          {
                              new ScreenPoint(legendBox.Right, y2), 
                              new ScreenPoint(legendBox.Left, y1)
                          };
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts0);
            pts.AddRange(pts1);
            rc.DrawLine(pts0, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            rc.DrawLine(pts1, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            rc.DrawPolygon(pts, Fill, null);
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            int index;
            TrackerHitResult result1 = null;
            TrackerHitResult result2 = null;
            DataPoint dpn;
            ScreenPoint spn1;
            ScreenPoint spn2;

            if (interpolate)
            {
                if (GetNearestInterpolatedPointInternal(points, point, out dpn, out spn1, out index))
                {
                    var item = GetItem(ItemsSource, index);
                    result1=new TrackerHitResult(this, dpn, spn1, item);
                }
                if (GetNearestInterpolatedPointInternal(points2, point, out dpn, out spn2, out index))
                {
                    var item = GetItem(ItemsSource, index);
                    result2=new TrackerHitResult(this, dpn, spn2, item);
                }
                
            }
            else
            {
                if (GetNearestPointInternal(points, point, out dpn, out spn1, out index))
                {
                    var item = GetItem(ItemsSource, index);
                    result1=new TrackerHitResult(this, dpn, spn1, item);
                }
                if (GetNearestPointInternal(points2, point, out dpn, out spn2, out index))
                {
                    var item = GetItem(ItemsSource, index);
                   result2=new TrackerHitResult(this, dpn, spn2, item);
                }
            }

            if (result1 != null && result2 != null)
            {
                double dist1 = spn1.DistanceTo(point);
                double dist2 = spn2.DistanceTo(point);
                return dist1 < dist2 ? result1 : result2;
            }
            if (result1 != null) return result1;
            if (result2 != null) return result2;
            return null;
        }
    }
}