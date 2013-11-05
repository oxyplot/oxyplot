// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
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
//   The graphics render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    using System;
    using System.Collections.Generic;
    using Cairo;
    using Gdk;
    using System.IO;
    using System.Linq;

    using OxyPlot;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    public class GraphicsRenderContext : RenderContextBase
    {
        /// <summary>
        /// The font size factor.
        /// </summary>
        private const double FontsizeFactor = 1.0;

        /// <summary>
        /// The GDI+ drawing surface.
        /// </summary>
        private Context g;

        /// <summary>
        /// Sets the graphics target.
        /// </summary>
        /// <param name="graphics">The graphics surface.</param>
        public void SetGraphicsTarget(Context graphics)
        {
            g = graphics;
            g.Antialias = Antialias.Subpixel; // TODO  .TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }

        /// <summary>
        /// Draws the ellipse.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            // center of ellipse
            double ex = rect.Left + rect.Width / 2.0;
            double ey = rect.Top + rect.Height / 2.0;
            // ellipse dimensions
            double ew = rect.Width;
            double eh = rect.Height;

            if (fill.IsVisible())
            {
                g.Save();

                g.Translate(ex, ey);  // make (ex, ey) == (0, 0)
                g.Scale(ew / 2.0, eh / 2.0);  // for width: ew / 2.0 == 1.0, eh / 2.0 == 1.0

                g.Arc(0.0, 0.0, 1.0, 0.0, 2.0 * Math.PI);  // 'circle' centered at (0, 0)
                g.ClosePath();
                g.SetSourceColor(fill);
                g.Fill();
                g.Restore();

            }

            if (stroke.IsVisible() && thickness > 0)
            {
                g.Save();
                // g.SmoothingMode = SmoothingMode.HighQuality; // TODO

                g.Translate(ex, ey);  // make (ex, ey) == (0, 0)
                g.Scale(ew / 2.0, eh / 2.0);  // for width: ew / 2.0 == 1.0
                // for height: eh / 2.0 == 1.0

                g.Arc(0.0, 0.0, 1.0, 0.0, 2.0 * Math.PI);  // 'circle' centered at (0, 0)
                g.SetSourceColor(stroke);
                g.LineWidth = thickness * 2.0 / ew;
                g.Stroke();
                g.Restore();
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
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (stroke.IsVisible() && thickness > 0 && points.Count >= 2)
            {
            // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                g.Save();
                g.SetSourceColor(stroke);
                g.LineJoin = lineJoin.ToLineJoin();
                g.LineWidth = thickness;
                if (dashArray != null)
                    g.SetDash(dashArray, 0);
                g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                    g.LineTo(point.ToPointD(aliased));
                g.Stroke();
                g.Restore();
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
            OxyPenLineJoin lineJoin,
            bool aliased)
        {

            if (fill.IsVisible() && points.Count >= 2)
            {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                g.Save();
                g.SetSourceColor(fill);
                g.LineJoin = lineJoin.ToLineJoin();
                g.LineWidth = thickness;
                if (dashArray != null)
                    g.SetDash(dashArray, 0);
                g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                    g.LineTo(point.ToPointD(aliased));
                //g.LineTo(points[0].ToPointD(aliased));
                g.ClosePath();
                g.Fill();
                g.Restore();
            }

            if (stroke.IsVisible() && thickness > 0 && points.Count >= 2)
            {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                g.Save();
                g.SetSourceColor(stroke);
                g.LineJoin = lineJoin.ToLineJoin();
                g.LineWidth = thickness;
                if (dashArray != null)
                    g.SetDash(dashArray, 0);
                g.MoveTo(points[0].ToPointD(aliased));
                foreach (var point in points.Skip(1))
                    g.LineTo(point.ToPointD(aliased));
                g.ClosePath();
                g.Stroke();
                g.Restore();
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
            if (fill.IsVisible())
            {
                g.Save();
                g.Rectangle(rect.ToRect(false));
                g.SetSourceColor(fill);
                g.Fill();
                g.Restore();
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                g.Save();
                g.SetSourceColor(stroke);
                g.LineWidth = thickness;
                g.Rectangle(rect.ToRect(false));
                g.Stroke();
                g.Restore();
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
            var fw = fontWeight >= 700 ? FontWeight.Bold : FontWeight.Normal;

            g.Save();
            g.SetFontSize(fontSize * FontsizeFactor);
            g.SelectFontFace(fontFamily, FontSlant.Normal, fw);
            //using (var sf = new StringFormat { Alignment = StringAlignment.Near })
            var size = g.TextExtents(text);
            if (maxSize != null)
            {
                size.Width = Math.Min(size.Width, maxSize.Value.Width);
                size.Height = Math.Min(size.Height, maxSize.Value.Height);
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

            g.Translate(p.X, p.Y);
            if (Math.Abs(rotate) > double.Epsilon)
            {
                g.Rotate(rotate * Math.PI / 180.0);
            }

            g.Translate(dx, dy);

            //g.Rectangle(0, 0, size.Width + 0.1f, size.Height + 0.1f);
            g.MoveTo(0, size.Height + 0.1f);
            g.SetSourceColor(fill);
            g.ShowText(text);

            g.Restore();
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

            var fs = (fontWeight >= 700) ? FontWeight.Bold : FontWeight.Normal;

            g.Save();
            g.SetFontSize((float)fontSize * FontsizeFactor);
            g.SelectFontFace(fontFamily, FontSlant.Normal, fs);
            var size = g.TextExtents(text);
            g.Restore();
            return new OxySize(size.Width, size.Height);
        }


        public override void CleanUp()
        {
            var imagesToRelease = imageCache.Keys.Where(i => !imagesInUse.Contains(i)).ToList();
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                imageCache.Remove(i);
            }

            imagesInUse.Clear();
        }

        public override void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double x, double y, double w, double h, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image != null)
            {
                // TODO: srcX, srcY
                g.Save();
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
                double rectw = w / scalex;
                double recth = h / scaley;
                g.Translate(x, y);
                g.Scale(scalex, scaley);
                g.Rectangle(0, 0, rectw, recth);
                Gdk.CairoHelper.SetSourcePixbuf(g, image, (rectw - image.Width) / 2.0, (recth - image.Height) / 2.0);
                g.Fill();

                // TODO: InterpolationMode
                // g.InterpolationMode = interpolate ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;
                
                g.Restore();
            }
        }

        private HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        private Dictionary<OxyImage, Gdk.Pixbuf> imageCache = new Dictionary<OxyImage, Gdk.Pixbuf>();


        private Gdk.Pixbuf GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            Gdk.Pixbuf src;
            if (this.imageCache.TryGetValue(source, out src))
            {
                return src;
            }

            Gdk.Pixbuf btm;
            using (var ms = new MemoryStream(source.GetData()))
            {
                btm = new Gdk.Pixbuf(ms);
            }

            this.imageCache.Add(source, btm);
            return btm;
        }

        public override bool SetClip(OxyRect rect)
        {
            g.Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
            g.Clip();
            return true;
        }

        public override void ResetClip()
        {
            g.ResetClip();
        }
    }
}
