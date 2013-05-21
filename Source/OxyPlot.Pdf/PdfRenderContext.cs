// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   PDF Render context using PdfSharp (and SilverPDF for Silverlight)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Pdf
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using PdfSharp.Drawing;
    using PdfSharp.Drawing.Layout;
    using PdfSharp.Pdf;

    /// <summary>
    /// Provides a render context for portable document format using PdfSharp (and SilverPDF for Silverlight)
    /// </summary>
    /// <remarks>
    /// see http://pdfsharp.codeplex.com and http://silverpdf.codeplex.com
    /// </remarks>
    internal class PdfRenderContext : RenderContextBase, IDisposable
    {
        /// <summary>
        /// The fontsize factor.
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
        /// The pdf page.
        /// </summary>
        private readonly PdfPage page;

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
            this.page = new PdfPage { Width = new XUnit(width), Height = new XUnit(height) };
            this.doc.AddPage(this.page);
            this.g = XGraphics.FromPdfPage(this.page);
            if (background != null)
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

        /// <summary>
        /// The draw ellipse.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
            {
                this.g.DrawEllipse(ToBrush(fill), rect.Left, rect.Top, rect.Width, rect.Height);
            }

            if (stroke != null && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);
                this.g.DrawEllipse(pen, rect.Left, rect.Top, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// The draw line.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (stroke == null || thickness <= 0)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? XSmoothingMode.None : XSmoothingMode.HighQuality;
            var pen = new XPen(ToColor(stroke), (float)thickness);

            if (dashArray != null)
            {
                pen.DashPattern = dashArray;
            }

            switch (lineJoin)
            {
                case OxyPenLineJoin.Round:
                    pen.LineJoin = XLineJoin.Round;
                    break;
                case OxyPenLineJoin.Bevel:
                    pen.LineJoin = XLineJoin.Bevel;
                    break;

                // The default LineJoin is Miter
            }

            this.g.DrawLines(pen, ToPoints(points));
        }

        /// <summary>
        /// The draw polygon.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            this.g.SmoothingMode = aliased ? XSmoothingMode.None : XSmoothingMode.HighQuality;

            var pts = ToPoints(points);

            if (fill != null)
            {
                this.g.DrawPolygon(ToBrush(fill), pts, XFillMode.Alternate);
            }

            if (stroke != null && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);

                if (dashArray != null)
                {
                    pen.DashPattern = dashArray;
                }

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        pen.LineJoin = XLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        pen.LineJoin = XLineJoin.Bevel;
                        break;

                    // The default LineJoin is Miter
                }

                this.g.DrawPolygon(pen, pts);
            }
        }

        /// <summary>
        /// The draw rectangle.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
            {
                this.g.DrawRectangle(ToBrush(fill), rect.Left, rect.Top, rect.Width, rect.Height);
            }

            if (stroke != null && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);
                this.g.DrawRectangle(pen, rect.Left, rect.Top, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// The draw text.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="fontFamily">
        /// The font family.
        /// </param>
        /// <param name="fontSize">
        /// The font size.
        /// </param>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <param name="rotate">
        /// The rotate.
        /// </param>
        /// <param name="halign">
        /// The halign.
        /// </param>
        /// <param name="valign">
        /// The valign.
        /// </param>
        /// <param name="maxSize">
        /// The maximum size of the text.
        /// </param>
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

            var font = new XFont(fontFamily, (float)fontSize * FontsizeFactor, fs);

            var size = this.g.MeasureString(text, font);

#if SILVERLIGHT
            // Correct the height, MeasureString returns 2x height
            size.Height *= 0.5;
#endif

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
                this.g.RotateAtTransform((float)rotate, new XPoint((float)p.X + (float)(size.Width / 2.0), (float)p.Y));
            }

            this.g.TranslateTransform((float)p.X, (float)p.Y);

            var layoutRectangle = new XRect(0, 0, size.Width, size.Height);
            var sf = new XStringFormat { Alignment = XStringAlignment.Near, LineAlignment = XLineAlignment.Near };
            g.DrawString(text, font, ToBrush(fill), layoutRectangle, sf);
            this.g.Restore(state);
        }

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="fontFamily">
        /// The font family.
        /// </param>
        /// <param name="fontSize">
        /// The font size.
        /// </param>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <returns>
        /// The text size.
        /// </returns>
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

            var font = new XFont(fontFamily, (float)fontSize * FontsizeFactor, fs);
            var size = this.g.MeasureString(text, font);

#if SILVERLIGHT
            // Correct the height, MeasureString returns 2x height
            size.Height *= 0.5;
#endif

            return new OxySize(size.Width, size.Height);
        }

        /// <summary>
        /// Save the document to a stream.
        /// </summary>
        /// <param name="s">
        /// The stream.
        /// </param>
        public void Save(Stream s)
        {
            this.doc.Save(s);
        }

        /// <summary>
        /// Converts an OxyColor to a brush.
        /// </summary>
        /// <param name="fill">
        /// The fill color.
        /// </param>
        /// <returns>
        /// The brush.
        /// </returns>
        private static XBrush ToBrush(OxyColor fill)
        {
            if (fill != null)
            {
                return new XSolidBrush(ToColor(fill));
            }

            return null;
        }

        /// <summary>
        /// Converts an OxyColor to an XColor.
        /// </summary>
        /// <param name="c">
        /// The source color.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        private static XColor ToColor(OxyColor c)
        {
            return XColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts a list of points.
        /// </summary>
        /// <param name="points">
        /// The list of points.
        /// </param>
        /// <returns>
        /// The points.
        /// </returns>
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
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
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

        /// <summary>
        /// Cleans up.
        /// </summary>
        public override void CleanUp()
        {
            var imagesToRelease = imageCache.Keys.Where(i => !imagesInUse.Contains(i));
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                imageCache.Remove(i);
            }

            imagesInUse.Clear();
        }

        /// <summary>
        /// Gets the image info.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public override OxyImageInfo GetImageInfo(OxyImage source)
        {
            var image = this.GetImage(source);
            return image == null ? null : new OxyImageInfo { Width = (uint)image.PixelWidth, Height = (uint)image.PixelHeight, DpiX = image.HorizontalResolution, DpiY = image.VerticalResolution };
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The SRC X.</param>
        /// <param name="srcY">The SRC Y.</param>
        /// <param name="srcWidth">Width of the SRC.</param>
        /// <param name="srcHeight">Height of the SRC.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">if set to <c>true</c> [interpolate].</param>
        public override void DrawImage(OxyImage source, uint srcX, uint srcY, uint srcWidth, uint srcHeight, double x, double y, double w, double h, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                // opacity not suported?
                // g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                g.DrawImage(image, new XRect(x, y, w, h), new XRect(srcX, srcY, srcWidth, srcHeight), XGraphicsUnit.Presentation);
            }
        }

        /// <summary>
        /// The images in use
        /// </summary>
        private HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private Dictionary<OxyImage, XImage> imageCache = new Dictionary<OxyImage, XImage>();


        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
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

#if !SILVERLIGHT
            XImage bitmap;
            using (var ms = new MemoryStream(source.GetData()))
            {
                var im = System.Drawing.Image.FromStream(ms);
                bitmap = XImage.FromGdiPlusImage(im);
            }

            this.imageCache.Add(source, bitmap);
            return bitmap;
#else
            using (var ms = new MemoryStream(source.GetData()))
            {
                var bitmap = XImage.FromStream(ms);
                this.imageCache.Add(source, bitmap);
                return bitmap;
            }
#endif
        }

        /// <summary>
        /// Sets the clip.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public override bool SetClip(OxyRect rect)
        {
            this.g.Save();
            this.g.IntersectClip(rect.ToXRect());
            return true;
        }

        /// <summary>
        /// Resets the clip.
        /// </summary>
        public override void ResetClip()
        {
            this.g.Restore();
        }
    }

    /// <summary>
    /// Provides extension methods for OxyPlot to PdfSharp type conversion.
    /// </summary>
    public static class PdfSharpExtensions
    {
        /// <summary>
        /// Converts an <see cref="OxyRect"/> to an <see cref="XRect"/>.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>The <see cref="XRect"/></returns>
        public static XRect ToXRect(this OxyRect r)
        {
            return new XRect((int)Math.Round(r.Left), (int)Math.Round(r.Top), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
        }
    }
}