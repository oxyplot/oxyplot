using System;
using System.Collections.Generic;
using System.IO;

namespace OxyPlot
{
    public class SvgRenderContext : RenderContextBase, IDisposable
    {
        private SvgWriter w;

        public SvgRenderContext(Stream s, double width, double height, bool isDocument)
        {
            w = new SvgWriter(s, width, height, isDocument);
            this.Width = width;
            this.Height = height;
        }

        public SvgRenderContext(string path, double width, double height)
        {
            w = new SvgWriter(path, width, height);
            this.Width = width;
            this.Height = height;
        }

        public override void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            w.WritePolyline(points, w.CreateStyle(null, stroke, thickness, dashArray, lineJoin));
        }

        public override void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            w.WritePolygon(points, w.CreateStyle(fill, stroke, thickness, dashArray, lineJoin));
        }

        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            w.WriteEllipse(rect.Left, rect.Top, rect.Width, rect.Height, w.CreateStyle(fill, stroke, thickness, null));
        }

        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            w.WriteRectangle(rect.Left, rect.Top, rect.Width, rect.Height, w.CreateStyle(fill, stroke, thickness, null));
        }
        public override void DrawText(ScreenPoint p, string text, OxyColor c, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            w.WriteText(p, text, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        }

        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            // todo: should improve text measuring, currently using p/invoke on GDI32
            // is it better to use winforms or wpf text measuring?

            return Gdi32.MeasureString(fontFamily, (int)fontSize, (int)fontWeight, text);
        }


        public void Complete()
        {
            w.Complete();
        }

        public void Flush()
        {
            w.Flush();
        }

        public void Close()
        {
            w.Close();
        }

        public void Dispose()
        {
            w.Dispose();
        }
    }
}