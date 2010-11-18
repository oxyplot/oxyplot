using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public class LineSeries : DataSeries
    {
        public LineSeries()
        {
            MinimumSegmentLength = 2;
            StrokeThickness = 2;

            MarkerSize = 3;
            MarkerStrokeThickness = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        /// <param name="color">The color of the line stroke.</param>
        /// <param name="strokeThickness">The stroke thickness (optional).</param>
        /// <param name="title">The title (optional).</param>
        public LineSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this()
        {
            this.Color = color;
            this.StrokeThickness = strokeThickness;
            this.Title = title;
        }

        public OxyColor Background { get; set; }

        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the dashes array. 
        /// If this is not null it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        /// Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker.
        /// </summary>
        /// <value>The size of the marker.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke thickness.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the marker fill color.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill { get; set; }


        /// <summary>
        /// Gets or sets the minimum length of the segment.
        /// Increasing this number will increase performance, 
        /// but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (points.Count == 0)
                return;

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            var clipping = new CohenSutherlandClipping(XAxis.ScreenMin.x, XAxis.ScreenMax.x,
                                                       YAxis.ScreenMin.y, YAxis.ScreenMax.y);

            // Transform all points
            int n = points.Count;
            var markerPoints = new ScreenPoint[n];

            for (int i = 0; i < n; i++)
                markerPoints[i] = XAxis.Transform(points[i].x, points[i].y, YAxis);


            ScreenPoint[] SP;
            if (Smooth)
            {
                SP = CanonicalSplineHelper.CreateSpline(markerPoints, 0.5, null, false, 0.25).ToArray();
            }
            else
            {
                SP = markerPoints;
            }

            var pts = new List<ScreenPoint>();
            var s0 = SP[0];
            var last = SP[0];

            n = SP.Length;
            for (int i = 1; i < n; i++)
            {
                var s1 = SP[i];

                var s0c = s0;
                var s1c = s1;
                bool isInside = clipping.ClipLine(ref s0c, ref s1c);
                s0 = s1;

                if (!isInside)
                {
                    // keep the previous coordinate
                    continue;
                }

                // render from s0c-s1c

                double dx = s0c.x - last.x;
                double dy = s1c.y - last.y;

                if (dx * dx + dy * dy > minDistSquared || i == 0)
                {
                if (!s0c.Equals(last) || i == 1)
                    pts.Add(s0c);
                pts.Add(s1c);
                last = s1c;
            }

            // render the line if we are leaving the clipping region););
                if (!clipping.IsInside(s1))
                {
                    RenderLine(rc, pts);
                    pts.Clear();
                    continue;
                }

                continue;
                // always add the first and last point
                //if (i==0 || i == n - 1)
                //{
                //    pts.Add(s1c);
                //    s0 = s1c;
                //}
                //else
                //{
                //    double dx = s1.x - s0.x;
                //    double dy = s1.y - s0.y;
                //    if (dx * dx + dy * dy > minDistSquared)
                //    {
                //        s0 = s1;
                //        if (clipping.IsInside(s1))
                //            pts.Add(s1);
                //    }
                //}
            }

            RenderLine(rc, pts);

            if (MarkerType != MarkerType.None)
            {
                foreach (var p in markerPoints)
                {
                    if (clipping.IsInside(p.x, p.y))
                        RenderMarker(rc, MarkerType, p, MarkerSize, MarkerFill, MarkerStroke,
                                     MarkerStrokeThickness);
                }
            }
        }

        private static readonly double M1 = Math.Tan(Math.PI / 6);
        private static readonly double M2 = Math.Sqrt(1 + M1 * M1);
        private static readonly double M3 = Math.Tan(Math.PI / 4);

        public void RenderLine(IRenderContext rc, List<ScreenPoint> pts)
        {
            if (pts.Count == 0)
                return;

#if SIMPLIFY_HERE         
            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            var pts2 = new List<ScreenPoint>();
            var last = pts[0];
            for (int i = 0; i < pts.Count; i++)
            {
                double dx = pts[i].x - last.x;
                double dy = pts[i].y - last.y;

                if (dx * dx + dy * dy > minDistSquared || i == 0)
                {
                    pts2.Add(pts[i]);
                    last = pts[i];
                }
            }
            rc.DrawLine(pts2.ToArray(), Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
#else
            rc.DrawLine(pts.ToArray(), Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
#endif

        }

        public void RenderMarker(IRenderContext rc, MarkerType markerType, ScreenPoint p, double markerSize, OxyColor fill, OxyColor stroke,
                                double strokeThickness)
        {
            switch (markerType)
            {
                case MarkerType.Circle:
                    {
                        rc.DrawEllipse(p.x - markerSize, p.y - markerSize, markerSize * 2, markerSize * 2, fill, stroke, strokeThickness);

                        //int n = 20;
                        //var pts = new ScreenPoint[n];
                        //for (int i = 0; i < n; i++)
                        //{
                        //    double th = 2 * Math.PI * i / (n - 1);
                        //    pts[i] = new ScreenPoint(p.x + markerSize * Math.Cos(th), p.y + markerSize * Math.Sin(th));
                        //}
                        //rc.DrawPolygon(pts, fill, stroke, strokeThickness);
                        break;
                    }
                case MarkerType.Square:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.x - markerSize, p.y - markerSize),
                                          new ScreenPoint(p.x + markerSize, p.y - markerSize),
                                          new ScreenPoint(p.x + markerSize, p.y + markerSize),
                                          new ScreenPoint(p.x - markerSize, p.y + markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Diamond:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.x, p.y - M2*markerSize),
                                          new ScreenPoint(p.x + M2*markerSize, p.y),
                                          new ScreenPoint(p.x, p.y + M2*markerSize),
                                          new ScreenPoint(p.x - M2*markerSize, p.y)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Triangle:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.x - markerSize, p.y + M1*markerSize),
                                          new ScreenPoint(p.x + markerSize, p.y + M1*markerSize),
                                          new ScreenPoint(p.x, p.y - M2*markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Plus:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new ScreenPoint(p.x - markerSize, p.y),
                                           new ScreenPoint(p.x + markerSize, p.y)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.x, p.y - markerSize),
                                           new ScreenPoint(p.x, p.y + markerSize)
                                       };
                        rc.DrawLine(pts1, stroke, strokeThickness);
                        rc.DrawLine(pts2, stroke, strokeThickness);
                        break;
                    }
            }
            switch (markerType)
            {
                case MarkerType.Cross:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new ScreenPoint(p.x - markerSize*M3, p.y - markerSize*M3),
                                           new ScreenPoint(p.x + markerSize*M3, p.y + markerSize*M3)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.x - markerSize*M3, p.y + markerSize*M3),
                                           new ScreenPoint(p.x + markerSize*M3, p.y - markerSize*M3)
                                       };
                        rc.DrawLine(pts1, stroke, strokeThickness);
                        rc.DrawLine(pts2, stroke, strokeThickness);
                        break;
                    }
            }
        }

        public override void RenderLegend(IRenderContext rc, OxyRect rect)
        {
            double xmid = (rect.Left + rect.Right) / 2;
            double ymid = (rect.Top + rect.Bottom) / 2;
            var pts = new[] { 
                new ScreenPoint(rect.Left,ymid), 
                new ScreenPoint(rect.Right,ymid) };
            rc.DrawLine(pts, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            var pm = new ScreenPoint(xmid, ymid);
            RenderMarker(rc, MarkerType, pm, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
        }
    }
}