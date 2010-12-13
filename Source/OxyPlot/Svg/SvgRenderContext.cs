using System;
using System.Collections.Generic;
using System.IO;

namespace OxyPlot
{
    public class SvgRenderContext : SvgWriter, IRenderContext
    {
        public SvgRenderContext(Stream s, double width, double height, bool isDocument)
            : base(s, width, height, isDocument)
        {
            this.Width = width;
            this.Height = height;
        }

        public SvgRenderContext(string path, double width, double height)
            : base(path, width, height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            for (int i = 0; i + 1 < points.Count; i+=2)
                DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            WritePolyline(points, CreateStyle(null, stroke, thickness, dashArray, lineJoin));
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            WritePolygon(points, CreateStyle(fill, stroke, thickness, dashArray, lineJoin));
        }

        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness)
        {
            WriteEllipse(x, y, width, height, CreateStyle(fill, stroke, thickness, null));
        }
        public void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness)
        {
            WriteRectangle(x, y, width, height, CreateStyle(fill, stroke, thickness, null));
        }
        public void DrawText(ScreenPoint p, string text, OxyColor c, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            WriteText(p, text, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            // todo: should improve text measuring, currently using p/invoke on GDI32
            // is it better to use winforms or wpf text measuring?

            return Gdi32.MeasureString(fontFamily, (int)fontSize, (int)fontWeight, text);
        }


    }
}