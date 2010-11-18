using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OxyPlot.Pdf
{
    public class PdfRenderContext : IRenderContext
    {
        private readonly PdfDocument doc;
        private readonly XGraphics g;
        private readonly PdfPage page;
        private const double FONTSIZE_FACTOR = 1.0;


        public PdfRenderContext(double width, double height)
        {
            Width = width;
            Height = height;
            doc = new PdfDocument();
            page = new PdfPage();
            page.Width = new XUnit(width);
            page.Height = new XUnit(height);
            doc.AddPage(page);
            g = XGraphics.FromPdfPage(page);
        }

        #region IRenderContext Members

        public double Width { get; private set; }
        public double Height { get; private set; }

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             bool aliased)
        {
            if (stroke == null || thickness <= 0)
                return;

            // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            var pen = new XPen(ToColor(stroke), (float)thickness);

            if (dashArray != null)
                pen.DashPattern = dashArray;
            g.DrawLines(pen, ToPoints(points));
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, bool aliased)
        {
            // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            XPoint[] pts = ToPoints(points);
            // todo: does not support fill?

            //if (fill != null)
            //    g.FillPolygon(ToBrush(fill), pts);

            if (stroke != null && thickness > 0)
            {
                var pen = new XPen(ToColor(stroke), (float)thickness);

                if (dashArray != null)
                    pen.DashPattern = dashArray;

                g.DrawPolygon(pen, pts);
            }
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            if (text == null)
                return;
            XFontStyle fs = XFontStyle.Regular;
            if (fontWeight >= 700)
                fs = XFontStyle.Bold;
            var font = new XFont(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);

            var sf = new XStringFormat();
            sf.Alignment = XStringAlignment.Near;

            XSize size = g.MeasureString(text, font);

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

            XGraphicsState state = g.Save();
            g.TranslateTransform(dx, dy);
            if (rotate != 0)
                g.RotateTransform((float)rotate);
            g.TranslateTransform((float)p.X, (float)p.Y);

            g.DrawString(text, font, ToBrush(fill), 0, 0, sf);
            g.Restore(state);
            // g.ResetTransform();
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
                return OxySize.Empty;
            XFontStyle fs = XFontStyle.Regular;
            if (fontWeight >= 500)
                fs = XFontStyle.Bold;
            var font = new XFont(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);

            var sf = new XStringFormat();
            sf.Alignment = XStringAlignment.Near;

            XSize size = g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness)
        {
            // todo: does not support fill?
            if (stroke == null || thickness <= 0)
                return;
            var pen = new XPen(ToColor(stroke), (float)thickness);
            g.DrawEllipse(pen, x, y, width, height);
        }


        #endregion

        private XPoint[] ToPoints(IEnumerable<ScreenPoint> points)
        {
            if (points == null)
                return null;
            var r = new XPoint[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
                r[i++] = new XPoint((float)p.X, (float)p.Y);
            return r;
        }

        private XColor ToColor(OxyColor c)
        {
            return XColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        private XBrush ToBrush(OxyColor fill)
        {
            if (fill != null)
                return new XSolidBrush(ToColor(fill));
            return null;
        }

        public void Save(Stream s)
        {
            doc.Save(s);
        }
    }

    public static class PdfPlotWriter
    {
        public static void Save(PlotModel model, string path, double width, double height)
        {
            using (FileStream s = File.OpenWrite(path))
            {
                Save(model, s, width, height);
            }
        }

        public static void Save(PlotModel model, Stream s, double width, double height)
        {
            var svgrc = new PdfRenderContext(width, height);
            model.Render(svgrc);
            svgrc.Save(s);
        }
    }
}