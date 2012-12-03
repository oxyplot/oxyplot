// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingExtensions.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The rendering extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The rendering extensions.
    /// </summary>
    public static class RenderingExtensions
    {
        //// Length constants used to draw triangles and stars
        //// ___
        //// /\                   |
        //// /  \                 |
        //// /    \               | M2
        //// /      \             |
        //// /        \           |
        //// /     +    \        ---
        //// /            \       |
        //// /              \     | M1
        //// /________________\  _|_
        //// |--------|-------|
        //// 1       1
        ////
        //// |
        //// \     |     /     ---
        //// \   |   /        | M3
        //// \ | /          |
        //// ---------+--------   ---
        //// / | \          | M3
        //// /   |   \        |
        //// /     |     \     ---
        //// |
        //// |-----|-----|
        //// M3    M3

        /// <summary>
        /// The m 1.
        /// </summary>
        private static readonly double M1 = Math.Tan(Math.PI / 6);

        /// <summary>
        /// The m 2.
        /// </summary>
        private static readonly double M2 = Math.Sqrt(1 + (M1 * M1));

        /// <summary>
        /// The m 3.
        /// </summary>
        private static readonly double M3 = Math.Tan(Math.PI / 4);

        /// <summary>
        /// Draws the clipped line.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points.</param>
        /// <param name="clippingRectangle">The clipping rectangle.</param>
        /// <param name="minDistSquared">The min dist squared.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        /// <param name="pointsRendered">The points rendered callback.</param>
        public static void DrawClippedLine(
            this IRenderContext rc,
            IList<ScreenPoint> points,
            OxyRect clippingRectangle,
            double minDistSquared,
            OxyColor stroke,
            double strokeThickness,
            LineStyle lineStyle,
            OxyPenLineJoin lineJoin,
            bool aliased,
            Action<IList<ScreenPoint>> pointsRendered = null)
        {
            var clipping = new CohenSutherlandClipping(
                clippingRectangle.Left, clippingRectangle.Right, clippingRectangle.Top, clippingRectangle.Bottom);

            var pts = new List<ScreenPoint>();
            int n = points.Count;
            if (n > 0)
            {

                if (n == 1)
                {
                    pts.Add(points[0]);
                }

                var last = points[0];
                for (int i = 1; i < n; i++)
                {
                    var s0 = points[i - 1];
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

                    if (dx * dx + dy * dy > minDistSquared || i == 1 || i == n - 1)
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
                        {
                            rc.DrawLine(
                                pts, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin, aliased);
                            if (pointsRendered != null)
                            {
                                pointsRendered(pts);
                            }
                            pts = new List<ScreenPoint>();
                        }
                    }
                }

                // Check if the line contains two points and they are at the same point
                if (pts.Count == 2)
                {
                    if (pts[0].DistanceTo(pts[1]) < 1)
                    {
                        // Modify to a small horizontal line to make sure it is being rendered
                        pts[1] = new ScreenPoint(pts[0].X + 1, pts[0].Y);
                        pts[0] = new ScreenPoint(pts[0].X - 1, pts[0].Y);
                    }
                }

                // Check if the line contains a single point
                if (pts.Count == 1)
                {
                    // Add a second point to make sure the line is being rendered as a small dot
                    pts.Add(new ScreenPoint(pts[0].X + 1, pts[0].Y));
                    pts[0] = new ScreenPoint(pts[0].X - 1, pts[0].Y);
                }

                if (pts.Count > 0)
                {
                    rc.DrawLine(pts, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin, aliased);

                    // Execute the 'callback'.
                    if (pointsRendered != null)
                    {
                        pointsRendered(pts);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the clipped line segments.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points.</param>
        /// <param name="clippingRectangle">The clipping rectangle.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        public static void DrawClippedLineSegments(
            this IRenderContext rc,
            IList<ScreenPoint> points,
            OxyRect clippingRectangle,
            OxyColor stroke,
            double strokeThickness,
            LineStyle lineStyle,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            var clipping = new CohenSutherlandClipping(clippingRectangle.Left, clippingRectangle.Right, clippingRectangle.Top, clippingRectangle.Bottom);

            var clippedPoints = new List<ScreenPoint>(points.Count);
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                var s0 = points[i];
                var s1 = points[i + 1];
                if (clipping.ClipLine(ref s0, ref s1))
                {
                    clippedPoints.Add(s0);
                    clippedPoints.Add(s1);
                }
            }

            rc.DrawLineSegments(clippedPoints, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin, aliased);
        }

        /// <summary>
        /// The draw clipped polygon.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="clippingRectangle">
        /// The clipping rectangle.
        /// </param>
        /// <param name="minDistSquared">
        /// The min dist squared.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness.
        /// </param>
        /// <param name="lineStyle">
        /// The line style.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public static void DrawClippedPolygon(
            this IRenderContext rc,
            IList<ScreenPoint> points,
            OxyRect clippingRectangle,
            double minDistSquared,
            OxyColor fill,
            OxyColor stroke,
            double strokeThickness = 1.0,
            LineStyle lineStyle = LineStyle.Solid,
            OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter,
            bool aliased = false)
        {
            var clippedPoints = SutherlandHodgmanClipping.ClipPolygon(clippingRectangle, points);

            rc.DrawPolygon(
                clippedPoints, fill, stroke, strokeThickness, LineStyleHelper.GetDashArray(lineStyle), lineJoin, aliased);
        }

        /// <summary>
        /// Draws the clipped rectangle.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="rect">
        /// The rectangle to draw.
        /// </param>
        /// <param name="clippingRectangle">
        /// The clipping rectangle.
        /// </param>
        /// <param name="fill">
        /// The fill color.
        /// </param>
        /// <param name="stroke">
        /// The stroke color.
        /// </param>
        /// <param name="thickness">
        /// The stroke thickness.
        /// </param>
        public static void DrawClippedRectangle(
            this IRenderContext rc,
            OxyRect rect,
            OxyRect clippingRectangle,
            OxyColor fill,
            OxyColor stroke,
            double thickness)
        {
            OxyRect? clippedRect = ClipRect(rect, clippingRectangle);
            if (clippedRect == null)
            {
                return;
            }

            rc.DrawRectangle(clippedRect.Value, fill, stroke, thickness);
        }

        /// <summary>
        /// Draws the clipped rectangle as a polygon.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="rect">
        /// The rectangle to draw.
        /// </param>
        /// <param name="clippingRectangle">
        /// The clipping rectangle.
        /// </param>
        /// <param name="fill">
        /// The fill color.
        /// </param>
        /// <param name="stroke">
        /// The stroke color.
        /// </param>
        /// <param name="thickness">
        /// The stroke thickness.
        /// </param>
        public static void DrawClippedRectangleAsPolygon(
            this IRenderContext rc,
            OxyRect rect,
            OxyRect clippingRectangle,
            OxyColor fill,
            OxyColor stroke,
            double thickness)
        {
            OxyRect? clippedRect = ClipRect(rect, clippingRectangle);
            if (clippedRect == null)
            {
                return;
            }

            rc.DrawRectangleAsPolygon(clippedRect.Value, fill, stroke, thickness);
        }

        /// <summary>
        /// Draws the clipped text.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRectangle">The clipping rectangle.</param>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="horizontalAlignment">The horizontal align.</param>
        /// <param name="verticalAlignment">The vertical align.</param>
        /// <param name="maxSize">Size of the max.</param>
        public static void DrawClippedText(
            this IRenderContext rc,
            OxyRect clippingRectangle,
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily = null,
            double fontSize = 10,
            double fontWeight = 500,
            double rotate = 0,
            HorizontalTextAlign horizontalAlignment = HorizontalTextAlign.Left,
            VerticalTextAlign verticalAlignment = VerticalTextAlign.Top,
            OxySize? maxSize = null)
        {
            if (clippingRectangle.Contains(p.X, p.Y))
            {
                rc.DrawText(p, text, fill, fontFamily, fontSize, fontWeight, rotate, horizontalAlignment, verticalAlignment, maxSize);
            }
        }

        /// <summary>
        /// Draws a line specified by coordinates.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="x0">
        /// The x0.
        /// </param>
        /// <param name="y0">
        /// The y0.
        /// </param>
        /// <param name="x1">
        /// The x1.
        /// </param>
        /// <param name="y1">
        /// The y1.
        /// </param>
        /// <param name="pen">
        /// The pen.
        /// </param>
        /// <param name="aliased">
        /// Aliased line if set to <c>true</c>.
        /// </param>
        public static void DrawLine(
            this IRenderContext rc, double x0, double y0, double x1, double y1, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLine(
                new[] { new ScreenPoint(x0, y0), new ScreenPoint(x1, y1) },
                pen.Color,
                pen.Thickness,
                pen.DashArray,
                pen.LineJoin,
                aliased);
        }

        /// <summary>
        /// Draws the line segments.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="pen">
        /// The pen.
        /// </param>
        /// <param name="aliased">
        /// if set to <c>true</c> [aliased].
        /// </param>
        public static void DrawLineSegments(
            this IRenderContext rc, IList<ScreenPoint> points, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLineSegments(points, pen.Color, pen.Thickness, pen.DashArray, pen.LineJoin, aliased);
        }

        /// <summary>
        /// Renders the marker.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="p">The center point of the marker.</param>
        /// <param name="clippingRect">The clipping rect.</param>
        /// <param name="type">The marker type.</param>
        /// <param name="outline">The outline.</param>
        /// <param name="size">The size of the marker.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        public static void DrawMarker(
            this IRenderContext rc,
            ScreenPoint p,
            OxyRect clippingRect,
            MarkerType type,
            IList<ScreenPoint> outline,
            double size,
            OxyColor fill,
            OxyColor stroke,
            double strokeThickness)
        {
            rc.DrawMarkers(new[] { p }, clippingRect, type, outline, new[] { size }, fill, stroke, strokeThickness);
        }

        /// <summary>
        /// Draws a list of markers.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="markerPoints">
        /// The marker points.
        /// </param>
        /// <param name="clippingRect">
        /// The clipping rectangle.
        /// </param>
        /// <param name="markerType">
        /// Type of the marker.
        /// </param>
        /// <param name="markerOutline">
        /// The marker outline.
        /// </param>
        /// <param name="markerSize">
        /// Size of the marker.
        /// </param>
        /// <param name="markerFill">
        /// The marker fill.
        /// </param>
        /// <param name="markerStroke">
        /// The marker stroke.
        /// </param>
        /// <param name="markerStrokeThickness">
        /// The marker stroke thickness.
        /// </param>
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        /// <param name="binOffset">
        /// The bin Offset.
        /// </param>
        public static void DrawMarkers(
            this IRenderContext rc,
            IList<ScreenPoint> markerPoints,
            OxyRect clippingRect,
            MarkerType markerType,
            IList<ScreenPoint> markerOutline,
            double markerSize,
            OxyColor markerFill,
            OxyColor markerStroke,
            double markerStrokeThickness,
            int resolution = 0,
            ScreenPoint binOffset = new ScreenPoint())
        {
            DrawMarkers(
                rc,
                markerPoints,
                clippingRect,
                markerType,
                markerOutline,
                new[] { markerSize },
                markerFill,
                markerStroke,
                markerStrokeThickness,
                resolution,
                binOffset);
        }

        /// <summary>
        /// Draws a list of markers.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="markerPoints">
        /// The marker points.
        /// </param>
        /// <param name="clippingRect">
        /// The clipping rectangle.
        /// </param>
        /// <param name="markerType">
        /// Type of the marker.
        /// </param>
        /// <param name="markerOutline">
        /// The marker outline.
        /// </param>
        /// <param name="markerSize">
        /// Size of the markers.
        /// </param>
        /// <param name="markerFill">
        /// The marker fill.
        /// </param>
        /// <param name="markerStroke">
        /// The marker stroke.
        /// </param>
        /// <param name="markerStrokeThickness">
        /// The marker stroke thickness.
        /// </param>
        /// <param name="resolution">
        /// The resolution.
        /// </param>
        /// <param name="binOffset">
        /// The bin Offset.
        /// </param>
        public static void DrawMarkers(
            this IRenderContext rc,
            IList<ScreenPoint> markerPoints,
            OxyRect clippingRect,
            MarkerType markerType,
            IList<ScreenPoint> markerOutline,
            IList<double> markerSize,
            OxyColor markerFill,
            OxyColor markerStroke,
            double markerStrokeThickness,
            int resolution = 0,
            ScreenPoint binOffset = new ScreenPoint())
        {
            if (markerType == MarkerType.None)
            {
                return;
            }

            int n = markerPoints.Count;
            var ellipses = new List<OxyRect>(n);
            var rects = new List<OxyRect>(n);
            var polygons = new List<IList<ScreenPoint>>(n);
            var lines = new List<ScreenPoint>(n);

            var hashset = new HashSet<uint>();

            int i = 0;

            double minx = clippingRect.Left;
            double maxx = clippingRect.Right;
            double miny = clippingRect.Top;
            double maxy = clippingRect.Bottom;

            foreach (var p in markerPoints)
            {
                if (resolution > 1)
                {
                    var x = (int)((p.X - binOffset.X) / resolution);
                    var y = (int)((p.Y - binOffset.Y) / resolution);
                    uint hash = (uint)(x << 16) + (uint)y;
                    if (hashset.Contains(hash))
                    {
                        i++;
                        continue;
                    }

                    hashset.Add(hash);
                }

                bool outside = p.x < minx || p.x > maxx || p.y < miny || p.y > maxy;
                if (!outside)
                {
                    int j = i < markerSize.Count ? i : 0;
                    AddMarkerGeometry(p, markerType, markerOutline, markerSize[j], ellipses, rects, polygons, lines);
                }

                i++;
            }

            if (ellipses.Count > 0)
            {
                rc.DrawEllipses(ellipses, markerFill, markerStroke, markerStrokeThickness);
            }

            if (rects.Count > 0)
            {
                rc.DrawRectangles(rects, markerFill, markerStroke, markerStrokeThickness);
            }

            if (polygons.Count > 0)
            {
                rc.DrawPolygons(polygons, markerFill, markerStroke, markerStrokeThickness);
            }

            if (lines.Count > 0)
            {
                rc.DrawLineSegments(lines, markerStroke, markerStrokeThickness);
            }
        }

        /// <summary>
        /// Draws the rectangle as an aliased polygon.
        /// (makes sure pixel alignment is the same as for lines)
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="rect">
        /// The rectangle.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public static void DrawRectangleAsPolygon(
            this IRenderContext rc, OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var sp0 = new ScreenPoint(rect.Left, rect.Top);
            var sp1 = new ScreenPoint(rect.Right, rect.Top);
            var sp2 = new ScreenPoint(rect.Right, rect.Bottom);
            var sp3 = new ScreenPoint(rect.Left, rect.Bottom);
            rc.DrawPolygon(new[] { sp0, sp1, sp2, sp3 }, fill, stroke, thickness, null, OxyPenLineJoin.Miter, true);
        }

        /// <summary>
        /// Draws the rectangle as an aliased polygon.
        /// (makes sure pixel alignment is the same as for lines)
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="rect">
        /// The rectangle.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public static void DrawRectangleAsPolygon(
            this IRenderContext rc, OxyRect rect, OxyColor fill, OxyColor stroke, OxyThickness thickness)
        {
            if (thickness.Left == thickness.Right && thickness.Left == thickness.Top
                && thickness.Left == thickness.Bottom)
            {
                DrawRectangleAsPolygon(rc, rect, fill, stroke, thickness.Left);
                return;
            }

            var sp0 = new ScreenPoint(rect.Left, rect.Top);
            var sp1 = new ScreenPoint(rect.Right, rect.Top);
            var sp2 = new ScreenPoint(rect.Right, rect.Bottom);
            var sp3 = new ScreenPoint(rect.Left, rect.Bottom);
            rc.DrawPolygon(new[] { sp0, sp1, sp2, sp3 }, fill, null, 0, null, OxyPenLineJoin.Miter, true);
            rc.DrawPolygon(new[] { sp0, sp1 }, null, stroke, thickness.Top, null, OxyPenLineJoin.Miter, true);
            rc.DrawPolygon(new[] { sp1, sp2 }, null, stroke, thickness.Right, null, OxyPenLineJoin.Miter, true);
            rc.DrawPolygon(new[] { sp2, sp3 }, null, stroke, thickness.Bottom, null, OxyPenLineJoin.Miter, true);
            rc.DrawPolygon(new[] { sp3, sp0 }, null, stroke, thickness.Left, null, OxyPenLineJoin.Miter, true);
        }

        /// <summary>
        /// The add marker geometry.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="outline">
        /// The outline.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="ellipses">
        /// The ellipses.
        /// </param>
        /// <param name="rects">
        /// The rects.
        /// </param>
        /// <param name="polygons">
        /// The polygons.
        /// </param>
        /// <param name="lines">
        /// The lines.
        /// </param>
        private static void AddMarkerGeometry(
            ScreenPoint p,
            MarkerType type,
            IEnumerable<ScreenPoint> outline,
            double size,
            IList<OxyRect> ellipses,
            IList<OxyRect> rects,
            IList<IList<ScreenPoint>> polygons,
            IList<ScreenPoint> lines)
        {
            if (type == MarkerType.Custom)
            {
                if (outline == null)
                {
                    throw new ArgumentNullException("outline", "The outline should be set when MarkerType is 'Custom'.");
                }
                var poly = outline.Select(o => new ScreenPoint(p.X + (o.x * size), p.Y + (o.y * size))).ToList();
                polygons.Add(poly);
                return;
            }

            switch (type)
            {
                case MarkerType.Circle:
                    {
                        ellipses.Add(new OxyRect(p.x - size, p.y - size, size * 2, size * 2));
                        break;
                    }

                case MarkerType.Square:
                    {
                        rects.Add(new OxyRect(p.x - size, p.y - size, size * 2, size * 2));
                        break;
                    }

                case MarkerType.Diamond:
                    {
                        polygons.Add(
                            new[]
                                {
                                    new ScreenPoint(p.x, p.y - M2 * size), new ScreenPoint(p.x + M2 * size, p.y),
                                    new ScreenPoint(p.x, p.y + M2 * size), new ScreenPoint(p.x - M2 * size, p.y)
                                });
                        break;
                    }

                case MarkerType.Triangle:
                    {
                        polygons.Add(
                            new[]
                                {
                                    new ScreenPoint(p.x - size, p.y + M1 * size),
                                    new ScreenPoint(p.x + size, p.y + M1 * size), new ScreenPoint(p.x, p.y - M2 * size)
                                });
                        break;
                    }

                case MarkerType.Plus:
                case MarkerType.Star:
                    {
                        lines.Add(new ScreenPoint(p.x - size, p.y));
                        lines.Add(new ScreenPoint(p.x + size, p.y));
                        lines.Add(new ScreenPoint(p.x, p.y - size));
                        lines.Add(new ScreenPoint(p.x, p.y + size));
                        break;
                    }
            }

            switch (type)
            {
                case MarkerType.Cross:
                case MarkerType.Star:
                    {
                        lines.Add(new ScreenPoint(p.x - size * M3, p.y - size * M3));
                        lines.Add(new ScreenPoint(p.x + size * M3, p.y + size * M3));
                        lines.Add(new ScreenPoint(p.x - size * M3, p.y + size * M3));
                        lines.Add(new ScreenPoint(p.x + size * M3, p.y - size * M3));
                        break;
                    }
            }
        }

        /// <summary>
        /// Calculates the clipped version of a rectangle.
        /// </summary>
        /// <param name="rect">
        /// The rectangle to clip.
        /// </param>
        /// <param name="clippingRectangle">
        /// The clipping rectangle.
        /// </param>
        /// <returns>
        /// The clipped rectangle, or null if the rectangle is outside the clipping area.
        /// </returns>
        private static OxyRect? ClipRect(OxyRect rect, OxyRect clippingRectangle)
        {
            if (rect.Right < clippingRectangle.Left)
            {
                return null;
            }

            if (rect.Left > clippingRectangle.Right)
            {
                return null;
            }

            if (rect.Top > clippingRectangle.Bottom)
            {
                return null;
            }

            if (rect.Bottom < clippingRectangle.Top)
            {
                return null;
            }

            if (rect.Right > clippingRectangle.Right)
            {
                rect.Right = clippingRectangle.Right;
            }

            if (rect.Left < clippingRectangle.Left)
            {
                rect.Width = rect.Right - clippingRectangle.Left;
                rect.Left = clippingRectangle.Left;
            }

            if (rect.Top < clippingRectangle.Top)
            {
                rect.Height = rect.Bottom - clippingRectangle.Top;
                rect.Top = clippingRectangle.Top;
            }

            if (rect.Bottom > clippingRectangle.Bottom)
            {
                rect.Bottom = clippingRectangle.Bottom;
            }

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return null;
            }

            return rect;
        }

    }
}