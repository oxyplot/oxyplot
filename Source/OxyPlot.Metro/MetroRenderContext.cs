// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetroRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Rendering Metro shapes to a Canvas
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Metro
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.Foundation;
    using Windows.Storage.Streams;
    using Windows.UI.Text;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    /// <summary>
    /// Rendering Metro shapes to a Canvas
    /// </summary>
    public class MetroRenderContext : IRenderContext
    {
        /// <summary>
        /// The brush cache.
        /// </summary>
        private readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        /// <summary>
        /// The canvas.
        /// </summary>
        private readonly Canvas canvas;

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, BitmapSource> imageCache = new Dictionary<OxyImage, BitmapSource>();

        /// <summary>
        /// The clip rectangle.
        /// </summary>
        private Rect? clip;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetroRenderContext" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public MetroRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Gets a value indicating whether the context renders to screen.
        /// </summary>
        /// <value><c>true</c> if the context renders to screen; otherwise, <c>false</c>.</value>
        public bool RendersToScreen
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle defining the ellipse.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var el = new Ellipse();
            if (stroke.IsVisible())
            {
                el.Stroke = stroke.ToBrush();
                el.StrokeThickness = thickness;
            }

            if (fill.IsVisible())
            {
                el.Fill = fill.ToBrush();
            }

            el.Width = rect.Width;
            el.Height = rect.Height;
            Canvas.SetLeft(el, rect.Left);
            Canvas.SetTop(el, rect.Top);
            this.Add(el, rect.Left, rect.Top);
        }

        /// <summary>
        /// The draw ellipses.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness);
            if (fill.IsVisible())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (OxyRect rect in rectangles)
            {
                gg.Children.Add(
                    new EllipseGeometry
                        {
                            Center = new Point(rect.Left + (rect.Width / 2), rect.Top + (rect.Height / 2)),
                            RadiusX = rect.Width / 2,
                            RadiusY = rect.Height / 2
                        });
            }

            path.Data = gg;
            this.Add(path);
        }

        /// <summary>
        /// The draw line.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">The aliased.</param>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var e = new Polyline();
            this.SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

            var pc = new PointCollection();
            foreach (ScreenPoint p in points)
            {
                pc.Add(p.ToPoint(aliased));
            }

            e.Points = pc;

            this.Add(e);
        }

        /// <summary>
        /// The draw line segments.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">The aliased.</param>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
            var pg = new PathGeometry();
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                // if (points[i].Y==points[i+1].Y)
                // {
                // var line = new Line();

                // line.X1 = 0.5+(int)points[i].X;
                // line.X2 = 0.5+(int)points[i+1].X;
                // line.Y1 = 0.5+(int)points[i].Y;
                // line.Y2 = 0.5+(int)points[i+1].Y;
                // SetStroke(line, OxyColors.DarkRed, thickness, lineJoin, dashArray, aliased);
                // Add(line);
                // continue;
                // }
                var figure = new PathFigure { StartPoint = points[i].ToPoint(aliased), IsClosed = false };
                figure.Segments.Add(new LineSegment { Point = points[i + 1].ToPoint(aliased) });
                pg.Figures.Add(figure);
            }

            path.Data = pg;
            this.Add(path);
        }

        /// <summary>
        /// The draw polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">The aliased.</param>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var po = new Polygon();
            this.SetStroke(po, stroke, thickness, lineJoin, dashArray, aliased);

            if (fill.IsVisible())
            {
                po.Fill = this.GetCachedBrush(fill);
            }

            var pc = new PointCollection();
            foreach (ScreenPoint p in points)
            {
                pc.Add(p.ToPoint(aliased));
            }

            po.Points = pc;

            this.Add(po);
        }

        /// <summary>
        /// The draw polygons.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">The aliased.</param>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            LineJoin lineJoin,
            bool aliased)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
            if (fill.IsVisible())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var pg = new PathGeometry { FillRule = FillRule.Nonzero };
            foreach (var polygon in polygons)
            {
                var figure = new PathFigure { IsClosed = true };
                bool first = true;
                foreach (ScreenPoint p in polygon)
                {
                    if (first)
                    {
                        figure.StartPoint = p.ToPoint(aliased);
                        first = false;
                    }
                    else
                    {
                        figure.Segments.Add(new LineSegment { Point = p.ToPoint(aliased) });
                    }
                }

                pg.Figures.Add(figure);
            }

            path.Data = pg;
            this.Add(path);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var el = new Rectangle();
            if (stroke.IsVisible())
            {
                el.Stroke = stroke.ToBrush();
                el.StrokeThickness = thickness;
            }

            if (fill.IsVisible())
            {
                el.Fill = fill.ToBrush();
            }

            el.Width = rect.Width;
            el.Height = rect.Height;
            Canvas.SetLeft(el, rect.Left);
            Canvas.SetTop(el, rect.Top);
            this.Add(el, rect.Left, rect.Top);
        }

        /// <summary>
        /// Draws a collection of rectangles, where all have the same stroke and fill.
        /// This performs better than calling DrawRectangle multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness);
            if (fill.IsVisible())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (OxyRect rect in rectangles)
            {
                gg.Children.Add(new RectangleGeometry { Rect = rect.ToRect(true) });
            }

            path.Data = gg;
            this.Add(path);
        }

        /// <summary>
        /// The draw text.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotate.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            OxyPlot.HorizontalAlignment halign,
            OxyPlot.VerticalAlignment valign,
            OxySize? maxSize)
        {
            if (fill.IsInvisible() || string.IsNullOrEmpty(text))
            {
                return;
            }

            var tb = new TextBlock { Text = text, Foreground = fill.ToBrush() };

            // tb.SetValue(TextOptions.TextHintingModeProperty, TextHintingMode.Animated);
            if (fontFamily != null)
            {
                tb.FontFamily = new FontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            tb.FontWeight = GetFontWeight(fontWeight);

            tb.Measure(new Size(1000, 1000));

            double dx = 0;
            if (halign == OxyPlot.HorizontalAlignment.Center)
            {
                dx = -tb.ActualWidth / 2;
            }

            if (halign == OxyPlot.HorizontalAlignment.Right)
            {
                dx = -tb.ActualWidth;
            }

            double dy = 0;
            if (valign == OxyPlot.VerticalAlignment.Middle)
            {
                dy = -tb.ActualHeight / 2;
            }

            if (valign == OxyPlot.VerticalAlignment.Bottom)
            {
                dy = -tb.ActualHeight;
            }

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform { X = (int)dx, Y = (int)dy });
            if (!rotate.Equals(0.0))
            {
                transform.Children.Add(new RotateTransform { Angle = rotate });
            }

            transform.Children.Add(new TranslateTransform { X = (int)p.X, Y = (int)p.Y });
            tb.RenderTransform = transform;

            if (this.clip.HasValue)
            {
                // add a clipping container that is not rotated
                var c = new Canvas();
                c.Children.Add(tb);
                this.Add(c);
            }
            else
            {
                this.Add(tb);
            }
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The text size.</returns>
        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            var tb = new TextBlock { Text = text };

            if (fontFamily != null)
            {
                tb.FontFamily = new FontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            tb.FontWeight = GetFontWeight(fontWeight);

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.ActualWidth, tb.ActualHeight);
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tooltip.</param>
        public void SetToolTip(string text)
        {
        }

        /// <summary>
        /// Draws the specified portion of the specified <see cref="OxyImage" /> at the specified location and with the specified size.
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

            var image = new Image();
            var bitmapChain = this.GetImageSource(source);

            if (srcX.Equals(0) && srcY.Equals(0) && srcWidth.Equals(bitmapChain.PixelWidth) && srcHeight.Equals(bitmapChain.PixelHeight))
            {
                // do not crop
            }
            else
            {
                throw new NotSupportedException("Use DrawClippedImage, CroppedBitmap is not supported here.");
            }

            image.Opacity = opacity;
            image.Width = destWidth;
            image.Height = destHeight;
            image.Stretch = Stretch.Fill;

            ////  RenderOptions.SetBitmapScalingMode(image, interpolate ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor);

            // Set the position of the image
            Canvas.SetLeft(image, destX);
            Canvas.SetTop(image, destY);

            //// alternative: image.RenderTransform = new TranslateTransform(destX, destY);
            //// TODO: check performance?

            image.Source = bitmapChain;
            this.Add(image, destX, destY);
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public bool SetClip(OxyRect clippingRect)
        {
            this.clip = clippingRect.ToRect(false);
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
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i)).ToArray();

            // Remove the images from the cache
            foreach (var i in imagesToRelease)
            {
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();
        }

        /// <summary>
        /// Creates the dash array collection.
        /// </summary>
        /// <param name="dashArray">The dash array.</param>
        /// <returns>The dash collection.</returns>
        private static DoubleCollection CreateDashArrayCollection(IList<double> dashArray)
        {
            var dac = new DoubleCollection();
            foreach (double v in dashArray)
            {
                dac.Add(v);
            }

            return dac;
        }

        /// <summary>
        /// Converts a font weight value to a FontWeight.
        /// </summary>
        /// <param name="fontWeight">The font weight value.</param>
        /// <returns>The font weight.</returns>
        private static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight > OxyPlot.FontWeights.Normal ? FontWeights.Bold : FontWeights.Normal;
        }

        /// <summary>
        /// Adds the specified shape to the canvas.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="clipOffsetX">The clip offset X.</param>
        /// <param name="clipOffsetY">The clip offset Y.</param>
        private void Add(UIElement element, double clipOffsetX = 0, double clipOffsetY = 0)
        {
            if (this.clip.HasValue)
            {
                this.ApplyClip(element, clipOffsetX, clipOffsetY);
            }

            this.canvas.Children.Add(element);
        }

        /// <summary>
        /// Gets a brush from the cache or creates a new one.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <returns>The brush.</returns>
        private Brush GetCachedBrush(OxyColor stroke)
        {
            Brush brush;
            if (!this.brushCache.TryGetValue(stroke, out brush))
            {
                brush = stroke.ToBrush();
                this.brushCache.Add(stroke, brush);
            }

            return brush;
        }

        /// <summary>
        /// Sets the stroke properties of the specified shape.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="aliased">The aliased.</param>
        // ReSharper disable UnusedParameter.Local
        private void SetStroke(
            Shape shape,
            OxyColor stroke,
            double thickness,
            LineJoin lineJoin = LineJoin.Miter,
            double[] dashArray = null,
            bool aliased = false)
        {
            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

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

            if (!thickness.Equals(1.0))
            {
                // default values is 1
                shape.StrokeThickness = thickness;
            }

            if (dashArray != null)
            {
                shape.StrokeDashArray = CreateDashArrayCollection(dashArray);
            }

            // shape.UseLayoutRounding = aliased;
        }

        /// <summary>
        /// Applies the clip rectangle.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="x">The x offset of the element.</param>
        /// <param name="y">The y offset of the element.</param>
        private void ApplyClip(UIElement image, double x, double y)
        {
            if (this.clip.HasValue)
            {
                image.Clip = new RectangleGeometry { Rect = new Rect(this.clip.Value.X - x, this.clip.Value.Y - y, this.clip.Value.Width, this.clip.Value.Height) };
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

            BitmapSource bitmapSource;
            if (this.imageCache.TryGetValue(image, out bitmapSource))
            {
                return bitmapSource;
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            Task writeTask = Task.Factory.StartNew(
                async () =>
                {
                    var writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                    writer.WriteBytes(image.GetData());
                    await writer.StoreAsync();
                });
            writeTask.Wait();

            bitmapSource = new BitmapImage();
            bitmapSource.SetSource(randomAccessStream);

            this.imageCache.Add(image, bitmapSource);
            return bitmapSource;
        }
    }
}