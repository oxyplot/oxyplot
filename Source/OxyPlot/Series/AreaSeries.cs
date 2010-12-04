using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    ///  The AreaSeries class fills the polygon defined by two sets of points.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        internal Collection<DataPoint> points2;

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

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            // if (DataFieldX == null || DataFieldY == null)
            // {
            // foreach (object item in ItemsSource)
            // {
            // var idpp = item as IDataPointProvider;
            // if (idpp == null)
            // continue;
            // points.Add(idpp.GetDataPoint());
            // }
            // return;
            // }

            // Using reflection on DataFieldX2 and DataFieldY2
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (var o in ItemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(DataFieldX2);
                    piy = t.GetProperty(DataFieldY2);
                    if (pix == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldX2, t));
                    }

                    if (piy == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldY2, t));
                    }
                }

                var x = (double)pix.GetValue(o, null);
                var y = (double)piy.GetValue(o, null);


                var pp = new DataPoint(x, y);
                points2.Add(pp);
            }
        }

        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            if (points2 != null)
            {
                foreach (var pt in points2)
                {
                    MinX = Math.Min(MinX, pt.x);
                    MaxX = Math.Max(MaxX, pt.x);

                    MinY = Math.Min(MinY, pt.y);
                    MaxY = Math.Max(MaxY, pt.y);
                }
            }
        }

        public override void Render(IRenderContext rc, PlotModel model)
        {
            if (points.Count == 0)
            {
                return;
            }

            Debug.Assert(XAxis != null && YAxis != null);

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            // todo: polygon clipping...
            // simple line clipping is not working

            var clipping = new CohenSutherlandClipping(
                XAxis.ScreenMin.X, XAxis.ScreenMax.X,
                YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);

            var pts0 = new List<ScreenPoint>();
            var pts1 = new List<ScreenPoint>();

            var p0 = new ScreenPoint();
            var b0 = new ScreenPoint();

            bool first = true;

            var s0 = ScreenPoint.Undefined;
            int n = Points.Count;

            for (int i = 0; i < n; i++)
            {
                var pt1 = Points[i];

                var s1 = XAxis.Transform(pt1.x, pt1.y, YAxis);
                if (i == 0)
                {
                    s0 = s1;
                }

                double x0 = s0.x;
                double y0 = s0.y;
                double x1 = s1.x;
                double y1 = s1.y;

                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);

                s1.x = x1;
                s1.y = y1;

                if (outside)
                {
                    continue;
                }

                if (first || i == n - 1)
                {
                    s0 = s1;
                    pts0.Add(s1);
                    first = false;
                }
                else
                {
                    double dx = s1.x - p0.x;
                    double dy = s1.y - p0.y;
                    if (dx * dx + dy * dy > minDistSquared)
                    {
                        p0 = s1;
                        pts0.Add(s1);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                double x2 = Points[i].x;
                double y2 = ConstantY2;
                if (i < Points2.Count)
                {
                    x2 = Points2[i].x;
                    y2 = Points2[i].y;
                }

                var pt2 = new ScreenPoint(x2, y2);

                var s2 = XAxis.Transform(pt2.x, pt2.y, YAxis);

                if (i == 0)
                {
                    s0 = s2;
                }

                double x0 = s0.x;
                double y0 = s0.y;
                double x1 = s2.x;
                double y1 = s2.y;
                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);
                pt2.x = x1;
                pt2.y = y1;

                s0 = s2;
                if (outside)
                {
                    continue;
                }

                if (first || i == n - 1)
                {
                    b0 = s2;
                    pts1.Add(s2);
                    first = false;
                }
                else
                {
                    double dx = s2.x - b0.x;
                    double dy = s2.y - b0.y;
                    if (dx * dx + dy * dy > minDistSquared)
                    {
                        b0 = s2;
                        pts1.Add(s2);
                    }
                }
            }

            if (Reverse2)
            {
                pts1.Reverse();
            }

            if (Smooth)
            {
                pts0 = CanonicalSplineHelper.CreateSpline(pts0, 0.5, null, false, 0.25);
                pts1 = CanonicalSplineHelper.CreateSpline(pts1, 0.5, null, false, 0.25);
            }

            // draw the lines
            rc.DrawLine(pts0, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            rc.DrawLine(pts1, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));

            // combine the two and draw the area
            pts1.AddRange(pts0);
            rc.DrawPolygon(pts1, Fill, null);

            if (MarkerType != MarkerType.None)
            {
                foreach (var p in pts0)
                {
                    RenderMarker(rc, MarkerType, p, MarkerSize, MarkerFill, MarkerStroke,
                                 MarkerStrokeThickness);
                }
            }
        }
    }
}