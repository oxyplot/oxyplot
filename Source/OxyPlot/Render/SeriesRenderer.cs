using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public class SeriesRenderer : PlotRenderer
    {
        private static readonly double LEGEND_PADDING = 8;

        private static readonly double M1 = Math.Tan(Math.PI / 6);
        private static readonly double M2 = Math.Sqrt(1 + M1 * M1);
        private static readonly double M3 = Math.Tan(Math.PI / 4);

        public SeriesRenderer(IRenderContext rc, PlotModel p)
            : base(rc, p)
        {
        }

        public void Render(DataSeries s)
        {
            if (s is LineSeries)
                Render(s as LineSeries);
            if (s is AreaSeries)
                Render(s as AreaSeries);
        }

        public void Render(LineSeries ls)
        {
            if (ls.Points.Count == 0)
                return;

            double minDistSquared = ls.MinimumSegmentLength * ls.MinimumSegmentLength;

            var clipping = new CohenSutherlandClipping(ls.XAxis.ScreenMin.X, ls.XAxis.ScreenMax.X,
                                                       ls.YAxis.ScreenMin.Y, ls.YAxis.ScreenMax.Y);

            var pts = new List<ScreenPoint>();
            // ScreenPoint p0 = ls.Points[0];
            var s0 = new ScreenPoint();
            bool first = true;
            int n = ls.Points.Count;
            for (int i = 0; i < n; i++)
            {
                var p1 = ls.Points[i];


                var s1 = ls.XAxis.Transform(p1.X, p1.Y, ls.YAxis);

                double x0 = s0.X;
                double y0 = s0.Y;
                double x1 = s1.X;
                double y1 = s1.Y;

                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);

                s1.X = x1;
                s1.Y = y1;

                if (outside)
                {
                    RenderLine(ls, pts);
                    pts.Clear();
                    continue;

                }

                if (first || i == n - 1)
                {
                    s0 = s1;
                    pts.Add(s1);
                    first = false;
                }
                else
                {
                    double dx = s1.X - s0.X;
                    double dy = s1.Y - s0.Y;
                    if (dx * dx + dy * dy > minDistSquared)
                    {
                        s0 = s1;
                        pts.Add(s0);
                    }
                }
            }

            RenderLine(ls, pts);

            if (ls.MarkerType != MarkerType.None)
            {
                foreach (ScreenPoint p in pts)
                {
                    RenderMarker(ls.MarkerType, p, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke,
                                 ls.MarkerStrokeThickness);
                }
            }
        }
        public void RenderLine(LineSeries ls, List<ScreenPoint> pts)
        {
            if (pts.Count == 0)
                return;

            if (ls.Smooth)
            {
                pts = CanonicalSplineHelper.CreateSpline(pts, 0.5, null, false, 0.25);
            }

            rc.DrawLine(pts.ToArray(), ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));

        }

        public void Render(AreaSeries ls)
        {
            if (ls.Points.Count == 0)
                return;

            double minDistSquared = ls.MinimumSegmentLength * ls.MinimumSegmentLength;

            // todo: polygon clipping...

            var clipping = new CohenSutherlandClipping(ls.XAxis.ScreenMin.X, ls.XAxis.ScreenMax.X,
                                                       ls.YAxis.ScreenMin.Y, ls.YAxis.ScreenMax.Y);

            var pts0 = new List<ScreenPoint>();
            var pts1 = new List<ScreenPoint>();

            var p0 = new ScreenPoint();
            var b0 = new ScreenPoint();

            bool first = true;

            var s0 = ScreenPoint.Undefined;
            int n = ls.Points.Count;

            for (int i = 0; i < n; i++)
            {
                var pt1 = ls.Points[i];

                var s1 = ls.XAxis.Transform(pt1.X, pt1.Y, ls.YAxis);
                if (i == 0) s0 = s1;

                double x0 = s0.X;
                double y0 = s0.Y;
                double x1 = s1.X;
                double y1 = s1.Y;

                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);

                s1.X = x1;
                s1.Y = y1;

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
                    double dx = s1.X - p0.X;
                    double dy = s1.Y - p0.Y;
                    if (dx * dx + dy * dy > minDistSquared)
                    {
                        p0 = s1;
                        pts0.Add(s1);
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                double b = ls.Baseline;
                if (i < ls.BaselineValues.Count)
                    b = ls.BaselineValues[i];
                var pt2 = new ScreenPoint(ls.Points[i].X, b);

                var s2 = ls.XAxis.Transform(pt2.X, pt2.Y, ls.YAxis);

                if (i == 0)
                {
                    s0 = s2;
                }

                double x0 = s0.X;
                double y0 = s0.Y;
                double x1 = s2.X;
                double y1 = s2.Y;
                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);
                pt2.X = x1;
                pt2.Y = y1;

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
                    double dx = s2.X - b0.X;
                    double dy = s2.Y - b0.Y;
                    if (dx * dx + dy * dy > minDistSquared)
                    {
                        b0 = s2;
                        pts1.Add(s2);
                    }
                }
            }

            pts1.Reverse();

            if (ls.Smooth)
            {
                pts0 = CanonicalSplineHelper.CreateSpline(pts0, 0.5, null, false, 0.25);
                pts1 = CanonicalSplineHelper.CreateSpline(pts1, 0.5, null, false, 0.25);
            }

            // draw the lines
            rc.DrawLine(pts0, ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));
            rc.DrawLine(pts1, ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));

            // combine the two and draw the area
            pts1.AddRange(pts0);
            rc.DrawPolygon(pts1, ls.Fill, null);

            if (ls.MarkerType != MarkerType.None)
            {
                foreach (var p in pts0)
                {
                    RenderMarker(ls.MarkerType, p, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke,
                                 ls.MarkerStrokeThickness);
                }
            }
        }

        public void RenderMarker(MarkerType markerType, ScreenPoint p, double markerSize, OxyColor fill, OxyColor stroke,
                                 double strokeThickness)
        {
            switch (markerType)
            {
                case MarkerType.Circle:
                    {
                        rc.DrawEllipse(p.X - markerSize, p.Y - markerSize, markerSize * 2, markerSize * 2, fill, stroke, strokeThickness);

                        //int n = 20;
                        //var pts = new ScreenPoint[n];
                        //for (int i = 0; i < n; i++)
                        //{
                        //    double th = 2 * Math.PI * i / (n - 1);
                        //    pts[i] = new ScreenPoint(p.X + markerSize * Math.Cos(th), p.Y + markerSize * Math.Sin(th));
                        //}
                        //rc.DrawPolygon(pts, fill, stroke, strokeThickness);
                        break;
                    }
                case MarkerType.Square:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.X - markerSize, p.Y - markerSize),
                                          new ScreenPoint(p.X + markerSize, p.Y - markerSize),
                                          new ScreenPoint(p.X + markerSize, p.Y + markerSize),
                                          new ScreenPoint(p.X - markerSize, p.Y + markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Diamond:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.X, p.Y - M2*markerSize),
                                          new ScreenPoint(p.X + M2*markerSize, p.Y),
                                          new ScreenPoint(p.X, p.Y + M2*markerSize),
                                          new ScreenPoint(p.X - M2*markerSize, p.Y)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Triangle:
                    {
                        var pts = new[]
                                      {
                                          new ScreenPoint(p.X - markerSize, p.Y + M1*markerSize),
                                          new ScreenPoint(p.X + markerSize, p.Y + M1*markerSize),
                                          new ScreenPoint(p.X, p.Y - M2*markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Plus:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new ScreenPoint(p.X - markerSize, p.Y),
                                           new ScreenPoint(p.X + markerSize, p.Y)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.X, p.Y - markerSize),
                                           new ScreenPoint(p.X, p.Y + markerSize)
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
                                           new ScreenPoint(p.X - markerSize*M3, p.Y - markerSize*M3),
                                           new ScreenPoint(p.X + markerSize*M3, p.Y + markerSize*M3)
                                       };
                        var pts2 = new[]
                                       {
                                           new ScreenPoint(p.X - markerSize*M3, p.Y + markerSize*M3),
                                           new ScreenPoint(p.X + markerSize*M3, p.Y - markerSize*M3)
                                       };
                        rc.DrawLine(pts1, stroke, strokeThickness);
                        rc.DrawLine(pts2, stroke, strokeThickness);
                        break;
                    }
            }
        }

        #region LEGENDS

        public void RenderLegends()
        {
            double maxWidth = 0;
            double maxHeight = 0;
            double totalHeight = 0;

            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                var oxySize = rc.MeasureText(s.Title, plot.LegendFont, plot.LegendFontSize);
                if (oxySize.Width > maxWidth) maxWidth = oxySize.Width;
                if (oxySize.Height > maxHeight) maxHeight = oxySize.Height;
                totalHeight += oxySize.Height;
            }

            double length = plot.LegendLineLength;
            double x0 = plot.bounds.Right - LEGEND_PADDING;
            double y0 = plot.bounds.Top + LEGEND_PADDING + maxHeight / 2;

            foreach (var s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                rc.DrawText(new ScreenPoint(x0 - length - LEGEND_PADDING, y0),
                            s.Title, plot.TextColor,
                            plot.LegendFont, plot.LegendFontSize, 500, 0,
                            HorizontalTextAlign.Right, VerticalTextAlign.Middle);
                RenderLegend(s, new ScreenPoint(x0 - length, y0), length, maxHeight);
                y0 += maxHeight;
            }
        }

        private void RenderLegend(DataSeries s, ScreenPoint screenPoint, double length, double maxh)
        {
            var ls = s as LineSeries;
            if (ls == null)
                return;

            var pts = new[] { screenPoint, 
                new ScreenPoint(screenPoint.X + length, screenPoint.Y) };
            rc.DrawLine(pts, ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));
            var pm = new ScreenPoint(screenPoint.X + length / 2, screenPoint.Y);
            RenderMarker(ls.MarkerType, pm, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke, ls.MarkerStrokeThickness);
        }

        #endregion
    }
}