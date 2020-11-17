// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for <see cref="IRenderContext" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="IRenderContext" />.
    /// </summary>
    public static class RenderingExtensions
    {
        /* Length constants used to draw triangles and stars
                             ___
         /\                   |
         /  \                 |
         /    \               | M2
         /      \             |
         /        \           |
         /     +    \        ---
         /            \       |
         /              \     | M1
         /________________\  _|_
         |--------|-------|
              1       1

                  |
            \     |     /     ---
              \   |   /        | M3
                \ | /          |
         ---------+--------   ---
                / | \          | M3
              /   |   \        |
            /     |     \     ---
                  |
            |-----|-----|
               M3    M3
        */

        /// <summary>
        /// The vertical distance to the bottom points of the triangles.
        /// </summary>
        private static readonly double M1 = Math.Tan(Math.PI / 6);

        /// <summary>
        /// The vertical distance to the top points of the triangles .
        /// </summary>
        private static readonly double M2 = Math.Sqrt(1 + (M1 * M1));

        /// <summary>
        /// The horizontal/vertical distance to the end points of the stars.
        /// </summary>
        private static readonly double M3 = Math.Tan(Math.PI / 4);

        /// <summary>
        /// Gets the actual edge rendering mode.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="defaultValue">The default value that is used if edgeRenderingMode is <see cref="EdgeRenderingMode.Automatic"/>.</param>
        /// <returns>The value of edgeRenderingMode if it is not <see cref="EdgeRenderingMode.Automatic"/>; the <paramref name="defaultValue"/> otherwise.</returns>
        public static EdgeRenderingMode GetActual(this EdgeRenderingMode edgeRenderingMode, EdgeRenderingMode defaultValue)
        {
            return edgeRenderingMode == EdgeRenderingMode.Automatic ? defaultValue : edgeRenderingMode;
        }

        /// <summary>
        /// Draws a clipped polyline through the specified points.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points.</param>
        /// <param name="minDistSquared">The minimum line segment length (squared).</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch).</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        /// <param name="pointsRendered">The points rendered callback.</param>
        public static void DrawReducedLine(
            this IRenderContext rc,
            IList<ScreenPoint> points,
            double minDistSquared,
            OxyColor stroke,
            double strokeThickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin,
            List<ScreenPoint> outputBuffer = null,
            Action<IList<ScreenPoint>> pointsRendered = null)
        {
            var n = points.Count;
            if (n == 0)
            {
                return;
            }

            if (outputBuffer != null)
            {
                outputBuffer.Clear();
                outputBuffer.Capacity = n;
            }
            else
            {
                outputBuffer = new List<ScreenPoint>(n);
            }

            ReducePoints(points, minDistSquared, outputBuffer);
            rc.DrawLine(outputBuffer, stroke, strokeThickness, edgeRenderingMode, dashArray, lineJoin);

            if (outputBuffer != null)
            {
                outputBuffer.Clear();
                outputBuffer.AddRange(points);
            }

            pointsRendered?.Invoke(outputBuffer);
        }

        /// <summary>
        /// Draws the polygon within the specified clipping rectangle.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points.</param>
        /// <param name="minDistSquared">The squared minimum distance between points.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="lineJoin">The line join.</param>
        public static void DrawReducedPolygon(
            this IRenderContext rc,
            IList<ScreenPoint> points,
            double minDistSquared,
            OxyColor fill,
            OxyColor stroke,
            double strokeThickness,
            EdgeRenderingMode edgeRenderingMode,
            LineStyle lineStyle = LineStyle.Solid,
            LineJoin lineJoin = LineJoin.Miter)
        {
            var n = points.Count;
            if (n == 0)
            {
                return;
            }

            if (lineStyle == LineStyle.None)
            {
                return;
            }

            var outputBuffer = new List<ScreenPoint>();
            ReducePoints(points, minDistSquared, outputBuffer);

            rc.DrawPolygon(outputBuffer, fill, stroke, strokeThickness, edgeRenderingMode, lineStyle.GetDashArray(), lineJoin);
        }

        /// <summary>
        /// Draws the specified image.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="image">The image.</param>
        /// <param name="x">The destination X position.</param>
        /// <param name="y">The destination Y position.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">Interpolate the image if set to <c>true</c>.</param>
        public static void DrawImage(
            this IRenderContext rc,
            OxyImage image,
            double x,
            double y,
            double w,
            double h,
            double opacity,
            bool interpolate)
        {
            rc.DrawImage(image, 0, 0, image.Width, image.Height, x, y, w, h, opacity, interpolate);
        }

        /// <summary>
        /// Draws multi-line text at the specified point.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="point">The point.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The text color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="dy">The line spacing.</param>
        public static void DrawMultilineText(this IRenderContext rc, ScreenPoint point, string text, OxyColor color, string fontFamily = null, double fontSize = 10, double fontWeight = FontWeights.Normal, double dy = 12)
        {
            var lines = StringHelper.SplitLines(text);
            for (int i = 0; i < lines.Length; i++)
            {
                rc.DrawText(
                    new ScreenPoint(point.X, point.Y + (i * dy)),
                    lines[i],
                    color,
                    fontWeight: fontWeight,
                    fontSize: fontSize);
            }
        }

        /// <summary>
        /// Draws a line specified by coordinates.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawLine(
            this IRenderContext rc, double x0, double y0, double x1, double y1, OxyPen pen, EdgeRenderingMode edgeRenderingMode)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLine(
                new[] { new ScreenPoint(x0, y0), new ScreenPoint(x1, y1) },
                pen.Color,
                pen.Thickness,
                edgeRenderingMode,
                pen.ActualDashArray,
                pen.LineJoin);
        }

        /// <summary>
        /// Draws the line segments.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="points">The points.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawLineSegments(
            this IRenderContext rc, IList<ScreenPoint> points, OxyPen pen, EdgeRenderingMode edgeRenderingMode)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLineSegments(points, pen.Color, pen.Thickness, edgeRenderingMode, pen.ActualDashArray, pen.LineJoin);
        }

        /// <summary>
        /// Renders the marker.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="p">The center point of the marker.</param>
        /// <param name="type">The marker type.</param>
        /// <param name="outline">The outline.</param>
        /// <param name="size">The size of the marker.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawMarker(
            this IRenderContext rc,
            ScreenPoint p,
            MarkerType type,
            IList<ScreenPoint> outline,
            double size,
            OxyColor fill,
            OxyColor stroke,
            double strokeThickness,
            EdgeRenderingMode edgeRenderingMode)
        {
            rc.DrawMarkers(new[] { p }, type, outline, new[] { size }, fill, stroke, strokeThickness, edgeRenderingMode);
        }

        /// <summary>
        /// Draws a list of markers.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="markerPoints">The marker points.</param>
        /// <param name="markerType">Type of the marker.</param>
        /// <param name="markerOutline">The marker outline.</param>
        /// <param name="markerSize">Size of the marker.</param>
        /// <param name="markerFill">The marker fill.</param>
        /// <param name="markerStroke">The marker stroke.</param>
        /// <param name="markerStrokeThickness">The marker stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="resolution">The resolution.</param>
        /// <param name="binOffset">The bin Offset.</param>
        public static void DrawMarkers(
            this IRenderContext rc,
            IList<ScreenPoint> markerPoints,
            MarkerType markerType,
            IList<ScreenPoint> markerOutline,
            double markerSize,
            OxyColor markerFill,
            OxyColor markerStroke,
            double markerStrokeThickness,
            EdgeRenderingMode edgeRenderingMode,
            int resolution = 0,
            ScreenPoint binOffset = new ScreenPoint())
        {
            DrawMarkers(
                rc,
                markerPoints,
                markerType,
                markerOutline,
                new[] { markerSize },
                markerFill,
                markerStroke,
                markerStrokeThickness,
                edgeRenderingMode,
                resolution,
                binOffset);
        }

        /// <summary>
        /// Draws a list of markers.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="markerPoints">The marker points.</param>
        /// <param name="markerType">Type of the marker.</param>
        /// <param name="markerOutline">The marker outline.</param>
        /// <param name="markerSize">Size of the markers.</param>
        /// <param name="markerFill">The marker fill.</param>
        /// <param name="markerStroke">The marker stroke.</param>
        /// <param name="markerStrokeThickness">The marker stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="resolution">The resolution.</param>
        /// <param name="binOffset">The bin Offset.</param>
        public static void DrawMarkers(
            this IRenderContext rc,
            IList<ScreenPoint> markerPoints,
            MarkerType markerType,
            IList<ScreenPoint> markerOutline,
            IList<double> markerSize,
            OxyColor markerFill,
            OxyColor markerStroke,
            double markerStrokeThickness,
            EdgeRenderingMode edgeRenderingMode,
            int resolution = 0,
            ScreenPoint binOffset = new ScreenPoint())
        {
            if (markerType == MarkerType.None)
            {
                return;
            }

            var n = markerPoints.Count;
            var ellipses = new List<OxyRect>(n);
            var rects = new List<OxyRect>(n);
            var polygons = new List<IList<ScreenPoint>>(n);
            var lines = new List<ScreenPoint>(n);

            var hashset = new Dictionary<uint, bool>();

            var i = 0;

            foreach (var p in markerPoints)
            {
                if (resolution > 1)
                {
                    var x = (int)((p.X - binOffset.X) / resolution);
                    var y = (int)((p.Y - binOffset.Y) / resolution);
                    uint hash = (uint)(x << 16) + (uint)y;
                    if (hashset.ContainsKey(hash))
                    {
                        i++;
                        continue;
                    }

                    hashset.Add(hash, true);
                }

                var j = i < markerSize.Count ? i : 0;
                AddMarkerGeometry(p, markerType, markerOutline, markerSize[j], ellipses, rects, polygons, lines);

                i++;
            }

            if (edgeRenderingMode == EdgeRenderingMode.Automatic)
            {
                edgeRenderingMode = EdgeRenderingMode.PreferGeometricAccuracy;
            }

            if (ellipses.Count > 0)
            {
                rc.DrawEllipses(ellipses, markerFill, markerStroke, markerStrokeThickness, edgeRenderingMode);
            }

            if (rects.Count > 0)
            {
                rc.DrawRectangles(rects, markerFill, markerStroke, markerStrokeThickness, edgeRenderingMode);
            }

            if (polygons.Count > 0)
            {
                rc.DrawPolygons(polygons, markerFill, markerStroke, markerStrokeThickness, edgeRenderingMode);
            }

            if (lines.Count > 0)
            {
                rc.DrawLineSegments(lines, markerStroke, markerStrokeThickness, edgeRenderingMode);
            }
        }

        /// <summary>
        /// Draws a circle at the specified position.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="x">The center x-coordinate.</param>
        /// <param name="y">The center y-coordinate.</param>
        /// <param name="r">The radius.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawCircle(this IRenderContext rc, double x, double y, double r, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            rc.DrawEllipse(new OxyRect(x - r, y - r, r * 2, r * 2), fill, stroke, thickness, edgeRenderingMode);
        }

        /// <summary>
        /// Draws a circle at the specified position.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="center">The center.</param>
        /// <param name="r">The radius.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawCircle(this IRenderContext rc, ScreenPoint center, double r, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            DrawCircle(rc, center.X, center.Y, r, fill, stroke, thickness, edgeRenderingMode);
        }

        /// <summary>
        /// Fills a circle at the specified position.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="center">The center.</param>
        /// <param name="r">The radius.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void FillCircle(this IRenderContext rc, ScreenPoint center, double r, OxyColor fill, EdgeRenderingMode edgeRenderingMode)
        {
            DrawCircle(rc, center.X, center.Y, r, fill, OxyColors.Undefined, 0d, edgeRenderingMode);
        }

        /// <summary>
        /// Fills a rectangle at the specified position.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void FillRectangle(this IRenderContext rc, OxyRect rectangle, OxyColor fill, EdgeRenderingMode edgeRenderingMode)
        {
            rc.DrawRectangle(rectangle, fill, OxyColors.Undefined, 0d, edgeRenderingMode);
        }

        /// <summary>
        /// Draws the outline of a rectangle with individual stroke thickness for each side.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="rect">The rectangle.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public static void DrawRectangle(this IRenderContext rc, OxyRect rect, OxyColor stroke, OxyThickness thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (thickness.Left.Equals(thickness.Right) && thickness.Left.Equals(thickness.Top) && thickness.Left.Equals(thickness.Bottom))
            {
                rc.DrawRectangle(rect, OxyColors.Undefined, stroke, thickness.Left, edgeRenderingMode);
                return;
            }

            var adjustedLeft = rect.Left - thickness.Left / 2 + 0.5;
            var adjustedRight = rect.Right + thickness.Right / 2 - 0.5;
            var adjustedTop = rect.Top - thickness.Top / 2 + 0.5;
            var adjustedBottom = rect.Bottom + thickness.Bottom / 2 - 0.5;

            var pointsTop = new[] { new ScreenPoint(adjustedLeft, rect.Top), new ScreenPoint(adjustedRight, rect.Top) };
            var pointsRight = new[] { new ScreenPoint(rect.Right, adjustedTop), new ScreenPoint(rect.Right, adjustedBottom) };
            var pointsBottom = new[] { new ScreenPoint(adjustedLeft, rect.Bottom), new ScreenPoint(adjustedRight, rect.Bottom) };
            var pointsLeft = new[] { new ScreenPoint(rect.Left, adjustedTop), new ScreenPoint(rect.Left, adjustedBottom) };

            rc.DrawLine(pointsTop, stroke, thickness.Top, edgeRenderingMode, null, LineJoin.Miter);
            rc.DrawLine(pointsRight, stroke, thickness.Right, edgeRenderingMode, null, LineJoin.Miter);
            rc.DrawLine(pointsBottom, stroke, thickness.Bottom, edgeRenderingMode, null, LineJoin.Miter);
            rc.DrawLine(pointsLeft, stroke, thickness.Left, edgeRenderingMode, null, LineJoin.Miter);
        }

        /// <summary>
        /// Measures the size of the specified text.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="angle">The angle of measured text (degrees).</param>
        /// <returns>The size of the text (in device independent units, 1/96 inch).</returns>
        public static OxySize MeasureText(this IRenderContext rc, string text, string fontFamily, double fontSize, double fontWeight, double angle)
        {
            var bounds = rc.MeasureText(text, fontFamily, fontSize, fontWeight);
            return MeasureRotatedRectangleBound(bounds, angle);
        }

        /// <summary>
        /// Applies the specified clipping rectangle the the render context and returns a reset token. The clipping is reset once this token is disposed.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRectangle">The clipping rectangle.</param>
        /// <returns>The reset token. Clipping is reset once this is disposed.</returns>
        public static IDisposable AutoResetClip(this IRenderContext rc, OxyRect clippingRectangle)
        {
            return new AutoResetClipToken(rc, clippingRectangle);
        }

        /// <summary>
        /// Adds a marker geometry to the specified collections.
        /// </summary>
        /// <param name="p">The position of the marker.</param>
        /// <param name="type">The marker type.</param>
        /// <param name="outline">The custom outline, if <paramref name="type" /> is <see cref="MarkerType.Custom" />.</param>
        /// <param name="size">The size of the marker.</param>
        /// <param name="ellipses">The output ellipse collection.</param>
        /// <param name="rects">The output rectangle collection.</param>
        /// <param name="polygons">The output polygon collection.</param>
        /// <param name="lines">The output line collection.</param>
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
                                    new ScreenPoint(p.x, p.y - (M2 * size)), new ScreenPoint(p.x + (M2 * size), p.y),
                                    new ScreenPoint(p.x, p.y + (M2 * size)), new ScreenPoint(p.x - (M2 * size), p.y)
                                });
                        break;
                    }

                case MarkerType.Triangle:
                    {
                        polygons.Add(
                            new[]
                                {
                                    new ScreenPoint(p.x - size, p.y + (M1 * size)),
                                    new ScreenPoint(p.x + size, p.y + (M1 * size)), new ScreenPoint(p.x, p.y - (M2 * size))
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
                        lines.Add(new ScreenPoint(p.x - (size * M3), p.y - (size * M3)));
                        lines.Add(new ScreenPoint(p.x + (size * M3), p.y + (size * M3)));
                        lines.Add(new ScreenPoint(p.x - (size * M3), p.y + (size * M3)));
                        lines.Add(new ScreenPoint(p.x + (size * M3), p.y - (size * M3)));
                        break;
                    }
            }
        }

        /// <summary>
        /// Calculates the bounds with respect to rotation angle and horizontal/vertical alignment.
        /// </summary>
        /// <param name="bounds">The size of the object to calculate bounds for.</param>
        /// <param name="angle">The rotation angle (degrees).</param>
        /// <returns>A minimum bounding rectangle.</returns>
        private static OxySize MeasureRotatedRectangleBound(OxySize bounds, double angle)
        {
            var oxyRect = bounds.GetBounds(angle, HorizontalAlignment.Center, VerticalAlignment.Middle);
            return new OxySize(oxyRect.Width, oxyRect.Height);
        }

        /// <summary>
        /// Reduces the specified list of points by the specified minimum squared distance. 
        /// </summary>
        /// <param name="points">The points that should be evaluated.</param>
        /// <param name="minDistSquared">The minimum line segment length (squared).</param>
        /// <param name="outputBuffer">The output buffer. Cannot be <c>null</c>.</param>
        /// <remarks>Points that are closer than the specified distance will not be included in the output buffer.</remarks>
        private static void ReducePoints(IList<ScreenPoint> points, double minDistSquared, List<ScreenPoint> outputBuffer)
        {
            var n = points.Count;
            if (n == 0)
            {
                return;
            }

            outputBuffer.Add(points[0]);
            int lastPointIndex = 0;
            for (int i = 1; i < n; i++)
            {
                var sc1 = points[i];

                // length calculation (inlined for performance)
                var dx = sc1.X - points[lastPointIndex].X;
                var dy = sc1.Y - points[lastPointIndex].Y;

                if ((dx * dx) + (dy * dy) > minDistSquared || i == n - 1)
                {
                    outputBuffer.Add(new ScreenPoint(sc1.X, sc1.Y));
                    lastPointIndex = i;
                }
            }
        }

        /// <summary>
        /// Represents the token that is used to automatically reset the clipping in the <see cref="AutoResetClip(IRenderContext, OxyRect)"/> method.
        /// </summary>
        private class AutoResetClipToken : IDisposable
        {
            private readonly IRenderContext renderContext;

            public AutoResetClipToken(IRenderContext renderContext, OxyRect clippingRectangle)
            {
                this.renderContext = renderContext;
                renderContext.PushClip(clippingRectangle);
            }

            void IDisposable.Dispose()
            {
                this.renderContext.PopClip();
            }
        }
    }
}
