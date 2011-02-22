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
        protected Collection<DataPoint> points2;

        public AreaSeries()
        {
            points2 = new Collection<DataPoint>();
            Reverse2 = true;
        }

        /// <summary>
        ///   Gets or sets the area fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        ///   Gets or sets the second Y data field.
        /// </summary>
        /// <value>The data field x2.</value>
        public string DataFieldX2 { get; set; }

        /// <summary>
        ///   Gets or sets the second X data field.
        /// </summary>
        /// <value>The data field y2.</value>
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
        public Collection<DataPoint> Points2
        {
            get { return points2; }
            set { points2 = value; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the second 
        ///   data collection should be reversed.
        ///   The first dataset is not reversed, and normally
        ///   the second dataset must be reversed to get a 
        ///   closed polygon.
        /// </summary>
        public bool Reverse2 { get; set; }

        public override void UpdateData()
        {
            base.UpdateData();

            if (ItemsSource == null)
            {
                return;
            }

            points2.Clear();

            // Using reflection on DataFieldX2 and DataFieldY2
            AddDataPoints(points2, DataFieldX2, DataFieldY2);
        }

        public override void UpdateMaxMin()
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
                pts0[i] = XAxis.Transform(points[i], YAxis);
            }

            int n1 = points2.Count;
            var pts1 = new ScreenPoint[n1];
            for (int i = 0; i < n1; i++)
            {
                int j = Reverse2 ? n1 - 1 - i : i;
                pts1[j] = XAxis.Transform(points2[i], YAxis);
            }

            if (Smooth)
            {
                pts0 = CanonicalSplineHelper.CreateSpline(pts0, 0.5, null, false, 0.25).ToArray();
                pts1 = CanonicalSplineHelper.CreateSpline(pts1, 0.5, null, false, 0.25).ToArray();
            }

            // draw the clipped lines
            rc.DrawClippedLine(pts0, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin);
            rc.DrawClippedLine(pts1, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin);

            // combine the two lines and draw the clipped area
            var pts = new List<ScreenPoint>();
            pts.AddRange(pts1);
            pts.AddRange(pts0);
            pts = SutherlandHodgmanClipping.ClipPolygon(clippingRect, pts);
            rc.DrawPolygon(pts, Fill, null);

            // draw the markers on top
            rc.DrawMarkers(pts0, clippingRect, MarkerType, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
            rc.DrawMarkers(pts1, clippingRect, MarkerType, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
        }
    }
}