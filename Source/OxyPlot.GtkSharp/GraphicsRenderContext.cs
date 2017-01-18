// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The graphics render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Cairo;
    using Pango;

    using Gdk;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    public class GraphicsRenderContext : RenderContextBase
    {
        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<OxyImage, Pixbuf> imageCache = new Dictionary<OxyImage, Pixbuf>();

        /// <summary>
        /// The images in use.
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The GDI+ drawing surface.
        /// </summary>
        private Cairo.Context g;

        /// <summary>
        /// Sets the graphics target.
        /// </summary>
        /// <param name="graphics">The graphics surface.</param>
        public void SetGraphicsTarget(Cairo.Context graphics)
        {
            this.g = graphics;
            this.g.Antialias = Antialias.Subpixel; // TODO  .TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            // center of ellipse
            var ex = rect.Left + (rect.Width / 2.0);
            var ey = rect.Top + (rect.Height / 2.0);

            // ellipse dimensions
            var ew = rect.Width;
            var eh = rect.Height;

            if (fill.IsVisible())
            {
                this.g.Save();

                this.g.Translate(ex, ey); // make (ex, ey) == (0, 0)
                this.g.Scale(ew / 2.0, eh / 2.0); // for width: ew / 2.0 == 1.0, eh / 2.0 == 1.0

                this.g.Arc(0.0, 0.0, 1.0, 0.0, 2.0 * Math.PI); // 'circle' centered at (0, 0)
                this.g.ClosePath();
                this.g.SetSourceColor(fill);
                this.g.Fill();
                this.g.Restore();
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.g.Save();

                // g.SmoothingMode = SmoothingMode.HighQuality; // TODO
                this.g.Translate(ex, ey); // make (ex, ey) == (0, 0)
                this.g.Scale(ew / 2.0, eh / 2.0); // for width: ew / 2.0 == 1.0

                // for height: eh / 2.0 == 1.0
                this.g.Arc(0.0, 0.0, 1.0, 0.0, 2.0 * Math.PI); // 'circle' centered at (0, 0)
                this.g.SetSourceColor(stroke);
                this.g.LineWidth = thickness * 2.0 / ew;
                this.g.Stroke();
                this.g.Restore();
            }
        }

        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPlot.LineJoin lineJoin,
            bool aliased)
        {
            if (stroke.IsVisible() && thickness > 0 && points.Count >= 2)
            {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                this.g.Save();
                this.g.SetSourceColor(stroke);
                this.g.LineJoin = lineJoin.ToLineJoin();
                this.g.LineWidth = thickness;
                if (dashArray != null)
                {
                    this.g.SetDash(dashArray, 0);
                }

                this.g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                {
                    this.g.LineTo(point.ToPointD(aliased));
                }

                this.g.Stroke();
                this.g.Restore();
            }
        }

        /// <summary>
        /// Draws the polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPlot.LineJoin lineJoin,
            bool aliased)
        {
            if (fill.IsVisible() && points.Count >= 2)
            {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                this.g.Save();
                this.g.SetSourceColor(fill);
                this.g.LineJoin = lineJoin.ToLineJoin();
                this.g.LineWidth = thickness;
                if (dashArray != null)
                {
                    this.g.SetDash(dashArray, 0);
                }

                this.g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                {
                    this.g.LineTo(point.ToPointD(aliased));
                }

                // g.LineTo(points[0].ToPointD(aliased));
                this.g.ClosePath();
                this.g.Fill();
                this.g.Restore();
            }

            if (stroke.IsVisible() && thickness > 0 && points.Count >= 2)
            {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                this.g.Save();
                this.g.SetSourceColor(stroke);
                this.g.LineJoin = lineJoin.ToLineJoin();
                this.g.LineWidth = thickness;
                if (dashArray != null)
                {
                    this.g.SetDash(dashArray, 0);
                }

                this.g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                {
                    this.g.LineTo(point.ToPointD(aliased));
                }

                this.g.ClosePath();
                this.g.Stroke();
                this.g.Restore();
            }
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill.IsVisible())
            {
                this.g.Save();
                this.g.Rectangle(rect.ToRect(false));
                this.g.SetSourceColor(fill);
                this.g.Fill();
                this.g.Restore();
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.g.Save();
                this.g.SetSourceColor(stroke);
                this.g.LineWidth = thickness;
                this.g.Rectangle(rect.ToRect(false));
                this.g.Stroke();
                this.g.Restore();
            }
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
            Pango.Layout layout = Pango.CairoHelper.CreateLayout(this.g);
            Pango.FontDescription font = new Pango.FontDescription();
            font.Family = fontFamily;
            font.Weight = (fontWeight >= 700) ? Pango.Weight.Bold : Pango.Weight.Normal;
            font.AbsoluteSize = (int)(fontSize * Pango.Scale.PangoScale);
            layout.FontDescription = font;
            layout.SetText(text);
            Pango.Rectangle inkRect;
            Pango.Rectangle size;
            layout.GetExtents(out inkRect, out size);
            size.Width /= (int)Pango.Scale.PangoScale;
            size.Height /= (int)Pango.Scale.PangoScale;
            if (maxSize != null)
            {
                int maxWidth = (int)Math.Min((Double)Int32.MaxValue, maxSize.Value.Width);
                int maxHeight = (int)Math.Min((Double)Int32.MaxValue, maxSize.Value.Height);
                size.Width = Math.Min(size.Width, maxWidth);
                size.Height = Math.Min(size.Height, maxHeight);
            }
            this.g.Save();
            double dx = 0;
            if (halign == HorizontalAlignment.Center)
            {
                dx = -size.Width / 2;
            }

            if (halign == HorizontalAlignment.Right)
            {
                dx = -size.Width;
            }

            double dy = 0;
            if (valign == VerticalAlignment.Middle)
            {
                dy = -size.Height / 2;
            }

            if (valign == VerticalAlignment.Bottom)
            {
                dy = -size.Height;
            }

            this.g.Translate(p.X, p.Y);
            if (Math.Abs(rotate) > double.Epsilon)
            {
                this.g.Rotate(rotate * Math.PI / 180.0);
            }

            this.g.Translate(dx, dy);

            g.Rectangle(0, 0, size.Width + 0.1f, size.Height + 0.1f);
            g.Clip();
            this.g.SetSourceColor(fill);
            Pango.CairoHelper.ShowLayout(this.g, layout);
            this.g.Restore();
        }

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }
            this.g.Save();
            Pango.Layout layout = Pango.CairoHelper.CreateLayout(this.g);
            Pango.FontDescription font = new Pango.FontDescription();
            font.Family = fontFamily;
            font.Weight = (fontWeight >= 700) ? Pango.Weight.Bold : Pango.Weight.Normal;
            font.AbsoluteSize = (int)(fontSize * Pango.Scale.PangoScale);
            layout.FontDescription = font;
            layout.SetText(text);
            Pango.Rectangle inkRect;
            Pango.Rectangle logicalRect;
            layout.GetExtents(out inkRect, out logicalRect);
            this.g.Restore();
            return new OxySize(logicalRect.Width / Pango.Scale.PangoScale, logicalRect.Height / Pango.Scale.PangoScale);
        }

        /// <summary>
        /// The clean up.
        /// </summary>
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
        /// <param name="interpolate">Interpolate if set to <c>true</c>.</param>
        public override void DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double x,
            double y,
            double w,
            double h,
            double opacity,
            bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                // TODO: srcX, srcY
                this.g.Save();

                /*
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

                */
                double scalex = w / image.Width;
                double scaley = h / image.Height;

                // This alternative handles opacity, but can exhibit unattractive antialiasing artifacts when scaling up by large factors.
                // Pixbuf imageScaled = new Pixbuf(image.Colorspace, image.HasAlpha, image.BitsPerSample, (int)w, (int)h);
                // image.Composite(image, 0, 0, (int)w, (int)h, srcX, srcY, scalex, scaley, interpolate ? InterpType.Bilinear : InterpType.Nearest, (int)(opacity * 255));
                // image = imageScaled;
                
                if (!interpolate)
                {
                    image = image.ScaleSimple((int)w, (int)h, InterpType.Nearest);
                    scalex = 1.0;
                    scaley = 1.0;
                }
                this.g.Translate(x, y);
                this.g.Scale(scalex, scaley);
                this.g.Rectangle(0, 0, image.Width, image.Height);
                Gdk.CairoHelper.SetSourcePixbuf(
                    this.g,
                    image,
                    0.0,
                    0.0);
                this.g.Fill();
                this.g.Restore();
            }
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public override bool SetClip(OxyRect rect)
        {
            this.g.Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
            this.g.Clip();
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
        /// Gets the cached <see cref="Pixbuf" /> of the specified <see cref="OxyImage" />.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <returns>The <see cref="Pixbuf" />.</returns>
        private Pixbuf GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            Pixbuf src;
            if (this.imageCache.TryGetValue(source, out src))
            {
                return src;
            }

            Pixbuf btm;
            using (var ms = new MemoryStream(source.GetData()))
            {
                btm = new Pixbuf(ms);
            }

            this.imageCache.Add(source, btm);
            return btm;
        }
    }
}