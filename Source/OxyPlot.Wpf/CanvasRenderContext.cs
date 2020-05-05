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
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    using FontWeights = OxyPlot.FontWeights;
    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using Path = System.Windows.Shapes.Path;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

    /// <summary>
    /// Implements <see cref="IRenderContext" /> for <see cref="System.Windows.Controls.Canvas" />.
    /// </summary>
    public class CanvasRenderContext : IRenderContext
    {
        /// <summary>
        /// The maximum number of figures per geometry.
        /// </summary>
        private const int MaxFiguresPerGeometry = 16;

        /// <summary>
        /// The maximum number of polylines per line.
        /// </summary>
        private const int MaxPolylinesPerLine = 64;

        /// <summary>
        /// The minimum number of points per polyline.
        /// </summary>
        private const int MinPointsPerPolyline = 16;

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, BitmapSource> imageCache = new Dictionary<OxyImage, BitmapSource>();

        /// <summary>
        /// The brush cache.
        /// </summary>
        private readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        /// <summary>
        /// The font family cache
        /// </summary>
        private readonly Dictionary<string, FontFamily> fontFamilyCache = new Dictionary<string, FontFamily>();

        /// <summary>
        /// The canvas.
        /// </summary>
        private readonly Canvas canvas;

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private Rect? clip;

        /// <summary>
        /// The current tool tip
        /// </summary>
        private string currentToolTip;

        /// <summary>
        /// The dpi scale.
        /// </summary>
        public double DpiScale { get; set; } = 1;

        /// <summary>
        /// The visual offset relative to visual root.
        /// </summary>
        public Point VisualOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRenderContext" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public CanvasRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            this.TextFormattingMode = TextFormattingMode.Display;
            this.TextMeasurementMethod = TextMeasurementMethod.TextBlock;
            this.UseStreamGeometry = true;
            this.RendersToScreen = true;
            this.BalancedLineDrawingThicknessLimit = 3.5;
        }

        /// <summary>
        /// Gets or sets the text measurement method.
        /// </summary>
        /// <value>The text measurement method.</value>
        public TextMeasurementMethod TextMeasurementMethod { get; set; }

        /// <summary>
        /// Gets or sets the text formatting mode.
        /// </summary>
        /// <value>The text formatting mode. The default value is <see cref="System.Windows.Media.TextFormattingMode.Display"/>.</value>
        public TextFormattingMode TextFormattingMode { get; set; }

        /// <summary>
        /// Gets or sets the thickness limit for "balanced" line drawing.
        /// </summary>
        public double BalancedLineDrawingThicknessLimit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use stream geometry for lines and polygons rendering.
        /// </summary>
        /// <value><c>true</c> if stream geometry should be used; otherwise, <c>false</c> .</value>
        /// <remarks>The XamlWriter does not serialize StreamGeometry, so set this to <c>false</c> if you want to export to XAML. Using stream geometry seems to be slightly faster than using path geometry.</remarks>
        public bool UseStreamGeometry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the context renders to screen.
        /// </summary>
        /// <value><c>true</c> if the context renders to screen; otherwise, <c>false</c>.</value>
        public bool RendersToScreen { get; set; }

        ///<inheritdoc/>
        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.DrawEllipses(new[] { rect }, fill, stroke, thickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new EllipseGeometry(ToRect(rect)));
            }

            path.Data = gg;
        }

        ///<inheritdoc/>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (thickness < this.BalancedLineDrawingThicknessLimit)
            {
                this.DrawLineBalanced(points, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
                return;
            }

            var e = this.CreateAndAdd<Polyline>();
            this.SetStroke(e, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
            e.Points = this.ToPointCollection(points, e.StrokeThickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (this.UseStreamGeometry)
            {
                this.DrawLineSegmentsByStreamGeometry(points, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
                return;
            }

            Path path = null;
            PathGeometry pathGeometry = null;

            int count = 0;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (path == null)
                {
                    path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
                    pathGeometry = new PathGeometry();
                }

                var actualPoints = this.GetActualPoints( new[] { points[i], points[i + 1] }, path.StrokeThickness, edgeRenderingMode).ToList();

                var figure = new PathFigure { StartPoint = actualPoints[0], IsClosed = false };
                figure.Segments.Add(new LineSegment(actualPoints[1], true) { IsSmoothJoin = false });
                pathGeometry.Figures.Add(figure);

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry || dashArray != null)
                {
                    path.Data = pathGeometry;
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                path.Data = pathGeometry;
            }
        }

        ///<inheritdoc/>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            var e = this.CreateAndAdd<Polygon>();
            this.SetStroke(e, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);

            if (!fill.IsUndefined())
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Points = this.ToPointCollection(points, e.StrokeThickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            var usg = this.UseStreamGeometry;
            Path path = null;
            StreamGeometry streamGeometry = null;
            StreamGeometryContext sgc = null;
            PathGeometry pathGeometry = null;
            int count = 0;

            foreach (var polygon in polygons)
            {
                if (path == null)
                {
                    path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
                    if (!fill.IsUndefined())
                    {
                        path.Fill = this.GetCachedBrush(fill);
                    }

                    if (usg)
                    {
                        streamGeometry = new StreamGeometry { FillRule = FillRule.Nonzero };
                        sgc = streamGeometry.Open();
                    }
                    else
                    {
                        pathGeometry = new PathGeometry { FillRule = FillRule.Nonzero };
                    }
                }

                PathFigure figure = null;
                bool first = true;
                foreach (var point in GetActualPoints(polygon, path.StrokeThickness, edgeRenderingMode))
                {
                    if (first)
                    {
                        if (usg)
                        {
                            sgc.BeginFigure(point, !fill.IsUndefined(), true);
                        }
                        else
                        {
                            figure = new PathFigure
                            {
                                StartPoint = point,
                                IsFilled = !fill.IsUndefined(),
                                IsClosed = true
                            };
                            pathGeometry.Figures.Add(figure);
                        }

                        first = false;
                    }
                    else
                    {
                        if (usg)
                        {
                            sgc.LineTo(point, !stroke.IsUndefined(), true);
                        }
                        else
                        {
                            figure.Segments.Add(new LineSegment(point, !stroke.IsUndefined()) { IsSmoothJoin = true });
                        }
                    }
                }

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry)
                {
                    if (usg)
                    {
                        sgc.Close();
                        path.Data = streamGeometry;
                    }
                    else
                    {
                        path.Data = pathGeometry;
                    }

                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                if (usg)
                {
                    sgc.Close();
                    path.Data = streamGeometry;
                }
                else
                {
                    path.Data = pathGeometry;
                }
            }
        }

        ///<inheritdoc/>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var e = this.CreateAndAdd<Path>();
            this.SetStroke(e, stroke, thickness, edgeRenderingMode);

            if (!fill.IsUndefined())
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            gg.Children.Add(new RectangleGeometry { Rect = this.GetActualRect(rect, e.StrokeThickness, edgeRenderingMode) });
            e.Data = gg;
        }

        ///<inheritdoc/>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new RectangleGeometry { Rect = this.GetActualRect(rect, path.StrokeThickness, edgeRenderingMode) });
            }

            path.Data = gg;
        }

        /// <summary>
        /// Draws text.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The text color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text (in device independent units, 1/96 inch).</param>
        public void DrawText(
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

        /// <summary>
        /// Measures the size of the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>
        /// The size of the text (in device independent units, 1/96 inch).
        /// </returns>
        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            if (this.TextMeasurementMethod == TextMeasurementMethod.GlyphTypeface)
            {
                return this.MeasureTextByGlyphTypeface(text, fontFamily, fontSize, fontWeight);
            }

            var tb = new TextBlock { Text = text };

            TextOptions.SetTextFormattingMode(tb, this.TextFormattingMode);

            if (fontFamily != null)
            {
                tb.FontFamily = new FontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            if (fontWeight > 0)
            {
                tb.FontWeight = GetFontWeight(fontWeight);
            }

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tool tip.</param>
        public void SetToolTip(string text)
        {
            this.currentToolTip = text;
        }

        /// <summary>
        /// Draws a portion of the specified <see cref="OxyImage" />.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcWidth">Width of the portion of the source image to draw.</param>
        /// <param name="srcHeight">Height of the portion of the source image to draw.</param>
        /// <param name="destX">The x-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destY">The y-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destWidth">The width of the drawn image.</param>
        /// <param name="destHeight">The height of the drawn image.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
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

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <returns><c>true</c> if the clip rectangle was set.</returns>
        public bool SetClip(OxyRect clippingRect)
        {
            this.clip = ToRect(clippingRect);
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public void ResetClip()
        {
            this.clip = null;
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public void CleanUp()
        {
            // Find the images in the cache that has not been used since last call to this method
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i)).ToList();

            // Remove the images from the cache
            foreach (var i in imagesToRelease)
            {
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();
        }

        /// <summary>
        /// Measures the size of the specified text by a faster method (using GlyphTypefaces).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        protected OxySize MeasureTextByGlyphTypeface(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            var typeface = new Typeface(
                new FontFamily(fontFamily), FontStyles.Normal, GetFontWeight(fontWeight), FontStretches.Normal);

            if (!typeface.TryGetGlyphTypeface(out var glyphTypeface))
            {
                throw new InvalidOperationException("No glyph typeface found");
            }

            return MeasureTextSize(glyphTypeface, fontSize, text);
        }

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <param name="fontWeight">The font weight value.</param>
        /// <returns>The font weight.</returns>
        private static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight > FontWeights.Normal ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
        }

        /// <summary>
        /// Fast text size calculation
        /// </summary>
        /// <param name="glyphTypeface">The glyph typeface.</param>
        /// <param name="sizeInEm">The size.</param>
        /// <param name="s">The text.</param>
        /// <returns>The text size.</returns>
        private static OxySize MeasureTextSize(GlyphTypeface glyphTypeface, double sizeInEm, string s)
        {
            double width = 0;
            double lineWidth = 0;
            int lines = 0;
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '\n':
                        lines++;
                        if (lineWidth > width)
                        {
                            width = lineWidth;
                        }

                        lineWidth = 0;
                        continue;
                    case '\t':
                        continue;
                }

                ushort glyph = glyphTypeface.CharacterToGlyphMap[ch];
                double advanceWidth = glyphTypeface.AdvanceWidths[glyph];
                lineWidth += advanceWidth;
            }

            lines++;
            if (lineWidth > width)
            {
                width = lineWidth;
            }

            return new OxySize(Math.Round(width * sizeInEm, 2), Math.Round(lines * glyphTypeface.Height * sizeInEm, 2));
        }

        /// <summary>
        /// Creates an element of the specified type and adds it to the canvas.
        /// </summary>
        /// <typeparam name="T">Type of element to create.</typeparam>
        /// <param name="clipOffsetX">The clip offset executable.</param>
        /// <param name="clipOffsetY">The clip offset asynchronous.</param>
        /// <returns>The element.</returns>
        private T CreateAndAdd<T>(double clipOffsetX = 0, double clipOffsetY = 0) where T : FrameworkElement, new()
        {
            // TODO: here we can reuse existing elements in the canvas.Children collection
            var element = new T();

            if (this.clip != null)
            {
                element.Clip = new RectangleGeometry(
                        new Rect(
                            this.clip.Value.X - clipOffsetX,
                            this.clip.Value.Y - clipOffsetY,
                            this.clip.Value.Width,
                            this.clip.Value.Height));
            }

            this.canvas.Children.Add(element);

            this.ApplyToolTip(element);
            return element;
        }

        /// <summary>
        /// Applies the current tool tip to the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        private void ApplyToolTip(FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(this.currentToolTip))
            {
                element.ToolTip = this.currentToolTip;
            }
        }

        /// <summary>
        /// Draws the line segments by stream geometry.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join.</param>
        private void DrawLineSegmentsByStreamGeometry(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            StreamGeometry streamGeometry = null;
            StreamGeometryContext streamGeometryContext = null;

            int count = 0;
            var actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (streamGeometry == null)
                {
                    streamGeometry = new StreamGeometry();
                    streamGeometryContext = streamGeometry.Open();
                }

                var actualPoints = this.GetActualPoints(new[] { points[i], points[i + 1] }, actualStrokeThickness, edgeRenderingMode).ToList();

                streamGeometryContext.BeginFigure(actualPoints[0], false, false);
                streamGeometryContext.LineTo(actualPoints[1], true, false);

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry || dashArray != null)
                {
                    streamGeometryContext.Close();
                    var path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
                    path.Data = streamGeometry;
                    streamGeometry = null;
                    count = 0;
                }
            }

            if (streamGeometry != null)
            {
                streamGeometryContext.Close();
                var path = this.CreateAndAdd<Path>();
                this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, null, 0);
                path.Data = streamGeometry;
            }
        }

        /// <summary>
        /// Gets the cached brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The brush.</returns>
        private Brush GetCachedBrush(OxyColor color)
        {
            if (color.A == 0)
            {
                return null;
            }

            if (!this.brushCache.TryGetValue(color, out var brush))
            {
                brush = new SolidColorBrush(color.ToColor());
                brush.Freeze();
                this.brushCache.Add(color, brush);
            }

            return brush;
        }

        /// <summary>
        /// Gets the cached font family.
        /// </summary>
        /// <param name="familyName">Name of the family.</param>
        /// <returns>The FontFamily.</returns>
        private FontFamily GetCachedFontFamily(string familyName)
        {
            if (familyName == null)
            {
                return null;
            }

            if (!this.fontFamilyCache.TryGetValue(familyName, out var ff))
            {
                ff = new FontFamily(familyName);
                this.fontFamilyCache.Add(familyName, ff);
            }

            return ff;
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
        private void SetStroke(
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

        /// <summary>
        /// Gets the bitmap source.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The bitmap source.</returns>
        private BitmapSource GetImageSource(OxyImage image)
        {
            if (image == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(image))
            {
                this.imagesInUse.Add(image);
            }

            if (this.imageCache.TryGetValue(image, out BitmapSource src))
            {
                return src;
            }

            using (var ms = new MemoryStream(image.GetData()))
            {
                var btm = new BitmapImage();
                btm.BeginInit();
                btm.StreamSource = ms;
                btm.CacheOption = BitmapCacheOption.OnLoad;
                btm.EndInit();
                btm.Freeze();
                this.imageCache.Add(image, btm);
                return btm;
            }
        }

        /// <summary>
        /// Draws the line using the MaxPolylinesPerLine and MinPointsPerPolyline properties.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join.</param>
        private void DrawLineBalanced(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            // balance the number of points per polyline and the number of polylines
            var numPointsPerPolyline = Math.Max(points.Count / MaxPolylinesPerLine, MinPointsPerPolyline);

            var polyline = this.CreateAndAdd<Polyline>();
            this.SetStroke(polyline, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
            var pc = new PointCollection(numPointsPerPolyline);

            var n = points.Count;
            double lineLength = 0;
            var dashPatternLength = (dashArray != null) ? dashArray.Sum() : 0;
            var last = new Point();
            var i = 0;
            foreach (var p in this.GetActualPoints(points, thickness, edgeRenderingMode))
            {
                pc.Add(p);

                // alt. 1
                if (dashArray != null)
                {
                    if (i > 0)
                    {
                        var delta = p - last;
                        var dist = Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
                        lineLength += dist;
                    }

                    last = p;
                }

                // use multiple polylines with limited number of points to improve WPF performance
                if (pc.Count >= numPointsPerPolyline)
                {
                    polyline.Points = pc;

                    if (i < n - 1)
                    {
                        // alt.2
                        ////if (dashArray != null)
                        ////{
                        ////    lineLength += this.GetLength(polyline);
                        ////}

                        // start a new polyline at last point so there is no gap (it is not necessary to use the % operator)
                        var dashOffset = dashPatternLength > 0 ? lineLength / thickness : 0;
                        polyline = this.CreateAndAdd<Polyline>();
                        this.SetStroke(polyline, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, dashOffset);
                        pc = new PointCollection(numPointsPerPolyline) { pc.Last() };
                    }
                }

                i++;
            }

            if (pc.Count > 1 || n == 1)
            {
                polyline.Points = pc;
            }
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>A <see cref="Rect" />.</returns>
        private static Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Snaps points to pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed points.</returns>
        private IEnumerable<Point> GetActualPoints(IList<ScreenPoint> points, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Adaptive when RenderContextBase.IsStraightLine(points):
                case EdgeRenderingMode.Automatic when RenderContextBase.IsStraightLine(points):
                case EdgeRenderingMode.PreferSharpness:
                    return points.Select(p => PixelLayout.Snap(p.X, p.Y, strokeThickness, this.VisualOffset, this.DpiScale));
                default:
                    return points.Select(p => new Point(p.X, p.Y));
            }
        }

        /// <summary>
        /// Snaps a rectangle to device pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed rectangle.</returns>
        private Rect GetActualRect(OxyRect rect, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferGeometricAccuracy:
                case EdgeRenderingMode.PreferSpeed:
                    return ToRect(rect);
                default:
                    return PixelLayout.Snap(ToRect(rect), strokeThickness, this.VisualOffset, this.DpiScale);
            }
        }

        /// <summary>
        /// Snaps a stroke thickness to device pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed stroke thickness.</returns>
        private double GetActualStrokeThickness(double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferSharpness:
                    return PixelLayout.SnapStrokeThickness(strokeThickness, this.DpiScale);
                default:
                    return strokeThickness;
            }
        }

        /// <summary>
        /// Creates a point collection from the specified points. The points are snapped to pixels if required by the edge rendering mode,
        /// </summary>
        /// <param name="points">The points to convert.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The point collection.</returns>
        private PointCollection ToPointCollection(IList<ScreenPoint> points, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            return new PointCollection(GetActualPoints(points, strokeThickness, edgeRenderingMode));
        }
    }
}
