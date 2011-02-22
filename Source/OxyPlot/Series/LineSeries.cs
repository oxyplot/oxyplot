using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OxyPlot
{
    /// <summary>
    ///   LineSeries are rendered to polylines.
    /// </summary>
    public class LineSeries : DataSeries
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        public LineSeries()
        {
            MinimumSegmentLength = 2;
            StrokeThickness = 2;
            MarkerSize = 3;
            MarkerStrokeThickness = 1;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name = "title">The title.</param>
        public LineSeries(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name = "color">The color of the line stroke.</param>
        /// <param name = "strokeThickness">The stroke thickness (optional).</param>
        /// <param name = "title">The title (optional).</param>
        public LineSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this()
        {
            Color = color;
            StrokeThickness = strokeThickness;
            Title = title;
        }

        /// <summary>
        ///   Gets or sets the background of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background.</value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets the color of the curve.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the dashes array. 
        ///   If this is not null it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        ///   Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        ///   Gets or sets the size of the marker.
        /// </summary>
        /// <value>The size of the marker.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        ///   Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        ///   Gets or sets the marker stroke thickness.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the marker fill color.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        ///   Gets or sets the minimum length of the segment.
        ///   Increasing this number will increase performance, 
        ///   but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        protected OxyRect GetClippingRect()
        {
            var minX = Math.Min(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var minY = Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);
            var maxX = Math.Max(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var maxY = Math.Max(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        ///   Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (points.Count == 0)
            {
                return;
            }

            Debug.Assert(XAxis != null && YAxis != null, "Axis has not been defined.");

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            var clippingRect = GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var allPoints = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                allPoints[i] = XAxis.Transform(points[i], YAxis);
            }

            // spline smoothing (should only be used on small datasets)
            // todo: could do spline smoothing only on the visible part of the curve...
            var screenPoints = Smooth
                                   ? CanonicalSplineHelper.CreateSpline(allPoints, 0.5, null, false, 0.25).ToArray()
                                   : allPoints;

            // clip the line segments with the clipping rectangle
            rc.DrawClippedLine(screenPoints, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin);
            rc.DrawMarkers(allPoints, clippingRect, MarkerType, MarkerSize, MarkerFill, MarkerStroke,
                           MarkerStrokeThickness);
        }

        //private void RenderClippedLine(IRenderContext rc, ScreenPoint[] points, CohenSutherlandClipping clipping, double minDistSquared)
        //{
        //    int n;
        //    var pts = new List<ScreenPoint>();
        //    n = points.Length;
        //    if (n > 0)
        //    {
        //        var s0 = points[0];
        //        var last = points[0];

        //        for (int i = 1; i < n; i++)
        //        {
        //            var s1 = points[i];

        //            // Clipped version of this and next point.

        //            var s0c = s0;
        //            var s1c = s1;
        //            bool isInside = clipping.ClipLine(ref s0c, ref s1c);
        //            s0 = s1;

        //            if (!isInside)
        //            {
        //                // keep the previous coordinate
        //                continue;
        //            }

        //            // render from s0c-s1c
        //            double dx = s1c.x - last.x;
        //            double dy = s1c.y - last.y;

        //            if (dx * dx + dy * dy > minDistSquared || i == 1)
        //            {
        //                if (!s0c.Equals(last) || i == 1)
        //                {
        //                    pts.Add(s0c);
        //                }

        //                pts.Add(s1c);
        //                last = s1c;
        //            }

        //            // render the line if we are leaving the clipping region););
        //            if (!clipping.IsInside(s1))
        //            {
        //                RenderLine(rc, pts);
        //                pts.Clear();
        //            }
        //        }
        //        RenderLine(rc, pts);
        //    }
        //}

        protected void RenderMarkers(IRenderContext rc, ScreenPoint[] markerPoints, CohenSutherlandClipping clipping)
        {
            // todo: Markers should be rendered to a DrawingContext for performance.
            if (MarkerType != MarkerType.None)
            {
                var actualMarkerFill = MarkerFill;
                var actualMarkerStroke = MarkerStroke;
                if (actualMarkerStroke == null)
                    actualMarkerStroke = Color;

                foreach (var p in markerPoints)
                {
                    if (clipping.IsInside(p.x, p.y))
                    {
                        rc.DrawMarker(MarkerType, p, MarkerSize, actualMarkerFill, actualMarkerStroke,
                                     MarkerStrokeThickness);
                    }
                }
            }
        }

//        protected void RenderLine(IRenderContext rc, List<ScreenPoint> pts)
//        {
//            if (pts.Count == 0)
//            {
//                return;
//            }

//#if SIMPLIFY_HERE         
//            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

//            var pts2 = new List<ScreenPoint>();
//            var last = pts[0];
//            for (int i = 0; i < pts.Count; i++)
//            {
//                double dx = pts[i].x - last.x;
//                double dy = pts[i].y - last.y;

//                if (dx * dx + dy * dy > minDistSquared || i == 0)
//                {
//                    pts2.Add(pts[i]);
//                    last = pts[i];
//                }
//            }
//            rc.DrawLine(pts2.ToArray(), Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
//#else
//            rc.DrawLine(pts.ToArray(), Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle), LineJoin);
//#endif
//        }

        /// <summary>
        ///   Renders the legend symbol for the line series on the 
        ///   specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var pts = new[]
                          {
                              new ScreenPoint(legendBox.Left, ymid), 
                              new ScreenPoint(legendBox.Right, ymid)
                          };
            rc.DrawLine(pts, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            var midpt = new ScreenPoint(xmid, ymid);
            rc.DrawMarker(MarkerType, midpt, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
        }
    }
}