//-----------------------------------------------------------------------
// <copyright file="SutherlandHodgmanClipping.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The sutherland hodgman clipping.
    /// </summary>
    public static class SutherlandHodgmanClipping
    {
        #region Enums

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

        #endregion

        #region Public Methods

        /// <summary>
        /// The Sutherland-Hodgman polygon clipping alrogithm.
        /// http://ezekiel.vancouver.wsu.edu/~cs442/lectures/clip/clip/index.html
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        /// <param name="v">
        /// The v.
        /// </param>
        public static List<ScreenPoint> ClipPolygon(OxyRect bounds, IList<ScreenPoint> v)
        {
            List<ScreenPoint> p1 = ClipOneAxis(bounds, RectangleEdge.Left, v);
            List<ScreenPoint> p2 = ClipOneAxis(bounds, RectangleEdge.Right, p1);
            List<ScreenPoint> p3 = ClipOneAxis(bounds, RectangleEdge.Top, p2);
            return ClipOneAxis(bounds, RectangleEdge.Bottom, p3);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The clip one axis.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        /// <param name="edge">
        /// The edge.
        /// </param>
        /// <param name="v">
        /// The v.
        /// </param>
        /// <returns>
        /// </returns>
        private static List<ScreenPoint> ClipOneAxis(OxyRect bounds, RectangleEdge edge, IList<ScreenPoint> v)
        {
            if (v.Count == 0)
            {
                return new List<ScreenPoint>();
            }

            var polygon = new List<ScreenPoint>(v.Count);

            ScreenPoint s = v[v.Count - 1];

            for (int i = 0; i < v.Count; ++i)
            {
                ScreenPoint p = v[i];
                bool pIn = IsInside(bounds, edge, p);
                bool sIn = IsInside(bounds, edge, s);

                if (sIn && pIn)
                {
                    // case 1: inside -> inside
                    polygon.Add(p);
                }
                else if (sIn && !pIn)
                {
                    // case 2: inside -> outside
                    polygon.Add(LineIntercept(bounds, edge, s, p));
                }
                else if (!sIn && !pIn)
                {
                    // case 3: outside -> outside
                    // emit nothing
                }
                else if (!sIn && pIn)
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
        /// The is inside.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        /// <param name="edge">
        /// The edge.
        /// </param>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// The is inside.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
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
        /// The line intercept.
        /// </summary>
        /// <param name="bounds">
        /// The bounds.
        /// </param>
        /// <param name="edge">
        /// The edge.
        /// </param>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        private static ScreenPoint LineIntercept(OxyRect bounds, RectangleEdge edge, ScreenPoint a, ScreenPoint b)
        {
            if (a.x == b.x && a.y == b.y)
            {
                return a;
            }

            switch (edge)
            {
                case RectangleEdge.Bottom:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case RectangleEdge.Left:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case RectangleEdge.Right:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case RectangleEdge.Top:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new ScreenPoint(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }

        #endregion
    }
}
