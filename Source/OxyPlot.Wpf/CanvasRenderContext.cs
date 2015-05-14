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
        /// The pixel scale
        /// </summary>
        private double pixelScale;

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

            // TODO: issue 10221 - try to find the size of physical pixels
            var presentationSource = PresentationSource.FromVisual(canvas);
            if (presentationSource != null && presentationSource.CompositionTarget != null)
            {
                this.pixelScale = presentationSource.CompositionTarget.TransformToDevice.M11;
            }
            else
            {
                this.pixelScale = 1;
            }
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

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the ellipse will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the ellipse will not be stroked.</param>
        /// <param name="thickness">The thickness (in device independent units, 1/96 inch).</param>
        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = this.CreateAndAdd<Ellipse>(rect.Left, rect.Top);
            this.SetStroke(e, stroke, thickness);
            if (fill.IsVisible())
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
        }

        /// <summary>
        /// Draws a collection of ellipses, where all have the same stroke and fill.
        /// This performs better than calling DrawEllipse multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the ellipses will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the ellipses will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new EllipseGeometry(this.ToRect(rect)));
            }

            path.Data = gg;
        }

        /// <summary>
        /// Draws a polyline.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch). Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            if (thickness < this.BalancedLineDrawingThicknessLimit)
            {
                this.DrawLineBalanced(points, stroke, thickness, dashArray, lineJoin, aliased);
                return;
            }

            var e = this.CreateAndAdd<Polyline>();
            this.SetStroke(e, stroke, thickness, lineJoin, dashArray, 0, aliased);

            e.Points = this.ToPointCollection(points, aliased);
        }

        /// <summary>
        /// Draws line segments defined by points (0,1) (2,3) (4,5) etc.
        /// This should have better performance than calling DrawLine for each segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch).</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            if (this.UseStreamGeometry)
            {
                this.DrawLineSegmentsByStreamGeometry(points, stroke, thickness, dashArray, lineJoin, aliased);
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
                    this.SetStroke(path, stroke, thickness, lineJoin, dashArray, 0, aliased);
                    pathGeometry = new PathGeometry();
                }

                var figure = new PathFigure { StartPoint = this.ToPoint(points[i], aliased), IsClosed = false };
                figure.Segments.Add(new LineSegment(this.ToPoint(points[i + 1], aliased), true) { IsSmoothJoin = false });
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

        /// <summary>
        /// Draws a polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the polygon will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the polygon will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch).</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">If set to <c>true</c> the polygon will be aliased.</param>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var e = this.CreateAndAdd<Polygon>();
            this.SetStroke(e, stroke, thickness, lineJoin, dashArray, 0, aliased);

            if (!fill.IsUndefined())
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Points = this.ToPointCollection(points, aliased);
        }

        /// <summary>
        /// Draws a collection of polygons, where all polygons have the same stroke and fill.
        /// This performs better than calling DrawPolygon multiple times.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the polygons will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the polygons will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch).</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
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
                    this.SetStroke(path, stroke, thickness, lineJoin, dashArray, 0, aliased);
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
                foreach (var p in polygon)
                {
                    var point = aliased ? this.ToPixelAlignedPoint(p) : this.ToPoint(p);
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

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the rectangle will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the rectangle will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = this.CreateAndAdd<Rectangle>(rect.Left, rect.Top);
            this.SetStroke(e, stroke, thickness, LineJoin.Miter, null, 0, true);

            if (!fill.IsUndefined())
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
        }

        /// <summary>
        /// Draws a collection of rectangles, where all have the same stroke and fill.
        /// This performs better than calling DrawRectangle multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the rectangles will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the rectangles will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new RectangleGeometry { Rect = this.ToPixelAlignedRect(rect) });
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
            this.clip = this.ToRect(clippingRect);
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

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
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
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">Draw aliased line if set to <c>true</c> .</param>
        private void DrawLineSegmentsByStreamGeometry(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            StreamGeometry streamGeometry = null;
            StreamGeometryContext streamGeometryContext = null;

            int count = 0;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (streamGeometry == null)
                {
                    streamGeometry = new StreamGeometry();
                    streamGeometryContext = streamGeometry.Open();
                }

                streamGeometryContext.BeginFigure(this.ToPoint(points[i], aliased), false, false);
                streamGeometryContext.LineTo(this.ToPoint(points[i + 1], aliased), true, false);

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry || dashArray != null)
                {
                    streamGeometryContext.Close();
                    var path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, lineJoin, dashArray, 0, aliased);
                    path.Data = streamGeometry;
                    streamGeometry = null;
                    count = 0;
                }
            }

            if (streamGeometry != null)
            {
                streamGeometryContext.Close();
                var path = this.CreateAndAdd<Path>();
                this.SetStroke(path, stroke, thickness, lineJoin, null, 0, aliased);
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

            Brush brush;
            if (!this.brushCache.TryGetValue(color, out brush))
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

            FontFamily ff;
            if (!this.fontFamilyCache.TryGetValue(familyName, out ff))
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
        /// <param name="lineJoin">The line join.</param>
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="dashOffset">The dash offset.</param>
        /// <param name="aliased">The aliased.</param>
        private void SetStroke(
            Shape shape,
            OxyColor stroke,
            double thickness,
            LineJoin lineJoin = LineJoin.Miter,
            IEnumerable<double> dashArray = null,
            double dashOffset = 0,
            bool aliased = false)
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

                if (Math.Abs(thickness - 1) > double.Epsilon)
                {
                    // only set if different from the default value (1)
                    shape.StrokeThickness = thickness;
                }

                if (dashArray != null)
                {
                    shape.StrokeDashArray = new DoubleCollection(dashArray);
                    shape.StrokeDashOffset = dashOffset;
                }
            }

            if (aliased)
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

            BitmapSource src;
            if (this.imageCache.TryGetValue(image, out src))
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
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">Render aliased if set to <c>true</c>.</param>
        /// <remarks>See <a href="https://oxyplot.codeplex.com/discussions/456679">discussion</a>.</remarks>
        private void DrawLineBalanced(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased)
        {
            // balance the number of points per polyline and the number of polylines
            var numPointsPerPolyline = Math.Max(points.Count / MaxPolylinesPerLine, MinPointsPerPolyline);

            var polyline = this.CreateAndAdd<Polyline>();
            this.SetStroke(polyline, stroke, thickness, lineJoin, dashArray, 0, aliased);
            var pc = new PointCollection(numPointsPerPolyline);

            var n = points.Count;
            double lineLength = 0;
            var dashPatternLength = (dashArray != null) ? dashArray.Sum() : 0;
            var last = new Point();
            for (int i = 0; i < n; i++)
            {
                var p = aliased ? this.ToPixelAlignedPoint(points[i]) : this.ToPoint(points[i]);
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
                        this.SetStroke(polyline, stroke, thickness, lineJoin, dashArray, dashOffset, aliased);
                        pc = new PointCollection(numPointsPerPolyline) { pc.Last() };
                    }
                }
            }

            if (pc.Count > 1 || n == 1)
            {
                polyline.Points = pc;
            }
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="Point" />.
        /// </summary>
        /// <param name="pt">The screen point.</param>
        /// <returns>A <see cref="Point" />.</returns>
        private Point ToPoint(ScreenPoint pt)
        {
            return new Point(pt.X, pt.Y);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a pixel aligned<see cref="Point" />.
        /// </summary>
        /// <param name="pt">The screen point.</param>
        /// <returns>A pixel aligned <see cref="Point" />.</returns>
        private Point ToPixelAlignedPoint(ScreenPoint pt)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            // TODO: issue 10221 - should consider line thickness and logical to physical size of pixels
            return new Point(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>A <see cref="Rect" />.</returns>
        private Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a pixel aligned <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>A pixel aligned<see cref="Rect" />.</returns>
        private Rect ToPixelAlignedRect(OxyRect r)
        {
            // TODO: similar changes as in ToPixelAlignedPoint
            double x = 0.5 + (int)r.Left;
            double y = 0.5 + (int)r.Top;
            double ri = 0.5 + (int)r.Right;
            double bo = 0.5 + (int)r.Bottom;
            return new Rect(x, y, ri - x, bo - y);
        }

        /// <summary>
        /// Converts a <see cref="ScreenPoint" /> to a <see cref="Point" />.
        /// </summary>
        /// <param name="pt">The screen point.</param>
        /// <param name="aliased">use pixel alignment conversion if set to <c>true</c>.</param>
        /// <returns>A <see cref="Point" />.</returns>
        private Point ToPoint(ScreenPoint pt, bool aliased)
        {
            return aliased ? this.ToPixelAlignedPoint(pt) : this.ToPoint(pt);
        }

        /// <summary>
        /// Creates a point collection from the specified points.
        /// </summary>
        /// <param name="points">The points to convert.</param>
        /// <param name="aliased">convert to pixel aligned points if set to <c>true</c>.</param>
        /// <returns>The point collection.</returns>
        private PointCollection ToPointCollection(IEnumerable<ScreenPoint> points, bool aliased)
        {
            return new PointCollection(aliased ? points.Select(this.ToPixelAlignedPoint) : points.Select(this.ToPoint));
        }
    }
}