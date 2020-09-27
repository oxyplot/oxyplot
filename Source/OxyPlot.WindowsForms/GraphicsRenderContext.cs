// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The graphics render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if OXYPLOT_COREDRAWING
namespace OxyPlot.Core.Drawing
#else
namespace OxyPlot.WindowsForms
#endif
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Rendering;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    public class GraphicsRenderContext : ClippingRenderContext, IDisposable, ITextMeasurer
    {
        /// <summary>
        /// The font size factor.
        /// </summary>
        private const float FontsizeFactor = 0.8f;

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, Image> imageCache = new Dictionary<OxyImage, Image>();

        /// <summary>
        /// The brush cache.
        /// </summary>
        private readonly Dictionary<OxyColor, Brush> brushes = new Dictionary<OxyColor, Brush>();

        /// <summary>
        /// The pen cache.
        /// </summary>
        private readonly Dictionary<GraphicsPenDescription, Pen> pens = new Dictionary<GraphicsPenDescription, Pen>();

        /// <summary>
        /// The string format.
        /// </summary>
        private readonly StringFormat stringFormat;

        /// <summary>
        /// The GDI+ drawing surface.
        /// </summary>
        private Graphics g;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsRenderContext" /> class.
        /// </summary>
        /// <param name="graphics">The drawing surface.</param>
        public GraphicsRenderContext(Graphics graphics = null)
        {
            this.g = graphics;
            if (this.g != null)
            {
                this.g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            }

            this.stringFormat = StringFormat.GenericTypographic;

            this.TextArranger = new TextArranger(this, new SimpleTextTrimmer());
        }

        /// <summary>
        /// Gets or sets the <see cref="TextArranger"/> for this instance.
        /// </summary>
        public TextArranger TextArranger { get; set; }

        /// <summary>
        /// Sets the graphics target.
        /// </summary>
        /// <param name="graphics">The graphics surface.</param>
        public void SetGraphicsTarget(Graphics graphics)
        {
            this.g = graphics;
            this.g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }

        /// <inheritdoc/>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var isStroked = stroke.IsVisible() && thickness > 0;

            this.SetSmoothingMode(this.ShouldUseAntiAliasingForEllipse(edgeRenderingMode));

            if (fill.IsVisible())
            {
                this.g.FillEllipse(this.GetCachedBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (!isStroked)
            {
                return;
            }

            var pen = this.GetCachedPen(stroke, thickness);
            this.g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        /// <inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            OxyPlot.LineJoin lineJoin)
        {
            if (stroke.IsInvisible() || thickness <= 0 || points.Count < 2)
            {
                return;
            }

            this.SetSmoothingMode(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));

            var pen = this.GetCachedPen(stroke, thickness, dashArray, lineJoin);
            this.g.DrawLines(pen, this.ToPoints(points));
        }

        /// <inheritdoc/>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            OxyPlot.LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            this.SetSmoothingMode(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));

            var pts = this.ToPoints(points);
            if (fill.IsVisible())
            {
                this.g.FillPolygon(this.GetCachedBrush(fill), pts);
            }

            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            var pen = this.GetCachedPen(stroke, thickness, dashArray, lineJoin);
            this.g.DrawPolygon(pen, pts);
        }

        /// <inheritdoc/>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.SetSmoothingMode(this.ShouldUseAntiAliasingForRect(edgeRenderingMode));

            if (fill.IsVisible())
            {
                this.g.FillRectangle(
                    this.GetCachedBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            var pen = this.GetCachedPen(stroke, thickness);
            this.g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotation">The rotation angle.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotation,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            OxySize? maxSize)
        {
            if (text == null)
            {
                return;
            }

            var fontStyle = fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;

            using (var font = CreateFont(fontFamily, fontSize, fontStyle))
            {
                this.stringFormat.Alignment = StringAlignment.Near;
                this.stringFormat.LineAlignment = StringAlignment.Near;

                var graphicsState = this.g.Save();

                this.g.TranslateTransform((float)p.X, (float)p.Y);

                if (Math.Abs(rotation) > double.Epsilon)
                {
                    this.g.RotateTransform((float)rotation);
                }

                // arrange around the origin with no rotation, because Graphics does the rotation for us
                this.TextArranger.ArrangeText(new ScreenPoint(0, 0), text, fontFamily, fontSize, fontWeight, 0.0, horizontalAlignment, verticalAlignment, maxSize, HorizontalAlignment.Left, TextVerticalAlignment.Top, out var lines, out var linePositions);

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var linePosition = new PointF((float)linePositions[i].X, (float)linePositions[i].Y);

                    this.g.DrawString(line, font, this.GetCachedBrush(fill), linePosition, this.stringFormat);
                }

                this.g.Restore(graphicsState);
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
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            return this.TextArranger.MeasureText(text, fontFamily, fontSize, fontWeight);
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public override void CleanUp()
        {
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i)).ToList();
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The source executable.</param>
        /// <param name="srcY">The source asynchronous.</param>
        /// <param name="srcWidth">Width of the source.</param>
        /// <param name="srcHeight">Height of the source.</param>
        /// <param name="x">The executable.</param>
        /// <param name="y">The asynchronous.</param>
        /// <param name="w">The forward.</param>
        /// <param name="h">The authentication.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">if set to <c>true</c> [interpolate].</param>
        public override void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double x, double y, double w, double h, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                ImageAttributes ia = null;
                if (opacity < 1)
                {
                    var cm = new ColorMatrix
                                 {
                                     Matrix00 = 1f,
                                     Matrix11 = 1f,
                                     Matrix22 = 1f,
                                     Matrix33 = 1f,
                                     Matrix44 = (float)opacity
                                 };

                    ia = new ImageAttributes();
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                }

                this.g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                int sx = (int)Math.Floor(x);
                int sy = (int)Math.Floor(y);
                int sw = (int)Math.Ceiling(x + w) - sx;
                int sh = (int)Math.Ceiling(y + h) - sy;
                var destRect = new Rectangle(sx, sy, sw, sh);
                this.g.DrawImage(image, destRect, (float)srcX - 0.5f, (float)srcY - 0.5f, (float)srcWidth, (float)srcHeight, GraphicsUnit.Pixel, ia);
            }
        }

        /// <inheritdoc/>
        protected override void SetClip(OxyRect rect)
        {
            this.g.SetClip(rect.ToRect(false));
        }

        /// <inheritdoc/>
        protected override void ResetClip()
        {
            this.g.ResetClip();
        }

        /// <inheritdoc/>
        public FontMetrics GetFontMetrics(string fontFamily, double fontSize, double fontWeight)
        {
            // TODO: DPI support
            var fontStyle = fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;
            using (var font = CreateFont(fontFamily, fontSize, fontStyle))
            {
                var factor = font.Height / (double)Math.Abs(font.FontFamily.GetLineSpacing(fontStyle));

                var ascender = factor * Math.Abs(font.FontFamily.GetCellAscent(fontStyle));
                var descender = factor * Math.Abs(font.FontFamily.GetCellDescent(fontStyle));
                var leading = font.Height - ascender - descender;

                return new FontMetrics(ascender, descender, leading);
            }
        }

        /// <inheritdoc/>
        public double MeasureTextWidth(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return 0.0;
            }

            var fontStyle = fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;
            using (var font = CreateFont(fontFamily, fontSize, fontStyle))
            {
                this.stringFormat.Alignment = StringAlignment.Near;
                this.stringFormat.LineAlignment = StringAlignment.Near;
                var size = this.g.MeasureString(text, font, int.MaxValue, this.stringFormat);
                return size.Width;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // dispose images
            foreach (var i in this.imageCache)
            {
                i.Value.Dispose();
            }
            this.imageCache.Clear();

            // dispose pens, brushes etc.
            this.stringFormat.Dispose();

            foreach (var brush in this.brushes.Values)
            {
                brush.Dispose();
            }
            this.brushes.Clear();

            foreach (var pen in this.pens.Values)
            {
                pen.Dispose();
            }
            this.pens.Clear();
        }

        /// <summary>
        /// Creates a font.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <returns>A font</returns>
        private static Font CreateFont(string fontFamily, double fontSize, FontStyle fontStyle)
        {
            return new Font(fontFamily, (float)fontSize * FontsizeFactor, fontStyle);
        }

        /// <summary>
        /// Returns the ceiling of the given <see cref="SizeF"/> as a <see cref="SizeF"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>A <see cref="SizeF"/>.</returns>
        private static SizeF Ceiling(SizeF size)
        {
            var ceiling = Size.Ceiling(size);
            return new SizeF(ceiling.Width, ceiling.Height);
        }

        /// <summary>
        /// Loads the image from the specified source.
        /// </summary>
        /// <param name="source">The image source.</param>
        /// <returns>A <see cref="Image" />.</returns>
        private Image GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            Image src;
            if (this.imageCache.TryGetValue(source, out src))
            {
                return src;
            }

            Image btm;
            using (var ms = new MemoryStream(source.GetData()))
            {
                btm = Image.FromStream(ms);
            }

            this.imageCache.Add(source, btm);
            return btm;
        }

        /// <summary>
        /// Gets the cached brush.
        /// </summary>
        /// <param name="fill">The fill color.</param>
        /// <returns>A <see cref="Brush" />.</returns>
        private Brush GetCachedBrush(OxyColor fill)
        {
            Brush brush;
            if (this.brushes.TryGetValue(fill, out brush))
            {
                return brush;
            }

            return this.brushes[fill] = fill.ToBrush();
        }

        /// <summary>
        /// Gets the cached pen.
        /// </summary>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>A <see cref="Pen" />.</returns>
        private Pen GetCachedPen(OxyColor stroke, double thickness, double[] dashArray = null, OxyPlot.LineJoin lineJoin = OxyPlot.LineJoin.Miter)
        {
            GraphicsPenDescription description = new GraphicsPenDescription(stroke, thickness, dashArray, lineJoin);

            Pen pen;
            if (this.pens.TryGetValue(description, out pen))
            {
                return pen;
            }

            return this.pens[description] = CreatePen(stroke, thickness, dashArray, lineJoin);
        }

        /// <summary>
        /// Creates a pen.
        /// </summary>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>A <see cref="Pen" />.</returns>
        private Pen CreatePen(OxyColor stroke, double thickness, double[] dashArray = null, OxyPlot.LineJoin lineJoin = OxyPlot.LineJoin.Miter)
        {
            var pen = new Pen(stroke.ToColor(), (float)thickness);

            if (dashArray != null)
            {
                pen.DashPattern = this.ToFloatArray(dashArray);
            }

            switch (lineJoin)
            {
                case OxyPlot.LineJoin.Round:
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    break;
                case OxyPlot.LineJoin.Bevel:
                    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                    break;
                // The default LineJoin is Miter
            }

            return pen;
        }

        /// <summary>
        /// Sets the smoothing mode.
        /// </summary>
        /// <param name="useAntiAliasing">A value indicating whether to use Anti-Aliasing.</param>
        private void SetSmoothingMode(bool useAntiAliasing)
        {
            this.g.SmoothingMode = useAntiAliasing ? SmoothingMode.HighQuality : SmoothingMode.None;
        }

        /// <summary>
        /// Converts a double array to a float array.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <returns>The float array.</returns>
        private float[] ToFloatArray(double[] a)
        {
            if (a == null)
            {
                return null;
            }

            var r = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = (float)a[i];
            }

            return r;
        }

        /// <summary>
        /// Converts a list of point to an array of PointF.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>An array of points.</returns>
        private PointF[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
            {
                return null;
            }

            var r = new PointF[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
            {
                r[i++] = new PointF((float)p.X, (float)p.Y);
            }

            return r;
        }
    }
}
