using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    /// AreaSeries
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// Gets or sets the area fill color.
        /// </summary>
        /// <value>The fill.</value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the data field used to find 
        /// the basis values of the area.
        /// </summary>
        /// <value>The data field base.</value>
        public string DataFieldBase { get; set; }

        /// <summary>
        /// Gets or sets the constant baseline.
        /// This is used if DataFieldBase and BaselineValues are null.
        /// </summary>
        /// <value>The baseline.</value>
        public double Baseline { get; set; }

        /// <summary>
        /// Gets or sets the baseline values.
        /// </summary>
        /// <value>The baseline values.</value>
        public Collection<double> BaselineValues { get; set; }

        public AreaSeries()
        {
            BaselineValues = new Collection<double>();
        }

        internal override void UpdatePointsFromItemsSource()
        {
            base.UpdatePointsFromItemsSource();

            if (ItemsSource == null) return;
            BaselineValues.Clear();

            // Do nothing if ItemsSource is set before DataFields are set
            if (DataFieldBase == null)
                return;

            PropertyInfo piy = null;
            Type t = null;

            foreach (object o in ItemsSource)
            {
                if (piy == null || o.GetType() != t)
                {
                    t = o.GetType();
                    piy = t.GetProperty(DataFieldBase);
                    if (piy == null)
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldBase, t));
                }
                var y = (double)piy.GetValue(o, null);

                BaselineValues.Add(y);
            }
        }
        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();

            if (BaselineValues != null)
                foreach (double d in BaselineValues)
                {
                    MinY = Math.Min(MinY, d);
                    MaxY = Math.Max(MaxY, d);
                }
        }

        public override void Render(IRenderContext rc, PlotModel model)
        {
            // base.Render(rc, model);
            if (Points.Count == 0)
                return;

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            // todo: polygon clipping...

            var clipping = new CohenSutherlandClipping(XAxis.ScreenMin.x, XAxis.ScreenMax.x,
                                                       YAxis.ScreenMin.y, YAxis.ScreenMax.y);

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
                if (i == 0) s0 = s1;

                double x0 = s0.x;
                double y0 = s0.y;
                double x1 = s1.x;
                double y1 = s1.y;

                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);

                s1.x = x1;
                s1.y = y1;

                if (outside)
                    continue;

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
                double b = Baseline;
                if (i < BaselineValues.Count)
                    b = BaselineValues[i];
                var pt2 = new ScreenPoint(Points[i].x, b);

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
                    continue;

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

            pts1.Reverse();

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
                    RenderMarker(rc,MarkerType, p, MarkerSize, MarkerFill, MarkerStroke,
                                 MarkerStrokeThickness);
                }
            }
        }
    }
}