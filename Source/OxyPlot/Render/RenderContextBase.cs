using System.Collections.Generic;

namespace OxyPlot
{
    public abstract class RenderContextBase : IRenderContext
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public virtual void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            for (int i = 0; i + 1 < points.Count; i += 2)
                DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public abstract void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased);

        public abstract void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased);

        public virtual void DrawPolygons(IEnumerable<IEnumerable<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            foreach (var polygon in polygons)
                DrawPolygon(polygon, fill, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public abstract void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness);

        public virtual void DrawRectangles(IEnumerable<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            foreach (var r in rectangles)
                DrawRectangle(r, fill, stroke, thickness);
        }

        public abstract void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness);

        public virtual void DrawEllipses(IEnumerable<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            foreach (var r in rectangles)
                DrawEllipse(r, fill, stroke, thickness);
        }

        public abstract void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign);

        public abstract OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight);
    }
}