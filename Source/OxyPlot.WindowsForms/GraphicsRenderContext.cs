using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using OxyPlot;

namespace Oxyplot.WindowsForms
{
    internal class GraphicsRenderContext : IRenderContext
    {
        private readonly Graphics g;
        private readonly Plot pc;
        private const float FONTSIZE_FACTOR = 0.8f;


        public GraphicsRenderContext(Plot pc, Graphics graphics, Rectangle clipRectangle)
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

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            for (int i = 0; i + 1 < points.Count; i+=2)
                DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke == null || thickness <= 0)
                return;

            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            var pen = new Pen(ToColor(stroke), (float)thickness);

            if (dashArray != null)
                pen.DashPattern = ToFloatArray(dashArray);
            switch (lineJoin)
            {
                case OxyPenLineJoin.Round:
                    pen.LineJoin = LineJoin.Round;
                    break;
                case OxyPenLineJoin.Bevel:
                    pen.LineJoin = LineJoin.Bevel;
                    break;
                //  The default LineJoin is Miter
            }
            g.DrawLines(pen, ToPoints(points));
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            PointF[] pts = ToPoints(points);
            if (fill != null)
                g.FillPolygon(ToBrush(fill), pts);

            if (stroke != null && thickness > 0)
            {
                var pen = new Pen(ToColor(stroke), (float)thickness);

                if (dashArray != null)
                    pen.DashPattern = ToFloatArray(dashArray);
              
                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        pen.LineJoin = LineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        pen.LineJoin = LineJoin.Bevel;
                        break;
                    //  The default LineJoin is Miter
                }

                g.DrawPolygon(pen, pts);
            }
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight,
                             double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            FontStyle fs = FontStyle.Regular;
            if (fontWeight >= 700)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            SizeF size = g.MeasureString(text, font);

            float dx = 0;
            if (halign == HorizontalTextAlign.Center)
            {
                dx = -size.Width / 2;
                //              sf.Alignment = StringAlignment.Center;
            }
            if (halign == HorizontalTextAlign.Right)
            {
                dx = -size.Width;
                //                sf.Alignment = StringAlignment.Far;
            }

            float dy = 0;
            sf.LineAlignment = StringAlignment.Near;
            if (valign == VerticalTextAlign.Middle)
            {
                // sf.LineAlignment = StringAlignment.Center;
                dy = -size.Height / 2;
            }
            if (valign == VerticalTextAlign.Bottom)
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

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
                return OxySize.Empty;

            FontStyle fs = FontStyle.Regular;
            if (fontWeight >= 500)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float)fontSize * FONTSIZE_FACTOR, fs);

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            SizeF size = g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
        }

        public void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
                g.FillRectangle(ToBrush(fill), (float)x, (float)y, (float)width, (float)height);
            if (stroke == null || thickness <= 0)
                return;
            var pen = new Pen(ToColor(stroke), (float)thickness);
            g.DrawRectangle(pen, (float)x, (float)y, (float)width, (float)height);
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
                g.FillEllipse(ToBrush(fill), (float)x, (float)y, (float)width, (float)height);
            if (stroke == null || thickness <= 0)
                return;
            var pen = new Pen(ToColor(stroke), (float)thickness);
            g.DrawEllipse(pen, (float)x, (float)y, (float)width, (float)height);
        }
        #endregion

        private Brush ToBrush(OxyColor fill)
        {
            if (fill != null)
                return new SolidBrush(ToColor(fill));
            return null;
        }

        private PointF[] ToPoints(IEnumerable<ScreenPoint> points)
        {
            if (points == null)
                return null;
            var r = new PointF[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
                r[i++] = new PointF((float)p.X, (float)p.Y);
            return r;
        }

        private Color ToColor(OxyColor c)
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