using System.Collections.Generic;

namespace OxyPlot
{
    public enum HorizontalTextAlign
    {
        Left,
        Center,
        Right
    } ;
    public enum VerticalTextAlign
    {
        Top,
        Middle,
        Bottom
    }

    public interface IRenderContext
    {
        double Width { get; }
        double Height { get; }

        void DrawLine(IEnumerable<Point> points, Color stroke, double thickness = 1.0, double[] dashArray = null, bool aliased = false);
        void DrawPolygon(IEnumerable<Point> points, Color fill, Color stroke, double thickness = 1.0, double[] dashArray = null, bool aliased = false);
        void DrawText(Point p, string text, Color fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left, VerticalTextAlign valign = VerticalTextAlign.Top);
        Size MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500);
    }
}