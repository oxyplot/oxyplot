using System;
using System.Collections.Generic;
using System.IO;

namespace OxyPlot
{
    public class SvgRenderContext : SvgWriter, IRenderContext
    {
        public SvgRenderContext(Stream s, double width, double height, bool isDocument)
            : base(s, width, height,isDocument)
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

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, bool aliased)
        {
            WritePolyline(points, CreateStyle(null, stroke, thickness, dashArray));
        }
        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, bool aliased)
        {
            WritePolygon(points, CreateStyle(fill, stroke, thickness, dashArray));
        }

        public void DrawText(ScreenPoint p, string text, OxyColor c, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            WriteText(p, text, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            // todo: how to measure svg text?
            return new OxySize(text.Length * 20, fontSize * 4 / 3);
        }
    }
}