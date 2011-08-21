using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OxyPlot
{
    public static class SutherlandHodgmanClipping
    {
        /// <summary>
        /// The Sutherland-Hodgman polygon clipping alrogithm.
        /// http://ezekiel.vancouver.wsu.edu/~cs442/lectures/clip/clip/index.html
        /// </summary>
        public static List<ScreenPoint> ClipPolygon(OxyRect bounds, List<ScreenPoint> v)
        {
            var p1 = ClipOneAxis(bounds, RectangleEdge.Left, v);
            var p2 = ClipOneAxis(bounds, RectangleEdge.Right, p1);
            var p3 = ClipOneAxis(bounds, RectangleEdge.Top, p2);
            return ClipOneAxis(bounds, RectangleEdge.Bottom, p3);
        }

        private enum RectangleEdge
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private static List<ScreenPoint> ClipOneAxis(OxyRect bounds, RectangleEdge edge, List<ScreenPoint> v)
        {
            if (v.Count == 0)
            {
                return new List<ScreenPoint>();
            }

            List<ScreenPoint> polygon = new List<ScreenPoint>(v.Count);

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
    }
}
