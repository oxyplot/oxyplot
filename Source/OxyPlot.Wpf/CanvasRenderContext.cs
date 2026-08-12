// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements <see cref="IRenderContext" /> for <see cref="System.Windows.Controls.Canvas" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using Path = System.Windows.Shapes.Path;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

    /// <summary>
    /// Implements <see cref="IRenderContext" /> for <see cref="System.Windows.Controls.Canvas" />.
    /// </summary>
    public class CanvasRenderContext : WpfRenderContext
    {
        /// <summary>
        /// The canvas.
        /// </summary>
        private readonly Canvas _canvas;

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private Rect? _clip;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRenderContext" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public CanvasRenderContext(Canvas canvas)
        {
            this._canvas = canvas;
            this.TextFormattingMode = TextFormattingMode.Display;
            this.TextMeasurementMethod = TextMeasurementMethod.TextBlock;
            this.RendersToScreen = true;
        }

        ///<inheritdoc/>
        public override void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var isFilled = !fill.IsUndefined();
            var isStroke = !stroke.IsUndefined();
            var streamGeometry = new StreamGeometry { FillRule = FillRule.Nonzero };
            using (var sgc = streamGeometry.Open())
            {
                foreach (var rect in rectangles)
                {
                    var centerY = rect.Center.Y;
                    sgc.BeginFigure(new Point(rect.Right, centerY), isFilled, true);

                    var size = new Size(rect.Width / 2, rect.Height / 2);
                    sgc.ArcTo(new Point(rect.Left, centerY), size, 180, false, SweepDirection.Clockwise, isStroke, false);
                    sgc.ArcTo(new Point(rect.Right, centerY), size, 180, false, SweepDirection.Clockwise, isStroke, false);
                }
            }

            streamGeometry.Freeze();
            path.Data = streamGeometry;
        }

        ///<inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray);

            var actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);

            var actualPoints = this.GetActualPoints(points, actualStrokeThickness, edgeRenderingMode);
            var firstPoint = GetFirstAndRest(actualPoints, out var otherPoints);

            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(firstPoint, false, false);
                streamGeometryContext.PolyLineTo(otherPoints, !stroke.IsUndefined(), false);
            }

            streamGeometry.Freeze();
            path.Data = streamGeometry;
        }

        ///<inheritdoc/>
        public override void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);

            var actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);
            var actualPoints = this.GetActualPoints(points, actualStrokeThickness, edgeRenderingMode);
            var firstPoint = GetFirstAndRest(actualPoints, out var otherPoints);

            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(firstPoint, false, false);
                for (var i = 0; i < otherPoints.Count; i++)
                {
                    streamGeometryContext.LineTo(otherPoints[i], (i & 0x1) == 0, false);
                }
            }

            streamGeometry.Freeze();
            path.Data = streamGeometry;
        }

        ///<inheritdoc/>
        public override void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (polygons.Count == 0)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var streamGeometry = new StreamGeometry { FillRule = FillRule.Nonzero };
            using (var sgc = streamGeometry.Open())
            {
                foreach (var polygon in polygons)
                {
                    var points = this.GetActualPoints(polygon, path.StrokeThickness, edgeRenderingMode);
                    var firstPoint = GetFirstAndRest(points, out var otherPoints);

                    sgc.BeginFigure(firstPoint, !fill.IsUndefined(), true);
                    foreach (var point in otherPoints)
                    {
                        sgc.LineTo(point, !stroke.IsUndefined(), false);
                    }
                }
            }

            streamGeometry.Freeze();
            path.Data = streamGeometry;
        }

        ///<inheritdoc/>
        public override void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var streamGeometry = new StreamGeometry { FillRule = FillRule.Nonzero };
            using (var context = streamGeometry.Open())
            {
                foreach (var rect in rectangles)
                {
                    var r = this.GetActualRect(rect, thickness, edgeRenderingMode);
                    context.BeginFigure(r.TopLeft, !fill.IsUndefined(), true);
                    context.PolyLineTo(new[] { r.TopRight, r.BottomRight, r.BottomLeft }, !stroke.IsUndefined(), false);
                }
            }

            streamGeometry.Freeze();
            path.Data = streamGeometry;
        }

        ///<inheritdoc/>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize)
        {
            var tb = this.CreateAndAdd<TextBlock>();
            tb.Text = text;
            tb.Foreground = this.GetCachedBrush(fill);
            if (fontFamily != null)
            {
                tb.FontFamily = this.GetCachedFontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            if (fontWeight > 0)
            {
                tb.FontWeight = GetFontWeight(fontWeight);
            }

            TextOptions.SetTextFormattingMode(tb, this.TextFormattingMode);

            double dx = 0;
            double dy = 0;

            if (maxSize != null || halign != HorizontalAlignment.Left || valign != VerticalAlignment.Top)
            {
                tb.Measure(new Size(1000, 1000));
                var size = tb.DesiredSize;
                if (maxSize != null)
                {
                    if (size.Width > maxSize.Value.Width + 1e-3)
                    {
                        size.Width = Math.Max(maxSize.Value.Width, 0);
                    }

                    if (size.Height > maxSize.Value.Height + 1e-3)
                    {
                        size.Height = Math.Max(maxSize.Value.Height, 0);
                    }

                    tb.Width = size.Width;
                    tb.Height = size.Height;
                }

                if (halign == HorizontalAlignment.Center)
                {
                    dx = -size.Width / 2;
                }

                if (halign == HorizontalAlignment.Right)
                {
                    dx = -size.Width;
                }

                if (valign == VerticalAlignment.Middle)
                {
                    dy = -size.Height / 2;
                }

                if (valign == VerticalAlignment.Bottom)
                {
                    dy = -size.Height;
                }
            }

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (Math.Abs(rotate) > double.Epsilon)
            {
                transform.Children.Add(new RotateTransform(rotate));
            }

            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;
            if (tb.Clip != null)
            {
                tb.Clip.Transform = tb.RenderTransform.Inverse as Transform;
            }

            tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);
        }

        ///<inheritdoc/>
        public override void DrawImage(
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
            if (destWidth <= 0 || destHeight <= 0 || srcWidth <= 0 || srcHeight <= 0)
            {
                return;
            }

            var image = this.CreateAndAdd<Image>(destX, destY);
            var bitmapChain = this.GetImageSource(source);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (srcX == 0 && srcY == 0 && srcWidth == bitmapChain.PixelWidth && srcHeight == bitmapChain.PixelHeight)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                // do not crop
            }
            else
            {
                bitmapChain = new CroppedBitmap(bitmapChain, new Int32Rect((int)srcX, (int)srcY, (int)srcWidth, (int)srcHeight));
            }

            image.Opacity = opacity;
            image.Width = destWidth;
            image.Height = destHeight;
            image.Stretch = Stretch.Fill;
            RenderOptions.SetBitmapScalingMode(image, interpolate ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor);

            // Set the position of the image
            Canvas.SetLeft(image, destX);
            Canvas.SetTop(image, destY);
            //// alternative: image.RenderTransform = new TranslateTransform(destX, destY);

            image.Source = bitmapChain;
        }

        /// <inheritdoc/>
        protected override void SetClip(OxyRect clippingRectangle)
        {
            this._clip = ToRect(clippingRectangle);
        }

        /// <inheritdoc/>
        protected override void ResetClip()
        {
            this._clip = null;
        }


        /// <summary>
        /// Creates an element of the specified type and adds it to the canvas.
        /// </summary>
        /// <typeparam name="T">Type of element to create.</typeparam>
        /// <param name="clipOffsetX">The clip offset executable.</param>
        /// <param name="clipOffsetY">The clip offset asynchronous.</param>
        /// <returns>The element.</returns>
        protected T CreateAndAdd<T>(double clipOffsetX = 0, double clipOffsetY = 0) where T : FrameworkElement, new()
        {
            // TODO: here we can reuse existing elements in the canvas.Children collection
            var element = new T();

            if (this._clip != null)
            {
                element.Clip = new RectangleGeometry(
                        new Rect(
                            this._clip.Value.X - clipOffsetX,
                            this._clip.Value.Y - clipOffsetY,
                            this._clip.Value.Width,
                            this._clip.Value.Height));
            }

            this._canvas.Children.Add(element);

            this.ApplyToolTip(element);
            return element;
        }

        /// <summary>
        /// Applies the current tool tip to the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        private void ApplyToolTip(FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                element.ToolTip = this._currentToolTip;
            }
        }

        /// <summary>
        /// Sets the stroke properties of the specified shape object.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="dashOffset">The dash offset.</param>
        protected void SetStroke(
            Shape shape,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            LineJoin lineJoin = LineJoin.Miter,
            IEnumerable<double> dashArray = null,
            double dashOffset = 0)
        {
            if (!stroke.IsUndefined() && thickness > 0)
            {
                shape.Stroke = this.GetCachedBrush(stroke);

                switch (lineJoin)
                {
                    case LineJoin.Round:
                    shape.StrokeLineJoin = PenLineJoin.Round;
                    break;
                    case LineJoin.Bevel:
                    shape.StrokeLineJoin = PenLineJoin.Bevel;
                    break;

                    // The default StrokeLineJoin is Miter
                }

                shape.StrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);

                if (dashArray != null)
                {
                    shape.StrokeDashArray = new DoubleCollection(dashArray);
                    shape.StrokeDashOffset = dashOffset;
                }
            }
            else
            {
                shape.StrokeThickness = 0;
            }

            if (edgeRenderingMode == EdgeRenderingMode.PreferSpeed)
            {
                shape.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                shape.SnapsToDevicePixels = true;
            }
        }

        private static T GetFirstAndRest<T>(IEnumerable<T> items, out IList<T> rest)
        {
            using var e = items.GetEnumerator();
            if (!e.MoveNext())
            {
                rest = new T[0];
                return default;
            }

            var ret = e.Current;

            rest = new List<T>();
            while (e.MoveNext())
            {
                rest.Add(e.Current);
            }

            return ret;
        }
    }
}
