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

        // bool CanSetClipRectangle { get; }
        // void SetClipRectangle(OxyRect rect);

        /// <summary>
        /// Draws the multiple line segments defined by points (0,1) (2,3) (4,5) etc.
        /// This should have better performance than calling DrawLine for each segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness = 1.0,
                      double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false);

        /// <summary>
        /// Draws the polyline from the specified points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness = 1.0,
                      double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false);

        /// <summary>
        /// Draws the polygon from the specified points. The polygon can have stroke and/or fill.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness = 1.0,
                         double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false);

        /// <summary>
        /// Draws a collection of polygons, where all polygons have the same stroke and fill.
        /// This performs better than calling DrawPolygon multiple times.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">if set to <c>true</c> [aliased].</param>
        void DrawPolygons(IEnumerable<IEnumerable<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness = 1.0,
                         double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false);

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness = 1.0);

        /// <summary>
        /// Draws a collection of the rectangles, where all have the same stroke and fill.
        /// This performs better than calling DrawRectangle multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        void DrawRectangles(IEnumerable<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness = 1.0);

        /// <summary>
        /// Draws the ellipse.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness = 1.0);

        /// <summary>
        /// Draws the collection of ellipses, where all have the same stroke and fill.
        /// This performs better than calling DrawEllipse multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        void DrawEllipses(IEnumerable<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness = 1.0);

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotatation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10,
                      double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left,
                      VerticalTextAlign valign = VerticalTextAlign.Top);

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns></returns>
        OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500);
    }
}