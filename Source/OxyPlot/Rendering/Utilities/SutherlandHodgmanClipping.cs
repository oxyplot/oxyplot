// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SutherlandHodgmanClipping.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides polygon clipping by the Sutherland-Hodgman algorithm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides polygon clipping by the Sutherland-Hodgman algorithm.
    /// </summary>
    public static class SutherlandHodgmanClipping
    {
        /// <summary>
        /// The rectangle edge.
        /// </summary>
        private enum RectangleEdge
        {
            /// <summary>
            /// The left.
            /// </summary>
            Left,

            /// <summary>
            /// The right.
            /// </summary>
            Right,

            /// <summary>
            /// The top.
            /// </summary>
            Top,

            /// <summary>
            /// The bottom.
            /// </summary>
            Bottom
        }

        /// <summary>
        /// The Sutherland-Hodgman polygon clipping algorithm.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="v">The polygon points.</param>
        /// <returns>The clipped points.</returns>
        /// <remarks>See <a href="http://ezekiel.vancouver.wsu.edu/~cs442/lectures/clip/clip/index.html">link</a>.</remarks>
        public static List<ScreenPoint> ClipPolygon(OxyRect bounds, IList<ScreenPoint> v)
        {
            List<ScreenPoint> p1 = ClipOneAxis(bounds, RectangleEdge.Left, v);
            List<ScreenPoint> p2 = ClipOneAxis(bounds, RectangleEdge.Right, p1);
            List<ScreenPoint> p3 = ClipOneAxis(bounds, RectangleEdge.Top, p2);
            return ClipOneAxis(bounds, RectangleEdge.Bottom, p3);
        }

        /// <summary>
        /// Clips to one axis.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="edge">The edge.</param>
        /// <param name="v">The points of the polygon.</param>
        /// <returns>The clipped points.</returns>
        private static List<ScreenPoint> ClipOneAxis(OxyRect bounds, RectangleEdge edge, IList<ScreenPoint> v)
        {
            if (v.Count == 0)
            {
                return new List<ScreenPoint>();
            }

            var polygon = new List<ScreenPoint>(v.Count);

            var s = v[v.Count - 1];

            for (int i = 0; i < v.Count; ++i)
            {
                var p = v[i];
                bool pin = IsInside(bounds, edge, p);
                bool sin = IsInside(bounds, edge, s);

                if (sin && pin)
                {
                    // case 1: inside -> inside
                    polygon.Add(p);
                }
                else if (sin)
                {
                    // case 2: inside -> outside
                    polygon.Add(LineIntercept(bounds, edge, s, p));
                }
                else if (!pin)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else
                {
                    // case 4: outside -> inside
                    polygon.Add(LineIntercept(bounds, edge, s, p));
                    polygon.Add(p);
                }

                s = p;
            }

            return polygon;
        }

        /// <summary>
        /// Determines whether the specified point is inside the edge/bounds.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="edge">The edge to test.</param>
        /// <param name="p">The point.</param>
        /// <returns><c>true</c> if the specified point is inside; otherwise, <c>false</c>.</returns>
        private static bool IsInside(OxyRect bounds, RectangleEdge edge, ScreenPoint p)
        {
            switch (edge)
            {
                case RectangleEdge.Left:
                    return !(p.X < bounds.Left);

                case RectangleEdge.Right:
                    return !(p.X >= bounds.Right);

                case RectangleEdge.Top:
                    return !(p.Y < bounds.Top);

                case RectangleEdge.Bottom:
                    return !(p.Y >= bounds.Bottom);

                default:
                    throw new ArgumentException("edge");
            }
        }

        /// <summary>
        /// Fines the edge interception.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="edge">The edge.</param>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The interception.</returns>
        private static ScreenPoint LineIntercept(OxyRect bounds, RectangleEdge edge, ScreenPoint a, ScreenPoint b)
        {
            if (a.x.Equals(b.x) && a.y.Equals(b.y))
            {
                return a;
            }

            switch (edge)
            {
                case RectangleEdge.Bottom:
                    if (b.Y.Equals(a.Y))
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case RectangleEdge.Left:
                    if (b.X.Equals(a.X))
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case RectangleEdge.Right:
                    if (b.X.Equals(a.X))
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case RectangleEdge.Top:
                    if (b.Y.Equals(a.Y))
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }
    }
}