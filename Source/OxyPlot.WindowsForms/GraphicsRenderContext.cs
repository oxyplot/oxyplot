using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Oxyplot.WindowsForms
{
    public class GraphicsRenderContext : OxyPlot.IRenderContext
    {
        private readonly Graphics g;
        private readonly PlotControl pc;
        private float FONTSIZEFACTOR = 0.8f;

        public GraphicsRenderContext(PlotControl pc, Graphics graphics, Rectangle clipRectangle)
        {
            this.pc = pc;
            g = graphics;
        }

        #region IRenderContext Members

        public double Width
        {
            get { return pc.Width; }
        }

        public double Height
        {
            get { return pc.Height; }
        }

        public void DrawLine(IEnumerable<OxyPlot.Point> points, OxyPlot.Color stroke, double thickness, double[] dashArray, bool aliased)
        {
            if (stroke == null || thickness <= 0)
                return;

            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            var pen = new Pen(ToColor(stroke), (float)thickness);

            if (dashArray != null)
                pen.DashPattern = ToFloatArray(dashArray);
            g.DrawLines(pen, ToPoints(points));
        }

        public void DrawPolygon(IEnumerable<OxyPlot.Point> points, OxyPlot.Color fill, OxyPlot.Color stroke, double thickness,
                                double[] dashArray, bool aliased)
        {

            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            var pts = ToPoints(points);
            if (fill != null)
                g.FillPolygon(ToBrush(fill), pts);

            if (stroke != null && thickness > 0)
            {
                var pen = new Pen(ToColor(stroke), (float)thickness);

                if (dashArray != null)
                    pen.DashPattern = ToFloatArray(dashArray);

                g.DrawPolygon(pen, pts);
            }
        }

        private Brush ToBrush(OxyPlot.Color fill)
        {
            if (fill != null)
                return new SolidBrush(ToColor(fill));
            return null;
        }

        public void DrawText(OxyPlot.Point p, string text, OxyPlot.Color fill, string fontFamily, double fontSize, double fontWeight,
                             double rotate, OxyPlot.HorizontalTextAlign halign, OxyPlot.VerticalTextAlign valign)
        {
            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float)fontSize * FONTSIZEFACTOR, fs);

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            var size = g.MeasureString(text, font);

            float dx = 0;
            if (halign == OxyPlot.HorizontalTextAlign.Center)
            {
                dx = -size.Width / 2;
                //              sf.Alignment = StringAlignment.Center;
            }
            if (halign == OxyPlot.HorizontalTextAlign.Right)
            {
                dx = -size.Width;
                //                sf.Alignment = StringAlignment.Far;
            }

            float dy = 0;
            sf.LineAlignment = StringAlignment.Near;
            if (valign == OxyPlot.VerticalTextAlign.Middle)
            {
                // sf.LineAlignment = StringAlignment.Center;
                dy = -size.Height / 2;
            }
            if (valign == OxyPlot.VerticalTextAlign.Bottom)
            {
                //  sf.LineAlignment = StringAlignment.Far;
                dy = -size.Height;
            }

            g.TranslateTransform(dx, dy);
            if (rotate != 0)
                g.RotateTransform((float)rotate);
            g.TranslateTransform((float)p.X, (float)p.Y);

            g.DrawString(text, font, ToBrush(fill), 0, 0, sf);

            g.ResetTransform();
        }

        public OxyPlot.Size MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            var fs = FontStyle.Regular;
            if (fontWeight >= 500)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float)fontSize * FONTSIZEFACTOR, fs);

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            var size = g.MeasureString(text, font);
            return new OxyPlot.Size(size.Width, size.Height);
        }

        #endregion

        private PointF[] ToPoints(IEnumerable<OxyPlot.Point> points)
        {
            if (points == null)
                return null;
            var r = new PointF[points.Count()];
            int i = 0;
            foreach (var p in points)
                r[i++] = new PointF((float)p.X, (float)p.Y);
            return r;
        }

        private Color ToColor(OxyPlot.Color c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        private float[] ToFloatArray(double[] a)
        {
            if (a == null)
                return null;
            var r = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
                r[i] = (float)a[i];
            return r;
        }
    }
}