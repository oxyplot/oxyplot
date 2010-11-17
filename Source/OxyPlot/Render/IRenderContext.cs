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

        void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness = 1.0, double[] dashArray = null, bool aliased = false);
        void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness = 1.0, double[] dashArray = null, bool aliased = false);
        void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke, double thickness = 1.0);
        void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left, VerticalTextAlign valign = VerticalTextAlign.Top);
        OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500);
    }
}