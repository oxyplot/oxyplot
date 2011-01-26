using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public static class RenderingExtensions
    {
        public static void DrawRectangle(this IRenderContext rc, OxyRect rect, OxyColor fill, OxyColor borderColor,
                                         double borderThickness)
        {
            //var border = new[]
            //                 {
            //                     new ScreenPoint(rect.Left, rect.Top), new ScreenPoint(rect.Right, rect.Top), 
            //                     new ScreenPoint(rect.Right, rect.Top), new ScreenPoint(rect.Right, rect.Bottom), 
            //                     new ScreenPoint(rect.Right, rect.Bottom), new ScreenPoint(rect.Left, rect.Bottom), 
            //                     new ScreenPoint(rect.Left, rect.Bottom),new ScreenPoint(rect.Left, rect.Top)
            //                 };
            //rc.DrawLineSegments(border,borderColor,borderThickness,null,OxyPenLineJoin.Miter,true);

            //             rc.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height, fill, borderColor, borderThickness);

            var border = new[]
                             {
                                 new ScreenPoint(rect.Left, rect.Top), new ScreenPoint(rect.Right, rect.Top), 
                                 new ScreenPoint(rect.Right, rect.Bottom), new ScreenPoint(rect.Left, rect.Bottom), 
                                 new ScreenPoint(rect.Left, rect.Top)
                             };

            rc.DrawPolygon(border, fill, borderColor, borderThickness, null, OxyPenLineJoin.Miter, true);
        }

        public static void DrawLine(this IRenderContext rc, double x0, double y0, double x1, double y1, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLine(new[]
                            {
                                new ScreenPoint(x0, y0), 
                                new ScreenPoint(x1, y1)
                            }, pen.Color, pen.Thickness, pen.DashArray, pen.LineJoin, aliased);
        }

        public static void DrawLineSegments(this IRenderContext rc, IList<ScreenPoint> points, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLineSegments(points, pen.Color, pen.Thickness, pen.DashArray, pen.LineJoin, aliased);
        }

        public static void DrawMarkers(this IRenderContext rc, ScreenPoint[] markerPoints, OxyRect clippingRect, MarkerType markerType, double markerSize, OxyColor markerFill, OxyColor markerStroke, double markerStrokeThickness)
        {
            // todo: Markers should be rendered to a DrawingContext for performance.
            if (markerType != MarkerType.None)
            {
                var clipping = new CohenSutherlandClipping(clippingRect);
                foreach (var p in markerPoints)
                {
                    if (clipping.IsInside(p.x, p.y))
                    {
                        rc.DrawMarker(markerType, p, markerSize, markerFill, markerStroke, markerStrokeThickness);
                    }
                }
            }
        }

        private static readonly double M1 = Math.Tan(Math.PI / 6);
        private static readonly double M2 = Math.Sqrt(1 + M1 * M1);
        private static readonly double M3 = Math.Tan(Math.PI / 4);


        /// <summary>
        /// Renders the marker.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="type">The marker type.</param>
        /// <param name="p">The center point of the marker.</param>
        /// <param name="size">The size of the marker.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        public static void DrawMarker(this IRenderContext rc, MarkerType type, ScreenPoint p, double size,
                                    OxyColor fill, OxyColor stroke, double strokeThickness)
        {
            switch (type)
            {
                case MarkerType.Circle:
                    {
                        rc.DrawEllipse(p.x - size, p.y - size, size * 2, size * 2, fill, stroke,
                                       strokeThickness);

                        // int n = 20;
                        // var pts = new ScreenPoint[n];
                        // for (int i = 0; i < n; i++)
                        // {
                        // double th = 2 * Math.PI * i / (n - 1);
                        // pts[i] = new ScreenPoint(p.x + markerSize * Math.Cos(th), p.y + markerSize * Math.Sin(th));
                        // }
                        // rc.DrawPolygon(pts, fill, stroke, strokeThickness);
                        break;
                    }

                case MarkerType.Square:
                    {
                        //var pts = new[]
                        //              {
                        //                  new ScreenPoint(p.x - size, p.y - size), 
                        //                  new ScreenPoint(p.x + size, p.y - size), 
                        //                  new ScreenPoint(p.x + size, p.y + size), 
                        //                  new ScreenPoint(p.x - size, p.y + size)
                        //              };
                        //rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        rc.DrawRectangle(p.x - size, p.y - size, size * 2, size * 2, fill, stroke, strokeThickness);
                        break;
                    }

                case MarkerType.Diamond:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.x, p.y - M2*size), 
                                          new ScreenPoint(p.x + M2*size, p.y), 
                                          new ScreenPoint(p.x, p.y + M2*size), 
                                          new ScreenPoint(p.x - M2*size, p.y)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, OxyPenLineJoin.Miter, true);
                        break;
                    }

                case MarkerType.Triangle:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.x - size, p.y + M1*size), 
                                          new ScreenPoint(p.x + size, p.y + M1*size), 
                                          new ScreenPoint(p.x, p.y - M2*size)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, OxyPenLineJoin.Miter, true);
                        break;
                    }

                case MarkerType.Plus:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new ScreenPoint(p.x - size, p.y), 
                                           new ScreenPoint(p.x + size, p.y)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.x, p.y - size), 
                                           new ScreenPoint(p.x, p.y + size)
                                       };
                        rc.DrawLine(pts1, stroke, strokeThickness);
                        rc.DrawLine(pts2, stroke, strokeThickness);
                        break;
                    }
            }

            switch (type)
            {
                case MarkerType.Cross:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new ScreenPoint(p.x - size*M3, p.y - size*M3), 
                                           new ScreenPoint(p.x + size*M3, p.y + size*M3)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.x - size*M3, p.y + size*M3), 
                                           new ScreenPoint(p.x + size*M3, p.y - size*M3)
                                       };
                        rc.DrawLine(pts1, stroke, strokeThickness);
                        rc.DrawLine(pts2, stroke, strokeThickness);
                        break;
                    }
            }
        }

        public static void DrawClippedLine(this IRenderContext rc, ScreenPoint[] points,
           OxyRect clippingRectangle, double minDistSquared,
           OxyColor stroke, double strokeThickness, LineStyle lineStyle, OxyPenLineJoin lineJoin)
        {
            var clipping = new CohenSutherlandClipping(clippingRectangle.Left, clippingRectangle.Right, clippingRectangle.Top, clippingRectangle.Bottom);

            int n;
            var pts = new List<ScreenPoint>();
            n = points.Length;
            if (n > 0)
            {
                var s0 = points[0];
                var last = points[0];

                for (int i = 1; i < n; i++)
                {
                    var s1 = points[i];

                    // Clipped version of this and next point.

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
                    double dx = s1c.x - last.x;
                    double dy = s1c.y - last.y;

                    if (dx * dx + dy * dy > minDistSquared || i == 1)
                    {
                        if (!s0c.Equals(last) || i == 1)
                        {
                            pts.Add(s0c);
                        }

                        pts.Add(s1c);
                        last = s1c;
                    }

                    // render the line if we are leaving the clipping region););
                    if (!clipping.IsInside(s1))
                    {
                        if (pts.Count > 0)
                            rc.DrawLine(pts, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin);
                        pts.Clear();
                    }
                }
                if (pts.Count > 0)
                    rc.DrawLine(pts, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin);
            }
        }
    }
}