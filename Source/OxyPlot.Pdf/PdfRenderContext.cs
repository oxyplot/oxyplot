// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfRenderContext.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   PDF Render context using PdfSharp (and SilverPDF for Silverlight)
//   see http://pdfsharp.codeplex.com
//   and http://silverpdf.codeplex.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using PdfSharp.Drawing;
    using PdfSharp.Pdf;

    /// <summary>
    /// PDF Render context using PdfSharp (and SilverPDF for Silverlight)
    /// see http://pdfsharp.codeplex.com
    /// and http://silverpdf.codeplex.com
    /// </summary>
    internal class PdfRenderContext : RenderContextBase
    {
        #region Constants and Fields

        /// <summary>
        /// The fontsiz e_ factor.
        /// </summary>
        private const double FONTSIZE_FACTOR = 1.0;

        /// <summary>
        /// The doc.
        /// </summary>
        private readonly PdfDocument doc;

        /// <summary>
        /// The g.
        /// </summary>
        private readonly XGraphics g;

        /// <summary>
        /// The page.
        /// </summary>
        private readonly PdfPage page;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfRenderContext"/> class.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public PdfRenderContext(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            this.PaintBackground = true;
            this.doc = new PdfDocument();
            this.page = new PdfPage { Width = new XUnit(width), Height = new XUnit(height) };
            this.doc.AddPage(this.page);
            this.g = XGraphics.FromPdfPage(this.page);
        }

        #endregion

        #region Public Methods

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

            XPoint[] pts = ToPoints(points);

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
        public override void DrawText(
            ScreenPoint p, 
            string text, 
            OxyColor fill, 
            string fontFamily, 
            double fontSize, 
            double fontWeight, 
            double rotate, 
            HorizontalTextAlign halign, 
            VerticalTextAlign valign)
        {
            if (text == null)
            {
                return;
            }

            XFontStyle fs = XFontStyle.Regular;
            if (fontWeight > FontWeights.Normal)
            {
                fs = XFontStyle.Bold;
            }

            var font = new XFont(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);

            var sf = new XStringFormat { Alignment = XStringAlignment.Near };

            XSize size = this.g.MeasureString(text, font);

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
            {
                dx = -size.Width / 2;
            }

            if (halign == HorizontalTextAlign.Right)
            {
                dx = -size.Width;
            }

            double dy = 0;
            sf.LineAlignment = XLineAlignment.Near;
            if (valign == VerticalTextAlign.Middle)
            {
                dy = -size.Height / 2;
            }

            if (valign == VerticalTextAlign.Bottom)
            {
                dy = -size.Height;
            }

            XGraphicsState state = this.g.Save();
            this.g.TranslateTransform(dx, dy);
            if (rotate != 0)
            {
                this.g.RotateTransform((float)rotate);
            }

            this.g.TranslateTransform((float)p.X, (float)p.Y);

            this.g.DrawString(text, font, ToBrush(fill), 0, 0, sf);
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
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            XFontStyle fs = XFontStyle.Regular;
            if (fontWeight > FontWeights.Normal)
            {
                fs = XFontStyle.Bold;
            }

            var font = new XFont(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);
            XSize size = this.g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public void Save(Stream s)
        {
            this.doc.Save(s);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The to brush.
        /// </summary>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <returns>
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
        /// The to color.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// </returns>
        private static XColor ToColor(OxyColor c)
        {
            return XColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// The to points.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// </returns>
        private static XPoint[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
            {
                return null;
            }

            var r = new XPoint[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
            {
                r[i++] = new XPoint((float)p.X, (float)p.Y);
            }

            return r;
        }

        #endregion
    }
}