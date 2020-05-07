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

    /// <summary>
    /// The graphics render context.
    /// </summary>
    public class GraphicsRenderContext : RenderContextBase, IDisposable
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
        }

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
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
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
            if (text == null)
            {
                return;
            }

            var fontStyle = fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;

            using (var font = CreateFont(fontFamily, fontSize, fontStyle))
            {
                this.stringFormat.Alignment = StringAlignment.Near;
                this.stringFormat.LineAlignment = StringAlignment.Near;
                var size = Ceiling(this.g.MeasureString(text, font, int.MaxValue, this.stringFormat));
                if (maxSize != null)
                {
                    if (size.Width > maxSize.Value.Width)
                    {
                        size.Width = (float)maxSize.Value.Width;
                    }

                    if (size.Height > maxSize.Value.Height)
                    {
                        size.Height = (float)maxSize.Value.Height;
                    }
                }

                float dx = 0;
                if (halign == HorizontalAlignment.Center)
                {
                    dx = -size.Width / 2;
                }

                if (halign == HorizontalAlignment.Right)
                {
                    dx = -size.Width;
                }

                float dy = 0;
                this.stringFormat.LineAlignment = StringAlignment.Near;
                if (valign == VerticalAlignment.Middle)
                {
                    dy = -size.Height / 2;
                }

                if (valign == VerticalAlignment.Bottom)
                {
                    dy = -size.Height;
                }

                var graphicsState = this.g.Save();

                this.g.TranslateTransform((float)p.X, (float)p.Y);

                var layoutRectangle = new RectangleF(0, 0, size.Width, size.Height);
                if (Math.Abs(rotate) > double.Epsilon)
                {
                    this.g.RotateTransform((float)rotate);

                    layoutRectangle.Height += (float)(fontSize / 18.0);
                }

                this.g.TranslateTransform(dx, dy);

                this.g.DrawString(text, font, this.GetCachedBrush(fill), layoutRectangle, this.stringFormat);

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
            if (text == null)
            {
                return OxySize.Empty;
            }

            var fontStyle = fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;
            using (var font = CreateFont(fontFamily, fontSize, fontStyle))
            {
                this.stringFormat.Alignment = StringAlignment.Near;
                this.stringFormat.LineAlignment = StringAlignment.Near;
                var size = this.g.MeasureString(text, font, int.MaxValue, this.stringFormat);
                return new OxySize(size.Width, size.Height);
            }
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

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public override bool SetClip(OxyRect rect)
        {
            this.g.SetClip(rect.ToRect(false));
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.g.ResetClip();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
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
