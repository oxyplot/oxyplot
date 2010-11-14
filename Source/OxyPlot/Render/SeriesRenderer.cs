using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public class SeriesRenderer : PlotRenderer
    {
        private static readonly double LEGEND_PADDING = 8;

        private static readonly double M1 = Math.Tan(Math.PI / 6);
        private static readonly double M2 = Math.Sqrt(1 + M1*M1);
        private static readonly double M3 = Math.Tan(Math.PI/4);

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

            double minDistSquared = ls.MinimumSegmentLength*ls.MinimumSegmentLength;

            var clipping = new CohenSutherlandClipping(ls.XAxis.ActualMinimum, ls.XAxis.ActualMaximum,
                                                       ls.YAxis.ActualMinimum, ls.YAxis.ActualMaximum);

            var pts = new List<Point>();
            Point p0 = ls.Points[0];
            var s0 = new Point();
            bool first = true;
            int n = ls.Points.Count;
            for (int i = 0; i < n; i++)
            {
                Point p1 = ls.Points[i];
                double x0 = p0.X;
                double y0 = p0.Y;
                double x1 = p1.X;
                double y1 = p1.Y;
                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);

                p1.X = x1;
                p1.Y = y1;
                p0 = p1;

                if (outside)
                    continue;

                var s1 = new Point(ls.XAxis.Transform(p1.X), ls.YAxis.Transform(p1.Y));


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
                    if (dx*dx + dy*dy > minDistSquared)
                    {
                        s0 = s1;
                        pts.Add(s0);
                    }
                }
            }

            if (ls.Smooth)
            {
                pts = CanonicalSplineHelper.CreateSpline(pts, 0.5, null, false, 0.25);
            }

            rc.DrawLine(pts.ToArray(), ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));

            if (ls.MarkerType != MarkerType.None)
            {
                foreach (Point p in pts)
                {
                    RenderMarker(ls.MarkerType, p, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke,
                                 ls.MarkerStrokeThickness);
                }
            }
        }

        public void Render(AreaSeries ls)
        {
            if (ls.Points.Count == 0)
                return;

            double minDistSquared = ls.MinimumSegmentLength*ls.MinimumSegmentLength;

            var clipping = new CohenSutherlandClipping(ls.XAxis.ActualMinimum, ls.XAxis.ActualMaximum,
                                                       ls.YAxis.ActualMinimum, ls.YAxis.ActualMaximum);

            var pts0 = new List<Point>();
            var pts1 = new List<Point>();

            var p0 = new Point();
            var b0 = new Point();

            bool first = true;

            Point pt0 = ls.Points[0];

            int n = ls.Points.Count;

            for (int i = 0; i < n; i++)
            {
                Point pt1 = ls.Points[i];

                double x0 = pt0.X;
                double y0 = pt0.Y;
                double x1 = pt1.X;
                double y1 = pt1.Y;
                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);
                pt1.X = x1;
                pt1.Y = y1;

                pt0 = pt1;
                if (outside)
                    continue;

                var s1 = new Point(ls.XAxis.Transform(pt1.X), ls.YAxis.Transform(pt1.Y));

                if (first || i == n - 1)
                {
                    p0 = s1;
                    pts0.Add(s1);
                    first = false;
                }
                else
                {
                    double dx = s1.X - p0.X;
                    double dy = s1.Y - p0.Y;
                    if (dx*dx + dy*dy > minDistSquared)
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
                var pt1 = new Point(ls.Points[i].X, b);

                if (i == 0)
                {
                    pt0 = pt1;
                }

                double x0 = pt0.X;
                double y0 = pt0.Y;
                double x1 = pt1.X;
                double y1 = pt1.Y;
                bool outside = !clipping.ClipLine(ref x0, ref y0, ref x1, ref y1);
                pt1.X = x1;
                pt1.Y = y1;

                pt0 = pt1;
                if (outside)
                    continue;

                var s2 = new Point(ls.XAxis.Transform(pt1.X), ls.YAxis.Transform(pt1.Y));

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
                    if (dx*dx + dy*dy > minDistSquared)
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
                foreach (Point p in pts0)
                {
                    RenderMarker(ls.MarkerType, p, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke,
                                 ls.MarkerStrokeThickness);
                }
            }
        }

        public void RenderMarker(MarkerType markerType, Point p, double markerSize, Color fill, Color stroke,
                                 double strokeThickness)
        {
            switch (markerType)
            {
                case MarkerType.Circle:
                    {
                        int n = 20;
                        var pts = new Point[n];
                        for (int i = 0; i < n; i++)
                        {
                            double th = 2*Math.PI*i/(n - 1);
                            pts[i] = new Point(p.X + markerSize*Math.Cos(th), p.Y + markerSize*Math.Sin(th));
                        }
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness);
                        break;
                    }
                case MarkerType.Square:
                    {
                        var pts = new[]
                                      {
                                          new Point(p.X - markerSize, p.Y - markerSize),
                                          new Point(p.X + markerSize, p.Y - markerSize),
                                          new Point(p.X + markerSize, p.Y + markerSize),
                                          new Point(p.X - markerSize, p.Y + markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Diamond:
                    {
                        var pts = new[]
                                      {
                                          new Point(p.X, p.Y - M2*markerSize),
                                          new Point(p.X + M2*markerSize, p.Y),
                                          new Point(p.X, p.Y + M2*markerSize),
                                          new Point(p.X - M2*markerSize, p.Y)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Triangle:
                    {
                        var pts = new[]
                                      {
                                          new Point(p.X - markerSize, p.Y + M1*markerSize),
                                          new Point(p.X + markerSize, p.Y + M1*markerSize),
                                          new Point(p.X, p.Y - M2*markerSize)
                                      };
                        rc.DrawPolygon(pts, fill, stroke, strokeThickness, null, true);
                        break;
                    }
                case MarkerType.Plus:
                case MarkerType.Star:
                    {
                        var pts1 = new[]
                                       {
                                           new Point(p.X - markerSize, p.Y),
                                           new Point(p.X + markerSize, p.Y)
                                       };
                        var pts2 = new[]
                                       {
                                           new Point(p.X, p.Y - markerSize),
                                           new Point(p.X, p.Y + markerSize)
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
                                           new Point(p.X - markerSize*M3, p.Y - markerSize*M3),
                                           new Point(p.X + markerSize*M3, p.Y + markerSize*M3)
                                       };
                        var pts2 = new[]
                                       {
                                           new Point(p.X - markerSize*M3, p.Y + markerSize*M3),
                                           new Point(p.X + markerSize*M3, p.Y - markerSize*M3)
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
                Size size = rc.MeasureText(s.Title, plot.LegendFont, plot.LegendFontSize);
                if (size.Width > maxWidth) maxWidth = size.Width;
                if (size.Height > maxHeight) maxHeight = size.Height;
                totalHeight += size.Height;
            }
            double ll = plot.LegendLineLength;
            double x0 = plotBounds.Right - LEGEND_PADDING;
            double y0 = plotBounds.Top + LEGEND_PADDING + maxHeight/2;

            foreach (DataSeries s in plot.Series)
            {
                if (String.IsNullOrEmpty(s.Title))
                    continue;
                rc.DrawText(new Point(x0 - ll - LEGEND_PADDING, y0), s.Title, plot.TextColor,
                            plot.LegendFont, plot.LegendFontSize, 500, 0,
                            HorizontalTextAlign.Right, VerticalTextAlign.Middle);
                RenderLegend(s, new Point(x0 - ll, y0), ll, maxHeight);
                y0 += maxHeight;
            }
        }

        private void RenderLegend(DataSeries dataSeries, Point point, double linew, double maxh)
        {
            var ls = dataSeries as LineSeries;
            var pts = new[] {point, new Point(point.X + linew, point.Y)};
            rc.DrawLine(pts, ls.Color, ls.Thickness, LineStyleHelper.GetDashArray(ls.LineStyle));
            var pm = new Point(point.X + linew/2, point.Y);
            RenderMarker(ls.MarkerType, pm, ls.MarkerSize, ls.MarkerFill, ls.MarkerStroke, ls.MarkerStrokeThickness);
        }

        #endregion
    }
}