using System.Collections.Generic;

namespace OxyPlot
{
    public static class RenderingExtensions
    {
        public static void DrawRectangle(this IRenderContext rc, OxyRect rect, OxyColor fill, OxyColor borderColor,
                                         double borderThickness)
        {
            //var border = new[]
            //                 {
            //                     new ScreenPoint(rect.Left, rect.Top), new ScreenPoint(rect.Right, rect.Top), 
            //                     new ScreenPoint(rect.Right, rect.Top), new ScreenPoint(rect.Right, rect.Bottom), 
            //                     new ScreenPoint(rect.Right, rect.Bottom), new ScreenPoint(rect.Left, rect.Bottom), 
            //                     new ScreenPoint(rect.Left, rect.Bottom),new ScreenPoint(rect.Left, rect.Top)
            //                 };
            //rc.DrawLineSegments(border,borderColor,borderThickness,null,OxyPenLineJoin.Miter,true);

            //             rc.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height, fill, borderColor, borderThickness);

            var border = new[]
                             {
                                 new ScreenPoint(rect.Left, rect.Top), new ScreenPoint(rect.Right, rect.Top), 
                                 new ScreenPoint(rect.Right, rect.Bottom), new ScreenPoint(rect.Left, rect.Bottom), 
                                 new ScreenPoint(rect.Left, rect.Top)
                             };

            rc.DrawPolygon(border, fill, borderColor, borderThickness, null, OxyPenLineJoin.Miter, true);
        }

        public static void DrawLine(this IRenderContext rc, double x0, double y0, double x1, double y1, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLine(new[]
                            {
                                new ScreenPoint(x0, y0), 
                                new ScreenPoint(x1, y1)
                            }, pen.Color, pen.Thickness, pen.DashArray, pen.LineJoin, aliased);
        }

        public static void DrawLineSegments(this IRenderContext rc, IList<ScreenPoint> points, OxyPen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            rc.DrawLineSegments(points, pen.Color, pen.Thickness, pen.DashArray, pen.LineJoin, aliased);
        }
    }
}