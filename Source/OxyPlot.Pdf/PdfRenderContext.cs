// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a render context for portable document format using PdfSharp (and SilverPDF for Silverlight)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;

    /// <summary>
    /// Provides a render context for portable document format using PdfSharp (and SilverPDF for Silverlight)
    /// </summary>
    /// <remarks>see http://pdfsharp.codeplex.com and http://silverpdf.codeplex.com</remarks>
    internal class PdfRenderContext : RenderContextBase, IDisposable
    {
        /// <summary>
        /// The font size factor.
        /// </summary>
        private const double FontsizeFactor = 1.0;

        /// <summary>
        /// The pdf document.
        /// </summary>
        private readonly PdfDocument doc;

        /// <summary>
        /// The PdfSharp graphics context.
        /// </summary>
        private readonly XGraphics g;

        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, XImage> imageCache = new Dictionary<OxyImage, XImage>();

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfRenderContext" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        public PdfRenderContext(double width, double height, OxyColor background)
        {
            this.RendersToScreen = false;
            this.doc = new PdfDocument();
            var page = new PdfPage { Width = new XUnit(width), Height = new XUnit(height) };
            this.doc.AddPage(page);
            this.g = XGraphics.FromPdfPage(page);
            if (background.IsVisible())
            {
                this.g.DrawRectangle(ToBrush(background), 0, 0, width, height);
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

        /// <inheritdoc/>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.SetSmoothingMode(this.ShouldUseAntiAliasingForEllipse(edgeRenderingMode));
            
            if (fill.IsVisible())
            {
                this.g.DrawEllipse(ToBrush(fill), rect.Left, rect.Top, rect.Width, rect.Height);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);
                this.g.DrawEllipse(pen, rect.Left, rect.Top, rect.Width, rect.Height);
            }
        }

        /// <inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (stroke.IsInvisible() || thickness <= 0)
            {
                return;
            }

            this.SetSmoothingMode(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));
            var pen = new XPen(ToColor(stroke), (float)thickness);

            if (dashArray != null)
            {
                pen.DashPattern = dashArray;
            }

            switch (lineJoin)
            {
                case LineJoin.Round:
                    pen.LineJoin = XLineJoin.Round;
                    break;
                case LineJoin.Bevel:
                    pen.LineJoin = XLineJoin.Bevel;
                    break;

                    // The default LineJoin is Miter
            }

            this.g.DrawLines(pen, ToPoints(points));
        }

        /// <inheritdoc/>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            this.SetSmoothingMode(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));

            var pts = ToPoints(points);

            if (fill.IsVisible())
            {
                this.g.DrawPolygon(ToBrush(fill), pts, XFillMode.Alternate);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);

                if (dashArray != null)
                {
                    pen.DashPattern = dashArray;
                }

                switch (lineJoin)
                {
                    case LineJoin.Round:
                        pen.LineJoin = XLineJoin.Round;
                        break;
                    case LineJoin.Bevel:
                        pen.LineJoin = XLineJoin.Bevel;
                        break;

                        // The default LineJoin is Miter
                }

                this.g.DrawPolygon(pen, pts);
            }
        }

        /// <inheritdoc/>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.SetSmoothingMode(this.ShouldUseAntiAliasingForRect(edgeRenderingMode));
            if (fill.IsVisible())
            {
                this.g.DrawRectangle(ToBrush(fill), rect.Left, rect.Top, rect.Width, rect.Height);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);
                this.g.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
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
            if (text == null)
            {
                return;
            }

            var fs = XFontStyle.Regular;
            if (fontWeight > FontWeights.Normal)
            {
                fs = XFontStyle.Bold;
            }

            var font = CreateFont(fontFamily, fontSize, fs);

            var size = this.g.MeasureString(text, font);

            if (maxSize != null)
            {
                if (size.Width > maxSize.Value.Width)
                {
                    size.Width = Math.Max(maxSize.Value.Width, 0);
                }

                if (size.Height > maxSize.Value.Height)
                {
                    size.Height = Math.Max(maxSize.Value.Height, 0);
                }
            }

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

            var state = this.g.Save();
            this.g.TranslateTransform(dx, dy);
            if (Math.Abs(rotate) > double.Epsilon)
            {
                this.g.RotateAtTransform((float)rotate, new XPoint((float)p.X - dx, (float)p.Y - dy));
            }

            this.g.TranslateTransform((float)p.X, (float)p.Y);

            var layoutRectangle = new XRect(0, 0, size.Width, size.Height);
            var sf = new XStringFormat { Alignment = XStringAlignment.Near, LineAlignment = XLineAlignment.Near };
            this.g.DrawString(text, font, ToBrush(fill), layoutRectangle, sf);
            this.g.Restore(state);
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

            var fs = XFontStyle.Regular;
            if (fontWeight > FontWeights.Normal)
            {
                fs = XFontStyle.Bold;
            }

            var font = CreateFont(fontFamily, fontSize, fs);
            var size = this.g.MeasureString(text, font);

            return new OxySize(size.Width, size.Height);
        }

        /// <summary>
        /// Saves the document to a stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        public void Save(Stream s)
        {
            this.doc.Save(s);
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        public override void CleanUp()
        {
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i));
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();
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
        public override void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double destX, double destY, double destWidth, double destHeight, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                // opacity not suported?
                // g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                this.g.DrawImage(image, new XRect(destX, destY, destWidth, destHeight), new XRect(srcX, srcY, srcWidth, srcHeight), XGraphicsUnit.Presentation);
            }
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public override bool SetClip(OxyRect rect)
        {
            this.g.Save();
            this.g.IntersectClip(rect.ToXRect());
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.g.Restore();
        }

        /// <summary>
        /// Converts an OxyColor to a brush.
        /// </summary>
        /// <param name="fill">The fill color.</param>
        /// <returns>The brush.</returns>
        private static XBrush ToBrush(OxyColor fill)
        {
            if (fill.IsVisible())
            {
                return new XSolidBrush(ToColor(fill));
            }

            return null;
        }

        /// <summary>
        /// Converts an OxyColor to an XColor.
        /// </summary>
        /// <param name="c">The source color.</param>
        /// <returns>The color.</returns>
        private static XColor ToColor(OxyColor c)
        {
            return XColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts a list of points.
        /// </summary>
        /// <param name="points">The list of points.</param>
        /// <returns>The points.</returns>
        private static XPoint[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
            {
                return null;
            }

            var r = new XPoint[points.Count()];
            int i = 0;
            foreach (var p in points)
            {
                r[i++] = new XPoint((float)p.X, (float)p.Y);
            }

            return r;
        }

        /// <summary>
        /// Creates the specified font.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontStyle">The font style.</param>
        /// <returns>The font.</returns>
        private static XFont CreateFont(string fontFamily, double fontSize, XFontStyle fontStyle)
        {
            var pdfOptions = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var font = new XFont(fontFamily ?? "Arial", (float)fontSize * FontsizeFactor, fontStyle, pdfOptions);
            return font;
        }

        /// <summary>
        /// Gets or creates a <see cref="XImage" /> from the specified image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The image</returns>
        private XImage GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            XImage src;
            if (this.imageCache.TryGetValue(source, out src))
            {
                return src;
            }

            XImage bitmap;
            using (var ms = new MemoryStream(source.GetData()))
            {
                var im = System.Drawing.Image.FromStream(ms);
                bitmap = XImage.FromGdiPlusImage(im);
            }

            this.imageCache.Add(source, bitmap);
            return bitmap;
        }

        /// <summary>
        /// Sets the smoothing mode.
        /// </summary>
        /// <param name="useAntiAliasing">A value indicating whether to use Anti-Aliasing.</param>
        private void SetSmoothingMode(bool useAntiAliasing)
        {
            this.g.SmoothingMode = useAntiAliasing ? XSmoothingMode.HighQuality : XSmoothingMode.None;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.doc != null)
                    {
                        this.doc.Dispose();
                    }
                }
            }

            this.disposed = true;
        }
    }
}
