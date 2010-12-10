using System.Collections.Generic;

namespace OxyPlot
{
    public enum HorizontalTextAlign
    {
        Left = -1,
        Center = 0,
        Right = 1
    }

    public enum VerticalTextAlign
    {
        Top = -1,
        Middle = 0,
        Bottom = 1
    }
    public enum OxyPenLineJoin
    {
        Miter,
        Round,
        Bevel
    }

    public interface IRenderContext
    {
        double Width { get; }
        double Height { get; }

        void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness = 1.0,
                      double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false);

        void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness = 1.0,
                         double[] dashArray = null, bool aliased = false);

        void DrawRectangle(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                           double thickness = 1.0);

        void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                            double thickness = 1.0);

        void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10,
                      double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left,
                      VerticalTextAlign valign = VerticalTextAlign.Top);

        OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500);
    }
}