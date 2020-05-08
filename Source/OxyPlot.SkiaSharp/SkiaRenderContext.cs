// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkiaRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using global::SkiaSharp;
    using global::SkiaSharp.HarfBuzz;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements <see cref="IRenderContext" /> based on SkiaSharp.
    /// </summary>
    public class SkiaRenderContext : IRenderContext, IDisposable
    {
        private readonly Dictionary<FontDescriptor, SKShaper> shaperCache = new Dictionary<FontDescriptor, SKShaper>();
        private readonly Dictionary<FontDescriptor, SKTypeface> typefaceCache = new Dictionary<FontDescriptor, SKTypeface>();
        private SKPaint paint = new SKPaint();
        private SKPath path = new SKPath();

        /// <summary>
        /// Gets or sets the DPI scaling factor. A value of 1 corresponds to 96 DPI (dots per inch).
        /// </summary>
        public float DpiScale { get; set; } = 1;

        /// <inheritdoc />
        public bool RendersToScreen => this.RenderTarget == RenderTarget.Screen;

        /// <summary>
        /// Gets or sets the render target.
        /// </summary>
        public RenderTarget RenderTarget { get; set; } = RenderTarget.Screen;

        /// <summary>
        /// Gets or sets the <see cref="SKCanvas"/> the <see cref="SkiaRenderContext"/> renders to. This must be set before any draw calls.
        /// </summary>
        public SKCanvas SkCanvas { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text shaping should be used when rendering text.
        /// </summary>
        public bool UseTextShaping { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether the context renders to pixels.
        /// </summary>
        /// <value><c>true</c> if the context renders to pixels; otherwise, <c>false</c>.</value>
        private bool RendersToPixels => this.RenderTarget != RenderTarget.VectorGraphic;

        /// <inheritdoc/>
        public void CleanUp()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public void DrawEllipse(OxyRect extents, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0))
            {
                return;
            }

            var actualRect = this.Convert(extents);

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawOval(actualRect, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetStrokePaint(stroke, thickness, edgeRenderingMode);
                this.SkCanvas.DrawOval(actualRect, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawEllipses(IList<OxyRect> extents, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (!fill.IsVisible() && (!stroke.IsVisible() || thickness <= 0))
            {
                return;
            }

            var path = this.GetPath();
            foreach (var extent in extents)
            {
                path.AddOval(this.Convert(extent));
            }

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetStrokePaint(stroke, thickness, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            if (source == null)
            {
                return;
            }

            var bytes = source.GetData();
            var image = SKBitmap.Decode(bytes);

            var src = new SKRect((float)srcX, (float)srcY, (float)(srcX + srcWidth), (float)(srcY + srcHeight));
            var dest = new SKRect(this.Convert(destX), this.Convert(destY), this.Convert(destX + destWidth), this.Convert(destY + destHeight));

            var paint = this.GetImagePaint(opacity, interpolate);
            this.SkCanvas.DrawBitmap(image, src, dest, paint);
        }

        /// <inheritdoc/>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray = null,
            LineJoin lineJoin = LineJoin.Miter)
        {
            if (points.Count < 2 || !stroke.IsVisible() || thickness <= 0)
            {
                return;
            }

            var path = this.GetPath();
            var paint = this.GetLinePaint(stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
            var actualPoints = this.GetActualPoints(points, thickness, edgeRenderingMode);
            AddPoints(actualPoints, path);

            this.SkCanvas.DrawPath(path, paint);
        }

        /// <inheritdoc/>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray = null,
            LineJoin lineJoin = LineJoin.Miter)
        {
            if (points.Count < 2 || !stroke.IsVisible() || thickness <= 0)
            {
                return;
            }

            var paint = this.GetLinePaint(stroke, thickness, edgeRenderingMode, dashArray, lineJoin);

            var skPoints = new SKPoint[points.Count];
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Automatic when this.RendersToPixels:
                case EdgeRenderingMode.Adaptive when this.RendersToPixels:
                case EdgeRenderingMode.PreferSharpness when this.RendersToPixels:
                    var snapOffset = this.GetSnapOffset(thickness, edgeRenderingMode);
                    for (var i = 0; i < points.Count - 1; i += 2)
                    {
                        var p1 = points[i];
                        var p2 = points[i + 1];
                        if (RenderContextBase.IsStraightLine(p1, p2))
                        {
                            skPoints[i] = this.ConvertSnap(p1, snapOffset);
                            skPoints[i + 1] = this.ConvertSnap(p2, snapOffset);
                        }
                        else
                        {
                            skPoints[i] = this.Convert(p1);
                            skPoints[i + 1] = this.Convert(p2);
                        }
                    }

                    break;
                default:
                    for (var i = 0; i < points.Count; i += 2)
                    {
                        skPoints[i] = this.Convert(points[i]);
                        skPoints[i + 1] = this.Convert(points[i + 1]);
                    }

                    break;
            }

            this.SkCanvas.DrawPoints(SKPointMode.Lines, skPoints, paint);
        }

        /// <inheritdoc/>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray = null,
            LineJoin lineJoin = LineJoin.Miter)
        {
            if (!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0) || points.Count < 2)
            {
                return;
            }

            var path = this.GetPath();
            var actualPoints = this.GetActualPoints(points, thickness, edgeRenderingMode);
            AddPoints(actualPoints, path);
            path.Close();

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetLinePaint(stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
                this.SkCanvas.DrawPath(path, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray = null,
            LineJoin lineJoin = LineJoin.Miter)
        {
            if (!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0) || polygons.Count == 0)
            {
                return;
            }

            var path = this.GetPath();
            foreach (var polygon in polygons)
            {
                if (polygon.Count < 2)
                {
                    continue;
                }

                var actualPoints = this.GetActualPoints(polygon, thickness, edgeRenderingMode);
                AddPoints(actualPoints, path);
                path.Close();
            }

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetLinePaint(stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
                this.SkCanvas.DrawPath(path, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawRectangle(OxyRect rectangle, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0))
            {
                return;
            }

            var actualRectangle = this.GetActualRect(rectangle, thickness, edgeRenderingMode);

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawRect(actualRectangle, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetStrokePaint(stroke, thickness, edgeRenderingMode);
                this.SkCanvas.DrawRect(actualRectangle, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0) || rectangles.Count == 0)
            {
                return;
            }

            var path = this.GetPath();
            foreach (var rectangle in this.GetActualRects(rectangles, thickness, edgeRenderingMode))
            {
                path.AddRect(rectangle);
            }

            if (fill.IsVisible())
            {
                var paint = this.GetFillPaint(fill, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var paint = this.GetStrokePaint(stroke, thickness, edgeRenderingMode);
                this.SkCanvas.DrawPath(path, paint);
            }
        }

        /// <inheritdoc/>
        public void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily = null,
            double fontSize = 10,
            double fontWeight = 400,
            double rotation = 0,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment verticalAlignment = VerticalAlignment.Top,
            OxySize? maxSize = null)
        {
            if (text == null || !fill.IsVisible())
            {
                return;
            }

            var paint = this.GetTextPaint(fontFamily, fontSize, fontWeight, out var shaper);
            paint.Color = fill.ToSKColor();

            var x = this.Convert(p.X);
            var y = this.Convert(p.Y);

            var lines = StringHelper.SplitLines(text);
            var lineHeight = paint.GetFontMetrics(out var metrics);

            var deltaY = verticalAlignment switch
            {
                VerticalAlignment.Top => -metrics.Ascent,
                VerticalAlignment.Middle => -(metrics.Ascent + metrics.Descent + lineHeight * (lines.Length - 1)) / 2,
                VerticalAlignment.Bottom => -metrics.Descent - lineHeight * (lines.Length - 1),
                _ => throw new ArgumentOutOfRangeException(nameof(verticalAlignment))
            };

            using var _ = new SKAutoCanvasRestore(this.SkCanvas);
            this.SkCanvas.Translate(x, y);
            this.SkCanvas.RotateDegrees((float)rotation);

            foreach (var line in lines)
            {
                if (this.UseTextShaping)
                {
                    var width = this.MeasureText(line, shaper, paint);
                    var deltaX = horizontalAlignment switch
                    {
                        HorizontalAlignment.Left => 0,
                        HorizontalAlignment.Center => -width / 2,
                        HorizontalAlignment.Right => -width,
                        _ => throw new ArgumentOutOfRangeException(nameof(horizontalAlignment))
                    };

                    this.paint.TextAlign = SKTextAlign.Left;
                    this.SkCanvas.DrawShapedText(shaper, line, deltaX, deltaY, paint);
                }
                else
                {
                    paint.TextAlign = horizontalAlignment switch
                    {
                        HorizontalAlignment.Left => SKTextAlign.Left,
                        HorizontalAlignment.Center => SKTextAlign.Center,
                        HorizontalAlignment.Right => SKTextAlign.Right,
                        _ => throw new ArgumentOutOfRangeException(nameof(horizontalAlignment))
                    };

                    this.SkCanvas.DrawText(line, 0, deltaY, paint);
                }

                deltaY += lineHeight;
            }
        }

        /// <inheritdoc/>
        public OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
        {
            if (text == null)
            {
                return new OxySize(0, 0);
            }

            var lines = StringHelper.SplitLines(text);
            var paint = this.GetTextPaint(fontFamily, fontSize, fontWeight, out var shaper);
            var height = paint.GetFontMetrics(out _) * lines.Length;
            var width = lines.Max(line => this.MeasureText(line, shaper, paint)); 

            return new OxySize(this.ConvertBack(width), this.ConvertBack(height));
        }

        /// <inheritdoc/>
        public void ResetClip()
        {
            this.SkCanvas.Restore();
        }

        /// <inheritdoc/>
        public bool SetClip(OxyRect clippingRectangle)
        {
            // if a clipping is already set, we have to restore it first
            if (this.SkCanvas.SaveCount > 0)
            {
                this.SkCanvas.Restore();
            }

            this.SkCanvas.Save();
            this.SkCanvas.ClipRect(this.Convert(clippingRectangle));
            return true;
        }

        /// <inheritdoc/>
        public void SetToolTip(string text)
        {
        }

        /// <summary>
        /// Disposes managed resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method is called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            this.paint?.Dispose();
            this.paint = null;
            this.path?.Dispose();
            this.path = null;

            foreach (var typeface in this.typefaceCache.Values)
            {
                typeface.Dispose();
            }

            this.typefaceCache.Clear();

            foreach (var shaper in this.shaperCache.Values)
            {
                shaper.Dispose();
            }

            this.shaperCache.Clear();
        }

        /// <summary>
        /// Adds the <see cref="SKPoint"/>s to the <see cref="SKPath"/> as a series of connected lines.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="path">The path.</param>
        private static void AddPoints(IEnumerable<SKPoint> points, SKPath path)
        {
            using var e = points.GetEnumerator();
            if (!e.MoveNext())
            {
                return;
            }

            path.MoveTo(e.Current);
            while (e.MoveNext())
            {
                path.LineTo(e.Current);
            }
        }

        /// <summary>
        /// Gets the pixel offset that a line with the specified thickness should snap to.
        /// </summary>
        /// <remarks>
        /// This takes into account that lines with even stroke thickness should be snapped to the border between two pixels while lines with odd stroke thickness should be snapped to the middle of a pixel.
        /// </remarks>
        /// <param name="thickness">The line thickness.</param>
        /// <returns>The snap offset.</returns>
        private static float GetSnapOffset(float thickness)
        {
            var mod = thickness % 2;
            var isOdd = mod >= 0.5 && mod < 1.5;
            return isOdd ? 0.5f : 0;
        }

        /// <summary>
        /// Snaps a value to a pixel with the specified offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The snapped value.</returns>
        private static float Snap(float value, float offset)
        {
            return (float)Math.Round(value + offset, MidpointRounding.AwayFromZero) - offset;
        }

        /// <summary>
        /// Converts a <see cref="OxyRect"/> to a <see cref="SKRect"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The converted rectangle.</returns>
        private SKRect Convert(OxyRect rect)
        {
            var left = this.Convert(rect.Left);
            var right = this.Convert(rect.Right);
            var top = this.Convert(rect.Top);
            var bottom = this.Convert(rect.Bottom);
            return new SKRect(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="double"/> to a <see cref="float"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private float Convert(double value)
        {
            return (float)value * this.DpiScale;
        }

        /// <summary>
        /// Converts <see cref="ScreenPoint"/> to a <see cref="SKPoint"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The converted point.</returns>
        private SKPoint Convert(ScreenPoint point)
        {
            return new SKPoint(this.Convert(point.X), this.Convert(point.Y));
        }

        /// <summary>
        /// Converts a <see cref="float"/> to a <see cref="double"/>, applying reversed DPI scaling.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private double ConvertBack(float value)
        {
            return value / this.DpiScale;
        }

        /// <summary>
        /// Converts <see cref="double"/> dash array to a <see cref="float"/> array, taking into account DPI scaling.
        /// </summary>
        /// <param name="values">The array of values.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <returns>The array of converted values.</returns>
        private float[] ConvertDashArray(double[] values, float strokeThickness)
        {
            var ret = new float[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                ret[i] = this.Convert(values[i]) * strokeThickness;
            }

            return ret;
        }

        /// <summary>
        /// Converts a <see cref="OxyRect"/> to a <see cref="SKRect"/>, taking into account DPI scaling and snapping the corners to pixels.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted rectangle.</returns>
        private SKRect ConvertSnap(OxyRect rect, float snapOffset)
        {
            var left = this.ConvertSnap(rect.Left, snapOffset);
            var right = this.ConvertSnap(rect.Right, snapOffset);
            var top = this.ConvertSnap(rect.Top, snapOffset);
            var bottom = this.ConvertSnap(rect.Bottom, snapOffset);
            return new SKRect(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="double"/> to a <see cref="float"/>, taking into account DPI scaling and snapping the value to a pixel.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted value.</returns>
        private float ConvertSnap(double value, float snapOffset)
        {
            return Snap(this.Convert(value), snapOffset);
        }

        /// <summary>
        /// Converts <see cref="ScreenPoint"/> to a <see cref="SKPoint"/>, taking into account DPI scaling and snapping the point to a pixel.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted point.</returns>
        private SKPoint ConvertSnap(ScreenPoint point, float snapOffset)
        {
            return new SKPoint(this.ConvertSnap(point.X, snapOffset), this.ConvertSnap(point.Y, snapOffset));
        }

        /// <summary>
        /// Gets the <see cref="SKPoint"/>s that should actually be rendered from the list of <see cref="ScreenPoint"/>s, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="screenPoints">The points.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual points.</returns>
        private IEnumerable<SKPoint> GetActualPoints(IList<ScreenPoint> screenPoints, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Automatic when this.RendersToPixels && RenderContextBase.IsStraightLine(screenPoints):
                case EdgeRenderingMode.Adaptive when this.RendersToPixels && RenderContextBase.IsStraightLine(screenPoints):
                case EdgeRenderingMode.PreferSharpness when this.RendersToPixels:
                    var snapOffset = this.GetSnapOffset(strokeThickness, edgeRenderingMode);
                    return screenPoints.Select(p => this.ConvertSnap(p, snapOffset));
                default:
                    return screenPoints.Select(this.Convert);
            }
        }

        /// <summary>
        /// Gets the <see cref="SKRect"/> that should actually be rendered from the <see cref="OxyRect"/>, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual rectangle.</returns>
        private SKRect GetActualRect(OxyRect rect, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Adaptive when this.RendersToPixels:
                case EdgeRenderingMode.Automatic when this.RendersToPixels:
                case EdgeRenderingMode.PreferSharpness when this.RendersToPixels:
                    var actualThickness = this.GetActualThickness(strokeThickness, edgeRenderingMode);
                    var snapOffset = GetSnapOffset(actualThickness);
                    return this.ConvertSnap(rect, snapOffset);
                default:
                    return this.Convert(rect);
            }
        }

        /// <summary>
        /// Gets the <see cref="SKRect"/>s that should actually be rendered from the list of <see cref="OxyRect"/>s, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual rectangles.</returns>
        private IEnumerable<SKRect> GetActualRects(IEnumerable<OxyRect> rects, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Adaptive when this.RendersToPixels:
                case EdgeRenderingMode.Automatic when this.RendersToPixels:
                case EdgeRenderingMode.PreferSharpness when this.RendersToPixels:
                    var actualThickness = this.GetActualThickness(strokeThickness, edgeRenderingMode);
                    var snapOffset = GetSnapOffset(actualThickness);
                    return rects.Select(rect => this.ConvertSnap(rect, snapOffset));
                default:
                    return rects.Select(this.Convert);
            }
        }

        /// <summary>
        /// Gets the stroke thickness that should actually be used for rendering, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual stroke thickness.</returns>
        private float GetActualThickness(double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            var scaledThickness = this.Convert(strokeThickness);
            if (edgeRenderingMode == EdgeRenderingMode.PreferSharpness && this.RendersToPixels)
            {
                scaledThickness = Snap(scaledThickness, 0);
            }

            return scaledThickness;
        }

        /// <summary>
        /// Gets a <see cref="SKPaint"/> containing information needed to render the fill of a shape.
        /// </summary>
        /// <remarks>
        /// This modifies and returns the local <see cref="paint"/> instance.
        /// </remarks>
        /// <param name="fillColor">The fill color.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The paint.</returns>
        private SKPaint GetFillPaint(OxyColor fillColor, EdgeRenderingMode edgeRenderingMode)
        {
            this.paint.Color = fillColor.ToSKColor();
            this.paint.Style = SKPaintStyle.Fill;
            this.paint.IsAntialias = this.ShouldUseAntiAliasing(edgeRenderingMode);
            this.paint.PathEffect = null;
            return this.paint;
        }

        /// <summary>
        /// Gets a <see cref="SKPaint"/> containing information needed to render an image.
        /// </summary>
        /// <remarks>
        /// This modifies and returns the local <see cref="paint"/> instance.
        /// </remarks>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">A value indicating whether interpolation should be used.</param>
        /// <returns>The paint.</returns>
        private SKPaint GetImagePaint(double opacity, bool interpolate)
        {
            this.paint.Color = new SKColor(0, 0, 0, (byte)(255 * opacity));
            this.paint.FilterQuality = interpolate ? SKFilterQuality.High : SKFilterQuality.None;
            this.paint.IsAntialias = true;
            return this.paint;
        }

        /// <summary>
        /// Gets a <see cref="SKPaint"/> containing information needed to render a line.
        /// </summary>
        /// <remarks>
        /// This modifies and returns the local <see cref="paint"/> instance.
        /// </remarks>
        /// <param name="strokeColor">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>The paint.</returns>
        private SKPaint GetLinePaint(OxyColor strokeColor, double strokeThickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            var paint = this.GetStrokePaint(strokeColor, strokeThickness, edgeRenderingMode);

            if (dashArray != null)
            {
                var actualDashArray = this.ConvertDashArray(dashArray, paint.StrokeWidth);
                paint.PathEffect = SKPathEffect.CreateDash(actualDashArray, 0);
            }

            paint.StrokeJoin = lineJoin switch
            {
                LineJoin.Miter => SKStrokeJoin.Miter,
                LineJoin.Round => SKStrokeJoin.Round,
                LineJoin.Bevel => SKStrokeJoin.Bevel,
                _ => throw new ArgumentOutOfRangeException(nameof(lineJoin))
            };

            return paint;
        }

        /// <summary>
        /// Gets an empty <see cref="SKPath"/>.
        /// </summary>
        /// <remarks>
        /// This clears and returns the local <see cref="path"/> instance.
        /// </remarks>
        /// <returns>The path.</returns>
        private SKPath GetPath()
        {
            this.path.Reset();
            return this.path;
        }

        /// <summary>
        /// Gets the snapping offset for the specified stroke thickness.
        /// </summary>
        /// <remarks>
        /// This takes into account that lines with even stroke thickness should be snapped to the border between two pixels while lines with odd stroke thickness should be snapped to the middle of a pixel.
        /// </remarks>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The snap offset.</returns>
        private float GetSnapOffset(double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var actualThickness = this.GetActualThickness(thickness, edgeRenderingMode);
            return GetSnapOffset(actualThickness);
        }

        /// <summary>
        /// Gets a <see cref="SKPaint"/> containing information needed to render a stroke.
        /// </summary>
        /// <remarks>
        /// This modifies and returns the local <see cref="paint"/> instance.
        /// </remarks>
        /// <param name="strokeColor">The stroke color.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The paint.</returns>
        private SKPaint GetStrokePaint(OxyColor strokeColor, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.paint.Color = strokeColor.ToSKColor();
            this.paint.Style = SKPaintStyle.Stroke;
            this.paint.IsAntialias = this.ShouldUseAntiAliasing(edgeRenderingMode);
            this.paint.StrokeWidth = this.GetActualThickness(strokeThickness, edgeRenderingMode);
            this.paint.PathEffect = null;
            this.paint.StrokeJoin = SKStrokeJoin.Miter;
            return this.paint;
        }

        /// <summary>
        /// Gets a <see cref="SKPaint"/> containing information needed to render text.
        /// </summary>
        /// <remarks>
        /// This modifies and returns the local <see cref="paint"/> instance.
        /// </remarks>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="shaper">The font shaper.</param>
        /// <returns>The paint.</returns>
        private SKPaint GetTextPaint(string fontFamily, double fontSize, double fontWeight, out SKShaper shaper)
        {
            var fontDescriptor = new FontDescriptor(fontFamily, fontWeight);
            if (!this.typefaceCache.TryGetValue(fontDescriptor, out var typeface))
            {
                typeface = SKTypeface.FromFamilyName(fontFamily, new SKFontStyle((int)fontWeight, (int)SKFontStyleWidth.Normal, SKFontStyleSlant.Upright));
                this.typefaceCache.Add(fontDescriptor, typeface);
            }

            if (this.UseTextShaping)
            {
                if (!this.shaperCache.TryGetValue(fontDescriptor, out shaper))
                {
                    shaper = new SKShaper(typeface);
                    this.shaperCache.Add(fontDescriptor, shaper);
                }
            }
            else
            {
                shaper = null;
            }

            this.paint.Typeface = typeface;
            this.paint.TextSize = this.Convert(fontSize);
            this.paint.IsAntialias = true;
            this.paint.Style = SKPaintStyle.Fill;
            this.paint.HintingLevel = this.RendersToScreen ? SKPaintHinting.Full : SKPaintHinting.NoHinting;
            this.paint.SubpixelText = this.RendersToScreen;
            return this.paint;
        }

        /// <summary>
        /// Measures text using the specified <see cref="SKShaper"/> and <see cref="SKPaint"/>.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="shaper">The text shaper.</param>
        /// <param name="paint">The paint.</param>
        /// <returns>The width of the text when rendered using the specified shaper and paint.</returns>
        private float MeasureText(string text, SKShaper shaper, SKPaint paint)
        {
            if (!this.UseTextShaping)
            {
                return paint.MeasureText(text);
            }

            // we have to get a bit creative here as SKShaper does not offer a direct overload for this.
            // see also https://github.com/mono/SkiaSharp/blob/master/source/SkiaSharp.HarfBuzz/SkiaSharp.HarfBuzz.Shared/SKShaper.cs
            using var buffer = new HarfBuzzSharp.Buffer();
            switch (paint.TextEncoding)
            {
                case SKTextEncoding.Utf8:
                    buffer.AddUtf8(text);
                    break;
                case SKTextEncoding.Utf16:
                    buffer.AddUtf16(text);
                    break;
                case SKTextEncoding.Utf32:
                    buffer.AddUtf32(text);
                    break;
                default:
                    throw new NotSupportedException("TextEncoding is not supported.");
            }

            buffer.GuessSegmentProperties();
            shaper.Shape(buffer, paint);
            return buffer.GlyphPositions.Sum(gp => gp.XAdvance) * paint.TextSize / 512;
        }

        /// <summary>
        /// Gets a value indicating whether anti-aliasing should be used taking in account the specified edge rendering mode.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns><c>true</c> if anti-aliasing should be used; <c>false</c> otherwise.</returns>
        private bool ShouldUseAntiAliasing(EdgeRenderingMode edgeRenderingMode)
        {
            return edgeRenderingMode != EdgeRenderingMode.PreferSpeed;
        }

        /// <summary>
        /// Represents a font description.
        /// </summary>
        private struct FontDescriptor
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FontDescriptor"/> struct.
            /// </summary>
            /// <param name="fontFamily">The font family.</param>
            /// <param name="fontWeight">The font weight.</param>
            public FontDescriptor(string fontFamily, double fontWeight)
            {
                this.FontFamily = fontFamily;
                this.FontWeight = fontWeight;
            }

            /// <summary>
            /// The font family.
            /// </summary>
            public string FontFamily { get; }

            /// <summary>
            /// The font weight.
            /// </summary>
            public double FontWeight { get; }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is FontDescriptor other && this.FontFamily == other.FontFamily && this.FontWeight == other.FontWeight;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                var hashCode = -1030903623;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FontFamily);
                hashCode = hashCode * -1521134295 + this.FontWeight.GetHashCode();
                return hashCode;
            }
        }
    }
}
