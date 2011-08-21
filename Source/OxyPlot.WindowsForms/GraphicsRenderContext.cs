using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using OxyPlot;

namespace Oxyplot.WindowsForms
{
    internal class GraphicsRenderContext : RenderContextBase
    {
        private readonly Graphics g;
        private const float FONTSIZE_FACTOR = 0.8f;

        public GraphicsRenderContext(Graphics graphics, double width, double height)
        {
            this.Width = width;
            this.Height = height;
            g = graphics;
        }

        #region IRenderContext Members

        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke == null || thickness <= 0)
                return;

            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            var pen = new Pen(ToColor(stroke), (float) thickness);

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

        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            var pts = ToPoints(points);
            if (fill != null)
                g.FillPolygon(ToBrush(fill), pts);

            if (stroke != null && thickness > 0)
            {
                var pen = new Pen(ToColor(stroke), (float) thickness);

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
               
        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight,
                             double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float) fontSize*FONTSIZE_FACTOR, fs);

            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            var size = g.MeasureString(text, font);

            float dx = 0;
            if (halign == HorizontalTextAlign.Center)
            {
                dx = -size.Width/2;
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
                dy = -size.Height/2;
            }
            if (valign == VerticalTextAlign.Bottom)
            {
                //  sf.LineAlignment = StringAlignment.Far;
                dy = -size.Height;
            }

            g.TranslateTransform((float) p.X, (float) p.Y);
            if (rotate != 0)
                g.RotateTransform((float) rotate);
            g.TranslateTransform(dx, dy);

            g.DrawString(text, font, ToBrush(fill), 0, 0, sf);

            g.ResetTransform();
        }

        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
                return OxySize.Empty;

            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
                fs = FontStyle.Bold;
            var font = new Font(fontFamily, (float) fontSize*FONTSIZE_FACTOR, fs);
            var size = g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
        }

        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke,
                                  double thickness)
        {
            if (fill != null)
                g.FillRectangle(ToBrush(fill), (float) rect.Left, (float) rect.Top, (float) rect.Width, (float) rect.Height);
            if (stroke == null || thickness <= 0)
                return;
            var pen = new Pen(ToColor(stroke), (float) thickness);
            g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            if (fill != null)
                g.FillEllipse(ToBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            if (stroke == null || thickness <= 0)
                return;
            var pen = new Pen(ToColor(stroke), (float) thickness);
            g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

        #endregion

        private Brush ToBrush(OxyColor fill)
        {
            if (fill != null)
                return new SolidBrush(ToColor(fill));
            return null;
        }

        private PointF[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
                return null;
            var r = new PointF[points.Count()];
            int i = 0;
            foreach (var p in points)
                r[i++] = new PointF((float) p.X, (float) p.Y);
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
                r[i] = (float) a[i];
            return r;
        }
    }
}