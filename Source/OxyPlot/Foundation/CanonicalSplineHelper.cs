using System;
using System.Collections.Generic;

namespace OxyPlot
{
    /// <summary>
    /// Interpolates a list of points using a canonical spline.
    /// </summary>
    internal static class CanonicalSplineHelper
    {
        //   CanonicalSplineHelper.cs (c) 2009 by Charles Petzold (WPF and Silverlight)
        //   www.charlespetzold.com/blog/2009/01/Canonical-Splines-in-WPF-and-Silverlight.html

        internal static List<ScreenPoint> CreateSpline(IList<ScreenPoint> pts,
                                                       double tension, IList<double> tensions,
                                                       bool isClosed, double tolerance)
        {
            var result = new List<ScreenPoint>();
            if (pts == null)
            {
                return result;
            }

            int n = pts.Count;
            if (n < 1)
            {
                return result;
            }


            if (n < 2)
            {
                result.AddRange(pts);
                return result;
            }

            if (n == 2)
            {
                if (!isClosed)
                {
                    Segment(result, pts[0], pts[0], pts[1], pts[1], tension, tension, tolerance);
                }
                else
                {
                    Segment(result, pts[1], pts[0], pts[1], pts[0], tension, tension, tolerance);
                    Segment(result, pts[0], pts[1], pts[0], pts[1], tension, tension, tolerance);
                }
            }
            else
            {
                bool useTensionCollection = tensions != null && tensions.Count > 0;

                for (int i = 0; i < n; i++)
                {
                    double t1 = useTensionCollection ? tensions[i % tensions.Count] : tension;
                    double t2 = useTensionCollection ? tensions[(i + 1) % tensions.Count] : tension;

                    if (i == 0)
                    {
                        Segment(result, isClosed ? pts[n - 1] : pts[0],
                                pts[0], pts[1], pts[2], t1, t2, tolerance);
                    }
                    else if (i == n - 2)
                    {
                        Segment(result, pts[i - 1], pts[i], pts[i + 1],
                                isClosed ? pts[0] : pts[i + 1], t1, t2, tolerance);
                    }
                    else if (i == n - 1)
                    {
                        if (isClosed)
                        {
                            Segment(result, pts[i - 1], pts[i], pts[0], pts[1], t1, t2, tolerance);
                        }
                    }
                    else
                    {
                        Segment(result, pts[i - 1], pts[i], pts[i + 1], pts[i + 2], t1, t2, tolerance);
                    }
                }
            }

            return result;
        }

        private static void Segment(IList<ScreenPoint> points,
                                    ScreenPoint pt0, ScreenPoint pt1, ScreenPoint pt2, ScreenPoint pt3,
                                    double t1, double t2, double tolerance)
        {
            // See Petzold, "Programming Microsoft Windows with C#", pages 645-646 or 
            // Petzold, "Programming Microsoft Windows with Microsoft Visual Basic .NET", pages 638-639
            // for derivation of the following formulas:
            double sx1 = t1 * (pt2.X - pt0.X);
            double sy1 = t1 * (pt2.Y - pt0.Y);
            double sx2 = t2 * (pt3.X - pt1.X);
            double sy2 = t2 * (pt3.Y - pt1.Y);

            double ax = sx1 + sx2 + 2 * pt1.X - 2 * pt2.X;
            double ay = sy1 + sy2 + 2 * pt1.Y - 2 * pt2.Y;
            double bx = -2 * sx1 - sx2 - 3 * pt1.X + 3 * pt2.X;
            double by = -2 * sy1 - sy2 - 3 * pt1.Y + 3 * pt2.Y;

            double cx = sx1;
            double cy = sy1;
            double dx = pt1.X;
            double dy = pt1.Y;

            var num = (int)((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / tolerance);

            // Notice begins at 1 so excludes the first point (which is just pt1)
            for (int i = 1; i < num; i++)
            {
                double t = (double)i / (num - 1);
                var pt = new ScreenPoint(ax * t * t * t + bx * t * t + cx * t + dx,
                                         ay * t * t * t + by * t * t + cy * t + dy);
                points.Add(pt);
            }
        }
    }
}