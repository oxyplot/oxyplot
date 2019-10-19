// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatmullRomSplineHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to interpolate a list of points by a canonical spline.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides functionality to interpolate a list of points by a Centripetal Catmull–Rom spline.
    /// </summary>
    /// <remarks>Based on CanonicalSplineHelper.cs (c) 2009 by Charles Petzold (WPF and Silverlight)
    /// See also <a href="http://www.charlespetzold.com/blog/2009/01/Canonical-Splines-in-WPF-and-Silverlight.html">blog post</a>.</remarks>
    public class CatmullRomSpline : IInterpolationAlgorithm
    {
        /// <summary>
        /// Gets the alpha value.
        /// </summary>
        public double Alpha { get; }
        
        /// <summary>
        /// Gets or sets the maximum number of segments.
        /// </summary>
        public int MaxSegments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref = "CatmullRomSpline" /> class.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        public CatmullRomSpline(double alpha)
        {
            this.Alpha = alpha;
            this.MaxSegments = 100;
        }

        /// <summary>
        /// Creates a spline of data points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A list of data points.</returns>
        public List<DataPoint> CreateSpline(List<DataPoint> points, bool isClosed, double tolerance)
        {
            return CreateSpline(points, this.Alpha, isClosed, tolerance, MaxSegments);
        }

        /// <summary>
        /// Creates a spline of screen points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>A list of screen points.</returns>
        public List<ScreenPoint> CreateSpline(IList<ScreenPoint> points, bool isClosed, double tolerance)
        {
            return CreateSpline(points, this.Alpha, isClosed, tolerance, MaxSegments);
        }

        /// <summary>
        /// Creates a spline of data points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="maxSegments">The maximum number of segments.</param>
        /// <returns>A list of data points.</returns>
        internal static List<DataPoint> CreateSpline(List<DataPoint> points, double alpha, bool isClosed, double tolerance, int maxSegments)
        {
            var screenPoints = points.Select(p => new ScreenPoint(p.X, p.Y)).ToList();
            var interpolatedScreenPoints = CreateSpline(screenPoints, alpha, isClosed, tolerance, maxSegments);
            var interpolatedDataPoints = new List<DataPoint>(interpolatedScreenPoints.Count);

            foreach (var s in interpolatedScreenPoints)
            {
                interpolatedDataPoints.Add(new DataPoint(s.X, s.Y));
            }

            return interpolatedDataPoints;
        }

        /// <summary>
        /// Creates a spline of screen points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="isClosed">True if the spline is closed.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="maxSegments">The maximum number of segments.</param>
        /// <returns>A list of screen points.</returns>
        internal static List<ScreenPoint> CreateSpline(
            IList<ScreenPoint> points, double alpha, bool isClosed, double tolerance, int maxSegments)
        {
            var result = new List<ScreenPoint>();
            if (points == null)
            {
                return result;
            }

            int n = points.Count;
            if (n < 1)
            {
                return result;
            }

            if (n < 2)
            {
                result.AddRange(points);
                return result;
            }

            if (n == 2)
            {
                if (!isClosed)
                {
                    Segment(result, points[0], points[0], points[1], points[1], alpha, tolerance, maxSegments);
                }
                else
                {
                    Segment(result, points[1], points[0], points[1], points[0], alpha, tolerance, maxSegments);
                    Segment(result, points[0], points[1], points[0], points[1], alpha, tolerance, maxSegments);
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == 0)
                    {
                        Segment(
                            result,
                            isClosed ? points[n - 1] : points[0],
                            points[0],
                            points[1],
                            points[2],
                            alpha,
                            tolerance,
                            maxSegments);
                    }
                    else if (i == n - 2)
                    {
                        Segment(
                            result,
                            points[i - 1],
                            points[i],
                            points[i + 1],
                            isClosed ? points[0] : points[i + 1],
                            alpha,
                            tolerance,
                            maxSegments);
                    }
                    else if (i == n - 1)
                    {
                        if (isClosed)
                        {
                            Segment(result, points[i - 1], points[i], points[0], points[1], alpha, tolerance, maxSegments);
                        }
                    }
                    else
                    {
                        Segment(result, points[i - 1], points[i], points[i + 1], points[i + 2], alpha, tolerance, maxSegments);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// The segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="pt0">The pt 0.</param>
        /// <param name="pt1">The pt 1.</param>
        /// <param name="pt2">The pt 2.</param>
        /// <param name="pt3">The pt 3.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="maxSegments">The maximum number of segments.</param>
        private static void Segment(
            IList<ScreenPoint> points,
            ScreenPoint pt0,
            ScreenPoint pt1,
            ScreenPoint pt2,
            ScreenPoint pt3,
            double alpha,
            double tolerance,
            int maxSegments)
        {
            if (Equals(pt1, pt2))
            {
                points.Add(pt1);
                return;
            }

            if (Equals(pt0, pt1))
            {
                pt0 = Prev(pt1, pt2);
            }

            if (Equals(pt2, pt3))
            {
                pt3 = Prev(pt2, pt1);
            }

            double t0 = 0d;
            double t1 = GetT(t0, pt0, pt1, alpha);
            double t2 = GetT(t1, pt1, pt2, alpha);
            double t3 = GetT(t2, pt2, pt3, alpha);


            int iterations = (int)((Math.Abs(pt1.X - pt2.X) + Math.Abs(pt1.Y - pt2.Y)) / tolerance);
            //Make sure it is positive (negative means an integer overflow)
            iterations = Math.Max(0, iterations);
            //Never more iterations than maxSegments
            iterations = Math.Min(maxSegments, iterations);
            for (double t = t1; t < t2; t += (t2 - t1) / iterations)
            {
                ScreenPoint a1 = Sum(Mult((t1 - t) / (t1 - t0), pt0), Mult((t - t0) / (t1 - t0), pt1));
                ScreenPoint a2 = Sum(Mult((t2 - t) / (t2 - t1), pt1), Mult((t - t1) / (t2 - t1), pt2));
                ScreenPoint a3 = Sum(Mult((t3 - t) / (t3 - t2), pt2), Mult((t - t2) / (t3 - t2), pt3));

                ScreenPoint b1 = Sum(Mult((t2 - t) / (t2 - t0), a1), Mult((t - t0) / (t2 - t0), a2));
                ScreenPoint b2 = Sum(Mult((t3 - t) / (t3 - t1), a2), Mult((t - t1) / (t3 - t1), a3));

                ScreenPoint c1 = Sum(Mult((t2 - t) / (t2 - t1), b1), Mult((t - t1) / (t2 - t1), b2));
                points.Add(c1);
            }
        }

        private static double GetT(double t, ScreenPoint p0, ScreenPoint p1, double alpha)
        {
            double a = Math.Pow(p1.X - p0.X, 2d) + Math.Pow(p1.Y - p0.Y, 2d);
            double b = Math.Pow(a, 0.5);
            double c = Math.Pow(b, alpha);
            return c + t;
        }

        private static ScreenPoint Mult(double d, ScreenPoint s)
        {
            return new ScreenPoint(s.X * d, s.Y * d);
        }

        private static bool Equals(ScreenPoint a, ScreenPoint b)
        {
            return Equals(a.X, b.X) && Equals(a.Y, b.Y);
        }

        private static ScreenPoint Prev(ScreenPoint s0, ScreenPoint s1)
        {
            return new ScreenPoint(s0.X - 0.0001 * (s1.X - s0.X), s0.Y - 0.0001 * (s1.Y - s0.Y));
        }

        private static ScreenPoint Sum(ScreenPoint a, ScreenPoint b)
        {
            return new ScreenPoint(a.X + b.X, a.Y + b.Y);
        }
    }
}
